Option Strict On
Namespace Mx '2020m05d09
    Partial Public Class MxText
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function ParseHTML(ur_html_data As String, ur_notice_msg As Strap, Optional ur_filter As Object = Nothing) As Objlist(Of html_tag_entry)
            Dim lstRET = New Objlist(Of html_tag_entry)
            ParseHTML = lstRET
            Dim fnFILTER As ITagFilter = Nothing
            If ur_filter IsNot Nothing Then
                For Each objTYPE In ur_filter.GetType.GetInterfaces
                    If objTYPE Is GetType(ITagFilter) Then
                        fnFILTER = DirectCast(ur_filter, ITagFilter)
                        Exit For
                    End If
                Next objTYPE

                If fnFILTER Is Nothing Then
                    Throw New System.Exception("Filter object does not implement ITagFilter interface")
                End If
            End If

            If fnFILTER Is Nothing Then
                fnFILTER = New prvHTML.TagFilter(lstRET)
            End If

            Call prvHTML.ParseHTML(ur_html_data, ur_notice_msg, fnFILTER)
        End Function 'ParseHTML

        Interface ITagFilter
            Sub Add(ur_entry As html_tag_entry)
        End Interface

        Public Class html_tag_entry
            Public key As String
            Public value As String

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_key As String, ur_value As String)
                Me.key = ur_key
                Me.value = ur_value
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean, Optional ur_quoted As Boolean = True) As Strap
                Dim stpRET = New Strap
                ToString = stpRET
                If ur_hdr Then : stpRET.d("key").d(vbTab).d("value").dLine() : End If
                If ur_quoted Then : stpRET.d(qs).d(Me.key.Replace(qs, qs & qs)).d(qs).d(vbTab).d(qs).d(Me.value.Replace(qs, qs & qs)).d(qs).dLine() : Else : stpRET.d(Me.key).d(vbTab).d(Me.value) : End If : stpRET.dLine()
            End Function
        End Class 'html_tag_entry

        Private Class prvHTML
            Public Class TagFilter
                Implements ITagFilter

                Private prvROW_LIST As Objlist(Of html_tag_entry)

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New(ur_list As Objlist(Of html_tag_entry))
                    Me.prvROW_LIST = ur_list
                End Sub 'New

                <System.Diagnostics.DebuggerHidden()>
                Public Sub Add(ur_entry As html_tag_entry) Implements ITagFilter.Add
                    Me.prvROW_LIST.Add(ur_entry)
                End Sub 'Add
            End Class 'TagFilter


            Public Class enmCHR_TYPE
                Inherits bitBASE
                Public Shared ws As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared opentag_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared endingtag_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared closetag_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared comment_name_start As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared comment_name_cont As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_delim_qs As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_delim_qt As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_chr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
            End Class 'enmCHR_TYPE

            Public Class enmFILE_SCTN
                Inherits bitBASE
                Public Shared ws_pretag As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- space before first open tag "<" '- -<html>
                Public Shared tag_name As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon open tag "<" and name not starting like a comment "!--", include chars until first whitespace '-<-html_e1 attr_e1="val_e1">
                Public Shared attr_text As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag name space, include chars until unquoted empty tag "/" or close tag ">" '<html_e1- -attr_e1="val_e1">
                '- upon empty tag "/", include chars until close tag ">" '<html_e1-/->
                Public Shared closetag_name As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag name "/", include chars until close tag ">" '<html_e1>text_e1<-/-html_e1>
                Public Shared plain_text As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon close tag ">", include chars until open tag "<" '<html_e1->-text_e1</html_e1>
                Public Shared comment_plain_or_tag_name As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon open tag "<", split out comment tag name starting with "!--" or plain text starting with whitespace
                Public Shared comment_or_tag_name2 As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon open tag "<", split out comment tag name starting with "!--" 
                Public Shared comment_or_tag_name3 As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon open tag "<", split out comment tag name starting with "!--" 
                Public Shared comment_open_tag As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon comment tag name "!--", include hyph chars "-" until first non-hyph char
                Public Shared comment_text As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon comment tag name "!--", include chars until comment ending "-->"
                Public Shared comment_text_or_close As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon comment name cont "-", split out comment ending with "-->"
                Public Shared comment_text_or_close2 As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon comment name cont "-", split out comment ending with "-->"
                Public Shared attr_text_in_qs As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag text quotes ("), include chars and structural characters until quotes (quoted quotes uses ampersand escape code) '<html_e1 attr_e1="val_e1<&quot;cont_e1">
                Public Shared attr_text_in_qt As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag text s-quote ('), include chars and structural characters until s-quote (quoted s-quote uses ampersand escape code) '<html_e1 attr_e1='val_e1<&#39;cont_e1'>
                Public Shared ignore_rem_data As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
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
            Public Shared Sub ParseHTML(ur_html_data As String, ur_notice_msg As Strap, ur_list As ITagFilter)
                Dim intLEN = ur_html_data.Length
                Dim CHRCTR = 0
                Dim lnpCUR_OFFSET = New line_pos
                Dim sdaIGNORE_STACK = New Sdata
                Dim kvpCUR_COL = New kvp_entry
                Dim chtCUR_ENTRY = New chr_type
                Dim flgCUR_STATE = enmFILE_SCTN.ws_pretag
                Dim bolLAST_CHAR = False
                Dim intUNNAMED_VALUE_SEQ = b0(1)
                Dim chrEXCLM = "!"c
                Dim chrFSLASH = "/"c
                Dim chrHYPH = "-"c
                Dim chrQS = qs(0)
                Dim chrQT = qt(0)
                Dim chrTAG_CLOSE = ">"c
                Dim chrTAG_OPEN = "<"c
                Dim strLIT_UNHANDLED_CHAR_GROUP = "Unhandled char-group"
                Dim strLIT_UNHANDLED_FILE_SECTION = "Unhandled file-section"

                For Each chrENTRY In ur_html_data
                    chtCUR_ENTRY.value = chrENTRY
                    CHRCTR += 1
                    If CHRCTR = intLEN Then
                        bolLAST_CHAR = True
                    End If

                    lnpCUR_OFFSET.Adv(chrENTRY)
                    If chrENTRY = s OrElse
                      chrENTRY = Chr(10) OrElse
                      chrENTRY = Chr(13) OrElse
                      chrENTRY = Chr(9) Then 'whitespace
                        chtCUR_ENTRY.group = enmCHR_TYPE.ws
                    ElseIf chrENTRY = chrTAG_OPEN Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.opentag_sprtr
                    ElseIf chrENTRY = chrFSLASH Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.endingtag_sprtr
                    ElseIf chrENTRY = chrTAG_CLOSE Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.closetag_sprtr
                    ElseIf chrENTRY = chrEXCLM Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.comment_name_start
                    ElseIf chrENTRY = chrHYPH Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.comment_name_cont
                    ElseIf chrENTRY = chrQS Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_delim_qs
                    ElseIf chrENTRY = chrQT Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_delim_qt
                    Else 'string_data_character
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_chr
                    End If

                    'If kvpCUR_COL.value.ToString = "exclmh_hyph tag" Then
                    '    Dim abc = 1
                    'End If

                    If flgCUR_STATE Is enmFILE_SCTN.ws_pretag OrElse
                      flgCUR_STATE Is enmFILE_SCTN.plain_text Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            flgCUR_STATE = enmFILE_SCTN.plain_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.comment_plain_or_tag_name

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmFILE_SCTN.plain_text
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.tag_name Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            '- upon tag name space, include chars until unquoted empty tag "/" or close tag ">" '<html_e1- -attr_e1="val_e1">
                            flgCUR_STATE = enmFILE_SCTN.attr_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr Then
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            If HasText(kvpCUR_COL.key) = False Then
                                flgCUR_STATE = enmFILE_SCTN.closetag_name
                                kvpCUR_COL.value.d(chrENTRY)

                            Else
                                flgCUR_STATE = enmFILE_SCTN.attr_text
                                kvpCUR_COL.value.d(chrENTRY)
                            End If

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.plain_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.plain_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text_in_qs
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text_in_qt
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.closetag_name Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.plain_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_plain_or_tag_name Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            flgCUR_STATE = enmFILE_SCTN.plain_text
                            kvpCUR_COL.value.d("<").d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.closetag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.plain_text
                            kvpCUR_COL.value.d("<").d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.comment_or_tag_name2
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt Then
                            flgCUR_STATE = enmFILE_SCTN.plain_text
                            kvpCUR_COL.value.d("<").d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.tag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_or_tag_name2 Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.tag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.plain_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start Then
                            flgCUR_STATE = enmFILE_SCTN.tag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            flgCUR_STATE = enmFILE_SCTN.comment_or_tag_name3
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmFILE_SCTN.tag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_or_tag_name3 Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.tag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.plain_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start Then
                            flgCUR_STATE = enmFILE_SCTN.tag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            flgCUR_STATE = enmFILE_SCTN.comment_open_tag
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmFILE_SCTN.tag_name
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_open_tag Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.plain_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            kvpCUR_COL.key.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_text Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text_or_close

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_text_or_close Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start Then
                            kvpCUR_COL.value.d("-").d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text_or_close2

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d("-").d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_text_or_close2 Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text
                            kvpCUR_COL.value.d("--").d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.plain_text

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text
                            kvpCUR_COL.value.d("--").d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmFILE_SCTN.comment_text
                            kvpCUR_COL.value.d("--").d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qt Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.opentag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.endingtag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.closetag_sprtr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_start OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.comment_name_cont OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    Else
                        flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_FILE_SECTION)
                    End If

                    If bolLAST_CHAR Then
                        If flgCUR_STATE Is enmFILE_SCTN.ws_pretag Then
                            'assume key has null value

                        ElseIf flgCUR_STATE Is enmFILE_SCTN.plain_text Then
                            Call prvHTML.AssignKeyValue(ur_list, kvpCUR_COL)

                        ElseIf flgCUR_STATE Is enmFILE_SCTN.tag_name OrElse
                          flgCUR_STATE Is enmFILE_SCTN.attr_text OrElse
                          flgCUR_STATE Is enmFILE_SCTN.closetag_name Then
                            'the open tag did not close by end of data
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf flgCUR_STATE Is enmFILE_SCTN.comment_plain_or_tag_name OrElse
                          flgCUR_STATE Is enmFILE_SCTN.comment_or_tag_name2 OrElse
                          flgCUR_STATE Is enmFILE_SCTN.comment_open_tag OrElse
                          flgCUR_STATE Is enmFILE_SCTN.comment_text OrElse
                          flgCUR_STATE Is enmFILE_SCTN.comment_text_or_close OrElse
                          flgCUR_STATE Is enmFILE_SCTN.comment_text_or_close2 Then
                            'the open tag did not close by end of data
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qs OrElse
                          flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qt Then
                            'the quoted value did not complete by end of data
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        Else
                            flgCUR_STATE = prvHTML.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_FILE_SECTION)
                        End If
                    End If

                    If flgCUR_STATE Is enmFILE_SCTN.ignore_rem_data Then
                        Exit For
                    End If
                Next chrENTRY
            End Sub 'ParseCLine

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignError(ret_message As Strap, ur_section As enmFILE_SCTN, ur_chr As chr_type, ur_line_pos As line_pos, Optional ur_error_type As String = "Invalid") As enmFILE_SCTN
                AssignError = enmFILE_SCTN.ignore_rem_data
                ret_message.d(ur_error_type).dS(ur_chr.group.name).dS("(").dS(ur_chr.value).dS(")").dS("inside of").dS(ur_section.name).dS("at position:").dS("line").dS(ur_line_pos.line_num.ToString).dS("character").dS(ur_line_pos.chr_pos.ToString)
            End Function 'AssignError

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignKeyValue(ur_list As ITagFilter, ur_kvp As kvp_entry) As Boolean
                AssignKeyValue = False
                If HasText(ur_kvp.key) OrElse
                  HasText(ur_kvp.value) Then
                    Dim trwNEW = New html_tag_entry(ur_kvp.key, ur_kvp.value)
                    ur_list.Add(trwNEW)
                End If

                Call ur_kvp.Clear()
            End Function 'AssignKeyValue
        End Class 'prvHTML
    End Class 'MxText-HTML

    Partial Public Class MxText
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function ParseAttr(ur_attrlist_data As String, ur_notice_msg As Strap, Optional ur_filter As Object = Nothing) As Objlist(Of attr_field_entry)
            Dim lstRET = New Objlist(Of attr_field_entry)
            ParseAttr = lstRET
            Dim fnFILTER As IFieldFilter = Nothing
            If ur_filter IsNot Nothing Then
                For Each objTYPE In ur_filter.GetType.GetInterfaces
                    If objTYPE Is GetType(IFieldFilter) Then
                        fnFILTER = DirectCast(ur_filter, IFieldFilter)
                        Exit For
                    End If
                Next objTYPE

                If fnFILTER Is Nothing Then
                    Throw New System.Exception("Filter object does not implement IFieldFilder interface")
                End If
            End If

            If fnFILTER Is Nothing Then
                fnFILTER = New prvATTR.FieldFilter(lstRET)
            End If

            Call prvATTR.ParseAttr(ur_attrlist_data, ur_notice_msg, fnFILTER)
        End Function 'ParseHTML

        Public Class attr_field_entry
            Public key As String
            Public value As String

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_key As String, ur_value As String)
                Me.key = ur_key
                Me.value = ur_value
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean, Optional ur_quoted As Boolean = True) As Strap
                Dim stpRET = New Strap
                ToString = stpRET
                If ur_hdr Then : stpRET.d("key").d(vbTab).d("value").dLine() : End If
                If ur_quoted Then : stpRET.d(qs).d(Me.key.Replace(qs, qs & qs)).d(qs).d(vbTab).d(qs).d(Me.value.Replace(qs, qs & qs)).d(qs).dLine() : Else : stpRET.d(Me.key).d(vbTab).d(Me.value) : End If : stpRET.dLine()
            End Function
        End Class 'attr_field_entry

        Interface IFieldFilter
            Sub Add(ur_entry As attr_field_entry)
        End Interface

        Private Class prvATTR
            Public Class FieldFilter
                Implements IFieldFilter

                Private prvROW_LIST As Objlist(Of attr_field_entry)

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New(ur_list As Objlist(Of attr_field_entry))
                    Me.prvROW_LIST = ur_list
                End Sub 'New

                <System.Diagnostics.DebuggerHidden()>
                Public Sub Add(ur_entry As attr_field_entry) Implements IFieldFilter.Add
                    Me.prvROW_LIST.Add(ur_entry)
                End Sub 'Add
            End Class 'FieldFilter

            Public Class enmCHR_TYPE
                Inherits bitBASE
                Public Shared ws As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared val_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared ending_tag_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_delim_qs As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_delim_qt As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_chr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
            End Class 'enmCHR_TYPE

            Public Class enmFILE_SCTN
                Inherits bitBASE
                Public Shared ws_pre_keyname As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- space before first key name char '- -attr_e1="val_e1"
                Public Shared key_name As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon first key name char, include chars until first whitespace '-a-ttr_e1 = "val_e1"
                '- upon first key name char, include chars until first equals '-a-ttr_e1="val_e1"
                '- exit search when empty tag "/" 'attr_e1-/-attr_e2="val_e2"
                Public Shared ws_pre_valsprtr As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag name space, include ws chars until equal sign 'attr_e1- -="val_e1"
                '- exit search when other char found 'attr_e1 -a-ttr_e2="val_e2"
                Public Shared ws_pre_valqs As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag name space, include chars until unquoted empty tag "/" or close tag ">" '<html_e1- -attr_e1="val_e1">
                Public Shared attr_text As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon unquoted attr value char, include chars until ws char 'attr_e1=val_e1- -
                '- upon unquoted attr value char, include chars until end of file 'attr_e1=val_e1- -
                Public Shared attr_text_in_qs As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag text quotes ("), include chars and structural characters until quotes 'attr_e1="val_e1>&quot;cont_e1"
                Public Shared attr_text_in_qt As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
                '- upon tag text s-quote ('), include chars and structural characters until s-quote 'attr_e1='val_e1>&#39;cont_e1'
                Public Shared ignore_rem_data As enmFILE_SCTN = TRow(Of enmFILE_SCTN).glbl.NewBitBase()
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
            Public Shared Sub ParseAttr(ur_html_data As String, ur_notice_msg As Strap, ur_list As IFieldFilter)
                Dim intLEN = ur_html_data.Length
                Dim CHRCTR = 0
                Dim lnpCUR_OFFSET = New line_pos
                Dim sdaIGNORE_STACK = New Sdata
                Dim kvpCUR_COL = New kvp_entry
                Dim chtCUR_ENTRY = New chr_type
                Dim flgCUR_STATE = enmFILE_SCTN.ws_pre_keyname
                Dim bolLAST_CHAR = False
                Dim intUNNAMED_VALUE_SEQ = b0(1)
                Dim chrEQUAL = "="c
                Dim chrFSLASH = "/"c
                Dim chrQS = qs(0)
                Dim chrQT = qt(0)
                Dim strLIT_UNHANDLED_CHAR_GROUP = "Unhandled char-group"
                Dim strLIT_UNHANDLED_FILE_SECTION = "Unhandled file-section"

                For Each chrENTRY In ur_html_data
                    chtCUR_ENTRY.value = chrENTRY
                    CHRCTR += 1
                    If CHRCTR = intLEN Then
                        bolLAST_CHAR = True
                    End If

                    lnpCUR_OFFSET.Adv(chrENTRY)
                    If chrENTRY = s OrElse
                      chrENTRY = Chr(10) OrElse
                      chrENTRY = Chr(13) OrElse
                      chrENTRY = Chr(9) Then 'whitespace
                        chtCUR_ENTRY.group = enmCHR_TYPE.ws
                    ElseIf chrENTRY = chrEQUAL Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.val_sprtr
                    ElseIf chrENTRY = chrEQUAL Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.ending_tag_sprtr
                    ElseIf chrENTRY = chrQS Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_delim_qs
                    ElseIf chrENTRY = chrQT Then
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_delim_qt
                    Else 'string_data_character
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_chr
                    End If

                    If flgCUR_STATE Is enmFILE_SCTN.ws_pre_keyname Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.ending_tag_sprtr Then
                            kvpCUR_COL.key.d(chrENTRY)
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_keyname

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmFILE_SCTN.key_name
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.key_name Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_valsprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_valqs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.ending_tag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt Then
                            kvpCUR_COL.key.d(chrENTRY)
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_keyname

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.ws_pre_valsprtr Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_valqs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.ending_tag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt Then
                            kvpCUR_COL.key.d(chrENTRY)
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_keyname

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.key_name
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.ws_pre_valqs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.ending_tag_sprtr Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text_in_qt

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmFILE_SCTN.attr_text
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_keyname

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.ending_tag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.ending_tag_sprtr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs Then
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_keyname

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qt Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.ending_tag_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qs Then
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim_qt Then
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)
                            flgCUR_STATE = enmFILE_SCTN.ws_pre_keyname

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    Else
                        flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_FILE_SECTION)
                    End If

                    If bolLAST_CHAR Then
                        If flgCUR_STATE Is enmFILE_SCTN.ws_pre_keyname Then
                            'assume key has null value

                        ElseIf flgCUR_STATE Is enmFILE_SCTN.key_name OrElse
                          flgCUR_STATE Is enmFILE_SCTN.ws_pre_valsprtr OrElse
                          flgCUR_STATE Is enmFILE_SCTN.ws_pre_valqs OrElse
                          flgCUR_STATE Is enmFILE_SCTN.attr_text Then
                            Call prvATTR.AssignKeyValue(ur_list, kvpCUR_COL)

                        ElseIf flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qs OrElse
                          flgCUR_STATE Is enmFILE_SCTN.attr_text_in_qt Then
                            'the quoted value did not complete by end of data
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        Else
                            flgCUR_STATE = prvATTR.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_FILE_SECTION)
                        End If
                    End If

                    If flgCUR_STATE Is enmFILE_SCTN.ignore_rem_data Then
                        Exit For
                    End If
                Next chrENTRY
            End Sub 'ParseAttr

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignError(ret_message As Strap, ur_section As enmFILE_SCTN, ur_chr As chr_type, ur_line_pos As line_pos, Optional ur_error_type As String = "Invalid") As enmFILE_SCTN
                AssignError = enmFILE_SCTN.ignore_rem_data
                ret_message.d(ur_error_type).dS(ur_chr.group.name).dS("(").dS(ur_chr.value).dS(")").dS("inside of").dS(ur_section.name).dS("at position:").dS("line").dS(ur_line_pos.line_num.ToString).dS("character").dS(ur_line_pos.chr_pos.ToString)
            End Function 'AssignError

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignKeyValue(ur_list As IFieldFilter, ur_kvp As kvp_entry) As Boolean
                AssignKeyValue = False
                If HasText(ur_kvp.key) OrElse
                  HasText(ur_kvp.value) Then
                    Dim trwNEW = New attr_field_entry(ur_kvp.key, ur_kvp.value)
                    ur_list.Add(trwNEW)
                End If

                Call ur_kvp.Clear()
            End Function 'AssignKeyValue
        End Class 'prvATTR
    End Class 'MxText-ATTR
End Namespace 'Mx
