using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeChannel
{
    class HttpFn
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static string DownloadPage(string ur_url)
        {
            var ret_text = "";
            var client = new System.Net.WebClient();
            //client.DownloadFile(yt_first_dl, localFilename);
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            using (var stm_page = client.OpenRead(ur_url))
            {
                using (var srd_page = new System.IO.StreamReader(stm_page))
                {
                    ret_text = srd_page.ReadToEnd();
                }
            }

            return ret_text;
        }
    }
}
