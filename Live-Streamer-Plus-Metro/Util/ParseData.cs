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

namespace LiveStreamerPlus.Util
{
    class ParseData
    {
        public string getData(string broadcaster)
        {
            string dataURL = "http://api.justin.tv/api/stream/list.json?channel=" + broadcaster;
            WebClient dataWC = new WebClient();
            dataWC.Proxy = null;
            string returnData = dataWC.DownloadString(dataURL);
            if (returnData.Contains("[]"))
            {
                return "!(LIVE)";
            }

            // Horrible way of splitting data #shootme
            string allReturnData = JsonConvert.DeserializeObject(returnData).ToString();

            if (Regex.Matches(allReturnData, "subcategory").Count == 2)
            {
                string allReturnDataZ = allReturnData;

                string parseTitle = returnLine(allReturnDataZ, 8, 14);
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
                string parseTitleZ = returnLine(allReturnData, 29, 17);
                string parseTitleFinalZ = parseTitleZ.Substring(0, parseTitleZ.Length - 2);
                string parseGameZ = returnLine(allReturnData, 45, 18);
                string parseGameFinalZ = parseGameZ.Substring(0, parseGameZ.Length - 2);
                string parseViewersZ = returnLine(allReturnData, 11, 20);
                string parseViewersFinalZ = parseViewersZ.Substring(0, parseViewersZ.Length - 1);
                string parseOfflineImageZ = returnLine(allReturnData, 32, 26);
                string parseOfflineImageFinalZ = parseOfflineImageZ.Substring(0, parseOfflineImageZ.Length - 2);

                return parseTitleFinalZ + "$" + parseGameFinalZ + "$" + parseViewersFinalZ + "$" + parseOfflineImageFinalZ;
            }
        }

        private string returnLine(string s, int line, int count)
        {
            using (var sr = new StringReader(s))
            {
                for (int i = 1; i < line; i++)
                    sr.ReadLine();
                return sr.ReadLine().Substring(count);
            }
        }
    }
}
