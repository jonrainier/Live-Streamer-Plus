using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Live_Streamer_Plus
{
    public partial class LiveStreamerPlus : Form
    {
        public LiveStreamerPlus()
        {
            InitializeComponent();
            this.Text = "Live Streamer Plus :: " + Application.ProductVersion;
        }

        //Things done when the program is loaded.
        private void MainForm_Load(object sender, EventArgs e)
        {
            LogNoTime("Live Streamer Plus created by Pwnoz0r: https://github.com/pwnoz0r");
            LogNoTime("VLC Media Player: http://www.videolan.org/vlc/index.html");
            LogNoTime("Livestreamer: http://livestreamer.tanuki.se/en/latest/");
            LogNoTime("-------------------------------------------------------------");

            this.CheckInstalledPrograms(1);

            this.DownloadChangeLog();

            rtb_Console.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(ClickLink);

            this.CheckForUpdates();
        }

        //Check for updates for the application.
        private void CheckForUpdates()
        {
            WebClient GetUpdateURL = new WebClient();
            GetUpdateURL.Proxy = null;
            try
            {
                string GetUpdateURLInt = GetUpdateURL.DownloadString("http://74.91.121.95:8080/LiveStreamerPlus/Configs/version.txt");
                int UpdateAsInt;
                int.TryParse(GetUpdateURLInt, out UpdateAsInt);
                int ProductVersion;
                int.TryParse(Application.ProductVersion, out ProductVersion);

                if (!(ProductVersion == UpdateAsInt))
                {
                    MessageBox.Show("Update available!");
                    this.DoUpdate();
                }
            }
            catch (WebException ex)
            {
                Log(ex.ToString());
                Application.Exit();
            }
        }

        //Call the updater.
        private void DoUpdate()
        {
            try
            {
                Process.Start(Directory.GetCurrentDirectory() + @"\Live-Streamer-Plus-Updater.exe");
                Application.Exit();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
        }

        //Get the latest changelog from a remote location.
        private void DownloadChangeLog()
        {
            WebClient DownloadChangeLogClient = new WebClient();
            DownloadChangeLogClient.Proxy = null;
            string DownloadChangeLogString = DownloadChangeLogClient.DownloadString("http://74.91.121.95:8080/LiveStreamerPlus/Configs/changelog.txt");
            
            try
            {
                rtb_ChangeLog.Text = DownloadChangeLogString;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
        }

        //When links are clicked in the console they will open in your browser.
        private void ClickLink(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        //Check if the prerequisites are installed.
        private void CheckInstalledPrograms(int Programs)
        {
            string ApplicationDataLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string VLCInstallLocation = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\VideoLAN\\VLC", "", null);
            string LivestreamerConfigLocation = ApplicationDataLocation + @"\livestreamer\livestreamerrc";
            string LivestreamerReadFile = File.ReadAllText(LivestreamerConfigLocation);

            switch (Programs)
            {
                case 1:
                    if (File.Exists(LivestreamerConfigLocation) && File.Exists(VLCInstallLocation))
                    {
                        rtb_ConfigEditor.Text = LivestreamerReadFile;
                    }
                    else
                    {
                        System.Media.SystemSounds.Hand.Play();
                        Log("VLC or Livestreamer is not installed! Please install them from the links above!");
                        btn_Go.Enabled = false;
                        btn_SaveConfig.Enabled = false;
                    }
                    break;

                case 2:
                    try
                    {
                        StreamWriter LivestreamerConfigStreamWriter = new StreamWriter(LivestreamerConfigLocation);
                        LivestreamerConfigStreamWriter.Write(rtb_ConfigEditor.Text);
                        LivestreamerConfigStreamWriter.Close();
                        System.Media.SystemSounds.Asterisk.Play();
                        tc_MainForm.SelectedIndex = 0;
                        Log("Successfully saved Livestreamer config file!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Application.Exit();
                    }
                    break;

                default:
                    break;
            }
        }

        //Instead of "Console.WriteLine(string)" use our custom console.
        private void Log(string log)
        {
            DateTime DTN = DateTime.Now;
            string LogDateTime = DateTime.SpecifyKind(DTN, DateTimeKind.Local).ToString();
            rtb_Console.SelectionAlignment = HorizontalAlignment.Left;
            try
            {
                rtb_Console.AppendText(LogDateTime + ": " + log + "\r\n");
                rtb_Console.ScrollToCaret();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
        }

        //Same as the log, but without timestamps.
        private void LogNoTime(string log)
        {
            rtb_Console.SelectionAlignment = HorizontalAlignment.Center;
            try
            {
                rtb_Console.AppendText(log + "\r\n");
                rtb_Console.ScrollToCaret();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
        }

        //When the "Go!" button is clicked, do things.
        private void btn_Go_Click(object sender, EventArgs e)
        {
            if (cb_SelectedStream.Text == String.Empty || tb_DoStreamer.Text == string.Empty || cb_Quality.Text == string.Empty)
            {
                System.Media.SystemSounds.Hand.Play();
                Log("Error: One of the required fields were left blank!");
            }
            else
            {
                Log("Started Stream: " + "'" + tb_DoStreamer.Text + "'" + " " + "on" + " '" + cb_SelectedStream.Text + "' " + " with" + " '" + cb_Quality.Text + "' " + "quality.");
                Process StartCMD = new Process();
                StartCMD.StartInfo.FileName = "cmd.exe";
                StartCMD.StartInfo.Arguments = "/c livestreamer " + cb_SelectedStream.Text + "/" + tb_DoStreamer.Text + " " + cb_Quality.Text;
                StartCMD.StartInfo.RedirectStandardOutput = true;
                StartCMD.StartInfo.CreateNoWindow = true;
                StartCMD.StartInfo.UseShellExecute = false;
                StartCMD.Start();
            }
        }

        //Call another method to save the config.
        private void btn_SaveConfig_Click(object sender, EventArgs e)
        {
            this.CheckInstalledPrograms(2);
        }
    }
}