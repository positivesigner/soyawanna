Option Strict On
Namespace Mx '2020m05d04a00m30 from 2020m04d29
    Partial Public Class MxText
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function ParseJSON(ur_json_data As String, ur_notice_msg As Strap) As Objlist(Of Objlist(Of json_row_entry))
            ParseJSON = prvJSON.ParseJSON(ur_json_data, ur_notice_msg)
        End Function 'ParseJSON

        Public Class json_row_entry
            Public row_seq As String
            Public key As String
            Public value As String

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_row_seq As String, ur_key As String, ur_value As String)
                Me.row_seq = ur_row_seq
                Me.key = ur_key
                Me.value = ur_value
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean, Optional ur_quoted As Boolean = True) As Strap
                Dim stpRET = New Strap
                ToString = stpRET
                If ur_hdr Then : stpRET.d("row_seq").d(vbTab).d("key").d(vbTab).d("value").dLine() : End If
                If ur_quoted Then : stpRET.d(qs).d(Me.row_seq.Replace(qs, qs & qs)).d(qs).d(vbTab).d(qs).d(Me.key.Replace(qs, qs & qs)).d(qs).d(vbTab).d(qs).d(Me.value.Replace(qs, qs & qs)).d(qs).dLine() : Else : stpRET.d(Me.row_seq).d(vbTab).d(Me.key).d(vbTab).d(Me.value) : End If : stpRET.dLine()
            End Function
        End Class 'json_row_entry

        Private Class prvJSON
            Public Class enmCHR_TYPE
                Inherits bitBASE
                'ws = space, tab, cr, lf
                Public Shared ws As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'row_open_tag = "{"
                Public Shared row_open_tag As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'row_close_tag = "}"
                Public Shared row_close_tag As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'array_open_tag = "["
                Public Shared array_open_tag As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'array_close_tag = "]"
                Public Shared array_close_tag As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'array_entry_sprtr = ","
                Public Shared array_entry_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'val_sprtr = "="
                Public Shared val_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'esc_chr = "\"
                Public Shared esc_chr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'string_delim = qs
                Public Shared string_delim As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                'string_chr = anything else
                Public Shared string_chr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
            End Class 'enmCHR_TYPE

            Public Class enmJSON_SCTN
                Inherits bitBASE
                'ws_preroot = space before root array_open_tag or root row_open_tag
                Public Shared ws_preroot As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'root_close_tag = this code stops processing characters after the matching "}" or "]" for the root "{" or "["
                Public Shared root_close_tag As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'root_array_ws = space after root array open tag "[" or after root array row separator ","
                Public Shared root_array_ws As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_ws_prekey = space after row open tag "{" and after row separator tag ","
                Public Shared kvp_ws_prekey As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_ws_preval_sprtr = space after key close qs and before "="
                Public Shared kvp_ws_preval_sprtr As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_ws_preval = space after "="
                Public Shared kvp_ws_preval As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_ws_newkey_sprtr = space after val close qs and before ","
                Public Shared kvp_ws_newkey_sprtr As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_key_in_qs = char after key open qs
                Public Shared kvp_key_in_qs As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_key_in_qs_esc = char after key open qs and another qs
                Public Shared kvp_key_in_qs_esc As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_val_in_qs = char after val open qs
                Public Shared kvp_val_in_qs As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_val_in_qs_esc = char after val open qs and another qs
                Public Shared kvp_val_in_qs_esc As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'kvp_val_no_qs = numeric char after "=" and spaces without any val open qs
                Public Shared kvp_val_no_qs As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'ignore_rem_data = return error and stop processing when invalid or missing structure encountered
                Public Shared ignore_rem_data As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'ignore_root_subarray = this code skips unnamed arrays and their string data
                Public Shared ignore_root_subarray As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'ignore_root_substring_data = this skips unnamed arrays and their string data
                Public Shared ignore_root_substring_data As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'ignore_root_substring_data_esc = this skips unnamed arrays and their string data
                Public Shared ignore_root_substring_data_esc As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'store_value_substring_data = this code leaves unnamed string data unparsed
                Public Shared store_value_rootstring_data As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'store_value_rootstring_data_esc = this code leaves unnamed string data unparsed
                Public Shared store_value_rootstring_data_esc As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'store_value_subarray = this code leaves named array data unparsed
                Public Shared store_value_subarray As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'store_value_substring_data = this code leaves named string data unparsed
                Public Shared store_value_substring_data As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
                'ignore_value_substring_data_esc = this code leaves named array data unparsed
                Public Shared store_value_substring_data_esc As enmJSON_SCTN = TRow(Of enmJSON_SCTN).glbl.NewBitBase()
            End Class

            Public Class chr_type
                Public group As enmCHR_TYPE
                Public value As String
            End Class

            Public Class kvp_entry
                Public key As Strap
                Public value As Strap

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New()
                    Me.key = Strapd()
                    Me.value = Strapd()
                End Sub

                <System.Diagnostics.DebuggerHidden()>
                Public Sub Clear()
                    Me.key.Clear()
                    Me.value.Clear()
                End Sub
            End Class 'kvp_entry

            Public Class line_pos
                Public chr_pos As Integer
                Public line_num As Integer
                Public prev_chr As Char

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New()
                    Me.chr_pos = 0
                    Me.line_num = 1
                    Me.prev_chr = s(0)
                End Sub

                <System.Diagnostics.DebuggerHidden()>
                Public Sub Adv(ur_chr As Char)
                    If prev_chr = Chr(10) Then
                        Me.chr_pos = 1
                        Me.line_num += 1

                    Else
                        Me.chr_pos += 1
                    End If 'ur_chr

                    prev_chr = ur_chr
                End Sub 'Adv
            End Class 'line_pos

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function ParseJSON(ur_json_data As String, ur_notice_msg As Strap) As Objlist(Of Objlist(Of json_row_entry))
                Dim retLIST = New Objlist(Of Objlist(Of json_row_entry))
                ParseJSON = retLIST
                Dim bolNEW_ROW = True
                Dim CHRCTR = 0
                Dim lnpCUR_OFFSET = New line_pos
                Dim sdaIGNORE_STACK = New Sdata
                Dim kvpCUR_COL = New kvp_entry
                Dim chtCUR_ENTRY = New chr_type
                Dim flgCUR_STATE = enmJSON_SCTN.ws_preroot
                Dim bolLAST_CHAR = False
                While CHRCTR < ur_json_data.Length
                    Dim chrENTRY = ur_json_data.Chars(CHRCTR)
                    chtCUR_ENTRY.value = chrENTRY
                    CHRCTR += 1
                    If CHRCTR = ur_json_data.Length Then
                        bolLAST_CHAR = True
                    End If

                    lnpCUR_OFFSET.Adv(chrENTRY)
                    If chrENTRY = s OrElse
                      chrENTRY = Chr(10) OrElse
                      chrENTRY = Chr(13) OrElse
                      chrENTRY = Chr(9) Then 'whitespace
                        chtCUR_ENTRY.group = enmCHR_TYPE.ws
                    ElseIf chrENTRY = "{" Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.row_open_tag
                    ElseIf chrENTRY = "}" Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.row_close_tag
                    ElseIf chrENTRY = "[" Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.array_open_tag
                    ElseIf chrENTRY = "]" Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.array_close_tag
                    ElseIf chrENTRY = "," Then 'array_entry_separator
                        chtCUR_ENTRY.group = enmCHR_TYPE.array_entry_sprtr
                    ElseIf chrENTRY = ":" Then 'value_separator
                        chtCUR_ENTRY.group = enmCHR_TYPE.val_sprtr
                    ElseIf chrENTRY = "\" Then 'escape_character
                        chtCUR_ENTRY.group = enmCHR_TYPE.esc_chr
                    ElseIf chrENTRY = qs Then 'string_delimiter
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_delim
                    Else 'string_data_character
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_chr
                    End If
                    'If kvpCUR_COL.value.ToString.Contains("@lit_ido_hdr_e2 NVARCHAR(MAX) =") Then
                    '    Dim abc = 1
                    'End If
                    If flgCUR_STATE Is enmJSON_SCTN.ws_preroot Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag Then
                            'assume no root array; there is only one object in the file
                            flgCUR_STATE = enmJSON_SCTN.kvp_ws_prekey
                            bolNEW_ROW = True

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'there was no row open tag, so error on the row close tag
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag Then
                            'this file has multiple objects in an unnamed root array
                            flgCUR_STATE = enmJSON_SCTN.root_array_ws

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'there was no open array tag, and there is no row data, so error on any of these options
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.root_array_ws Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag Then
                            'the root array has an object row with zero or more key-value pairs
                            flgCUR_STATE = enmJSON_SCTN.kvp_ws_prekey
                            bolNEW_ROW = True

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'the root array did not have a row open tag, so error on row close tag
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag Then
                            'this code does not support parsing sub-array entires within the root array, so skip it to look for further object row entries
                            flgCUR_STATE = enmJSON_SCTN.ignore_root_subarray
                            sdaIGNORE_STACK.d("]")

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag Then
                            'the root array did not have an array open tag, so error on array close tag
                            flgCUR_STATE = enmJSON_SCTN.root_close_tag

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr Then
                            'continue; assume the first root array entry is null

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'this code does support parsing string entries within the root array, so skip it to look for further object row entries
                            flgCUR_STATE = enmJSON_SCTN.store_value_rootstring_data
                            bolNEW_ROW = True
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_ws_prekey Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag Then
                            'a row key-value pair must start with quotes, so error on a row open tag
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'assume this row had zero key-value pairs
                            flgCUR_STATE = enmJSON_SCTN.root_array_ws

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag Then
                            'a row key-value pair must start with quotes, so error on array open tag or array close tag
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr Then
                            'assume the row's first key-value pair was null
                            flgCUR_STATE = enmJSON_SCTN.kvp_ws_prekey

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'a row key-value pair must start with quotes, so error on value separator tag or escape character tag
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'a row key-value pair must start with quotes
                            flgCUR_STATE = enmJSON_SCTN.kvp_key_in_qs
                            Call kvpCUR_COL.Clear()

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'a row key-value pair must start with quotes, so error on any non-structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_key_in_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters are contained within quotes
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'skip this escape character, and take the next charater as a literal value
                            flgCUR_STATE = enmJSON_SCTN.kvp_key_in_qs_esc

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'the row key must be quoted and followed by whitespace or the value separator ":"
                            flgCUR_STATE = enmJSON_SCTN.kvp_ws_preval_sprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'non-structural characters are contained within quotes
                            kvpCUR_COL.key.d(chrENTRY)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_key_in_qs_esc Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters are not valid escape values
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'return the literal escaped character value
                            flgCUR_STATE = prvJSON.EscapeReturn(ur_notice_msg, kvpCUR_COL, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_ws_preval_sprtr Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr Then
                            'the row key must be quoted and followed by whitespace or the value separator ":", so error on structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'the row key is followed by whitespace or the value separator ":"
                            flgCUR_STATE = enmJSON_SCTN.kvp_ws_preval

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'the row key is followed by whitespace or the value separator ":", so error on structural or non-structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_ws_preval Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'the value must be quoted or numeric, so error on structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag Then
                            'this code does not support parsing array values, store the array data unparsed
                            flgCUR_STATE = enmJSON_SCTN.store_value_subarray
                            kvpCUR_COL.value.d(chrENTRY)
                            sdaIGNORE_STACK.d("]")

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'the value must be quoted, so error on structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'the value is quoted
                            flgCUR_STATE = enmJSON_SCTN.kvp_val_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'the value is numeric
                            flgCUR_STATE = enmJSON_SCTN.kvp_val_no_qs
                            kvpCUR_COL.value.d(chrENTRY)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_val_in_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters are contained within quotes
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'skip this escape character, and take the next charater as a literal value
                            flgCUR_STATE = enmJSON_SCTN.kvp_val_in_qs_esc

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'the key-value pair is followed by a new row tag or the end-of-row tag
                            flgCUR_STATE = enmJSON_SCTN.kvp_ws_newkey_sprtr
                            bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'non-structural characters are contained within quotes
                            kvpCUR_COL.value.d(chrENTRY)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_val_in_qs_esc Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters are not valid escape codes
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'return the literal escaped character value
                            flgCUR_STATE = prvJSON.EscapeReturn(ur_notice_msg, kvpCUR_COL, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_val_no_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'a numeric value can be closed by whitespace
                            flgCUR_STATE = prvJSON.AssignValidNumeric(ur_notice_msg, kvpCUR_COL, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                            If flgCUR_STATE Is enmJSON_SCTN.kvp_ws_newkey_sprtr Then
                                bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)
                            End If 'flgCUR_STATE

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag Then
                            'structural characters cannot be used within a numeric value
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'a numeric value can be closed by end-of-row tag
                            flgCUR_STATE = prvJSON.AssignValidNumeric(ur_notice_msg, kvpCUR_COL, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                            If flgCUR_STATE Is enmJSON_SCTN.kvp_ws_newkey_sprtr Then
                                bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)
                            End If 'flgCUR_STATE

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag Then
                            'structural characters cannot be used within a numeric value
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr Then
                            'a numeric value can be closed by a new key-value pair tag
                            flgCUR_STATE = prvJSON.AssignValidNumeric(ur_notice_msg, kvpCUR_COL, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                            If flgCUR_STATE Is enmJSON_SCTN.kvp_ws_newkey_sprtr Then
                                bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)
                                flgCUR_STATE = enmJSON_SCTN.kvp_ws_prekey
                            End If 'flgCUR_STATE

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'structural characters cannot be used within a numeric value
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_ws_newkey_sprtr Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag Then
                            'a row key-value pair must be separated by a new key-value pair tag or a row close tag, so error on other structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'this row has no more key-value pairs
                            flgCUR_STATE = enmJSON_SCTN.root_array_ws

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag Then
                            'a row key-value pair must be separated by a new key-value pair tag or a row close tag, so error on other structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr Then
                            'this row can start a new key-value pair
                            flgCUR_STATE = enmJSON_SCTN.kvp_ws_prekey

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'a row key-value pair must be separated by a new key-value pair tag or a row close tag, so error on other structural or non-structural characters
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.ignore_root_subarray Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag Then
                            'ignore any sub-stuctures within a root-array's sub-array entry
                            sdaIGNORE_STACK.d("}")
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'this sub-structure was closed
                            flgCUR_STATE = prvJSON.SkipSubArray(ur_notice_msg, sdaIGNORE_STACK, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag Then
                            'ignore any sub-stuctures within a root-array's sub-array entry
                            sdaIGNORE_STACK.d("]")
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag Then
                            'this sub-structure was closed
                            flgCUR_STATE = prvJSON.SkipSubArray(ur_notice_msg, sdaIGNORE_STACK, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                            If flgCUR_STATE Is enmJSON_SCTN.root_array_ws Then
                                bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)

                            Else
                                kvpCUR_COL.value.d(chrENTRY)
                            End If

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'continue
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'ignore any sub-stuctures within a root-array's sub-array entry
                            flgCUR_STATE = enmJSON_SCTN.ignore_root_substring_data
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'continue
                            kvpCUR_COL.value.d(chrENTRY)

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.store_value_subarray Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag Then
                            'store any sub-stuctures within a value's sub-array
                            kvpCUR_COL.value.d(chrENTRY)
                            sdaIGNORE_STACK.d("}")

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag Then
                            'this sub-structure was closed
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = prvJSON.SkipSubArray(ur_notice_msg, sdaIGNORE_STACK, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                            If flgCUR_STATE Is enmJSON_SCTN.kvp_ws_newkey_sprtr Then
                                bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)
                            End If

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag Then
                            'store any sub-stuctures within a value's sub-array
                            kvpCUR_COL.value.d(chrENTRY)
                            sdaIGNORE_STACK.d("]")

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag Then
                            'this sub-structure was closed
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = prvJSON.SkipSubArray(ur_notice_msg, sdaIGNORE_STACK, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)
                            If flgCUR_STATE Is enmJSON_SCTN.kvp_ws_newkey_sprtr Then
                                bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)
                            End If

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'store any sub-stuctures within a value's sub-array
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'store any sub-stuctures within a value's sub-array
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = enmJSON_SCTN.store_value_substring_data

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.ignore_root_substring_data Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'continue
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'ignore the data while respecting the string escape pattern
                            flgCUR_STATE = enmJSON_SCTN.ignore_root_substring_data_esc
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'the root substring data may have more rows
                            flgCUR_STATE = enmJSON_SCTN.ignore_root_subarray
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'continue
                            kvpCUR_COL.value.d(chrENTRY)

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.store_value_rootstring_data Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'continue
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'ignore the data while respecting the string escape pattern
                            flgCUR_STATE = enmJSON_SCTN.store_value_rootstring_data_esc

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'the root substring data may have more rows
                            bolNEW_ROW = prvJSON.AssignKeyValue(retLIST, kvpCUR_COL, bolNEW_ROW)
                            flgCUR_STATE = enmJSON_SCTN.root_array_ws

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'continue
                            kvpCUR_COL.value.d(chrENTRY)

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.store_value_substring_data Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr Then
                            'store the data while respecting the string escape pattern
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = enmJSON_SCTN.store_value_substring_data_esc

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = enmJSON_SCTN.store_value_subarray

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)
                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.ignore_root_substring_data_esc Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'the ignored substring escape sequence was invalid
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            flgCUR_STATE = enmJSON_SCTN.ignore_root_substring_data
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'continue
                            flgCUR_STATE = enmJSON_SCTN.ignore_root_substring_data
                            kvpCUR_COL.value.d(chrENTRY)

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.store_value_rootstring_data_esc Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'the stored substring escape sequence was invalid
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = enmJSON_SCTN.store_value_rootstring_data

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            If chrENTRY = "n"c Then
                                kvpCUR_COL.value.d(vbLf)
                            Else
                                kvpCUR_COL.value.d(chrENTRY)
                            End If

                            flgCUR_STATE = enmJSON_SCTN.store_value_rootstring_data

                        End If

                    ElseIf flgCUR_STATE Is enmJSON_SCTN.store_value_substring_data_esc Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.row_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_open_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_close_tag OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.array_entry_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'the stored substring escape sequence was invalid
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.esc_chr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = enmJSON_SCTN.store_value_substring_data

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = enmJSON_SCTN.store_value_substring_data

                        End If
                    End If

                    If bolLAST_CHAR Then
                        If flgCUR_STATE Is enmJSON_SCTN.ws_preroot OrElse
                          flgCUR_STATE Is enmJSON_SCTN.root_close_tag OrElse
                          flgCUR_STATE Is enmJSON_SCTN.root_array_ws OrElse
                          flgCUR_STATE Is enmJSON_SCTN.kvp_ws_prekey OrElse
                          flgCUR_STATE Is enmJSON_SCTN.kvp_ws_preval_sprtr OrElse
                          flgCUR_STATE Is enmJSON_SCTN.kvp_ws_preval OrElse
                          flgCUR_STATE Is enmJSON_SCTN.kvp_ws_newkey_sprtr Then
                            'this code assumes the closing tags are optional and unstarted array entries or key-value pairs are null

                        ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_key_in_qs OrElse
                          flgCUR_STATE Is enmJSON_SCTN.kvp_key_in_qs_esc OrElse
                          flgCUR_STATE Is enmJSON_SCTN.kvp_val_in_qs OrElse
                          flgCUR_STATE Is enmJSON_SCTN.kvp_val_in_qs_esc Then
                            'error when a key-value pair did not complete by the end of the file
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf flgCUR_STATE Is enmJSON_SCTN.kvp_val_no_qs OrElse
                          flgCUR_STATE Is enmJSON_SCTN.ignore_root_subarray OrElse
                          flgCUR_STATE Is enmJSON_SCTN.ignore_root_substring_data OrElse
                          flgCUR_STATE Is enmJSON_SCTN.ignore_root_substring_data_esc Then
                            'this code assumes the closing tags are optional and unstarted array entries or key-value pairs are null


                        ElseIf flgCUR_STATE Is enmJSON_SCTN.store_value_subarray OrElse
                          flgCUR_STATE Is enmJSON_SCTN.store_value_substring_data OrElse
                          flgCUR_STATE Is enmJSON_SCTN.store_value_substring_data_esc Then
                            'error when a key-value pair did not complete by the end of the file
                            flgCUR_STATE = prvJSON.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        End If
                    End If

                    '\" \\ \/ \b=backspace \f=formfeed \n=lf \r=cr \t=tab \u002F \u002f (upper or lower hex characters)
                    '\uD834\uDD1E=U+1D11E (the UTF-16 surrogate pair corresponding to the code point)
                    'codepoints are no greater than 0x10FFFF
                    'The high ten bits (in the range 0x000–0x3FF) are added to 0xD800 to give the first 16-bit code unit or high surrogate (W1), which will be in the range 0xD800–0xDBFF.
                    'The low ten bits (also in the range 0x000–0x3FF) are added to 0xDC00 to give the second 16-bit code unit or low surrogate (W2), which will be in the range 0xDC00–0xDFFF.
                    'x00-xD7 are unicode code points, xD8-xDF are surrogate pairs, xE0-xFF are unicode code points

                    If flgCUR_STATE Is enmJSON_SCTN.ignore_rem_data OrElse
                      flgCUR_STATE Is enmJSON_SCTN.root_close_tag Then
                        Exit While
                    End If
                End While 'CHRCTR
            End Function 'ParseJSON

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignError(ret_message As Strap, ur_section As enmJSON_SCTN, ur_chr As chr_type, ur_line_pos As line_pos) As enmJSON_SCTN
                AssignError = enmJSON_SCTN.ignore_rem_data
                ret_message.d("Invalid").dS(ur_chr.group.name).dS("(").dS(ur_chr.value).dS(")").dS("inside of").dS(ur_section.name).dS("at position:").dS("line").dS(ur_line_pos.line_num.ToString).dS("character").dS(ur_line_pos.chr_pos.ToString)
            End Function 'AssignError

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignKeyValue(ur_list As Objlist(Of Objlist(Of json_row_entry)), ur_kvp As kvp_entry, ur_flag_new_row As Boolean) As Boolean
                AssignKeyValue = True
                If HasText(ur_kvp.key) OrElse
                  HasText(ur_kvp.value) Then
                    AssignKeyValue = False
                    Dim trwNEW = New json_row_entry((ur_list.Count + 1).ToString, ur_kvp.key, ur_kvp.value)
                    If ur_flag_new_row Then
                        ur_list.Add(New Objlist(Of json_row_entry))
                    End If

                    ur_list.tr_b1(ur_list.Count).Add(trwNEW)
                End If

                Call ur_kvp.Clear()
            End Function 'AssignKeyValue

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignValidNumeric(ret_message As Strap, ur_kvp As kvp_entry, ur_section As enmJSON_SCTN, ur_chr As chr_type, ur_line_pos As line_pos) As enmJSON_SCTN
                AssignValidNumeric = enmJSON_SCTN.kvp_ws_newkey_sprtr
                Dim strVALUE = ur_kvp.value.ToString
                Select Case strVALUE
                    Case "true", "false", "null"
                    Case Else
                        Dim bolVALID_NUMBER = True
                        For Each chrENTRY In strVALUE.ToCharArray
                            Select Case chrENTRY
                                Case "0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "-"c, "."c
                                Case Else
                                    bolVALID_NUMBER = False
                            End Select
                        Next chrENTRY

                        If bolVALID_NUMBER = False Then
                            AssignValidNumeric = prvJSON.AssignError(ret_message, ur_section, ur_chr, ur_line_pos)
                        End If
                End Select
            End Function 'AssignValidNumeric

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function EscapeReturn(ret_message As Strap, ret_kvp As kvp_entry, ur_section As enmJSON_SCTN, ur_chr As chr_type, ur_line_pos As line_pos) As enmJSON_SCTN
                Dim stpAPPEND_CHAR As Strap = Nothing
                If ur_section Is enmJSON_SCTN.kvp_key_in_qs_esc Then
                    EscapeReturn = enmJSON_SCTN.kvp_key_in_qs
                    stpAPPEND_CHAR = ret_kvp.key
                ElseIf ur_section Is enmJSON_SCTN.kvp_val_in_qs_esc Then
                    EscapeReturn = enmJSON_SCTN.kvp_val_in_qs
                    stpAPPEND_CHAR = ret_kvp.value
                Else
                    EscapeReturn = enmJSON_SCTN.ignore_rem_data
                End If

                If stpAPPEND_CHAR IsNot Nothing Then
                    Select Case ur_chr.value
                        Case "\", qs, "/"
                            stpAPPEND_CHAR.d(ur_chr.value)
                        Case "u"
                            stpAPPEND_CHAR.d("\").d(ur_chr.value)
                        Case "b"
                            stpAPPEND_CHAR.d(Constants.vbBack)
                        Case "f"
                            stpAPPEND_CHAR.d(Constants.vbFormFeed)
                        Case "r"
                            stpAPPEND_CHAR.d(Constants.vbCr)
                        Case "n"
                            stpAPPEND_CHAR.d(Constants.vbLf)
                        Case "t"
                            stpAPPEND_CHAR.d(Constants.vbTab)
                        Case Else
                            EscapeReturn = AssignError(ret_message, ur_section, ur_chr, ur_line_pos)
                    End Select 'ur_chr
                End If 'stpAPPEND_CHAR
            End Function 'EscapeReturn

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function SkipSubArray(ret_message As Strap, ret_ignore_stack As Sdata, ur_section As enmJSON_SCTN, ur_chr As chr_type, ur_line_pos As line_pos) As enmJSON_SCTN
                SkipSubArray = ur_section
                If AreEqual(ret_ignore_stack.v_b1(ret_ignore_stack.Count), ur_chr.value) Then
                    ret_ignore_stack.RemoveAt(b0(ret_ignore_stack.Count))
                    If ret_ignore_stack.Count = 0 Then
                        If ur_section Is enmJSON_SCTN.ignore_root_subarray Then
                            SkipSubArray = enmJSON_SCTN.root_array_ws
                        ElseIf ur_section Is enmJSON_SCTN.store_value_subarray Then
                            SkipSubArray = enmJSON_SCTN.kvp_ws_newkey_sprtr
                        End If
                    End If

                Else 'ret_ignore_stack
                    SkipSubArray = prvJSON.AssignError(ret_message, ur_section, ur_chr, ur_line_pos)
                End If
            End Function 'SkipSubArray
        End Class 'prvJSON
    End Class 'TRow
End Namespace 'Mx
