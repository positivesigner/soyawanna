using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeChannel
{
    public class TableVar
    {
        public Mx.bitbase<enrAkey>.bitlist enrAkey { get; set; } = Mx.bitbase<enrAkey>.Create();
        public Mx.bitbase<enrChannelL2>.bitlist enrChannelList { get; set; } = Mx.bitbase<enrChannelL2>.Create();
        public Mx.bitbase<enrChannelL1>.bitlist enrChannelL1 { get; set; } = Mx.bitbase<enrChannelL1>.Create();
        public Mx.bitbase<enrChannelL3>.bitlist enrChContentDetail { get; set; } = Mx.bitbase<enrChannelL3>.Create();
        public Mx.bitbase<enrChannelL4>.bitlist enrChRelPlaylist { get; set; } = Mx.bitbase<enrChannelL4>.Create();
        public Mx.bitbase<enrParmKvp>.bitlist enrParmKvp { get; set; } = Mx.bitbase<enrParmKvp>.Create();
        public Mx.bitbase<enrPlaylistL1>.bitlist enrPlaylistL1 { get; set; } = Mx.bitbase<enrPlaylistL1>.Create();
        public Mx.bitbase<enrPlaylistL2>.bitlist enrPlaylistL2 { get; set; } = Mx.bitbase<enrPlaylistL2>.Create();
        public Mx.bitbase<enrPlaylistL3>.bitlist enrPlaylistL3 { get; set; } = Mx.bitbase<enrPlaylistL3>.Create();
        public Mx.bitbase<enrPlaylistL4>.bitlist enrPlaylistL4 { get; set; } = Mx.bitbase<enrPlaylistL4>.Create();
        public Mx.bitbase<enrSearchL1>.bitlist enrSearchL1 { get; set; } = Mx.bitbase<enrSearchL1>.Create();
        public Mx.bitbase<enrSearchL2>.bitlist enrSearchL2 { get; set; } = Mx.bitbase<enrSearchL2>.Create();
        public Mx.bitbase<enrSearchL3a>.bitlist enrSearchL3a { get; set; } = Mx.bitbase<enrSearchL3a>.Create();
        public Mx.bitbase<enrSearchL3b>.bitlist enrSearchL3b { get; set; } = Mx.bitbase<enrSearchL3b>.Create();
        public Mx.bitbase<enrURL>.bitlist enrURL { get; set; } = Mx.bitbase<enrURL>.Create();

        public Mx.bitbase<enrAkey>.row_enum tbrAkey { get; set; } = new Mx.bitbase<enrAkey>.row_enum();
    }

    public class enrAkey : Mx.bitbase
    {
        public enrAkey key { get; set; }
    }
    public class enrChannelL1 : Mx.bitbase
    {
        public enrChannelL1 kind { get; set; }
        public enrChannelL1 id { get; set; }
        public enrChannelL1 items { get; set; }
        public enrChannelL1 search_object_path { get; set; }
        public enrChannelL1 uploads { get; set; }

        public class tbrChannelL1 : Mx.bitbase<enrChannelL1>.row_enum
        {
            public enrChannelL2.ttbChannelL2 sub_items;

            [System.Diagnostics.DebuggerStepThrough]
            public tbrChannelL1(Mx.bitbase<enrChannelL1>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
                this.sub_items = new enrChannelL2.ttbChannelL2(ur_ttb.enrChannelList, ur_ttb);
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrChannelL1>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                    var branch_2 = this[this.r.items];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        this.sub_items.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }

            [System.Diagnostics.DebuggerStepThrough]
            public string FormatTree()
            {
                var tsv_ret = new System.Text.StringBuilder();
                tsv_ret.Append("L1").AppendLine();
                tsv_ret.Append(this.Format_Table(true));
                foreach (var row in this.sub_items)
                {
                    tsv_ret.AppendLine().Append("L2").AppendLine();
                    tsv_ret.Append(row.Format_Table(true));

                    tsv_ret.AppendLine().Append("L3").AppendLine();
                    tsv_ret.Append(row.sub_content_detail.Format_Table(true));

                    tsv_ret.AppendLine().Append("L3").AppendLine();
                    tsv_ret.Append(row.sub_content_detail.sub_rel_playlist.Format_Table(true));
                }

                return tsv_ret.ToString();
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void PullBranchValues()
            {
                var l3_row = this.sub_items.FirstRow();
                var l4_row = l3_row.sub_content_detail;
                var l5_row = l4_row.sub_rel_playlist;
                this[this.r.uploads] = l5_row[l5_row.r.uploads];
                this[this.r.id] = l3_row[l3_row.r.id];
            }
        }
    }
    public class enrChannelL2 : Mx.bitbase
    {
        public enrChannelL2 contentDetails { get; set; }
        public enrChannelL2 id { get; set; }
        public enrChannelL2 kind { get; set; }
        public enrChannelL2 search_object_path { get; set; }

        public class tbrChannelL2 : Mx.bitbase<enrChannelL2>.row_enum
        {
            public enrChannelL3.tbrChannelL3 sub_content_detail;
            [System.Diagnostics.DebuggerStepThrough]
            public tbrChannelL2() : base()
            {
            }
        }

        public class ttbChannelL2 : Mx.bitbase<enrChannelL2>.table_row<tbrChannelL2>
        {
            [System.Diagnostics.DebuggerStepThrough]
            public ttbChannelL2(Mx.bitbase<enrChannelL2>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrChannelL2>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    var new_row = this.NewRow();
                    new_row.sub_content_detail = new enrChannelL3.tbrChannelL3(ur_ttb.enrChContentDetail, ur_ttb);
                    chroot_row.CloneTo(new_row);
                    var branch_2 = new_row[new_row.r.contentDetails];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        new_row.sub_content_detail.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }
        }
    }
    public class enrChannelL3 : Mx.bitbase
    {
        public enrChannelL3 search_object_path { get; set; }
        public enrChannelL3 relatedPlaylists { get; set; }

        public class tbrChannelL3 : Mx.bitbase<enrChannelL3>.row_enum
        {
            public enrChannelL4.tbrChannelL4 sub_rel_playlist;

            [System.Diagnostics.DebuggerStepThrough]
            public tbrChannelL3(Mx.bitbase<enrChannelL3>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
                this.sub_rel_playlist = new enrChannelL4.tbrChannelL4(ur_ttb.enrChRelPlaylist);
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrChannelL3>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                    var branch_2 = this[this.r.relatedPlaylists];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        this.sub_rel_playlist.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }
        }
    }
    public class enrChannelL4 : Mx.bitbase
    {
        public enrChannelL4 search_object_path { get; set; }
        public enrChannelL4 uploads { get; set; }

        public class tbrChannelL4 : Mx.bitbase<enrChannelL4>.row_enum
        {
            [System.Diagnostics.DebuggerStepThrough]
            public tbrChannelL4(Mx.bitbase<enrChannelL4>.bitlist ur_bitlist) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrChannelL4>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                }
            }
        }
    }
    public class enrParmKvp : Mx.bitbase
    {
        public enrParmKvp parameter { get; set; }
        public enrParmKvp value { get; set; }
    }
    public class enrPlaylistL1 : Mx.bitbase
    {
        public enrPlaylistL1 items { get; set; }
        public enrPlaylistL1 kind { get; set; }
        public enrPlaylistL1 nextPageToken { get; set; }
        public enrPlaylistL1 search_object_path { get; set; }

        public class tbrPlaylistL1 : Mx.bitbase<enrPlaylistL1>.row_enum
        {
            public enrPlaylistL2.ttbPlaylistL2 sub_items;

            [System.Diagnostics.DebuggerStepThrough]
            public tbrPlaylistL1(Mx.bitbase<enrPlaylistL1>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
                this.sub_items = new enrPlaylistL2.ttbPlaylistL2(ur_ttb.enrPlaylistL2, ur_ttb);
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                this[this.r.nextPageToken] = "";
                foreach (var chroot_row in JsonData<enrPlaylistL1>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                    var branch_2 = this[this.r.items];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        this.sub_items.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }

            [System.Diagnostics.DebuggerStepThrough]
            public string FormatTree()
            {
                var tsv_ret = new System.Text.StringBuilder();
                tsv_ret.Append("L1").AppendLine();
                tsv_ret.Append(this.Format_Table(true));
                tsv_ret.Append("sub_items count: ").Append(this.sub_items.Count).AppendLine();
                foreach (var entry_rvp in this.sub_items.rvp())
                {
                    var row = entry_rvp.cur_row;

                    tsv_ret.AppendLine().Append("L1 row: ").Append(entry_rvp.ROWSEQ).AppendLine();
                    tsv_ret.AppendLine().Append("L2").AppendLine();
                    tsv_ret.Append(row.Format_Table(true));

                    tsv_ret.AppendLine().Append("L3").AppendLine();
                    tsv_ret.Append(row.sub_snippet.Format_Table(true));

                    tsv_ret.AppendLine().Append("L4").AppendLine();
                    tsv_ret.Append(row.sub_snippet.sub_resourceId.Format_Table(true));
                }

                return tsv_ret.ToString();
            }
        }
    }
    public class enrPlaylistL2 : Mx.bitbase
    {
        public enrPlaylistL2 kind { get; set; }
        public enrPlaylistL2 snippet { get; set; }
        public enrPlaylistL2 search_object_path { get; set; }
        public enrPlaylistL2 publishedAt { get; set; }
        public enrPlaylistL2 resourceId { get; set; }
        public enrPlaylistL2 title { get; set; }
        public enrPlaylistL2 video_kind { get; set; }
        public enrPlaylistL2 videoId { get; set; }

        public class tbrPlaylistL2 : Mx.bitbase<enrPlaylistL2>.row_enum
        {
            public enrPlaylistL3.tbrPlaylistL3 sub_snippet;
            [System.Diagnostics.DebuggerStepThrough]
            public tbrPlaylistL2() : base()
            {
            }
        }

        public class ttbPlaylistL2 : Mx.bitbase<enrPlaylistL2>.table_row<tbrPlaylistL2>
        {
            [System.Diagnostics.DebuggerStepThrough]
            public ttbPlaylistL2(Mx.bitbase<enrPlaylistL2>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrPlaylistL2>.Parse_JsonArray(ur_json, ur_path, this.bitlist))
                {
                    var new_row = this.NewRow();
                    new_row.sub_snippet = new enrPlaylistL3.tbrPlaylistL3(ur_ttb.enrPlaylistL3, ur_ttb);
                    chroot_row.CloneTo(new_row);
                    var branch_2 = new_row[new_row.r.snippet];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        new_row.sub_snippet.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void PullBranchValues()
            {
                foreach (var l2_row in this)
                {
                    var l3_row = l2_row.sub_snippet;
                    var l4_row = l3_row.sub_resourceId;
                    l2_row[l2_row.r.publishedAt] = l3_row[l3_row.r.publishedAt];
                    l2_row[l2_row.r.title] = l3_row[l3_row.r.title];
                    l2_row[l2_row.r.resourceId] = l3_row[l3_row.r.resourceId];
                    l2_row[l2_row.r.video_kind] = l4_row[l4_row.r.kind];
                    l2_row[l2_row.r.videoId] = l4_row[l4_row.r.videoId];
                }
            }
        }
    }
    public class enrPlaylistL3 : Mx.bitbase
    {
        public enrPlaylistL3 publishedAt { get; set; }
        public enrPlaylistL3 resourceId { get; set; }
        public enrPlaylistL3 title { get; set; }
        public enrPlaylistL3 search_object_path { get; set; }

        public class tbrPlaylistL3 : Mx.bitbase<enrPlaylistL3>.row_enum
        {
            public enrPlaylistL4.tbrPlaylistL4 sub_resourceId;

            [System.Diagnostics.DebuggerStepThrough]
            public tbrPlaylistL3(Mx.bitbase<enrPlaylistL3>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
                this.sub_resourceId = new enrPlaylistL4.tbrPlaylistL4(ur_ttb.enrPlaylistL4);
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrPlaylistL3>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                    var branch_2 = this[this.r.resourceId];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        this.sub_resourceId.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }
        }
    }
    public class enrPlaylistL4 : Mx.bitbase
    {
        public enrPlaylistL4 kind { get; set; }
        public enrPlaylistL4 videoId { get; set; }
        public enrPlaylistL4 search_object_path { get; set; }

        public class tbrPlaylistL4 : Mx.bitbase<enrPlaylistL4>.row_enum
        {
            [System.Diagnostics.DebuggerStepThrough]
            public tbrPlaylistL4(Mx.bitbase<enrPlaylistL4>.bitlist ur_bitlist) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrPlaylistL4>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                }
            }
        }
    }
    public class enrSearchL1 : Mx.bitbase
    {
        public enrSearchL1 items { get; set; }
        public enrSearchL1 kind { get; set; }
        public enrSearchL1 nextPageToken { get; set; }
        public enrSearchL1 search_object_path { get; set; }

        public class tbrSearchL1 : Mx.bitbase<enrSearchL1>.row_enum
        {
            public enrSearchL2.ttbSearchL2 sub_items;

            [System.Diagnostics.DebuggerStepThrough]
            public tbrSearchL1(Mx.bitbase<enrSearchL1>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
                this.sub_items = new enrSearchL2.ttbSearchL2(ur_ttb.enrSearchL2, ur_ttb);
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                this[this.r.nextPageToken] = "";
                foreach (var chroot_row in JsonData<enrSearchL1>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                    var branch_2 = this[this.r.items];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        this.sub_items.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }

            [System.Diagnostics.DebuggerStepThrough]
            public string FormatTree()
            {
                var tsv_ret = new System.Text.StringBuilder();
                tsv_ret.Append("L1").AppendLine();
                tsv_ret.Append(this.Format_Table(true));
                tsv_ret.Append("sub_items count: ").Append(this.sub_items.Count).AppendLine();
                foreach (var entry_rvp in this.sub_items.rvp())
                {
                    var row = entry_rvp.cur_row;

                    tsv_ret.AppendLine().Append("L1 row: ").Append(entry_rvp.ROWSEQ).AppendLine();
                    tsv_ret.AppendLine().Append("L2").AppendLine();
                    tsv_ret.Append(row.Format_Table(true));

                    tsv_ret.AppendLine().Append("L3").AppendLine();
                    tsv_ret.Append(row.sub_id.Format_Table(true));

                    tsv_ret.AppendLine().Append("L4").AppendLine();
                    tsv_ret.Append(row.sub_snippet.Format_Table(true));
                }

                return tsv_ret.ToString();
            }
        }
    }
    public class enrSearchL2 : Mx.bitbase
    {
        public enrSearchL2 kind { get; set; }
        public enrSearchL2 snippet { get; set; }
        public enrSearchL2 search_object_path { get; set; }
        public enrSearchL2 publishedAt { get; set; }
        public enrSearchL2 title { get; set; }
        public enrSearchL2 video_kind { get; set; }
        public enrSearchL2 videoId { get; set; }

        public class tbrSearchL2 : Mx.bitbase<enrSearchL2>.row_enum
        {
            public enrSearchL3a.tbrSearchL3a sub_id;
            public enrSearchL3b.tbrSearchL3b sub_snippet;
            [System.Diagnostics.DebuggerStepThrough]
            public tbrSearchL2() : base()
            {
            }
        }

        public class ttbSearchL2 : Mx.bitbase<enrSearchL2>.table_row<tbrSearchL2>
        {
            [System.Diagnostics.DebuggerStepThrough]
            public ttbSearchL2(Mx.bitbase<enrSearchL2>.bitlist ur_bitlist, TableVar ur_ttb) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrSearchL2>.Parse_JsonArray(ur_json, ur_path, this.bitlist))
                {
                    var new_row = this.NewRow();
                    new_row.sub_id = new enrSearchL3a.tbrSearchL3a(ur_ttb.enrSearchL3a);
                    new_row.sub_snippet = new enrSearchL3b.tbrSearchL3b(ur_ttb.enrSearchL3b);
                    chroot_row.CloneTo(new_row);
                    var branch_2 = new_row[new_row.r.snippet];
                    if (string.IsNullOrWhiteSpace(branch_2) == false)
                    {
                        new_row.sub_snippet.LoadJson(branch_2, ur_json, ur_ttb);
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void PullBranchValues()
            {
                foreach (var l2_row in this)
                {
                    var l3_row = l2_row.sub_snippet;
                    var l4_row = l2_row.sub_id;
                    l2_row[l2_row.r.publishedAt] = l3_row[l3_row.r.publishedAt];
                    l2_row[l2_row.r.title] = l3_row[l3_row.r.title];
                    l2_row[l2_row.r.video_kind] = l4_row[l4_row.r.kind];
                    l2_row[l2_row.r.videoId] = l4_row[l4_row.r.videoId];
                }
            }
        }
    }
    public class enrSearchL3a : Mx.bitbase
    {
        public enrSearchL3a kind { get; set; }
        public enrSearchL3a videoId { get; set; }
        public enrSearchL3a search_object_path { get; set; }

        public class tbrSearchL3a : Mx.bitbase<enrSearchL3a>.row_enum
        {
            [System.Diagnostics.DebuggerStepThrough]
            public tbrSearchL3a(Mx.bitbase<enrSearchL3a>.bitlist ur_bitlist) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrSearchL3a>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                }
            }
        }
    }
    public class enrSearchL3b : Mx.bitbase
    {
        public enrSearchL3b publishedAt { get; set; }
        public enrSearchL3b title { get; set; }
        public enrSearchL3b search_object_path { get; set; }

        public class tbrSearchL3b : Mx.bitbase<enrSearchL3b>.row_enum
        {
            [System.Diagnostics.DebuggerStepThrough]
            public tbrSearchL3b(Mx.bitbase<enrSearchL3b>.bitlist ur_bitlist) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void LoadJson(string ur_path, string ur_json, TableVar ur_ttb)
            {
                foreach (var chroot_row in JsonData<enrSearchL3b>.Parse_JsonArray(ur_json, ur_path, this.bitlist).FilterFirst())
                {
                    chroot_row.CloneTo(this);
                }
            }
        }
    }
    public class enrURL : Mx.bitbase
    {
        public enrURL protocol { get; set; }
        public enrURL server_path { get; set; }
        public enrURL folder_path { get; set; }
        public enrURL parameter_list { get; set; }

        public class tbrURL : Mx.bitbase<enrURL>.row_enum
        {
            [System.Diagnostics.DebuggerStepThrough]
            public tbrURL(Mx.bitbase<enrURL>.bitlist ur_bitlist) : base(ur_bitlist)
            {
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void pick_folder_path(string[] ur_parm_rec)
            {
                var folder_stp = new System.Text.StringBuilder();
                foreach (var entry in ur_parm_rec)
                {
                    if (folder_stp.Length > 0)
                    {
                        folder_stp.Append("/");
                    }

                    folder_stp.Append(entry);
                }

                this[this.r.folder_path] = folder_stp.ToString();
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void pick_parameter_list(Mx.bitbase<enrParmKvp>.table_row ur_parm_rec)
            {
                var parm_stp = new System.Text.StringBuilder();
                foreach (var parm_row in ur_parm_rec)
                {
                    if (parm_stp.Length > 0)
                    {
                        parm_stp.Append("&");
                    }

                    parm_stp.Append($"{parm_row[parm_row.r.parameter]}={parm_row[parm_row.r.value]}");
                }

                this[this.r.parameter_list] = parm_stp.ToString();
            }

            [System.Diagnostics.DebuggerStepThrough]
            public string ToURL()
            {
                return TextFn.CombineURL(this[this.r.protocol], this[this.r.server_path], this[this.r.folder_path], this[this.r.parameter_list]);
            }
        }
    }
}
