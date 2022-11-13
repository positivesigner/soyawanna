using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeChannel
{
    class JsonData<T> where T : Mx.bitbase, new()
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static Mx.bitbase<T>.table_row Parse_JsonArray(
            string ur_file_contents,
            string ur_object_path,
            Mx.bitbase<T>.bitlist ur_eno_t)
        {
            return JsonData<T>.Parse_JsonArray(null, ur_file_contents, ur_object_path, ur_eno_t);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static Mx.bitbase<T>.table_row Parse_JsonArray(
            System.Text.StringBuilder ur_stp_message,
            string ur_file_contents,
            string ur_object_path,
            Mx.bitbase<T>.bitlist ur_eno_t)
        {
            var ret_json_rows = new Mx.bitbase<T>.table_row(ur_eno_t);
            if (ur_stp_message == null || ur_stp_message.Length == 0)
            {
                T object_path_key = null;
                foreach (var entry in ret_json_rows.bitlist.RefKeySearch("search_object_path"))
                {
                    object_path_key = entry;
                }

                var reader = new Newtonsoft.Json.JsonTextReader(new System.IO.StringReader(ur_file_contents));
                Mx.bitbase<T>.row_enum sdaCUR_OBJ = null;
                var lstLEVEL = new System.Collections.Generic.List<string>();
                var lstLEVEL_COUNT = new System.Collections.Generic.Dictionary<int, int>();
                var prev_propname = "";
                var gparent_path = "";
                var parent_path = "";
                var cur_label = "";
                var cur_path = "";
                var stpPATH = new System.Text.StringBuilder();
                var found_level = -1;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case Newtonsoft.Json.JsonToken.StartObject:
                            cur_label = prev_propname + "{";
                            if (lstLEVEL_COUNT.Count < lstLEVEL.Count + 1)
                            {
                                lstLEVEL_COUNT.Add(lstLEVEL.Count + 1, 0);
                            }
                            else
                            {
                                lstLEVEL_COUNT[lstLEVEL.Count + 1] += 1;
                            }

                            if (cur_path.Length > 0 && cur_path[cur_path.Length - 1] == '[')
                            {
                                cur_label = lstLEVEL_COUNT[lstLEVEL.Count + 1] + "]{";
                                if (cur_path == ur_object_path)
                                {
                                    if (found_level == -1)
                                    {
                                        found_level = lstLEVEL.Count + 1;
                                    }

                                    if (sdaCUR_OBJ == null)
                                    {
                                        sdaCUR_OBJ = ret_json_rows.NewRow();
                                        if (object_path_key != null)
                                        {
                                            sdaCUR_OBJ[object_path_key] = cur_path + cur_label;
                                        }
                                    }
                                }
                            }

                            gparent_path = "";
                            if (lstLEVEL.Count > 0 && lstLEVEL[lstLEVEL.Count - 1].EndsWith("]{"))
                            {
                                gparent_path = string.Join("", lstLEVEL.ToArray(), 0, lstLEVEL.Count - 1);
                            }

                            lstLEVEL.Add(cur_label);
                            stpPATH.Append(cur_label);
                            parent_path = cur_path;
                            cur_path = stpPATH.ToString();
                            if (cur_path == ur_object_path)
                            {
                                if (found_level == -1)
                                {
                                    found_level = lstLEVEL.Count;
                                }

                                if (sdaCUR_OBJ == null)
                                {
                                    sdaCUR_OBJ = ret_json_rows.NewRow();
                                    if (object_path_key != null)
                                    {
                                        sdaCUR_OBJ[object_path_key] = cur_path;
                                    }
                                }
                            }
                            else if (cur_label.EndsWith("]{") == false &&
                                (
                                    parent_path == ur_object_path ||
                                    gparent_path == ur_object_path
                                    )
                                )
                            {
                                foreach (var entry in ret_json_rows.bitlist.RefKeySearch(prev_propname))
                                {
                                    sdaCUR_OBJ[entry] = cur_path;
                                }
                            }

                            break;

                        case Newtonsoft.Json.JsonToken.StartArray:
                            cur_label = prev_propname + "[";
                            if (lstLEVEL_COUNT.Count < lstLEVEL.Count + 1)
                            {
                                lstLEVEL_COUNT.Add(lstLEVEL.Count + 1, 0);
                            }

                            if (cur_path.Length > 0)
                            {
                                if (cur_path[cur_path.Length - 1] == '[')
                                {
                                    cur_label = "[";
                                }
                                else if (found_level == lstLEVEL.Count)
                                {
                                    if (sdaCUR_OBJ == null)
                                    {
                                        sdaCUR_OBJ = ret_json_rows.NewRow();
                                        if (object_path_key != null)
                                        {
                                            sdaCUR_OBJ[object_path_key] = ur_object_path;
                                        }
                                    }
                                }
                            }

                            lstLEVEL.Add(cur_label);
                            stpPATH.Append(cur_label);
                            parent_path = cur_path;
                            cur_path = stpPATH.ToString();
                            if (parent_path == ur_object_path)
                            {
                                foreach (var entry in ret_json_rows.bitlist.RefKeySearch(prev_propname))
                                {
                                    sdaCUR_OBJ[entry] = cur_path;
                                }
                            }

                            break;

                        case Newtonsoft.Json.JsonToken.EndObject:
                        case Newtonsoft.Json.JsonToken.EndArray:
                            if (reader.TokenType == Newtonsoft.Json.JsonToken.EndObject &&
                                found_level == lstLEVEL.Count &&
                                sdaCUR_OBJ != null)
                            {
                                sdaCUR_OBJ = null;
                            }

                            cur_label = lstLEVEL[lstLEVEL.Count - 1];
                            stpPATH.Remove(stpPATH.Length - cur_label.Length, cur_label.Length);
                            cur_path = stpPATH.ToString();
                            if (lstLEVEL_COUNT.Count > lstLEVEL.Count)
                            {
                                lstLEVEL_COUNT.Remove(lstLEVEL.Count + 1);
                            }

                            lstLEVEL.RemoveAt(lstLEVEL.Count - 1);
                            if (found_level > lstLEVEL.Count)
                            {
                                found_level = -1;
                            }
                            break;

                        default:
                            if (reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName)
                            {
                                prev_propname = reader.Value.ToString();
                            }
                            else if (found_level == lstLEVEL.Count)
                            {
                                if (sdaCUR_OBJ == null)
                                {
                                    sdaCUR_OBJ = ret_json_rows.NewRow();
                                    if (object_path_key != null)
                                    {
                                        sdaCUR_OBJ[object_path_key] = ur_object_path;
                                    }
                                }

                                foreach (var entry in ret_json_rows.bitlist.RefKeySearch(prev_propname))
                                {
                                    var cur_val = "[blank]";
                                    if (reader.Value != null)
                                    {
                                        cur_val = reader.Value.ToString();
                                    }

                                    sdaCUR_OBJ[entry] = cur_val;
                                }
                            }

                            break;
                    }
                }
            }

            return ret_json_rows;
        }

    }
}
