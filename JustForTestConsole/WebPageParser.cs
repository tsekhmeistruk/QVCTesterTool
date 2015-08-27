using System;
using System.Net;

namespace JustForTestConsole
{
    public class WebPageParser
    {
        public static void FetchBuild(out string build, out string date, string webAdress)
        {
            const string buildTypeText = "Build QA";
            const string buildDataText = "Build Date";
            const string pMark = @"</p>";

            using (var client = new WebClient())
            {
                string qaWebText = client.DownloadString(webAdress);
                string buildDate =
                    qaWebText.Substring(qaWebText.IndexOf(buildDataText, StringComparison.Ordinal) +
                                        buildDataText.Length + 2);
                build =
                    qaWebText.Substring(
                        qaWebText.IndexOf(buildTypeText, StringComparison.Ordinal) + buildTypeText.Length - 2, 5);
                date = buildDate.Substring(0, buildDate.IndexOf(pMark, StringComparison.Ordinal));
            }
        }
    }
}
