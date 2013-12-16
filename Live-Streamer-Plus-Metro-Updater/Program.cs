using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Live_Streamer_Plus_Metro_Updater
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

        // Get the download URL from a remote host.
        private static void GetDownloadUrl()
        {
            WebClient GetUpdateURL = new WebClient();
            GetUpdateURL.Proxy = null;
            string GetUpdateURLString = GetUpdateURL.DownloadString("http://pwnoz0r.com/software/lsp/configs/download.txt");
            DownloadUpdate(GetUpdateURLString);
        }

        // Fetch the update.
        private static void DownloadUpdate(string url)
        {
            ConsoleOut("Fetching update from: https://github.com/Pwnoz0r/Live-Streamer-Plus/");

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

        // Another type of log writing, making things less repetitive.
        private static void ConsoleOut(string ConsoleOut)
        {
            Console.WriteLine(ConsoleOut + "\r\n");
        }

        /**
         * If the setup file exists on startup, make sure to delete it. 
         * This may be moved to the main application in the future if problems arise.
         **/
        private static void CheckIfFileExists()
        {
            if (File.Exists("setup.exe"))
            {
                File.Delete(DownloadFileLocation);
            }
            else
            {
                // Do nothing because the file doesn't exist.
            }
        }
    }
}