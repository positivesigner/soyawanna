using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeChannel
{
    class TextFn
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static string CombineURL(string ur_protocol, string ur_server, string ur_path_list, string ur_parm_list)
        {
            var ret_url = ur_protocol + "://" + ur_server + "/" + ur_path_list;
            if (ur_parm_list.Length > 0)
            {
                ret_url += "?" + ur_parm_list;
            }

            return ret_url;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Text.UTF8Encoding UTF8EncodingNoBOM()
        {
            return new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Text.UTF8Encoding UTF8EncodingWithBOM()
        {
            return new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true);
        }
    }
}
