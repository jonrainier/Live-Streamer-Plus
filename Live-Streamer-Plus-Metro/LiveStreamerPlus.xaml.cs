using LiveStreamerPlus.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
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
            this.clearContent();
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

        // Check the config for user settings.
        int doChat = Int32.Parse(ConfigurationManager.AppSettings["doChat"]);
        int doStats = Int32.Parse(ConfigurationManager.AppSettings["doStats"]);
        int getAvatar = Int32.Parse(ConfigurationManager.AppSettings["getAvatar"]);
        string getChattyPath = ConfigurationManager.AppSettings["getChattyPath"];

        private void checkSettings()
        {
            if (!(doChat == 0))
            {
                checkBox_doChat.IsChecked = true;
            }
            if (!(doStats == 0))
            {
                checkbox_doStats.IsChecked = true;
            }
            if (!(getAvatar == 0))
            {
                checkbox_getAvatar.IsChecked = true;
            }
            if (!(getChattyPath.Contains(".jar")))
            {
                checkBox_doChatty.IsChecked = false;
            }
            else
            {
                checkBox_doChatty.IsChecked = true;
            }
        }

        // When the "Go!" button is clicked, do things.
        private void btn_Go_Click(object sender, RoutedEventArgs e)
        {            
            if (tb_Channel.Text == string.Empty)
            {
                System.Media.SystemSounds.Hand.Play();
                classLog.logWithTime("Error: One of the required fields were left blank!", rtb_Console);
            }
            else
            {
                ProcessStartInfo StartChat = new ProcessStartInfo("http://www.twitch.tv/chat/embed?channel=" + tb_Channel.Text + "&popout_chat=true");
                StartChat.WindowStyle = ProcessWindowStyle.Normal;
                if (checkBox_doChat.IsChecked == true)
                {
                    Process.Start(StartChat);
                }
                if (checkBox_doChatty.IsChecked == true)
                {
                    this.startChatty(getChattyPath);
                }

                string sourceTwitch = "twitch.tv";

                Process StartCMD = new Process();
                StartCMD.StartInfo.FileName = "cmd.exe";
                if (cb_StreamSource.SelectedIndex == 0)
                {
                    StartCMD.StartInfo.Arguments = "/c livestreamer " + sourceTwitch + "/" + tb_Channel.Text + " " + cb_StreamQuality.Text;
                }
                StartCMD.StartInfo.RedirectStandardOutput = true;
                StartCMD.StartInfo.CreateNoWindow = true;
                StartCMD.StartInfo.UseShellExecute = false;
                try
                {
                    StartCMD.Start();
                    classLog.logWithTime("Started Stream: " + "'" + tb_Channel.Text + "'" + " " + "on" + " '" + cb_StreamSource.Text + "' " + " with" + " '" + cb_StreamQuality.Text + "' " + "quality.", rtb_Console);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
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
            if (checkbox_doStats.IsChecked == true)
            {
                appConfig.AppSettings.Settings.Remove("doStats");
                appConfig.AppSettings.Settings.Add("doStats", "1");
            }
            else
            {
                appConfig.AppSettings.Settings.Remove("doStats");
                appConfig.AppSettings.Settings.Add("doStats", "0");
            }
            if (checkbox_getAvatar.IsChecked == true)
            {
                appConfig.AppSettings.Settings.Remove("getAvatar");
                appConfig.AppSettings.Settings.Add("getAvatar", "1");
            }
            else
            {
                appConfig.AppSettings.Settings.Remove("getAvatar");
                appConfig.AppSettings.Settings.Add("getAvatar", "0");
            }
            if (checkBox_doChatty.IsChecked == true) { }
            else
            {
                appConfig.AppSettings.Settings.Remove("getChattyPath");
                appConfig.AppSettings.Settings.Add("getChattyPath", "0");
            }

            // Do the saving
            try
            {
                appConfig.Save(ConfigurationSaveMode.Minimal);
                SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_forceUpdate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to force update?", ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                this.DoUpdate();
            }
        }

        private void clearContent()
        {
            lbl_contentGetStreamer.Content = string.Empty;
            lbl_contentGetTitle.Content = string.Empty;
            lbl_StreamerStatus.Content = string.Empty;
            lbl_contentGetGame.Content = string.Empty;
            lbl_contentGetViewerCount.Content = string.Empty;

            img_streamerOffline.Source = new BitmapImage(new Uri(@"Resources/image_placeholder.png", UriKind.RelativeOrAbsolute));
        }

        private void btn_PollStreamer_Click(object sender, RoutedEventArgs e)
        {
            this.clearContent();
            if (tb_PollStreamer.Text == string.Empty)
            {
                MessageBox.Show("One of the required fields were left blank!", ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ParseData classParseData = new ParseData();
                string doReturnData = classParseData.getData(tb_PollStreamer.Text);

                var converter = new System.Windows.Media.BrushConverter();
                var brushRed = (Brush)converter.ConvertFromString("#FF0000");
                var brushGreen = (Brush)converter.ConvertFromString("#33FF00");

                lbl_contentGetStreamer.Content = tb_PollStreamer.Text;

                if (doReturnData.Contains("!(LIVE)"))
                {
                    lbl_StreamerStatus.Foreground = brushRed;
                    lbl_StreamerStatus.Content = "STREAMER OFFLINE";
                }
                else
                {
                    string finalTitle = doReturnData.Split('$')[0];
                    string finalGame = doReturnData.Split('$')[1];
                    string finalViewers = doReturnData.Split('$')[2];
                    var finalImage = doReturnData.Split('$')[3];
                    lbl_contentGetTitle.Content = finalTitle;
                    lbl_contentGetGame.Content = finalGame;
                    lbl_contentGetViewerCount.Content = finalViewers;

                    if (checkbox_getAvatar.IsChecked == true)
                    {
                        img_streamerOffline.Source = getStreamerImage(finalImage);
                    }

                    lbl_StreamerStatus.Foreground = brushGreen;
                    lbl_StreamerStatus.Content = "STREAMER ONLINE";
                }
            }
            tb_PollStreamer.Text = string.Empty;
        }

        private ImageSource getStreamerImage(string s)
        {
            var image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(s, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }

        private void chattyPath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                appConfig.AppSettings.Settings.Remove("getChattyPath");
                appConfig.AppSettings.Settings.Add("getChattyPath", ofd.FileName);

                // Do the saving
                try
                {
                    appConfig.Save(ConfigurationSaveMode.Minimal);
                    SystemSounds.Asterisk.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            else
            {
                checkBox_doChatty.IsChecked = false;
            }
        }

        private void startChatty(string path)
        {
            Process StartChatty = new Process();
            StartChatty.StartInfo.FileName = path;
            StartChatty.StartInfo.Arguments = "-channel " + tb_Channel.Text;
            StartChatty.Start();
        }

        private void checkBox_doChatty_Checked(object sender, RoutedEventArgs e)
        {
            string getChattyPath = ConfigurationManager.AppSettings["getChattyPath"];
            Console.WriteLine(getChattyPath);
            if (!(getChattyPath == "0"))
            {
                // do nothing
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Path not found for Chatty. Would you like to set one now?", ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    this.chattyPath();
                }
                else
                {
                    checkBox_doChatty.IsChecked = false;
                }
            }
        }
    }
}