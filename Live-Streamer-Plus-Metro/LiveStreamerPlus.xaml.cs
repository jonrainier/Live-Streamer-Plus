using LiveStreamerPlus.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveStreamerPlus
{
    public partial class MainWindow : Elysium.Controls.Window
    {
        public static string ApplicationName = "Live Streamer Plus :: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public MainWindow()
        {
            InitializeComponent();
            this.Title = ApplicationName;
        }

        // Initialize other classes
        Log classLog = new Log();

        // Do once the form is loaded
        private void formLiveStreamerPlus_Loaded(object sender, RoutedEventArgs e)
        {
            classLog.logWithoutTime(
                "Live Streamer Plus created by Pwnoz0r: https://github.com/pwnoz0r" + "\r\n" +
                "VLC Media Player: http://www.videolan.org/vlc/index.html" + "\r\n" +
                "Livestreamer: http://livestreamer.tanuki.se/en/latest" + "\r\n" +
                "-------------------------------------------------------------" + "\r\n", rtb_Console);
            this.CheckForUpdates();
            this.CheckInstalledPrograms(1);
            this.checkSettings();
        }

        private void checkSettings()
        {
            int doChat = Int32.Parse(ConfigurationManager.AppSettings["doChat"]);
            if (!(doChat == 0))
            {
                checkBox_doChat.IsChecked = true;
            }
        }

        // Check for updates for the application.
        private void CheckForUpdates()
        {
            WebClient GetUpdateURL = new WebClient();
            GetUpdateURL.Proxy = null;

            string GetUpdateURLAsString = GetUpdateURL.DownloadString("http://pwnoz0r.com/software/lsp/configs/version.txt");
            string ProductVersionAsString = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            try
            {
                if (!(GetUpdateURLAsString == ProductVersionAsString))
                {
                    MessageBox.Show("Update available!", ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    this.DoUpdate();
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.Close();
            }
        }

        // Call the updater.
        private void DoUpdate()
        {
            try
            {
                Process.Start(Directory.GetCurrentDirectory() + @"\Live-Streamer-Plus-Updater.exe");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        public static string ApplicationDataLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // Check if the prerequisites are installed.
        private void CheckInstalledPrograms(int Programs)
        {
            switch (Programs)
            {
                case 1:
                    try
                    {
                        string VLCInstallLocation = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\VideoLAN\\VLC", "", null);
                        string LivestreamerConfigLocation = ApplicationDataLocation + @"\livestreamer\livestreamerrc";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("One of the dependencies is not installed. Please make sure to install all dependencies before running this application.\r\n\r\n\r\n" +
                            ex.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Close();
                    }
                    break;

                default:
                    break;
            }
        }

        // When the "Go!" button is clicked, do things.
        private void btn_Go_Click(object sender, RoutedEventArgs e)
        {            
            if (tb_Channel.Text == string.Empty || cb_StreamQuality.Text == string.Empty)
            {
                System.Media.SystemSounds.Hand.Play();
                classLog.logWithTime("Error: One of the required fields were left blank!", rtb_Console);
            }
            else
            {
                //this.OpenTwitchChat();
                ProcessStartInfo StartChat = new ProcessStartInfo("http://www.twitch.tv/chat/embed?channel=" + tb_Channel.Text + "&popout_chat=true");
                StartChat.WindowStyle = ProcessWindowStyle.Normal;
                if (checkBox_doChat.IsChecked == true)
                {
                    Process.Start(StartChat);
                }

                string sourceTwitch = "twitch.tv";

                classLog.logWithTime("Started Stream: " + "'" + tb_Channel.Text + "'" + " " + "on" + " '" + cb_StreamSource.Text + "' " + " with" + " '" + cb_StreamQuality.Text + "' " + "quality.", rtb_Console);
                Process StartCMD = new Process();
                StartCMD.StartInfo.FileName = "cmd.exe";
                if (cb_StreamSource.SelectedIndex == 0)
                {
                    StartCMD.StartInfo.Arguments = "/c livestreamer " + sourceTwitch + "/" + tb_Channel.Text + " " + cb_StreamQuality.Text;
                }
                StartCMD.StartInfo.RedirectStandardOutput = true;
                StartCMD.StartInfo.CreateNoWindow = true;
                StartCMD.StartInfo.UseShellExecute = false;
                StartCMD.Start();
            }
        }

        Configuration appConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        private void btn_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_doChat.IsChecked == true)
            {
                appConfig.AppSettings.Settings.Remove("doChat");
                appConfig.AppSettings.Settings.Add("doChat", "1");
            }
            else
            {
                appConfig.AppSettings.Settings.Remove("doChat");
                appConfig.AppSettings.Settings.Add("doChat", "0");
            }
            appConfig.Save(ConfigurationSaveMode.Minimal);
        }
    }
}