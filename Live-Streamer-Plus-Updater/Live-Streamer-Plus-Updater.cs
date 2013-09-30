using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Live_Streamer_Plus_Updater
{
    class Program
    {
        static string DownloadFileLocation = Directory.GetCurrentDirectory() + @"\setup.exe";

        static void Main(string[] args)
        {
            Console.Title = "Live-Streamer-Plus-Updater";
            CheckIfFileExists();
            GetDownloadUrl();
        }

        private static void GetDownloadUrl()
        {
            WebClient GetUpdateURL = new WebClient();
            GetUpdateURL.Proxy = null;
            string GetUpdateURLString = GetUpdateURL.DownloadString("http://74.91.121.95:8080/LiveStreamerPlus/Configs/download.txt");
            DownloadUpdate(GetUpdateURLString);
        }

        private static void DownloadUpdate(string url)
        {
            ConsoleOut("Fetching update from: http://74.91.121.95:8080/LiveStreamerPlus/RC/");

            WebClient GetUpdate = new WebClient();            
            GetUpdate.Proxy = null;

            try
            {
                GetUpdate.DownloadFile(url, "setup.exe");
                FileVersionInfo NewFileVersion = FileVersionInfo.GetVersionInfo(DownloadFileLocation);
                ConsoleOut("Successfully downloaded update version: " + NewFileVersion.ProductVersion);
                ConsoleOut("Press any key to begin the installation...");
                Console.ReadKey();
                Process.Start("setup.exe");
            }
            catch (WebException ex) 
            {
                ConsoleOut(ex.ToString());
            }
        }

        private static void ConsoleOut(string ConsoleOut)
        {
            Console.WriteLine(ConsoleOut + "\r\n");
        }

        private static void CheckIfFileExists()
        {
            if (File.Exists("setup.exe"))
            {
                File.Delete(DownloadFileLocation);
            }
            else
            {
                //Do nothing because the file doesn't exist.
            }
        }
    }
}