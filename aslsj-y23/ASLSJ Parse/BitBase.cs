namespace Mx
{
    public class bitbase
    {
        public string pname { get; set; }
        public int seq { get; set; }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<string> CSV_Split(string ur_text, char ur_separator = ',')
        {
            var reta_uquote = new System.Collections.Generic.List<string>();
            var char_entry = ' ';
            var char_prev_entry = ' ';
            var stpFIELD = new System.Text.StringBuilder();
            var intEND_BEFORE = ur_text.Length;
            var balanced_quotes = true;
            var had_quotes = false;
            for (int CHRSEQ = 0; CHRSEQ < intEND_BEFORE; CHRSEQ += 1)
            {
                char_prev_entry = char_entry;
                char_entry = ur_text[CHRSEQ];
                if (char_entry == '\"')
                {
                    had_quotes = true;
                    if (balanced_quotes == false)
                    {
                        if (char_prev_entry == char_entry)
                        {
                            stpFIELD.Append(char_entry);
                        }
                    }

                    balanced_quotes = !balanced_quotes;
                }
                else if (balanced_quotes == true &&
                    ((char_entry == ur_separator) || (CHRSEQ == (intEND_BEFORE - 1))))
                {
                    if (char_entry != ur_separator)
                    {
                        stpFIELD.Append(char_entry);
                    }

                    var field_data = stpFIELD.ToString();
                    if (had_quotes == false)
                    {
                        field_data = field_data.Trim();
                    }

                    reta_uquote.Add(field_data);
                    stpFIELD.Clear();
                    had_quotes = false;
                }
                else
                {
                    stpFIELD.Append(char_entry);
                }
            }

            if (stpFIELD.Length > 0)
            {
                reta_uquote.Add(stpFIELD.ToString());
            }

            return reta_uquote;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static void Format_Table(System.Text.StringBuilder ur_stp, System.Collections.Generic.List<string> ur_field_data, bool ur_quoted = true, char ur_separator = ',')
        {
            for (int COLSEQ = 0; COLSEQ < ur_field_data.Count; COLSEQ += 1)
            {
                if (COLSEQ > 0)
                    ur_stp.Append(ur_separator);

                var found_quoted = ur_quoted;
                var cur_value = ur_field_data[COLSEQ];
                if (found_quoted == false)
                {
                    if (cur_value.IndexOf(' ') >= 0 || cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf(ur_separator) >= 0 || cur_value.IndexOf('"') >= 0)
                    {
                        found_quoted = true;
                    }
                }

                if (found_quoted)
                {
                    ur_stp.Append("\"").Append(cur_value.Replace("\"", "\"\"")).Append("\"");
                }
                else
                {
                    ur_stp.Append(cur_value);
                }
            }

            ur_stp.AppendLine();
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static string Format_Table(System.Collections.Generic.List<string> ur_field_data, bool ur_quoted = true, char ur_separator = ',')
        {
            var stm_table_row = new System.Text.StringBuilder();
            Mx.bitbase.Format_Table(stm_table_row, ur_field_data);
            return stm_table_row.ToString();
        }
    }

    public static class bitbase<T> where T : bitbase, new()
    {
        public static bool class_bitbase_written { get; set; } = false;
        private static System.Collections.Generic.List<string> class_name_list { get; set; } = new System.Collections.Generic.List<string>();
        private static System.Collections.Generic.List<T> class_prop_list { get; set; } = new System.Collections.Generic.List<T>();
        [System.Diagnostics.DebuggerStepThrough]
        public static void populate_bitbase()
        {
            if (bitbase<T>.class_bitbase_written == false)
            {
                var cur_class = typeof(T);
				var propa = cur_class.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
				for (var PRPSEQ = 0; PRPSEQ < propa.Length; PRPSEQ += 1)
				{
					var entry = propa[PRPSEQ];
					var bitbase_prop = (T)entry.GetValue(null);
					bitbase_prop.pname = entry.Name;
					bitbase_prop.seq = PRPSEQ;
					class_name_list.Add(entry.Name);
					class_prop_list.Add(bitbase_prop);
				}

                bitbase<T>.class_bitbase_written = true;
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<T> RefKeys()
        {
            populate_bitbase();
            return class_prop_list;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<T> RefKeySearch(string ur_prop_name)
        {
            populate_bitbase();
            var ret_bitbase = new System.Collections.Generic.List<T>();
            foreach (var entry in class_prop_list)
            {
                if (entry.pname == ur_prop_name)
                {
                    ret_bitbase.Add(entry);
                }
            }

            return ret_bitbase;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<string> RefNames()
        {
            populate_bitbase();
            return class_name_list;
        }

        public class row_enum : System.Collections.Generic.Dictionary<T, string>
        {
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum()
            {
                foreach (var entry in bitbase<T>.RefKeys())
                {
                    this.Add(entry, "");
                }
            }
            public T[] CompileColumnArray(params T[] ur_col_list)
            {
                return ur_col_list;
            }

            public DecimalEnum dec
            {
                get
                {
                    return new DecimalEnum(this);
                }
            }
            public class DecimalEnum
            {
                public row_enum parent_row;
                public DecimalEnum(row_enum ur_parent_row)
                {
                    this.parent_row = ur_parent_row;
                }

                public decimal this[T ur_key]
                {
                    get
                    {
                        return parent_row.GetDecimal(ur_key);
                    }
                }
            }
            public DateEnum date
            {
                get
                {
                    return new DateEnum(this);
                }
            }

            public class DateEnum
            {
                public row_enum parent_row;
                public DateEnum(row_enum ur_parent_row)
                {
                    this.parent_row = ur_parent_row;
                }

                public System.DateTime this[T ur_key]
                {
                    get
                    {
                        return parent_row.GetDate(ur_key);
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string Format_Table(bool ur_hdr, bool ur_quoted = true)
            {
                var stpRET = new System.Text.StringBuilder();
                this.Format_Table(stpRET, ur_hdr, ur_quoted);
                return stpRET.ToString();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void Format_Table(System.Text.StringBuilder ur_stp, bool ur_hdr, bool ur_quoted = true)
            {
                var stra_col = bitbase<T>.RefNames();
                if (ur_hdr)
                {
                    for (int COLSEQ = 0; COLSEQ < stra_col.Count; COLSEQ += 1)
                    {
                        if (COLSEQ > 0)
                            ur_stp.Append('\t');

                        var found_quoted = ur_quoted;
                        var cur_value = stra_col[COLSEQ];
                        if (found_quoted == false)
                        {
                            if (cur_value.IndexOf(' ') >= 0 || cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf('"') >= 0)
                            {
                                found_quoted = true;
                            }
                        }

                        if (found_quoted)
                        {
                            ur_stp.Append("\"").Append(cur_value.Replace("\"", "\"\"")).Append("\"");
                        }
                        else
                        {
                            ur_stp.Append(cur_value);
                        }
                    }

                    ur_stp.AppendLine();
                }

                for (int COLSEQ = 0; COLSEQ < stra_col.Count; COLSEQ += 1)
                {
                    if (COLSEQ > 0)
                        ur_stp.Append('\t');

                    var found_quoted = ur_quoted;
                    var cur_value = this.v_enm(COLSEQ);
                    if (found_quoted == false)
                    {
                        if (cur_value.IndexOf(' ') >= 0 || cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf('"') >= 0)
                        {
                            found_quoted = true;
                        }
                    }

                    if (found_quoted)
                    {
                        ur_stp.Append("\"").Append(cur_value.Replace("\"", "\"\"")).Append("\"");
                    }
                    else
                    {
                        ur_stp.Append(cur_value);
                    }
                }

                ur_stp.AppendLine();
            }
            public decimal GetDecimal(T ur_col)
            {
                var s_qty = this[ur_col];
                decimal d_qty = 0;
                decimal.TryParse(s_qty, out d_qty);
                return d_qty;
            }
            public System.DateTime GetDate(T ur_col)
            {
                var s_date = this[ur_col];
                if (string.IsNullOrWhiteSpace(s_date) == false && s_date.Contains("-") == false && s_date.Contains("/") == false)
                {
                    s_date = $"{s_date.Substring(0, 4)}-{s_date.Substring(4, 2)}-{s_date.Substring(6, s_date.Length - 6)}";
                }

                var date_val = System.DateTime.MinValue;
                System.DateTime.TryParse(s_date, out date_val);
                return date_val;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<T> RefKeys()
            {
                return bitbase<T>.RefKeys();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string v_enm(int ur_index)
            {
                var class_prop_list = bitbase<T>.RefKeys();
                if (ur_index >= class_prop_list.Count)
                    return "";
                else
                    return this[class_prop_list[ur_index]];
            }
        }

        public class table_row : System.Collections.Generic.List<bitbase<T>.row_enum>
        {
            public table_row()
            {
                bitbase<T>.populate_bitbase();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum add_row()
            {
                var ret_row = new row_enum();
                this.Add(ret_row);
                return ret_row;
            }

            public T[] CompileColumnArray(params T[] ur_col_list)
            {
                return ur_col_list;
            }

            public System.Collections.Generic.List<string> CompileFilterStrings(T[] ur_col_list, string ur_string_format, System.Collections.Generic.Dictionary<row_enum, table_row> ur_collection)
            {
                var ret_list = new System.Collections.Generic.List<string>();
                var filter_text = new System.Text.StringBuilder();
                foreach (var entry in ur_collection.Keys)
                {
                    var col_values = new System.Collections.Generic.List<string>();
                    foreach (var col_entry in ur_col_list)
                    {
                        col_values.Add(entry[col_entry].Replace("'", "''"));
                    }

                    if (filter_text.Length > 0)
                    {
                        filter_text.Append(" OR ");
                    }
                    filter_text.Append("(").Append(string.Format(ur_string_format, col_values.ToArray())).Append(")");
                    if (filter_text.Length > 3000)
                    {
                        ret_list.Add(filter_text.ToString());
                        filter_text.Clear();
                    }
                }

                if (filter_text.Length > 0)
                {
                    ret_list.Add(filter_text.ToString());
                }

                return ret_list;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row Filter(T ur_col, string ur_text)
            {
                var ret_table = new table_row();
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text))
                    {
                        ret_table.Add(row_entry);
                    }
                }

                return ret_table;
            }
            public System.Collections.Generic.Dictionary<row_enum, table_row> FilterDistinct(T[] ur_col_list)
            {
                var ret_table = new System.Collections.Generic.Dictionary<row_enum, table_row>();
                var track_unique = new System.Collections.Generic.Dictionary<string, row_enum>();
                foreach (var row_entry in this)
                {
                    var key_row = new row_enum();
                    foreach (var col_entry in ur_col_list)
                    {
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = key_row.Format_Table(false);
                    if (track_unique.ContainsKey(str_keyrow) == false)
                    {
                        track_unique.Add(str_keyrow, key_row);
                        ret_table.Add(key_row, new table_row());
                    }
                    else
                    {
                        key_row = track_unique[str_keyrow];
                    }

                    ret_table[key_row].Add(row_entry);
                }

                return ret_table;
            }
            public table_row FilterFirst()
            {
                var ret_table = new table_row();
                foreach (var row_entry in this)
                {
                    ret_table.Add(row_entry);
                    break;
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row FilterOne(T ur_col, string ur_text)
            {
                var ret_table = new table_row();
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text))
                    {
                        ret_table.Add(row_entry);
                        break;
                    }
                }

                return ret_table;
            }
            public row_enum FirstRow()
            {
                row_enum ret_row = new row_enum();
                foreach (var row_entry in this)
                {
                    ret_row = row_entry;
                    break;
                }

                return ret_row;
            }

            public row_enum FirstRow(T ur_col, string ur_text)
            {
                var ret_row = new row_enum();
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text))
                    {
                        ret_row = row_entry;
                        break;
                    }
                }

                return ret_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum FirstRow(T ur_col, string ur_text, System.Text.StringBuilder ur_notice_message)
            {
                var ret_row = new row_enum();
                var found_row = false;
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text))
                    {
                        ret_row = row_entry;
                        found_row = true;
                        break;
                    }
                }

                if (found_row == false)
                {
                    ur_notice_message.AppendLine($"Could not find {ur_text} in {ur_col.pname}.");
                }

                return ret_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string Format_Table(bool ur_hdr, bool ur_quoted = true)
            {
                var stpRET = new System.Text.StringBuilder();
                this.Format_Table(stpRET, ur_hdr, ur_quoted);
                return stpRET.ToString();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void Format_Table(System.Text.StringBuilder ur_ret, bool ur_hdr, bool ur_quoted = true)
            {
                for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ += 1)
                {
                    ur_ret.Append(this[ROWSEQ].Format_Table((ROWSEQ == 0) && ur_hdr, ur_quoted));
                }
            }
            public System.Collections.Generic.SortedDictionary<string, table_row> GroupByCol(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<string, table_row>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry[ur_col];
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row());
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            public System.Collections.Generic.SortedDictionary<System.DateTime, table_row> GroupByDate(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<System.DateTime, table_row>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDate(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row());
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }

            public System.Collections.Generic.SortedDictionary<decimal, table_row> GroupByDecimal(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<decimal, table_row>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDecimal(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row());
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public void Persist_Read(string ur_persist_path, bool ur_flag_overwrite_rows = true)
            {
                if (ur_flag_overwrite_rows)
                {
                    this.Clear();
                }

                if (System.IO.File.Exists(ur_persist_path))
                {
                    using (var stmIN_FILE = new System.IO.StreamReader(ur_persist_path, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)))
                    {
                        var header_processed = false;
                        var found_col_list = new System.Collections.Generic.List<T>();
                        var cur_line = "";
                        while (stmIN_FILE.EndOfStream == false)
                        {
                            cur_line += stmIN_FILE.ReadLine();
                            var balanced_quotes = true;
                            for (int CHRSEQ = 0; CHRSEQ < cur_line.Length; CHRSEQ += 1)
                            {
                                if (cur_line[CHRSEQ] == '\"')
                                {
                                    balanced_quotes = !balanced_quotes;
                                }
                            }

                            if (balanced_quotes == false)
                            {
                                cur_line += "\n";
                            }
                            else
                            {
                                var new_row = new row_enum();
                                if (cur_line.StartsWith("\n"))
                                {
                                    cur_line = cur_line.Substring("\n".Length);
                                }

                                if (header_processed == false)
                                {
                                    header_processed = true;
                                    var any_field_found = false;
                                    foreach (var entry in bitbase.CSV_Split(cur_line, '\t'))
                                    {
                                        T found_key = null;
                                        foreach (var cur_key in this.RefKeys())
                                        {
                                            if (cur_key.pname == entry)
                                            {
                                                found_key = cur_key;
                                                any_field_found = true;
                                            }
                                        }

                                        found_col_list.Add(found_key);
                                    }

                                    if (any_field_found == false)
                                    {
                                        break;
                                    }
                                }
                                else if (cur_line.Length > 0)
                                {
                                    var found_field_list = bitbase.CSV_Split(cur_line, '\t');
                                    var intEND_BEFORE = found_field_list.Count;
                                    if (found_col_list.Count < intEND_BEFORE)
                                    {
                                        intEND_BEFORE = found_col_list.Count;
                                    }

                                    for (var COLSEQ = 0; COLSEQ < intEND_BEFORE; COLSEQ++)
                                    {

                                        var key_entry = found_col_list[COLSEQ];
                                        if (key_entry != null)
                                        {
                                            new_row[key_entry] = found_field_list[COLSEQ];
                                        }
                                    }

                                    this.Add(new_row);
                                }

                                cur_line = "";
                            }
                        }
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void Persist_Write(string ur_persist_path)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ur_persist_path));
                using (var stmOUT_FILE = new System.IO.StreamWriter(ur_persist_path, false, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)))
                {
                    for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ++)
                    {
                        stmOUT_FILE.Write(this[ROWSEQ].Format_Table(ROWSEQ == 0, true));
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<T> RefKeys()
            {
                return bitbase<T>.RefKeys();
            }
            public decimal SumAsDecimal(T ur_col)
            {
                decimal ret_val = 0;
                foreach (var row_entry in this)
                {
                    ret_val += row_entry.GetDecimal(ur_col);
                }

                return ret_val;
            }
        }


        public class Trbase<W> where W : T, new()
        {
            public static W NewBitBase()
            {
                var objRET = new W();
                bitbase<T>.populate_bitbase();
                return objRET;
            }
        }
    }
}