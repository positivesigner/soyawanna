using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeChannel
{
    class YouTubeAPI
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static string DLChannelDetails(string ur_channel_name, TableVar ur_ttb)
        {
            var channel_url = new enrURL.tbrURL(ur_ttb.enrURL);
            {
                var querykvp_rec = new Mx.bitbase<enrParmKvp>.table_row(ur_ttb.enrParmKvp);
                channel_url[channel_url.r.protocol] = "https";
                channel_url[channel_url.r.server_path] = "youtube.googleapis.com";
                channel_url.pick_folder_path(new string[] { "youtube", "v3", "channels" });
                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "part";
                    new_row[new_row.r.value] = "contentDetails";
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "forUsername";
                    new_row[new_row.r.value] = ur_channel_name;
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "key";
                    new_row[new_row.r.value] = ur_ttb.tbrAkey[ur_ttb.tbrAkey.r.key];
                }

                channel_url.pick_parameter_list(querykvp_rec);
            }

            var chquery_url = channel_url.ToURL();
            return HttpFn.DownloadPage(chquery_url);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static string DLPlaylistPage(string ur_playlist_id, string ur_page_token, TableVar ur_ttb)
        {
            var video_url = new enrURL.tbrURL(ur_ttb.enrURL);
            {
                var querykvp_rec = new Mx.bitbase<enrParmKvp>.table_row(ur_ttb.enrParmKvp);
                video_url[video_url.r.protocol] = "https";
                video_url[video_url.r.server_path] = "youtube.googleapis.com";
                video_url.pick_folder_path(new string[] { "youtube", "v3", "playlistItems" });
                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "part";
                    new_row[new_row.r.value] = "snippet";
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "maxResults";
                    new_row[new_row.r.value] = "50";
                }

                if (string.IsNullOrWhiteSpace(ur_page_token) == false)
                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "pageToken";
                    new_row[new_row.r.value] = ur_page_token;
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "playlistId";
                    new_row[new_row.r.value] = ur_playlist_id;
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "key";
                    new_row[new_row.r.value] = "AIzaSyCvasLxS3XJ7vdNOJHG-i57-TzZK4wzICc";
                }

                video_url.pick_parameter_list(querykvp_rec);
            }

            var vidquery_url = video_url.ToURL();
            var vidlist_json = HttpFn.DownloadPage(vidquery_url);
            //var vidlist_json = System.IO.File.ReadAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\e2.txt", TextFn.UTF8EncodingNoBOM());
            return vidlist_json;
        }

        //[System.Diagnostics.DebuggerStepThrough]
        public static string DLVidSearchPage(string ur_channel_id, string ur_date_recent_from, string ur_date_recent_to, string ur_page_token, TableVar ur_ttb)
        {
            var video_url = new enrURL.tbrURL(ur_ttb.enrURL);
            {
                var querykvp_rec = new Mx.bitbase<enrParmKvp>.table_row(ur_ttb.enrParmKvp);
                video_url[video_url.r.protocol] = "https";
                video_url[video_url.r.server_path] = "youtube.googleapis.com";
                video_url.pick_folder_path(new string[] { "youtube", "v3", "search" });
                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "part";
                    new_row[new_row.r.value] = "snippet";
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "maxResults";
                    new_row[new_row.r.value] = "50";
                }

                if (string.IsNullOrWhiteSpace(ur_page_token) == false)
                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "pageToken";
                    new_row[new_row.r.value] = ur_page_token;
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "channelId";
                    new_row[new_row.r.value] = ur_channel_id;
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "type";
                    new_row[new_row.r.value] = "video";
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "order";
                    new_row[new_row.r.value] = "date";
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "publishedAfter";
                    new_row[new_row.r.value] = ur_date_recent_from;
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "publishedBefore";
                    new_row[new_row.r.value] = ur_date_recent_to;
                }

                {
                    var new_row = querykvp_rec.NewRow();
                    new_row[new_row.r.parameter] = "key";
                    new_row[new_row.r.value] = "AIzaSyCvasLxS3XJ7vdNOJHG-i57-TzZK4wzICc";
                }

                video_url.pick_parameter_list(querykvp_rec);
            }

            var vidquery_url = video_url.ToURL();
            var vidlist_json = string.Empty;
            try
            {
                vidlist_json = HttpFn.DownloadPage(vidquery_url);
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("exceeded your"))
                {
                    //what do?
                }
            }

            //var vidlist_json = System.IO.File.ReadAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\e2.txt", TextFn.UTF8EncodingNoBOM());
            return vidlist_json;
        }
    }
}
