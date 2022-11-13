//ue_GDL_MxBitBase_2022m11d10e1v472
//replaced ext class Mongoose.IDO.Protocol.IIDOCommands with desktop Mongoose.IDO.Client
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mx
{
    public class bitbase
    {
        public string pname { get; set; }
        public int seq { get; set; }
    }

    public class bitbase_fn
    {
        public class enrCSV : Mx.bitbase
        {
            public enrCSV new_field { get; set; }
            public enrCSV empty_quoted_field_or_quoted_field_inital_escaped_qs { get; set; }
            public enrCSV flat_field { get; set; }
            public enrCSV flat_field_escaped_qs_or_quoted_subsection { get; set; }
            public enrCSV quoted_field { get; set; }
            public enrCSV quoted_field_escaped_qs_or_new_field_or_flat_field { get; set; }
            public enrCSV start_quoted_field { get; set; }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<string> CSV_Split(string ur_text, char ur_separator = ',')
        {
            var enoCSV = Mx.bitbase<enrCSV>.Create();
            var csv_state = new Mx.bitbase<enrCSV>.row_names(enoCSV).RefKeys()[0];
            csv_state = enoCSV.r.new_field;
            var reta_uquote = new System.Collections.Generic.List<string>();
            var char_entry = ' ';
            var lit_mt = "";
            var lit_qs = '\"';
            var stpFIELD = new System.Text.StringBuilder();
            var intEND_BEFORE = ur_text.Length;
            for (int CHRSEQ = 0; CHRSEQ < intEND_BEFORE; CHRSEQ += 1)
            {
                char_entry = ur_text[CHRSEQ];
                if (csv_state == enoCSV.r.new_field)
                {
                    //expect qs to open quoted field; separator for empty flat field; other for unqouted flat field, 
                    if (char_entry == lit_qs)
                    {
                        csv_state = enoCSV.r.start_quoted_field;
                    }
                    else if (char_entry == ur_separator)
                    {
                        reta_uquote.Add(lit_mt);
                        csv_state = enoCSV.r.new_field;
                    }
                    else //misc char
                    {
                        stpFIELD.Append(char_entry);
                        csv_state = enoCSV.r.flat_field;
                    }
                }
                else if (csv_state == enoCSV.r.empty_quoted_field_or_quoted_field_inital_escaped_qs)
                {
                    //expect qs means post one escaped qs before quoted field; separator for empty quoted field; other for empty quoted subsection before flat field
                    if (char_entry == lit_qs)
                    {
                        stpFIELD.Append(lit_qs);
                        csv_state = enoCSV.r.quoted_field;
                    }
                    else if (char_entry == ur_separator)
                    {
                        reta_uquote.Add(lit_mt);
                        csv_state = enoCSV.r.new_field;
                    }
                    else //misc char
                    {
                        stpFIELD.Append(char_entry);
                        csv_state = enoCSV.r.flat_field;
                    }
                }
                else if (csv_state == enoCSV.r.flat_field)
                {
                    //expect qs means escaped qs or quoted text; separator means new field; other means continue flat field
                    if (char_entry == lit_qs)
                    {
                        csv_state = enoCSV.r.flat_field_escaped_qs_or_quoted_subsection;
                    }
                    else if (char_entry == ur_separator)
                    {
                        reta_uquote.Add(stpFIELD.ToString());
                        stpFIELD.Clear();
                        csv_state = enoCSV.r.new_field;
                    }
                    else //separator, misc char
                    {
                        stpFIELD.Append(char_entry);
                    }
                }
                else if (csv_state == enoCSV.r.flat_field_escaped_qs_or_quoted_subsection)
                {
                    //expect qs means post one escaped qs and continue flat field; other means continue quoted subsection
                    if (char_entry == lit_qs)
                    {
                        stpFIELD.Append(char_entry);
                        csv_state = enoCSV.r.flat_field;
                    }
                    else //separator, misc char
                    {
                        stpFIELD.Append(char_entry);
                    }
                }
                else if (csv_state == enoCSV.r.start_quoted_field)
                {
                    //expect qs for empty quoted field or quoted field with an inital escaped qs; other continues quoted field
                    if (char_entry == lit_qs)
                    {
                        csv_state = enoCSV.r.empty_quoted_field_or_quoted_field_inital_escaped_qs;
                    }
                    else //separator, misc char
                    {
                        stpFIELD.Append(char_entry);
                        csv_state = enoCSV.r.quoted_field;
                    }
                }
                else if (csv_state == enoCSV.r.quoted_field)
                {
                    //expect qs for escaped qs or closed quoted field; other continues quoted field
                    if (char_entry == lit_qs)
                    {
                        csv_state = enoCSV.r.quoted_field_escaped_qs_or_new_field_or_flat_field;
                    }
                    else //separator, misc char
                    {
                        stpFIELD.Append(char_entry);
                    }
                }
                else if (csv_state == enoCSV.r.quoted_field_escaped_qs_or_new_field_or_flat_field)
                {
                    //expect qs means post one escaped qs and continue quoted field; separator for new field; other means continue flat field
                    if (char_entry == lit_qs)
                    {
                        stpFIELD.Append(char_entry);
                        csv_state = enoCSV.r.quoted_field;
                    }
                    else if (char_entry == ur_separator)
                    {
                        reta_uquote.Add(stpFIELD.ToString());
                        stpFIELD.Clear();
                        csv_state = enoCSV.r.new_field;
                    }
                    else //misc char
                    {
                        stpFIELD.Append(char_entry);
                        csv_state = enoCSV.r.flat_field;
                    }
                }
            }

            if (stpFIELD.Length > 0)
            {
                reta_uquote.Add(stpFIELD.ToString());
            }

            return reta_uquote;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static string ExpandKeyByLength(int ur_field_length, string ur_key_val)
        {
            var ret_text = "";
            var cur_key = ur_key_val.Trim();
            var seq_num = "";
            var leading_zeros = "";
            foreach (var cur_char in cur_key.Reverse())
            {
                var found_non_numeric = false;
                switch (cur_char)
                {
                    case '0':
                        seq_num = cur_char + seq_num;
                        leading_zeros += cur_char;
                        break;
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        seq_num = cur_char + seq_num;
                        leading_zeros = "";
                        break;
                    default:
                        found_non_numeric = true;
                        break;
                }

                if (found_non_numeric)
                {
                    break;
                }
            }

            var rem_text = cur_key.Substring(0, cur_key.Length - seq_num.Length);
            var pad_char = '0';
            if (rem_text.Length == 0)
            {
                pad_char = ' ';
            }

            if (seq_num.Length >= 0 && leading_zeros.Length > 0)
            {
                if (leading_zeros.Length == seq_num.Length)
                {
                    seq_num = "0";
                }
                else
                {
                    seq_num = seq_num.Substring(leading_zeros.Length);
                }
            }

            var pad_size = ur_field_length - rem_text.Length - seq_num.Length;
            if (seq_num.Length == 0 || pad_size < 0)
            {
                pad_size = 0;
            }

            ret_text = rem_text + new string(pad_char, pad_size) + seq_num;
            return ret_text;
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
                    if (cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf('"') >= 0)
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
                    ur_stp.Append(cur_value.Replace("\"", "\"\""));
                }
            }

            ur_stp.AppendLine();
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static string Format_Table(System.Collections.Generic.List<string> ur_field_data, bool ur_quoted = true, char ur_separator = ',')
        {
            var stm_table_row = new System.Text.StringBuilder();
            Mx.bitbase_fn.Format_Table(stm_table_row, ur_field_data, ur_quoted, ur_separator);
            return stm_table_row.ToString();
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static string KeyRecase(string ur_key, IEnumerable<string> ur_list)
        {
            var ret_result = ur_key;
            foreach (var entry in ur_list)
            {
                if (string.Equals(ur_key, entry, StringComparison.CurrentCultureIgnoreCase))
                {
                    ret_result = entry;
                    break;
                }
            }

            return ret_result;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<(int ROWSEQ, bool is_first, bool is_last)> ntseq(int ur_count)
        {
            var ret_list = new System.Collections.Generic.List<(int ROWSEQ, bool is_first, bool is_last)>();
            for (var ROWSEQ = 0; ROWSEQ < ur_count; ROWSEQ++)
            {
                ret_list.Add((ROWSEQ, ROWSEQ == 0, ROWSEQ == ur_count - 1));
            }

            return ret_list;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<(int ROWSEQ, string text, bool is_first)> nvp(System.Collections.Generic.IEnumerable<string> ur_list)
        {
            var ret_list = new System.Collections.Generic.List<(int ROWSEQ, string text, bool is_first)>();
            var ROWSEQ = -1;
            foreach (var text in ur_list)
            {
                ROWSEQ++;
                ret_list.Add((ROWSEQ, text, ROWSEQ == 0));
            }

            return ret_list;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static System.Collections.Generic.List<(int ROWSEQ, string text, bool is_first, bool is_last)> nvtp(System.Collections.Generic.IEnumerable<string> ur_list)
        {
            var ret_list = new System.Collections.Generic.List<(int ROWSEQ, string text, bool is_first, bool is_last)>();
            var ROWSEQ = -1;
            var max_seq = -1;
            foreach (var text in ur_list)
            {
                max_seq++;
            }

            foreach (var text in ur_list)
            {
                ROWSEQ++;
                ret_list.Add((ROWSEQ, text, ROWSEQ == 0, ROWSEQ == max_seq));
            }

            return ret_list;
        }
        
        [System.Diagnostics.DebuggerStepThrough]
        public static string SqlLiteralQuoted(string ur_val)
        {
            return string.Format("{0}'{1}'", "N", ur_val.Replace("'", "''"));
        }
    }

    public class bitbase<T> where T : bitbase, new()
    {
        private System.Collections.Generic.List<string> class_name_list { get; set; }
        private System.Collections.Generic.List<T> class_prop_list { get; set; }
        private row_names row_names_list { get; set; }
        private T parent_prop { get; set; }

        public static bitlist Create()
        {
            return new bitlist(new T());
        }
        public class bitlist : bitbase<T>
        {
            [System.Diagnostics.DebuggerStepThrough]
            public bitlist(T ur_parent_prop)
            {
                this.parent_prop = ur_parent_prop;
                this.class_name_list = new System.Collections.Generic.List<string>();
                this.class_prop_list = new System.Collections.Generic.List<T>();
                var cur_class = typeof(T);
                var propa = cur_class.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var PRPSEQ = -1;
                foreach (var entry in propa)
                {
                    if (entry.PropertyType.Name == cur_class.Name)
                    {
                        PRPSEQ += 1;
                        var bitbase_prop = new T();
                        entry.SetValue(ur_parent_prop, bitbase_prop);
                        bitbase_prop.pname = entry.Name;
                        bitbase_prop.seq = PRPSEQ;
                        this.class_name_list.Add(bitbase_prop.pname);
                        this.class_prop_list.Add(bitbase_prop);
                    }
                }

                if (PRPSEQ == -1)
                {
                    throw new System.Exception("bitbase class must have same-type subclasses on " + cur_class.Name);
                }
            }

            public T r
            {
                get { return this.parent_prop; }
            }

            [System.Diagnostics.DebuggerStepThrough]
            public row_names RowNames()
            {
                if (this.row_names_list == null)
                {
                    this.row_names_list = new row_names(this);
                }

                return this.row_names_list;
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public System.Collections.Generic.List<T> RefKeys()
        {
            return class_prop_list;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public System.Collections.Generic.List<T> RefKeySearch(string ur_prop_name, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
        {
            var ret_bitbase = new System.Collections.Generic.List<T>();
            foreach (var entry in class_prop_list)
            {
                if (string.Equals(entry.pname, ur_prop_name, ur_filter_type))
                {
                    ret_bitbase.Add(entry);
                    break;
                }
            }

            return ret_bitbase;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public T RefKeySearch(string ur_prop_name, T ur_default, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
        {
            var found_key = ur_default;
            foreach (var entry in class_prop_list)
            {
                if (string.Equals(entry.pname, ur_prop_name, ur_filter_type))
                {
                    found_key = entry;
                    break;
                }
            }

            return found_key;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public System.Collections.Generic.List<string> RefNames()
        {
            return class_name_list;
        }


        [System.Diagnostics.DebuggerStepThrough]
        public class Map<T_to> where T_to : bitbase, new()
        {
            public static System.Collections.Generic.Dictionary<T, T_to> KeyEntries(Mx.bitbase<T>.bitlist ur_key_from, Mx.bitbase<T_to>.bitlist ur_key_to,
                StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_list = new System.Collections.Generic.Dictionary<T, T_to>();
                foreach (var col_key in ur_key_from.RefKeys())
                {
                    foreach (var map_key in ur_key_to.RefKeys())
                    {
                        if (string.Equals(map_key.pname, col_key.pname, ur_filter_type)
                            )
                        {
                            ret_list.Add(col_key, map_key);
                            break;
                        }
                    }
                }

                return ret_list;
            }
        }

        public class dt_enum
        {
            public bitbase<T>.bitlist bitlist;
            public T r;
            public System.Data.DataTable record_collection;
            public int row_seq;
            public System.Collections.Generic.Dictionary<T, int> col_lookup;
            [System.Diagnostics.DebuggerStepThrough]
            public dt_enum(bitbase<T>.bitlist ur_bitlist, System.Data.DataTable ur_record_collection, int ur_row_seq, System.Collections.Generic.Dictionary<T, int> ur_col_lookup)
            {
                this.bitlist = ur_bitlist;
                this.r = ur_bitlist.r;
                this.record_collection = ur_record_collection;
                this.row_seq = ur_row_seq;
                this.col_lookup = ur_col_lookup;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public T[] CompileColumnArray(params T[] ur_col_list)
            {
                return ur_col_list;
            }

            public string this[T ur_key]
            {
                [System.Diagnostics.DebuggerStepThrough]
                get
                {
                    var col_seq = this.col_lookup[ur_key];
                    var ret_val = "";
                    if (col_seq >= 0)
                    {
                        ret_val = (this.record_collection.Rows[row_seq][col_seq] ?? "").ToString();
                    }

                    return ret_val;
                }
                [System.Diagnostics.DebuggerStepThrough]
                set
                {
                    var col_seq = this.col_lookup[ur_key];
                    if (col_seq >= 0)
                    {
                        this.record_collection.Rows[row_seq][col_seq] = value;
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void AssignDate(T ur_key, string ur_date)
            {
                var col_seq = this.col_lookup[ur_key];
                if (col_seq >= 0)
                {
                    var date_val = DateTime.MinValue;
                    var s_date = ur_date;
                        if (DateTime.TryParse(s_date, out date_val))
                        {
                            this.record_collection.Rows[row_seq][col_seq] = date_val;
                        }
                        else
                        {
                            this.record_collection.Rows[row_seq][col_seq] = System.DBNull.Value;
                        }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void AssignDate(T ur_key, DateTime ur_date)
            {
                var col_seq = this.col_lookup[ur_key];
                if (col_seq >= 0)
                {
                    if (ur_date != DateTime.MinValue)
                    {
                        this.record_collection.Rows[row_seq][col_seq] = ur_date;
                    }
                    else
                    {
                        this.record_collection.Rows[row_seq][col_seq] = System.DBNull.Value;
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void AssignDecimal(T ur_key, string ur_dec)
            {
                var col_seq = this.col_lookup[ur_key];
                if (col_seq >= 0)
                {
                    Decimal dec_val = 0;
                    var s_dec = ur_dec;
                    if (Decimal.TryParse(s_dec, out dec_val))
                    {
                        this.record_collection.Rows[row_seq][col_seq] = dec_val;
                    }
                    else
                    {
                        this.record_collection.Rows[row_seq][col_seq] = System.DBNull.Value;
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void AssignDecimal(T ur_key, Decimal ur_dec)
            {
                var col_seq = this.col_lookup[ur_key];
                if (col_seq >= 0)
                {
                    this.record_collection.Rows[row_seq][col_seq] = ur_dec;
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string ColName(T ur_key_val)
            {
                return ur_key_val.pname;
            }
            public DecimalEnum dec
            {
                [System.Diagnostics.DebuggerStepThrough]
                get
                {
                    return new DecimalEnum(this);
                }
            }
            public class DecimalEnum
            {
                public dt_enum parent_row;
                [System.Diagnostics.DebuggerStepThrough]
                public DecimalEnum(dt_enum ur_parent_row)
                {
                    this.parent_row = ur_parent_row;
                }

                public Decimal this[T ur_key]
                {
                    [System.Diagnostics.DebuggerStepThrough]
                    get
                    {
                        return parent_row.GetDecimal(ur_key);
                    }
                    [System.Diagnostics.DebuggerStepThrough]
                    set
                    {
                        this.parent_row.AssignDecimal(ur_key, value);
                    }
                }
            }
            public DateEnum date
            {
                [System.Diagnostics.DebuggerStepThrough]
                get
                {
                    return new DateEnum(this);
                }
            }

            public class DateEnum
            {
                public dt_enum parent_row;
                [System.Diagnostics.DebuggerStepThrough]
                public DateEnum(dt_enum ur_parent_row)
                {
                    this.parent_row = ur_parent_row;
                }

                public DateTime this[T ur_key]
                {
                    [System.Diagnostics.DebuggerStepThrough]
                    get
                    {
                        return parent_row.GetDate(ur_key);
                    }
                    [System.Diagnostics.DebuggerStepThrough]
                    set
                    {
                        this.parent_row.AssignDate(ur_key, value);
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void ExpandKeyByLength(int ur_field_length, T ur_key_col, T ur_exp_col = null)
            {
                var dest_col = ur_exp_col;
                if (dest_col == null)
                {
                    dest_col = ur_key_col;
                }

                this[dest_col] = Mx.bitbase_fn.ExpandKeyByLength(ur_field_length, this[ur_key_col]);
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
                var stra_col = this.bitlist.RefNames();
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
                            if (cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf('"') >= 0)
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
                            ur_stp.Append(cur_value.Replace("\"", "\"\""));
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
                        if (cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf('"') >= 0)
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
                        ur_stp.Append(cur_value.Replace("\"", "\"\""));
                    }
                }

                ur_stp.AppendLine();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public DateTime GetDate(T ur_col)
            {
                var s_date = this[ur_col];
                var date_val = DateTime.MinValue;
                    System.DateTime.TryParse(s_date, out date_val);

                return date_val;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public DateTime? GetDateOrNull(T ur_col)
            {
                var ret_datetime = new DateTime?();
                var s_date = this[ur_col];
                var date_val = DateTime.MinValue;
                    if (System.DateTime.TryParse(s_date, out date_val))
                    {
                        ret_datetime = date_val;
                    }

                return ret_datetime;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public decimal GetDecimal(T ur_col)
            {
                var s_qty = this[ur_col];
                decimal d_qty = 0;
                Decimal.TryParse(s_qty, out d_qty);
                return d_qty;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public Decimal? GetDecimalOrNull(T ur_col)
            {
                var ret_qty = new Decimal?();
                decimal d_qty = 0;
                var s_qty = this[ur_col];
                if (Decimal.TryParse(s_qty, out d_qty) == false)
                {
                    ret_qty = d_qty;
                }

                return ret_qty;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string JoinEquals(T ur_col_name, int ur_parm_seq)
            {
                return ur_col_name.pname + " = {" + ur_parm_seq.ToString() + "}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<T> RefKeys()
            {
                return this.bitlist.RefKeys();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_names RowNames()
            {
                return this.bitlist.RowNames();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteral(T ur_key_val)
            {
                return SqlLiteralText(this[ur_key_val]);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralEquals(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "=", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralLike(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "LIKE", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralOper(T ur_col_name, string ur_oper, string ur_value)
            {
                return $"{ur_col_name.pname} {ur_oper} {SqlLiteralText(ur_value)}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralText(string ur_val)
            {
                return Mx.bitbase_fn.SqlLiteralQuoted(ur_val);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string v_enm(int ur_index)
            {
                var class_prop_list = this.bitlist.RefKeys();
                if (ur_index >= class_prop_list.Count)
                    return "";
                else
                    return this[class_prop_list[ur_index]];
            }
        }
        public class dt_rows : System.Collections.Generic.List<dt_enum>
        {
            public bitbase<T>.bitlist bitlist;
            public T r;
            public System.Data.DataTable record_collection;
            public System.Collections.Generic.Dictionary<T, int> col_lookup;
            [System.Diagnostics.DebuggerStepThrough]
            public dt_rows(bitbase<T>.bitlist ur_bitlist)
            {
                this.bitlist = ur_bitlist;
                this.r = ur_bitlist.r;
                var new_table = new System.Data.DataTable();
                foreach (var col_key in ur_bitlist.RowNames())
                {
                    new_table.Columns.Add(col_key.Value, typeof(string));
                }

                this.WrapResponse(new_table);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public dt_rows(bitbase<T>.bitlist ur_bitlist, System.Data.DataTable ur_record_collection)
            {
                this.bitlist = ur_bitlist;
                this.r = ur_bitlist.r;
                if (ur_record_collection != null)
                {
                    this.WrapResponse(ur_record_collection);
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void Capitalize(params T[] ur_col_list)
            {
                foreach (var row in this)
                {
                    foreach (var col_entry in ur_col_list)
                    {
                        row[col_entry] = row[col_entry].ToUpper();
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void CapitalizeFrom(T ur_col, IEnumerable<string> ur_list)
            {
                foreach (var kvp_entry in this.GroupByCol(ur_col))
                {
                    foreach (var col_entry in ur_list)
                    {
                        if (string.Equals(kvp_entry.Key, col_entry, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var row in kvp_entry.Value)
                            {
                                row[ur_col] = kvp_entry.Key;
                            }

                            break;
                        }
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string ColName(T ur_key_val)
            {
                return ur_key_val.pname;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public T[] CompileColumnArray(params T[] ur_col_list)
            {
                return ur_col_list;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public bool ContainsKey(T ur_key)
            {
                var ret_test = false;
                if (this.col_lookup != null)
                {
                    ret_test = (this.col_lookup[ur_key] >= 0);
                }

                return ret_test;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<string> DistinctCol(T ur_col)
            {
                var ret_table = new System.Collections.Generic.List<string>();
                foreach (var row_entry in this)
                {
                    var key_val = Mx.bitbase_fn.KeyRecase(row_entry[ur_col], ret_table);
                    if (ret_table.Contains(key_val) == false)
                    {
                        ret_table.Add(key_val);
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void ExpandKeyByLength(int ur_field_length, T ur_key_col, T ur_exp_col = null)
            {
                var dest_col = ur_exp_col;
                if (dest_col == null)
                {
                    dest_col = ur_key_col;
                }

                foreach (var row in this)
                {
                    row[dest_col] = Mx.bitbase_fn.ExpandKeyByLength(ur_field_length, row[ur_key_col]);
                }
            }

            [System.Diagnostics.DebuggerStepThrough]
            public dt_rows Filter(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_table = new dt_rows(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_table.Add(row_entry);
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.Dictionary<row_enum, dt_rows> FilterDistinct(params T[] ur_col_list)
            {
                var ret_table = new System.Collections.Generic.Dictionary<row_enum, dt_rows>();
                var track_unique = new System.Collections.Generic.Dictionary<string, row_enum>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), track_unique.Keys);
                    if (track_unique.ContainsKey(str_keyrow) == false)
                    {
                        track_unique.Add(str_keyrow, key_row);
                        ret_table.Add(key_row, new dt_rows(this.bitlist));
                    }
                    else
                    {
                        key_row = track_unique[str_keyrow];
                    }

                    ret_table[key_row].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public FilterRef FilterDistinct(dt_rows ur_collection, string ur_string_format, params T[] ur_col_list)
            {
                var ret_table = new FilterRef();
                ret_table.key_list = ur_col_list;
                var group_rec = new System.Collections.Generic.Dictionary<row_enum, dt_rows>();
                ret_table.kvp = group_rec;
                var track_unique = new System.Collections.Generic.Dictionary<string, row_enum>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), track_unique.Keys);
                    if (track_unique.ContainsKey(str_keyrow) == false)
                    {
                        track_unique.Add(str_keyrow, key_row);
                        group_rec.Add(key_row, new dt_rows(this.bitlist));
                    }
                    else
                    {
                        key_row = track_unique[str_keyrow];
                    }

                    group_rec[key_row].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public dt_rows FilterFirst()
            {
                var ret_table = new dt_rows(this.bitlist);
                foreach (var row_entry in this)
                {
                    ret_table.Add(row_entry);
                    break;
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public dt_rows FilterOne(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_table = new dt_rows(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_table.Add(row_entry);
                        break;
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public dt_enum FirstRow()
            {
                var ret_row = this.NewRow();
                foreach (var row_entry in this)
                {
                    ret_row = row_entry;
                    break;
                }

                return ret_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public dt_enum FirstRow(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_row = this.NewRow();
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_row = row_entry;
                        break;
                    }
                }

                return ret_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public dt_enum FirstRow(T ur_col, string ur_text, System.Text.StringBuilder ur_notice_message, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_row = this.NewRow();
                var found_row = false;
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_row = row_entry;
                        found_row = true;
                        break;
                    }
                }

                if (found_row == false)
                {
                    ur_notice_message.AppendLine("Could not find " + ur_text + " in " + ur_col.pname + ".");
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

            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<string, dt_rows> GroupByCol(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<string, dt_rows>();
                foreach (var row_entry in this)
                {
                    var key_val = Mx.bitbase_fn.KeyRecase(row_entry[ur_col], ret_table.Keys);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new dt_rows(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<DateTime, dt_rows> GroupByDate(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<DateTime, dt_rows>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDate(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new dt_rows(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<decimal, dt_rows> GroupByDecimal(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<decimal, dt_rows>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDecimal(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new dt_rows(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<(row_enum KeyRow, dt_rows SubRows)> GroupByDistinct(params T[] ur_col_list)
            {
                var group_rec = new System.Collections.Generic.SortedDictionary<string, (row_enum KeyRow, dt_rows SubRows)>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), group_rec.Keys);
                    if (group_rec.ContainsKey(str_keyrow) == false)
                    {
                        group_rec.Add(str_keyrow, (key_row, new dt_rows(this.bitlist)));
                    }

                    group_rec[str_keyrow].SubRows.Add(row_entry);
                }

                var list_rec = new System.Collections.Generic.List<(row_enum KeyRow, dt_rows SubRows)>();
                foreach (var kvp_entry in group_rec)
                {
                    list_rec.Add((kvp_entry.Value.KeyRow, kvp_entry.Value.SubRows));
                }

                return list_rec;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public GroupRef GroupByDistinct(string ur_string_format, params T[] ur_col_list)
            {
                var ret_table = new GroupRef();
                ret_table.key_list = ur_col_list;
                var group_rec = new System.Collections.Generic.SortedDictionary<string, (row_enum KeyRow, dt_rows SubRows)>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), group_rec.Keys);
                    if (group_rec.ContainsKey(str_keyrow) == false)
                    {
                        group_rec.Add(str_keyrow, (key_row, new dt_rows(this.bitlist)));
                    }

                    group_rec[str_keyrow].SubRows.Add(row_entry);
                }

                var list_rec = new System.Collections.Generic.List<(row_enum KeyRow, dt_rows SubRows)>();
                ret_table.kvp = list_rec;
                foreach (var kvp_entry in group_rec)
                {
                    list_rec.Add((kvp_entry.Value.KeyRow, kvp_entry.Value.SubRows));
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string JoinEquals(T ur_col_name, int ur_parm_seq)
            {
                return ur_col_name.pname + " = {" + ur_parm_seq.ToString() + "}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public dt_enum NewRow()
            {
                var blank_row = this.record_collection.NewRow();
                var ROWSEQ = this.record_collection.Rows.Count;
                this.record_collection.Rows.Add(blank_row);
                var ret_row = new dt_enum(this.bitlist, this.record_collection, ROWSEQ, col_lookup);
                this.Add(ret_row);
                return ret_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void NormalizeCapitalization(T ur_col)
            {
                var highest_capitalization = new System.Collections.Generic.List<string>();
                var groupby_col = this.GroupByCol(ur_col);
                foreach (var kvp_entry in groupby_col)
                {
                    var found_capval = false;
                    foreach (var nvk_capval in Mx.bitbase_fn.nvp(highest_capitalization))
                    {
                        if (string.Equals(kvp_entry.Key, nvk_capval.text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            found_capval = true;
                            if (string.Compare(kvp_entry.Key, nvk_capval.text) > 0)
                            {
                                highest_capitalization[nvk_capval.ROWSEQ] = kvp_entry.Key;
                            }
                        }
                    }

                    if (found_capval == false)
                    {
                        highest_capitalization.Add(kvp_entry.Key);
                    }
                }

                foreach (var kvp_entry in groupby_col)
                {
                    foreach (var col_entry in highest_capitalization)
                    {
                        if (kvp_entry.Key != col_entry &&
                            string.Equals(kvp_entry.Key, col_entry, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var row in kvp_entry.Value)
                            {
                                row[ur_col] = col_entry;
                            }

                            break;
                        }
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void Persist_Write(string ur_persist_path, bool ur_flag_quoted = false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ur_persist_path));
                using (var stmOUT_FILE = new System.IO.StreamWriter(ur_persist_path, false, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)))
                {
                    for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ++)
                    {
                        stmOUT_FILE.Write(this[ROWSEQ].Format_Table(ROWSEQ == 0, ur_flag_quoted));
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<T> RefKeys()
            {
                return this.bitlist.RefKeys();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_names RowNames()
            {
                return this.bitlist.RowNames();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<(int ROWSEQ, dt_enum cur_row, bool is_first, bool is_last)> rvp()
            {
                var ret_list = new System.Collections.Generic.List<(int ROWSEQ, dt_enum cur_row, bool is_first, bool is_last)>();
                for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ++)
                {
                    var is_first = (ROWSEQ == 0);
                    var is_last = (ROWSEQ == this.Count - 1);
                    ret_list.Add((ROWSEQ, this[ROWSEQ], is_first, is_last));
                }

                return ret_list;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralEquals(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "=", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralLike(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "LIKE", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralOper(T ur_col_name, string ur_oper, string ur_value)
            {
                return $"{ur_col_name.pname} {ur_oper} {SqlLiteralText(ur_value)}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralText(string ur_val)
            {
                return Mx.bitbase_fn.SqlLiteralQuoted(ur_val);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public decimal SumAsDecimal(T ur_col)
            {
                decimal ret_val = 0;
                foreach (var row_entry in this)
                {
                    ret_val += row_entry.GetDecimal(ur_col);
                }

                return ret_val;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public dt_rows WrapResponse(System.Data.DataTable ur_record_collection)
            {
                this.record_collection = ur_record_collection;
                this.Clear();
                this.col_lookup = new System.Collections.Generic.Dictionary<T, int>();
                var property_list = ur_record_collection.Columns;
                foreach (var entry in this.RefKeys())
                {
                    var col_seq = property_list.IndexOf(entry.pname);
                    col_lookup.Add(entry, col_seq);
                }

                for (var ROWSEQ = 0; ROWSEQ < ur_record_collection.Rows.Count; ROWSEQ++)
                {
                    this.Add(new dt_enum(this.bitlist, ur_record_collection, ROWSEQ, col_lookup));
                }

                return this;
            }

            public class FilterRef
            {
                public System.Collections.Generic.List<string> filter_stack { get; set; }
                public T[] key_list { get; set; }
                public System.Collections.Generic.Dictionary<row_enum, dt_rows> kvp { get; set; }
            }
            public class GroupRef
            {
                public System.Collections.Generic.List<string> filter_stack { get; set; }
                public T[] key_list { get; set; }
                public System.Collections.Generic.List<(row_enum KeyRow, dt_rows SubRows)> kvp { get; set; }
            }
        }

        public class row_names : row_enum
        {
            [System.Diagnostics.DebuggerStepThrough]
            public row_names(bitbase<T>.bitlist ur_bitlist) : base(ur_bitlist)
            {
                foreach (var entry in this.RefKeys())
                {
                    this[entry] = entry.pname;
                }
            }
        }

        public class row_enum : System.Collections.Generic.Dictionary<T, string>
        {
            public bitbase<T>.bitlist bitlist;
            public T r;
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum()
            {
                //must run row_enum_init
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum(bitbase<T>.bitlist ur_bitlist)
            {
                this.row_enum_init(ur_bitlist);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void row_enum_init(bitbase<T>.bitlist ur_bitlist)
            {
                this.bitlist = ur_bitlist;
                this.r = ur_bitlist.r;
                this.Clear();
                foreach (var entry in this.bitlist.RefKeys())
                {
                    this.Add(entry, "");
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void AssignDate(T ur_key, DateTime ur_date)
            {
                this[ur_key] = ur_date.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void AssignDecimal(T ur_key, Decimal ur_dec)
            {
                var col_seq = this[ur_key] = ur_dec.ToString();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum Clone()
            {
                var new_row = new row_enum(this.bitlist);
                foreach (var entry in this.RefKeys())
                {
                    new_row[entry] = this[entry];
                }

                return new_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum CloneTo(row_enum ur_new_row)
            {
                foreach (var entry in this.RefKeys())
                {
                    ur_new_row[entry] = this[entry];
                }

                return ur_new_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string ColName(T ur_key_val)
            {
                return ur_key_val.pname;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public T[] CompileColumnArray(params T[] ur_col_list)
            {
                return ur_col_list;
            }

            public DecimalEnum dec
            {
                [System.Diagnostics.DebuggerStepThrough]
                get
                {
                    return new DecimalEnum(this);
                }
            }
            public class DecimalEnum
            {
                public row_enum parent_row;
                [System.Diagnostics.DebuggerStepThrough]
                public DecimalEnum(row_enum ur_parent_row)
                {
                    this.parent_row = ur_parent_row;
                }

                public Decimal this[T ur_key]
                {
                    [System.Diagnostics.DebuggerStepThrough]
                    get
                    {
                        return parent_row.GetDecimal(ur_key);
                    }
                    set
                    {
                        parent_row.AssignDecimal(ur_key, value);
                    }
                }
            }
            public DateEnum date
            {
                [System.Diagnostics.DebuggerStepThrough]
                get
                {
                    return new DateEnum(this);
                }
            }

            public class DateEnum
            {
                public row_enum parent_row;
                [System.Diagnostics.DebuggerStepThrough]
                public DateEnum(row_enum ur_parent_row)
                {
                    this.parent_row = ur_parent_row;
                }

                public DateTime this[T ur_key]
                {
                    [System.Diagnostics.DebuggerStepThrough]
                    get
                    {
                        return parent_row.GetDate(ur_key);
                    }
                    [System.Diagnostics.DebuggerStepThrough]
                    set
                    {
                        parent_row.AssignDate(ur_key, value);
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void ExpandKeyByLength(int ur_field_length, T ur_key_col, T ur_exp_col = null)
            {
                var dest_col = ur_exp_col;
                if (dest_col == null)
                {
                    dest_col = ur_key_col;
                }

                this[dest_col] = Mx.bitbase_fn.ExpandKeyByLength(ur_field_length, this[ur_key_col]);
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
                var stra_col = this.bitlist.RefNames();
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
                            if (cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf('"') >= 0)
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
                            ur_stp.Append(cur_value.Replace("\"", "\"\""));
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
                        if (cur_value.IndexOf('\n') >= 0 || cur_value.IndexOf('\t') >= 0 || cur_value.IndexOf('"') >= 0)
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
                        ur_stp.Append(cur_value.Replace("\"", "\"\""));
                    }
                }

                ur_stp.AppendLine();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public DateTime GetDate(T ur_col)
            {
                var s_date = this[ur_col];
                var date_val = DateTime.MinValue;
                    System.DateTime.TryParse(s_date, out date_val);

                return date_val;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public DateTime? GetDateOrNull(T ur_col)
            {
                var ret_datetime = new DateTime?();
                var s_date = this[ur_col];
                var date_val = DateTime.MinValue;
                    if (System.DateTime.TryParse(s_date, out date_val))
                    {
                        ret_datetime = date_val;
                    }

                return ret_datetime;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public decimal GetDecimal(T ur_col)
            {
                var s_qty = this[ur_col];
                decimal d_qty = 0;
                Decimal.TryParse(s_qty, out d_qty);
                return d_qty;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public Decimal? GetDecimalOrNull(T ur_col)
            {
                var ret_qty = new Decimal?();
                decimal d_qty = 0;
                var s_qty = this[ur_col];
                if (Decimal.TryParse(s_qty, out d_qty) == false)
                {
                    ret_qty = d_qty;
                }

                return ret_qty;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string JoinEquals(T ur_col_name, int ur_parm_seq)
            {
                return ur_col_name.pname + " = {" + ur_parm_seq.ToString() + "}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<T> RefKeys()
            {
                return this.bitlist.RefKeys();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_names RowNames()
            {
                return this.bitlist.RowNames();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteral(T ur_key_val)
            {
                return SqlLiteralText(this[ur_key_val]);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralEquals(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "=", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralLike(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "LIKE", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralOper(T ur_col_name, string ur_oper, string ur_value)
            {
                return $"{ur_col_name.pname} {ur_oper} {SqlLiteralText(ur_value)}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralText(string ur_val)
            {
                return Mx.bitbase_fn.SqlLiteralQuoted(ur_val);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string v_enm(int ur_index)
            {
                var class_prop_list = this.bitlist.RefKeys();
                if (ur_index >= class_prop_list.Count)
                    return "";
                else
                    return this[class_prop_list[ur_index]];
            }
        }
        public class table_row : System.Collections.Generic.List<bitbase<T>.row_enum>
        {
            public bitbase<T>.bitlist bitlist;
            public T r;
            [System.Diagnostics.DebuggerStepThrough]
            public table_row(bitbase<T>.bitlist ur_bitlist)
            {
                this.bitlist = ur_bitlist;
                this.r = ur_bitlist.r;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void Capitalize(params T[] ur_col_list)
            {
                foreach (var row in this)
                {
                    foreach (var col_entry in ur_col_list)
                    {
                        row[col_entry] = row[col_entry].ToUpper();
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void CapitalizeFrom(T ur_col, IEnumerable<string> ur_list)
            {
                foreach (var kvp_entry in this.GroupByCol(ur_col))
                {
                    foreach (var col_entry in ur_list)
                    {
                        if (string.Equals(kvp_entry.Key, col_entry, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var row in kvp_entry.Value)
                            {
                                row[ur_col] = kvp_entry.Key;
                            }

                            break;
                        }
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row Clone()
            {
                var new_table = new table_row(this.bitlist);
                foreach (var row in this)
                {
                    new_table.Add(row.Clone());
                }

                return new_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string ColName(T ur_key_val)
            {
                return ur_key_val.pname;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public T[] CompileColumnArray(params T[] ur_col_list)
            {
                return ur_col_list;
            }
            public bool ContainsKey(T ur_key)
            {
                return this.RefKeys().Contains(ur_key);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<string> DistinctCol(T ur_col)
            {
                var ret_table = new System.Collections.Generic.List<string>();
                foreach (var row_entry in this)
                {
                    var key_val = Mx.bitbase_fn.KeyRecase(row_entry[ur_col], ret_table);
                    if (ret_table.Contains(key_val) == false)
                    {
                        ret_table.Add(key_val);
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void ExpandKeyByLength(int ur_field_length, T ur_key_col, T ur_exp_col = null)
            {
                var dest_col = ur_exp_col;
                if (dest_col == null)
                {
                    dest_col = ur_key_col;
                }

                foreach (var row in this)
                {
                    row[dest_col] = Mx.bitbase_fn.ExpandKeyByLength(ur_field_length, row[ur_key_col]);
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row Filter(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_table = new table_row(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_table.Add(row_entry);
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.Dictionary<row_enum, table_row> FilterDistinct(T[] ur_col_list)
            {
                var ret_table = new System.Collections.Generic.Dictionary<row_enum, table_row>();
                var track_unique = new System.Collections.Generic.Dictionary<string, row_enum>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), track_unique.Keys);
                    if (track_unique.ContainsKey(str_keyrow) == false)
                    {
                        track_unique.Add(str_keyrow, key_row);
                        ret_table.Add(key_row, new table_row(this.bitlist));
                    }
                    else
                    {
                        key_row = track_unique[str_keyrow];
                    }

                    ret_table[key_row].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public FilterRef FilterDistinct(table_row ur_collection, string ur_string_format, params T[] ur_col_list)
            {
                var ret_table = new FilterRef();
                ret_table.key_list = ur_col_list;
                var group_rec = new System.Collections.Generic.Dictionary<row_enum, table_row>();
                ret_table.kvp = group_rec;
                var track_unique = new System.Collections.Generic.Dictionary<string, row_enum>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), track_unique.Keys);
                    if (track_unique.ContainsKey(str_keyrow) == false)
                    {
                        track_unique.Add(str_keyrow, key_row);
                        group_rec.Add(key_row, new table_row(this.bitlist));
                    }
                    else
                    {
                        key_row = track_unique[str_keyrow];
                    }

                    group_rec[key_row].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row FilterFirst()
            {
                var ret_table = new table_row(this.bitlist);
                foreach (var row_entry in this)
                {
                    ret_table.Add(row_entry);
                    break;
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row FilterOne(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_table = new table_row(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_table.Add(row_entry);
                        break;
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum FirstRow()
            {
                var ret_row = new row_enum(this.bitlist);
                foreach (var row_entry in this)
                {
                    ret_row = row_entry;
                    break;
                }

                return ret_row;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public row_enum FirstRow(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_row = new row_enum(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_row = row_entry;
                        break;
                    }
                }

                return ret_row;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public row_enum FirstRow(T ur_col, string ur_text, System.Text.StringBuilder ur_notice_message, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_row = new row_enum(this.bitlist);
                var found_row = false;
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_row = row_entry;
                        found_row = true;
                        break;
                    }
                }

                if (found_row == false)
                {
                    ur_notice_message.AppendLine("Could not find " + ur_text + " in " + ur_col.pname + ".");
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
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<string, table_row> GroupByCol(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<string, table_row>();
                foreach (var row_entry in this)
                {
                    var key_val = Mx.bitbase_fn.KeyRecase(row_entry[ur_col], ret_table.Keys);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<DateTime, table_row> GroupByDate(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<DateTime, table_row>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDate(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<Decimal, table_row> GroupByDecimal(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<Decimal, table_row>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDecimal(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<(row_enum KeyRow, table_row SubRows)> GroupByDistinct(params T[] ur_col_list)
            {
                var group_rec = new System.Collections.Generic.SortedDictionary<string, (row_enum KeyRow, table_row SubRows)>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), group_rec.Keys);
                    if (group_rec.ContainsKey(str_keyrow) == false)
                    {
                        group_rec.Add(str_keyrow, (key_row, new table_row(this.bitlist)));
                    }

                    group_rec[str_keyrow].SubRows.Add(row_entry);
                }

                var list_rec = new System.Collections.Generic.List<(row_enum KeyRow, table_row SubRows)>();
                foreach (var kvp_entry in group_rec)
                {
                    list_rec.Add((kvp_entry.Value.KeyRow, kvp_entry.Value.SubRows));
                }

                return list_rec;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public GroupRef GroupByDistinct(string ur_string_format, params T[] ur_col_list)
            {
                var ret_table = new GroupRef();
                ret_table.key_list = ur_col_list;
                var group_rec = new System.Collections.Generic.SortedDictionary<string, (row_enum KeyRow, table_row SubRows)>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new row_enum(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), group_rec.Keys);
                    if (group_rec.ContainsKey(str_keyrow) == false)
                    {
                        group_rec.Add(str_keyrow, (key_row, new table_row(this.bitlist)));
                    }

                    group_rec[str_keyrow].SubRows.Add(row_entry);
                }

                var list_rec = new System.Collections.Generic.List<(row_enum KeyRow, table_row SubRows)>();
                ret_table.kvp = list_rec;
                foreach (var kvp_entry in group_rec)
                {
                    list_rec.Add((kvp_entry.Value.KeyRow, kvp_entry.Value.SubRows));
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string JoinEquals(T ur_col_name, int ur_parm_seq)
            {
                return ur_col_name.pname + " = {" + ur_parm_seq.ToString() + "}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_enum NewRow()
            {
                var ret_row = new row_enum(this.bitlist);
                this.Add(ret_row);
                return ret_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void NormalizeCapitalization(T ur_col)
            {
                var highest_capitalization = new System.Collections.Generic.List<string>();
                var groupby_col = this.GroupByCol(ur_col);
                foreach (var kvp_entry in groupby_col)
                {
                    var found_capval = false;
                    foreach (var nvk_capval in Mx.bitbase_fn.nvp(highest_capitalization))
                    {
                        if (string.Equals(kvp_entry.Key, nvk_capval.text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            found_capval = true;
                            if (string.Compare(kvp_entry.Key, nvk_capval.text) > 0)
                            {
                                highest_capitalization[nvk_capval.ROWSEQ] = kvp_entry.Key;
                            }
                        }
                    }

                    if (found_capval == false)
                    {
                        highest_capitalization.Add(kvp_entry.Key);
                    }
                }

                foreach (var kvp_entry in groupby_col)
                {
                    foreach (var col_entry in highest_capitalization)
                    {
                        if (kvp_entry.Key != col_entry &&
                            string.Equals(kvp_entry.Key, col_entry, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var row in kvp_entry.Value)
                            {
                                row[ur_col] = col_entry;
                            }

                            break;
                        }
                    }
                }
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
                                var new_row = new row_enum(this.bitlist);
                                if (cur_line.StartsWith("\n"))
                                {
                                    cur_line = cur_line.Substring("\n".Length);
                                }

                                if (header_processed == false)
                                {
                                    header_processed = true;
                                    var any_field_found = false;
                                    foreach (var entry in bitbase_fn.CSV_Split(cur_line, '\t'))
                                    {
                                        T found_key = this.bitlist.RefKeySearch(entry, null);
                                        if (!(found_key is null))
                                        {
                                            any_field_found = true;
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
                                    var found_field_list = bitbase_fn.CSV_Split(cur_line, '\t');
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
            public void Persist_Write(string ur_persist_path, bool ur_flag_quoted = false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ur_persist_path));
                using (var stmOUT_FILE = new System.IO.StreamWriter(ur_persist_path, false, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)))
                {
                    for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ++)
                    {
                        stmOUT_FILE.Write(this[ROWSEQ].Format_Table(ROWSEQ == 0, ur_flag_quoted));
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<T> RefKeys()
            {
                return this.bitlist.RefKeys();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_names RowNames()
            {
                return this.bitlist.RowNames();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<(int ROWSEQ, row_enum cur_row, bool is_first, bool is_last)> rvp()
            {
                var ret_list = new System.Collections.Generic.List<(int ROWSEQ, row_enum cur_row, bool is_first, bool is_last)>();
                for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ++)
                {
                    var is_first = (ROWSEQ == 0);
                    var is_last = (ROWSEQ == this.Count - 1);
                    ret_list.Add((ROWSEQ, this[ROWSEQ], is_first, is_last));
                }

                return ret_list;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralEquals(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "=", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralLike(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "LIKE", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralOper(T ur_col_name, string ur_oper, string ur_value)
            {
                return $"{ur_col_name.pname} {ur_oper} {SqlLiteralText(ur_value)}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralText(string ur_val)
            {
                return Mx.bitbase_fn.SqlLiteralQuoted(ur_val);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void String_Read(string ur_table_text, bool ur_flag_overwrite_rows = true)
            {
                if (ur_flag_overwrite_rows)
                {
                    this.Clear();
                }

                if (string.IsNullOrEmpty(ur_table_text) == false)
                {
                    {
                        var header_processed = false;
                        var found_col_list = new System.Collections.Generic.List<T>();
                        var cur_line = "";
                        foreach (var read_line in ur_table_text.Split('\n'))
                        {
                            cur_line += read_line.Replace("\r", "");
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
                                var new_row = new row_enum(this.bitlist);
                                if (cur_line.StartsWith("\n"))
                                {
                                    cur_line = cur_line.Substring("\n".Length);
                                }

                                if (header_processed == false)
                                {
                                    header_processed = true;
                                    var any_field_found = false;
                                    foreach (var entry in bitbase_fn.CSV_Split(cur_line, '\t'))
                                    {
                                        T found_key = this.bitlist.RefKeySearch(entry, null);
                                        if (!(found_key is null))
                                        {
                                            any_field_found = true;
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
                                    var found_field_list = bitbase_fn.CSV_Split(cur_line, '\t');
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
            public decimal SumAsDecimal(T ur_col)
            {
                decimal ret_val = 0;
                foreach (var row_entry in this)
                {
                    ret_val += row_entry.GetDecimal(ur_col);
                }

                return ret_val;
            }

            public class FilterRef
            {
                public System.Collections.Generic.List<string> filter_stack { get; set; }
                public T[] key_list { get; set; }
                public System.Collections.Generic.Dictionary<row_enum, table_row> kvp { get; set; }
            }
            public class GroupRef
            {
                public System.Collections.Generic.List<string> filter_stack { get; set; }
                public T[] key_list { get; set; }
                public System.Collections.Generic.List<(row_enum KeyRow, table_row SubRows)> kvp { get; set; }
            }
        }

        public class table_row<RW> : System.Collections.Generic.List<RW> where RW : row_enum, new()
        {
            public bitbase<T>.bitlist bitlist;
            public T r;
            [System.Diagnostics.DebuggerStepThrough]
            public table_row(bitbase<T>.bitlist ur_bitlist)
            {
                this.bitlist = ur_bitlist;
                this.r = ur_bitlist.r;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void Capitalize(params T[] ur_col_list)
            {
                foreach (var row in this)
                {
                    foreach (var col_entry in ur_col_list)
                    {
                        row[col_entry] = row[col_entry].ToUpper();
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void CapitalizeFrom(T ur_col, IEnumerable<string> ur_list)
            {
                foreach (var kvp_entry in this.GroupByCol(ur_col))
                {
                    foreach (var col_entry in ur_list)
                    {
                        if (string.Equals(kvp_entry.Key, col_entry, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var row in kvp_entry.Value)
                            {
                                row[ur_col] = kvp_entry.Key;
                            }

                            break;
                        }
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row<RW> Clone()
            {
                var new_table = new table_row<RW>(this.bitlist);
                foreach (var row in this)
                {
                    var new_row = new RW();
                    new_row.row_enum_init(this.bitlist);
                    row.CloneTo(new_row);
                    new_table.Add(new_row);
                }

                return new_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string ColName(T ur_key_val)
            {
                return ur_key_val.pname;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public T[] CompileColumnArray(params T[] ur_col_list)
            {
                return ur_col_list;
            }
            public bool ContainsKey(T ur_key)
            {
                return this.RefKeys().Contains(ur_key);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<string> DistinctCol(T ur_col)
            {
                var ret_table = new System.Collections.Generic.List<string>();
                foreach (var row_entry in this)
                {
                    var key_val = Mx.bitbase_fn.KeyRecase(row_entry[ur_col], ret_table);
                    if (ret_table.Contains(key_val) == false)
                    {
                        ret_table.Add(key_val);
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void ExpandKeyByLength(int ur_field_length, T ur_key_col, T ur_exp_col = null)
            {
                var dest_col = ur_exp_col;
                if (dest_col == null)
                {
                    dest_col = ur_key_col;
                }

                foreach (var row in this)
                {
                    row[dest_col] = Mx.bitbase_fn.ExpandKeyByLength(ur_field_length, row[ur_key_col]);
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row<RW> Filter(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_table = new table_row<RW>(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_table.Add(row_entry);
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.Dictionary<RW, table_row<RW>> FilterDistinct(T[] ur_col_list)
            {
                var ret_table = new System.Collections.Generic.Dictionary<RW, table_row<RW>>();
                var track_unique = new System.Collections.Generic.Dictionary<string, RW>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new RW();
                    key_row.row_enum_init(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), track_unique.Keys);
                    if (track_unique.ContainsKey(str_keyrow) == false)
                    {
                        track_unique.Add(str_keyrow, key_row);
                        ret_table.Add(key_row, new table_row<RW>(this.bitlist));
                    }
                    else
                    {
                        key_row = track_unique[str_keyrow];
                    }

                    ret_table[key_row].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public FilterRef FilterDistinct(table_row<RW> ur_collection, string ur_string_format, params T[] ur_col_list)
            {
                var ret_table = new FilterRef();
                ret_table.key_list = ur_col_list;
                var group_rec = new System.Collections.Generic.Dictionary<RW, table_row<RW>>();
                ret_table.kvp = group_rec;
                var track_unique = new System.Collections.Generic.Dictionary<string, RW>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new RW();
                    key_row.row_enum_init(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), track_unique.Keys);
                    if (track_unique.ContainsKey(str_keyrow) == false)
                    {
                        track_unique.Add(str_keyrow, key_row);
                        group_rec.Add(key_row, new table_row<RW>(this.bitlist));
                    }
                    else
                    {
                        key_row = track_unique[str_keyrow];
                    }

                    group_rec[key_row].Add(row_entry);
                }


                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row<RW> FilterFirst()
            {
                var ret_table = new table_row<RW>(this.bitlist);
                foreach (var row_entry in this)
                {
                    ret_table.Add(row_entry);
                    break;
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public table_row<RW> FilterOne(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_table = new table_row<RW>(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_table.Add(row_entry);
                        break;
                    }
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public RW FirstRow()
            {
                var ret_row = new RW();
                ret_row.row_enum_init(this.bitlist);
                foreach (var row_entry in this)
                {
                    ret_row = row_entry;
                    break;
                }

                return ret_row;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public RW FirstRow(T ur_col, string ur_text, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_row = new RW();
                ret_row.row_enum_init(this.bitlist);
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_row = row_entry;
                        break;
                    }
                }

                return ret_row;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public RW FirstRow(T ur_col, string ur_text, System.Text.StringBuilder ur_notice_message, StringComparison ur_filter_type = StringComparison.CurrentCultureIgnoreCase)
            {
                var ret_row = new RW();
                ret_row.row_enum_init(this.bitlist);
                var found_row = false;
                foreach (var row_entry in this)
                {
                    if (string.Equals(row_entry[ur_col], ur_text, ur_filter_type))
                    {
                        ret_row = row_entry;
                        found_row = true;
                        break;
                    }
                }

                if (found_row == false)
                {
                    ur_notice_message.AppendLine("Could not find " + ur_text + " in " + ur_col.pname + ".");
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
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<string, table_row<RW>> GroupByCol(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<string, table_row<RW>>();
                foreach (var row_entry in this)
                {
                    var key_val = Mx.bitbase_fn.KeyRecase(row_entry[ur_col], ret_table.Keys);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row<RW>(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<DateTime, table_row<RW>> GroupByDate(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<DateTime, table_row<RW>>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDate(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row<RW>(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }

            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.SortedDictionary<Decimal, table_row<RW>> GroupByDecimal(T ur_col)
            {
                var ret_table = new System.Collections.Generic.SortedDictionary<Decimal, table_row<RW>>();
                foreach (var row_entry in this)
                {
                    var key_val = row_entry.GetDecimal(ur_col);
                    if (ret_table.ContainsKey(key_val) == false)
                    {
                        ret_table.Add(key_val, new table_row<RW>(this.bitlist));
                    }

                    ret_table[key_val].Add(row_entry);
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<(RW KeyRow, table_row<RW> SubRows)> GroupByDistinct(params T[] ur_col_list)
            {
                var group_rec = new System.Collections.Generic.SortedDictionary<string, (RW KeyRow, table_row<RW> SubRows)>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new RW();
                    key_row.row_enum_init(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), group_rec.Keys);
                    if (group_rec.ContainsKey(str_keyrow) == false)
                    {
                        group_rec.Add(str_keyrow, (key_row, new table_row<RW>(this.bitlist)));
                    }

                    group_rec[str_keyrow].SubRows.Add(row_entry);
                }

                var list_rec = new System.Collections.Generic.List<(RW KeyRow, table_row<RW> SubRows)>();
                foreach (var kvp_entry in group_rec)
                {
                    list_rec.Add((kvp_entry.Value.KeyRow, kvp_entry.Value.SubRows));
                }

                return list_rec;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public GroupRef GroupByDistinct(string ur_string_format, params T[] ur_col_list)
            {
                var ret_table = new GroupRef();
                ret_table.key_list = ur_col_list;
                var group_rec = new System.Collections.Generic.SortedDictionary<string, (RW KeyRow, table_row<RW> SubRows)>();
                foreach (var row_entry in this)
                {
                    var key_list = new System.Text.StringBuilder();
                    var key_row = new RW();
                    key_row.row_enum_init(this.bitlist);
                    foreach (var col_entry in ur_col_list)
                    {
                        if (key_list.Length > 0)
                        {
                            key_list.Append("\t");
                        }

                        key_list.Append(row_entry[col_entry]);
                        key_row[col_entry] = row_entry[col_entry];
                    }

                    var str_keyrow = Mx.bitbase_fn.KeyRecase(key_list.ToString(), group_rec.Keys);
                    if (group_rec.ContainsKey(str_keyrow) == false)
                    {
                        group_rec.Add(str_keyrow, (key_row, new table_row<RW>(this.bitlist)));
                    }

                    group_rec[str_keyrow].SubRows.Add(row_entry);
                }

                var list_rec = new System.Collections.Generic.List<(RW KeyRow, table_row<RW> SubRows)>();
                ret_table.kvp = list_rec;
                foreach (var kvp_entry in group_rec)
                {
                    list_rec.Add((kvp_entry.Value.KeyRow, kvp_entry.Value.SubRows));
                }

                return ret_table;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string JoinEquals(T ur_col_name, int ur_parm_seq)
            {
                return ur_col_name.pname + " = {" + ur_parm_seq.ToString() + "}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public RW NewRow()
            {
                var ret_row = new RW();
                ret_row.row_enum_init(this.bitlist);
                this.Add(ret_row);
                return ret_row;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void NormalizeCapitalization(T ur_col)
            {
                var highest_capitalization = new System.Collections.Generic.List<string>();
                var groupby_col = this.GroupByCol(ur_col);
                foreach (var kvp_entry in groupby_col)
                {
                    var found_capval = false;
                    foreach (var nvk_capval in Mx.bitbase_fn.nvp(highest_capitalization))
                    {
                        if (string.Equals(kvp_entry.Key, nvk_capval.text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            found_capval = true;
                            if (string.Compare(kvp_entry.Key, nvk_capval.text) > 0)
                            {
                                highest_capitalization[nvk_capval.ROWSEQ] = kvp_entry.Key;
                            }
                        }
                    }

                    if (found_capval == false)
                    {
                        highest_capitalization.Add(kvp_entry.Key);
                    }
                }

                foreach (var kvp_entry in groupby_col)
                {
                    foreach (var col_entry in highest_capitalization)
                    {
                        if (kvp_entry.Key != col_entry &&
                            string.Equals(kvp_entry.Key, col_entry, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var row in kvp_entry.Value)
                            {
                                row[ur_col] = col_entry;
                            }

                            break;
                        }
                    }
                }
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
                                var new_row = new RW();
                                new_row.row_enum_init(this.bitlist);
                                if (cur_line.StartsWith("\n"))
                                {
                                    cur_line = cur_line.Substring("\n".Length);
                                }

                                if (header_processed == false)
                                {
                                    header_processed = true;
                                    var any_field_found = false;
                                    foreach (var entry in bitbase_fn.CSV_Split(cur_line, '\t'))
                                    {
                                        T found_key = this.bitlist.RefKeySearch(entry, null);
                                        if (!(found_key is null))
                                        {
                                            any_field_found = true;
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
                                    var found_field_list = bitbase_fn.CSV_Split(cur_line, '\t');
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
            public void Persist_Write(string ur_persist_path, bool ur_flag_quoted = false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ur_persist_path));
                using (var stmOUT_FILE = new System.IO.StreamWriter(ur_persist_path, false, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)))
                {
                    for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ++)
                    {
                        stmOUT_FILE.Write(this[ROWSEQ].Format_Table(ROWSEQ == 0, ur_flag_quoted));
                    }
                }
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<T> RefKeys()
            {
                return this.bitlist.RefKeys();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public row_names RowNames()
            {
                return this.bitlist.RowNames();
            }
            [System.Diagnostics.DebuggerStepThrough]
            public System.Collections.Generic.List<(int ROWSEQ, RW cur_row, bool is_first, bool is_last)> rvp()
            {
                var ret_list = new System.Collections.Generic.List<(int ROWSEQ, RW cur_row, bool is_first, bool is_last)>();
                for (var ROWSEQ = 0; ROWSEQ < this.Count; ROWSEQ++)
                {
                    var is_first = (ROWSEQ == 0);
                    var is_last = (ROWSEQ == this.Count - 1);
                    ret_list.Add((ROWSEQ, this[ROWSEQ], is_first, is_last));
                }

                return ret_list;
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralEquals(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "=", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralLike(T ur_col_name, string ur_value)
            {
                return SqlLiteralOper(ur_col_name, "LIKE", ur_value);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralOper(T ur_col_name, string ur_oper, string ur_value)
            {
                return $"{ur_col_name.pname} {ur_oper} {SqlLiteralText(ur_value)}";
            }
            [System.Diagnostics.DebuggerStepThrough]
            public string SqlLiteralText(string ur_val)
            {
                return Mx.bitbase_fn.SqlLiteralQuoted(ur_val);
            }
            [System.Diagnostics.DebuggerStepThrough]
            public void String_Read(string ur_table_text, bool ur_flag_overwrite_rows = true)
            {
                if (ur_flag_overwrite_rows)
                {
                    this.Clear();
                }

                if (string.IsNullOrEmpty(ur_table_text) == false)
                {
                    {
                        var header_processed = false;
                        var found_col_list = new System.Collections.Generic.List<T>();
                        var cur_line = "";
                        foreach (var read_line in ur_table_text.Split('\n'))
                        {
                            cur_line += read_line.Replace("\r", "");
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
                                var new_row = new RW();
                                new_row.row_enum_init(this.bitlist);
                                if (cur_line.StartsWith("\n"))
                                {
                                    cur_line = cur_line.Substring("\n".Length);
                                }

                                if (header_processed == false)
                                {
                                    header_processed = true;
                                    var any_field_found = false;
                                    foreach (var entry in bitbase_fn.CSV_Split(cur_line, '\t'))
                                    {
                                        T found_key = this.bitlist.RefKeySearch(entry, null);
                                        if (!(found_key is null))
                                        {
                                            any_field_found = true;
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
                                    var found_field_list = bitbase_fn.CSV_Split(cur_line, '\t');
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
            public decimal SumAsDecimal(T ur_col)
            {
                decimal ret_val = 0;
                foreach (var row_entry in this)
                {
                    ret_val += row_entry.GetDecimal(ur_col);
                }

                return ret_val;
            }

            public class FilterRef
            {
                public System.Collections.Generic.List<string> filter_stack { get; set; }
                public T[] key_list { get; set; }
                public System.Collections.Generic.Dictionary<RW, table_row<RW>> kvp { get; set; }
            }
            public class GroupRef
            {
                public System.Collections.Generic.List<string> filter_stack { get; set; }
                public T[] key_list { get; set; }
                public System.Collections.Generic.List<(RW KeyRow, table_row<RW> SubRows)> kvp { get; set; }
            }
        }
    }
}
