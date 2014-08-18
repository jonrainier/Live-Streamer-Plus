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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiveStreamerPlus
{
    public partial class MainWindow : Elysium.Controls.Window
    {
        public static string ApplicationName = "Live Streamer Plus :: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private List<string> newlyAdded = new List<string>();
        private List<string> recentlyRemoved = new List<string>();

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
            //this.CheckForUpdates();
            this.CheckInstalledPrograms(1);
            this.checkSettings();
            this.clearContent();
            this.loadFavorites();

            classLog.logWithoutTime("-------------------------------------------------------------" + "\r\n", rtb_Console);
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
            if (tb_Channel.Text == string.Empty)
            {
                System.Media.SystemSounds.Hand.Play();
                classLog.logWithTime("Error: One of the required fields were left blank!", rtb_Console);
            }
            else
            {                
                if (checkBox_doChat.IsChecked == true)
                {
                    ProcessStartInfo StartChat = new ProcessStartInfo("http://www.twitch.tv/chat/embed?channel=" + tb_Channel.Text + "&popout_chat=true");
                    StartChat.WindowStyle = ProcessWindowStyle.Normal;
                    Process.Start(StartChat);
                }
                
                try
                {
                    backgroundProcess startStream = new backgroundProcess();
                    startStream.startProcess(tb_Channel.Text, cb_StreamQuality.SelectedIndex);
                    classLog.logWithTime("Started Stream: " + "'" + tb_Channel.Text + "'" + " " + "on" + " '" + cb_StreamSource.Text + "' " + " with" + " '" + cb_StreamQuality.Text + "' " + "quality.", rtb_Console);;
                    foreach (string s in startStream.consoleOutputs)
                    {
                        classLog.logWithTime(s, rtb_Console);
                    }

                    if (checkbox_doStats.IsChecked == true)
                    {
                        tabController.SelectedIndex = 1;
                        pollStreamerMainTab(tb_Channel.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
                rtb_Console.AppendText("\n");
            }
        }

        // Check the config for user settings.
        private void checkSettings()
        {
            int doStats;
            int getAvatar;
            int doChat = Int32.Parse(ConfigurationManager.AppSettings["doChat"]);

            if (ConfigurationManager.AppSettings["doStats"] != null)
                doStats = Int32.Parse(ConfigurationManager.AppSettings["doStats"]);
            else
                doStats = 0;

            if (ConfigurationManager.AppSettings["getAvatar"] != null)
                getAvatar = Int32.Parse(ConfigurationManager.AppSettings["getAvatar"]);
            else
                getAvatar = 0;

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
        }

        Configuration appConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        private void btn_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            appConfig.AppSettings.Settings.Remove("favStream");
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
            lbl_ErrorMessage.Content = string.Empty;

            img_streamerOffline.Source = new BitmapImage(new Uri(@"Resources/image_placeholder.png", UriKind.RelativeOrAbsolute));
        }

        public void loadFavorites()
        {
            // template - favStreamGrid.Items.Add(new favStreamers { streamerName = "test", streamerStatus = "online" });
            string placeHolder = ConfigurationManager.AppSettings["favStream"];
            if(placeHolder != null)
            {
                List<string> favStream = placeHolder.Split(',').ToList();
                foreach (string s in favStream)
                {
                    string status = ParseData.getStatus(s);

                    favStreamGrid.Items.Add(new favStreamer { streamerName = s, streamerStatus = status });
                }        
            }
            else
            {
                return;
            }
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

        //Begin Event Methods

            private void btn_Enter_Handler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btn_Go_Click(sender, e);
                e.Handled = true;
            }    
        }

            private void pollStreamer_Enter(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Return)
                {
                    btn_PollStreamer_Click(sender, e);
                    e.Handled = true;
                }  
            }

            private void addStreamBtn_Click(object sender, RoutedEventArgs e)
            {
                string textField = streamToAdd.Text;

                if (!(textField.Equals("Enter Stream Name") || textField.Equals("")))
                {
                    appConfig.AppSettings.Settings.Add("favStream", textField);
                    newlyAdded.Add(textField);
                    favStreamGrid.Items.Clear();
                    try
                    {
                        appConfig.Save(ConfigurationSaveMode.Minimal);                                                
                        SystemSounds.Asterisk.Play();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    refreshFavorites(textField);
                    streamToAdd.Text = "";
                }
            }

            private void refreshFavorites(string newStreamer)
            {
                string placeHolder = ConfigurationManager.AppSettings["favStream"];
                if (placeHolder != null)
                {
                    List<string> favStream = placeHolder.Split(',').ToList();                    
                    foreach (string s in favStream)
                    {
                        if (!(recentlyRemoved.Contains(s)))
                        {                        
                            string status = ParseData.getStatus(s);

                            favStreamGrid.Items.Add(new favStreamer { streamerName = s, streamerStatus = status });
                        }
                    }

                    foreach (string s in newlyAdded)
                    {
                        if(!(recentlyRemoved.Contains(s)))
                            favStreamGrid.Items.Add(new favStreamer { streamerName = s, streamerStatus = ParseData.getStatus(s) });
                    }
                }   
                else
                {
                    favStreamGrid.Items.Add(new favStreamer { streamerName = newStreamer, streamerStatus = ParseData.getStatus(newStreamer) });
                }
            }
        
            private void btn_PollStreamer_Click(object sender, RoutedEventArgs e)
            {
                this.clearContent();
                if (tb_PollStreamer.Text == string.Empty)
                {
                    var converter = new System.Windows.Media.BrushConverter();
                    var brushRed = (Brush)converter.ConvertFromString("#FF0000");
                    lbl_ErrorMessage.Foreground = brushRed;
                    lbl_ErrorMessage.Content = "TEXT BOX EMEPTY";
                    SystemSounds.Hand.Play();
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

            private void pollStreamerMainTab(string streamName)
            {
                ParseData classParseData = new ParseData();
                string doReturnData = classParseData.getData(streamName);

                var converter = new System.Windows.Media.BrushConverter();
                var brushRed = (Brush)converter.ConvertFromString("#FF0000");
                var brushGreen = (Brush)converter.ConvertFromString("#33FF00");

                lbl_contentGetStreamer.Content = streamName;

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

            private void favStreamGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
                var streamer = favStreamGrid.SelectedItem as favStreamer;

                if (streamer != null)
                {
                    string streamName = streamer.streamerName;

                    if (checkBox_doChat.IsChecked == true)
                    {
                        ProcessStartInfo StartChat = new ProcessStartInfo("http://www.twitch.tv/chat/embed?channel=" + streamName + "&popout_chat=true");
                        StartChat.WindowStyle = ProcessWindowStyle.Normal;
                        Process.Start(StartChat);
                    }

                    tabController.SelectedIndex = 0;
                    classLog.logWithTime("Started Stream: " + "'" + streamName + "'" + " " + "on" + " '" + "Twitch" + "' " + " with" + " '" + "Source" + "' " + "quality.", rtb_Console);

                    backgroundProcess startStream = new backgroundProcess();
                    startStream.startProcess(streamName, fav_CbBox.SelectedIndex);
                    
                    foreach (string s in startStream.consoleOutputs)
                    {
                        classLog.logWithTime(s, rtb_Console);
                    }
                    rtb_Console.AppendText("\n");
                }
                e.Handled = true;
            }

            private void streamToAdd_GotFocus(object sender, RoutedEventArgs e)
            {
                this.streamToAdd.Text = "";
                this.streamToAdd.Foreground = Brushes.White;
            }

            private void streamToAdd_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Return)
                {
                    addStreamBtn_Click(sender, e);
                    e.Handled = true;
                }
            }

            private void btn_RemoveFavStream_Click(object sender, RoutedEventArgs e)
            {
                var streamer = favStreamGrid.SelectedItem as favStreamer;
                string streamName = streamer.streamerName;
                recentlyRemoved.Add(streamName);
                string placeHolder = ConfigurationManager.AppSettings["favStream"];
                List<string> favStream = placeHolder.Split(',').ToList();
                appConfig.AppSettings.Settings.Remove("favStream");
                favStreamGrid.Items.Clear();
                foreach (string s in favStream)
                {
                    if (!(recentlyRemoved.Contains(s)) && !(s.Equals(streamName)))
                    {
                        appConfig.AppSettings.Settings.Add("favStream", s);
                        string status = ParseData.getStatus(s);

                        favStreamGrid.Items.Add(new favStreamer { streamerName = s, streamerStatus = status });
                    }
                }
                try
                {
                    foreach (string s in newlyAdded)
                    {
                        if (!(recentlyRemoved.Contains(s)) && !(s.Equals(streamName)))
                        {
                            appConfig.AppSettings.Settings.Add("favStream", s);
                            string status = ParseData.getStatus(s);

                            favStreamGrid.Items.Add(new favStreamer { streamerName = s, streamerStatus = status });
                        }
                        else if(s.Equals(streamName))
                        {
                            newlyAdded.Remove(s);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString() + "\r\n" +
                                    "Error Removing Streamer from list.");
                }                

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

            private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
            {
                var hyperlink = (Hyperlink)sender;
                try
                {
                    Process.Start(hyperlink.NavigateUri.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            private void favStreamGrid_MouseMove(object sender, MouseEventArgs e)
            {
                tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Relative;
                tt.HorizontalOffset = e.GetPosition((IInputElement)sender).X + 15;
                tt.VerticalOffset = e.GetPosition((IInputElement)sender).Y + 15;
            }

        //End Event Methods
    }
}