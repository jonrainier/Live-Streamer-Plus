using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveStreamerPlus.Util
{
    class backgroundProcess
    {
        public List<string> consoleOutputs = new List<string>();
        public void startProcess(string streamName, int streamQuality)
        {
            string sourceTwitch = "twitch.tv";

            Process StartCMD = new Process();
            StartCMD.StartInfo.FileName = "cmd.exe";
            switch (streamQuality)
            {
                case 0:
                    StartCMD.StartInfo.Arguments = "/c livestreamer " + sourceTwitch + "/" + streamName + " source";
                    break;
                case 1:
                    StartCMD.StartInfo.Arguments = "/c livestreamer " + sourceTwitch + "/" + streamName + " high";
                    break;
                case 2:
                    StartCMD.StartInfo.Arguments = "/c livestreamer " + sourceTwitch + "/" + streamName + " medium";
                    break;
                case 3:
                    StartCMD.StartInfo.Arguments = "/c livestreamer " + sourceTwitch + "/" + streamName + " low";
                    break;
            }
            StartCMD.StartInfo.RedirectStandardOutput = true;
            StartCMD.StartInfo.CreateNoWindow = true;
            StartCMD.StartInfo.UseShellExecute = false;

            StartCMD.Start();
            StreamReader newOutput = StartCMD.StandardOutput;

            for (int i = 0; i < 4; i++)
            {
                consoleOutputs.Add(newOutput.ReadLine());
            }
        }
    }
}
