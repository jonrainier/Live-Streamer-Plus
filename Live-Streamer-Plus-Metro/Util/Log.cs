using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace LiveStreamerPlus.Util
{
    class Log
    {
        public void logWithTime(string s, RichTextBox rtb)
        {
            DateTime DTN = DateTime.Now;
            string LogDateTime = DateTime.SpecifyKind(DTN, DateTimeKind.Local).ToString();
            try
            {
                rtb.AppendText(LogDateTime + ": " + s + "\r\n");
                rtb.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void logWithoutTime(string s, RichTextBox rtb)
        {
            rtb.AppendText(s + "\r\n");
            rtb.ScrollToEnd();
        }
    }
}
