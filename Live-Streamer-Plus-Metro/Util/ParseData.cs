using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Schema;
using System.Text.RegularExpressions;
using System.Windows;

namespace LiveStreamerPlus.Util
{
    class ParseData
    {
        public string getData(string broadcaster)
        {
            string dataURL = "https://api.twitch.tv/kraken/streams/" + broadcaster;
            WebClient dataWC = new WebClient();
            dataWC.Proxy = null;
            string returnData = dataWC.DownloadString(dataURL);
            
            // Horrible way of splitting data #shootme
            string allReturnData = JsonConvert.DeserializeObject(returnData).ToString();
            string onlineStatus = returnLine(allReturnData, 6, 0);

            if (onlineStatus.Equals("  \"stream\": null"))
            {
                return "!(LIVE)";
            }
            
            if (Regex.Matches(allReturnData, "subcategory").Count == 2)
            {
                string allReturnDataZ = allReturnData;
                string parseTitle = returnLine(allReturnDataZ, 20, 14);
                string parseTitleFinal = parseTitle.Substring(0, parseTitle.Length - 2);
                string parseGame = returnLine(allReturnDataZ, 36, 18);
                string parseGameFinal = parseGame.Substring(0, parseGame.Length - 2);
                string parseViewers = returnLine(allReturnDataZ, 9, 21);
                string parseViewersFinal = parseViewers.Substring(0, parseViewers.Length - 1);
                string parseOfflineImage = returnLine(allReturnDataZ, 31, 26);
                string parseOfflineImageFinal = parseOfflineImage.Substring(0, parseOfflineImage.Length - 2);

                return parseTitleFinal + "$" + parseGameFinal + "$" + parseViewersFinal + "$" + parseOfflineImageFinal;
            }
            else
            {
                string parseTitleZ = returnLine(allReturnData, 38, 17);
                string parseTitleFinalZ = parseTitleZ.Substring(0, parseTitleZ.Length - 2);
                string parseGameZ = returnLine(allReturnData, 35, 15);
                string parseGameFinalZ = parseGameZ.Substring(0, parseGameZ.Length - 2);
                string parseViewersZ = returnLine(allReturnData, 9, 15);
                string parseViewersFinalZ = parseViewersZ.Substring(0, parseViewersZ.Length - 1);
                string parseOfflineImageZ = returnLine(allReturnData, 36, 15);
                string parseOfflineImageFinalZ = parseOfflineImageZ.Substring(0, parseOfflineImageZ.Length - 2);

                return parseTitleFinalZ + "$" + parseGameFinalZ + "$" + parseViewersFinalZ + "$" + parseOfflineImageFinalZ;
            }
        }

        public static string getStatus(string broadcaster)
        {
            string dataURL = "https://api.twitch.tv/kraken/streams/" + broadcaster;
            WebClient dataWC = new WebClient();
            dataWC.Proxy = null;
            string returnData = dataWC.DownloadString(dataURL);
            string allReturnData = JsonConvert.DeserializeObject(returnData).ToString();
            string onlineStatus = returnStaticLine(allReturnData, 6, 0);

            if (onlineStatus.Equals("  \"stream\": null"))
            {
                return "OFFLINE";
            }
            else
            {
                return "ONLINE";
            }
        }

        private string returnLine(string s, int line, int count)
        {
            using (var sr = new StringReader(s))
            {
                for (int i = 1; i < line; i++)
                    sr.ReadLine();
                try
                {
                    return sr.ReadLine().Substring(count);
                }
                catch (Exception)
                {
                    //figure out error with parsing some channels viewers.
                    return "Error in parsing data ";
                }
            }
        }

        private static string returnStaticLine(string s, int line, int count)
        {
            using (var sr = new StringReader(s))
            {
                for (int i = 1; i < line; i++)
                    sr.ReadLine();
                try
                {
                    return sr.ReadLine().Substring(count);
                }
                catch (Exception)
                {
                    //figure out error with parsing some channels viewers.
                    return "Error in parsing data ";
                }
            }
        }
    }
}
