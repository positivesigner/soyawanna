using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTubeChannel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private TableVar ttb;
        private DateTime bgw_time_start;
        private DateTime bgw_time_caption;
        private decimal bgw_chunk_progress;
        private void Form1_Load(object sender, EventArgs e)
        {
            ttb = new TableVar();
            var ttbAkey = new Mx.bitbase<enrAkey>.table_row(ttb.enrAkey);
            ttbAkey.Persist_Read(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\akey.tsv");
            ttb.tbrAkey.row_enum_init(ttb.enrAkey);
            foreach (var row in ttbAkey)
            {
                row.CloneTo(ttb.tbrAkey);
            }

            this.CancelBtn.Visible = false;
            this.YTDownloadBgw.WorkerReportsProgress = true;
            this.YTDownloadBgw.WorkerSupportsCancellation = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.CancelBtn.Visible = true;
            this.CancelBtn.Enabled = true;
            this.button1.Enabled = false;
            this.bgw_time_start = DateTime.Now;
            this.bgw_time_caption = DateTime.Now.AddSeconds(-1);
            this.bgw_chunk_progress = 0;
#pragma warning disable CS0162 // Unreachable code detected
            this.YTDownloadBgw.RunWorkerAsync(); if (1 == 0) YTDownloadBgw_DoWork(null, null);
#pragma warning restore CS0162 // Unreachable code detected
        }
        private void DownloadChannelDetails()
        {
            //System.IO.File.WriteAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\e1.txt", txt_page, TextFn.UTF8EncodingNoBOM());
            //MessageBox.Show("File created");
        }


        #region "Background Task"

        private void YTDownloadBgw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as System.ComponentModel.BackgroundWorker;

            var yt_channel = "smartsigndictionary";
            //var chlist_json = YouTubeAPI.DLChannelDetails("smartsigndictionary", ttb);
            //System.IO.File.WriteAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\" + $"channel_{yt_channel}.json", playlist_json, TextFn.UTF8EncodingNoBOM());
            var chlist_json = System.IO.File.ReadAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\channel_" + $"{yt_channel}.json", TextFn.UTF8EncodingNoBOM());
            var channel_root = new enrChannelL1.tbrChannelL1(ttb.enrChannelL1, ttb);
            channel_root.LoadJson("{", chlist_json, ttb);
            channel_root.PullBranchValues();
            var channel_id = channel_root[channel_root.r.id];
            var playlist_id = channel_root[channel_root.r.uploads];
            if (string.IsNullOrWhiteSpace(playlist_id) == false)
            {
                var playlist_root = new enrPlaylistL1.tbrPlaylistL1(ttb.enrPlaylistL1, ttb);
                var next_fromdate = string.Empty;
                var next_todate = string.Empty;
                foreach (var rvp_date in Mx.bitbase_fn.ntseq(100))
                {
                    if (rvp_date.is_first)
                    {
                        var next_page = string.Empty;
                        foreach (var rvp in Mx.bitbase_fn.ntseq(30000))
                        {
                            var playlist_json = YouTubeAPI.DLPlaylistPage(playlist_id, next_page, ttb);
                            //System.IO.File.WriteAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\" + $"vidlist_{rvp.ROWSEQ}.json", playlist_json, TextFn.UTF8EncodingNoBOM());
                            //var playlist_json = System.IO.File.ReadAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\" + $"vidlist_{rvp.ROWSEQ}.json", TextFn.UTF8EncodingNoBOM());
                            playlist_root.LoadJson("{", playlist_json, ttb);

                            next_page = playlist_root[playlist_root.r.nextPageToken];
#pragma warning disable CS0162 // Unreachable code detected
                            worker.ReportProgress(1); if (1 == 0) YTDownloadBgw_ProgressChanged(null, null);
#pragma warning restore CS0162 // Unreachable code detected
                            if (worker.CancellationPending || string.IsNullOrWhiteSpace(next_page))
                            {
                                break;
                            }
                        }

                        playlist_root.sub_items.PullBranchValues();
                        if (playlist_root.sub_items.Count < 20000)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var found_records = false;
                        var next_page = string.Empty;
                        foreach (var rvp in Mx.bitbase_fn.ntseq(30000))
                        {
                            var playlist_json = YouTubeAPI.DLVidSearchPage(channel_id, next_fromdate, next_todate, next_page, ttb);
                            //System.IO.File.WriteAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\" + $"vidlist_{rvp.ROWSEQ}.json", playlist_json, TextFn.UTF8EncodingNoBOM());
                            //var playlist_json = System.IO.File.ReadAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\" + $"vidlist_{rvp.ROWSEQ}.json", TextFn.UTF8EncodingNoBOM());
                            var searchlist_root = new enrSearchL1.tbrSearchL1(ttb.enrSearchL1, ttb);
                            searchlist_root.LoadJson("{", playlist_json, ttb);
                            searchlist_root.sub_items.PullBranchValues();
                            foreach (var search_row in searchlist_root.sub_items)
                            {
                                var new_row = playlist_root.sub_items.NewRow();
                                new_row[new_row.r.kind] = search_row[search_row.r.kind];
                                new_row[new_row.r.publishedAt] = search_row[search_row.r.publishedAt];
                                new_row[new_row.r.search_object_path] = search_row[search_row.r.search_object_path];
                                new_row[new_row.r.title] = search_row[search_row.r.title];
                                new_row[new_row.r.videoId] = search_row[search_row.r.videoId];
                                new_row[new_row.r.video_kind] = search_row[search_row.r.video_kind];
                            }

                            if (searchlist_root.sub_items.Count > 0)
                            {
                                found_records = true;
                            }

                            next_page = searchlist_root[searchlist_root.r.nextPageToken];
#pragma warning disable CS0162 // Unreachable code detected
                            worker.ReportProgress(1); if (1 == 0) YTDownloadBgw_ProgressChanged(null, null);
#pragma warning restore CS0162 // Unreachable code detected
                            if (worker.CancellationPending || string.IsNullOrWhiteSpace(next_page))
                            {
                                break;
                            }
                        }

                        if (found_records == false)
                        {
                            break;
                        }
                    }

                    if (playlist_root.sub_items.Count > 0)
                    {
                        var last_row = playlist_root.sub_items[playlist_root.sub_items.Count - 1];
                        var recent_date = last_row.date[last_row.r.publishedAt];
                        next_fromdate = new DateTime(recent_date.AddYears(-1).Year, 1, 1).ToString("yyyy-MM-ddThh:mm:ssZ");
                        next_todate = recent_date.AddSeconds(-1).ToString("yyyy-MM-ddThh:mm:ssZ");
                    }
                }

                System.IO.File.WriteAllText(@"C:\Users\Dad\Downloads\2022m11d12 YouTube API\" + $"vidlist_{yt_channel}.tsv", playlist_root.sub_items.Format_Table(true), TextFn.UTF8EncodingNoBOM());
            }


#pragma warning disable CS0162 // Unreachable code detected
            if (1 == 0) YTDownloadBgw_RunWorkerCompleted(null, null);
#pragma warning restore CS0162 // Unreachable code detected
        }

        private void YTDownloadBgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.bgw_chunk_progress += 1;
            if (DateTime.Now.Subtract(this.bgw_time_caption).TotalSeconds >= 2)
            {
                this.bgw_time_caption = DateTime.Now;
                var minute_elapsed = this.bgw_chunk_progress / ((decimal)DateTime.Now.Subtract(this.bgw_time_start).TotalSeconds / (decimal)60);
                DownloadLbl.Text = $"Downloading {this.bgw_chunk_progress} pages @ {string.Format("{0:N1}", minute_elapsed)} pages / minute";
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            YTDownloadBgw.CancelAsync();
            this.CancelBtn.Enabled = false;
        }

        private void YTDownloadBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var minute_elapsed = this.bgw_chunk_progress / ((decimal)DateTime.Now.Subtract(this.bgw_time_start).TotalSeconds / (decimal)60);
            var completed_state = "Completed";
            if (this.CancelBtn.Enabled == false)
            {
                completed_state = "Canceled";
            }
            DownloadLbl.Text = $"{completed_state} downloading {this.bgw_chunk_progress} pages @ {string.Format("{0:N1}", minute_elapsed)} pages / minute";
            this.button1.Enabled = true;
            this.CancelBtn.Visible = false;
        }

        #endregion
    }
}
