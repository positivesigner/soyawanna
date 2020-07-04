start "" "%~dp0\MxClasses\MxClasses\VBNetScript.exe" /path=%0
exit
MxClasses\MxBaseEc12.vb
RetVal = Mx.Want.Clipboard_TextChuck_Table_errhnd
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.Clipboard_TextChuck_Table_errhnd
'            If RetVal <> "QUIT" Then MsgBox(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = "UrFolder"
'        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
'    End Class
'End Namespace 'Mx

Namespace Mx
    Public Class Want
        Public Shared Sub Clipboard_TextChuck_Table(ur_ret As Strap)
            Dim userbowl_cart = Have.UserBowl
            Dim appname_bowlname = enmUN.app_name
            Dim windows_clipboard_cart = Have.Clipboard
            Dim tempfield_cart = Have.TempField
            Dim messagebox_cart = Have.MessageBox
            Dim from_clipboard_bowlname = userbowl_cart.Apply(windows_clipboard_cart)
            tempfield_cart.Apply(from_clipboard_bowlname)
            Dim report_output_bowlname = userbowl_cart.Apply(tempfield_cart)
            Dim from_messagebox_bowlname = userbowl_cart.Apply(report_output_bowlname, appname_bowlname, messagebox_cart)
            userbowl_cart.Apply(from_messagebox_bowlname, report_output_bowlname, windows_clipboard_cart)
        End Sub 'Clipboard_TextChuck_Table

        Public Shared Function Clipboard_TextChuck_Table_errhnd() As Strap
            Dim stpRET = Strapd()
            Clipboard_TextChuck_Table_errhnd = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Call Clipboard_TextChuck_Table(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'Clipboard_TextChuck_Table_errhnd
    End Class 'Want

    Partial Public Class Have
        Partial Public Class sTempField
            Public Sub Apply(ur_from_clipboard As enmUN.zfrom_clipboard)
                Call Use.Compile_SplitRecords(Me, Have.UserBowl.SelKey(ur_from_clipboard))
            End Sub 'Apply(ur_from_clipboard
        End Class 'sTempField

        Partial Public Class sUserBowl
            Public Function Apply(ur_report_output As enmUN.zreport_output, ur_userbowl_appname As enmUN.zapp_name, ur_messagebox_cart As Have.sMessageBox) As enmUN.zfrom_messagebox
                Dim retUN = enmUN.from_messagebox
                Apply = retUN
                Dim ins_msgbox = ur_messagebox_cart.Ins(
                    New Have.rMessageBox().
                        vt(enmMB.text, Me.SelKey(ur_report_output).v(enmUB.contents)).
                        vt(enmMB.title, Me.SelKey(ur_userbowl_appname).v(enmUB.contents)),
                    MsgBoxStyle.OkCancel
                    )

                If ins_msgbox.vUserResponse = MsgBoxResult.Ok Then
                    Me.InsKey(retUN, enmUR.Ok)
                End If
            End Function 'Apply(ur_report_output

            Public Function Apply(ur_tempfield_cart As Have.sTempField) As enmUN.zreport_output
                Dim retUN = enmUN.report_output
                Apply = retUN
                Dim stpREPORT = New Strap
                For Each krw In ur_tempfield_cart.SelAll.kvp
                    If krw.Indexb1 = 1 Then
                        For Each kcl In krw.row.RefColNames.kvp
                            If kcl.Indexb1 > 1 Then
                                stpREPORT.d(",")
                            End If

                            stpREPORT.d(kcl.v)
                        Next kcl
                    End If 'krw

                    stpREPORT.dLine()
                    stpREPORT.d(krw.Indexb1).d(qs).d(krw.row.v(enmTF.code_type)).d(qs)
                    stpREPORT.d(krw.row.v(enmTF.code_text))
                Next krw

                Me.InsKey(retUN, stpREPORT.ToString)
            End Function 'Apply(ur_tempfield_cart

            Public Function Apply(ur_windows_clipboard_cart As Have.sClipboard) As enmUN.zfrom_clipboard
                Dim retUN = enmUN.from_clipboard
                Apply = retUN
                Dim ins_rec = ur_windows_clipboard_cart.InsFrom_Windows()
                Dim strFOUND_TEXT = ins_rec.v(enmCB.text)
                If HasText(strFOUND_TEXT) Then
                    Me.InsKey(retUN, strFOUND_TEXT)

                Else
                    Throw New System.Exception("Text not found on windows clipboard: Length = " & strFOUND_TEXT.Length)
                End If
            End Function 'Apply(ur_windows_clipboard_cart

            Public Function Apply(ur_from_messagebox As enmUN.zfrom_messagebox, ur_userbowl_text As enmUN.zreport_output, ur_clipboard_cart As Have.sClipboard) As enmUN.zclipboard_recdate
                Dim retKEY = enmUN.clipboard_recdate
                Apply = retKEY
                Dim trwCB_RECDATE = Me.SelKey(retKEY)
                If Have.UserBowl.SelKey(ur_from_messagebox).v_is(enmUB.contents, enmUR.Ok) Then
                    ur_clipboard_cart.Ins(
                        New Have.rClipboard().
                        vt(enmCB.text, Have.UserBowl.SelKey(ur_userbowl_text).v(enmUB.contents))
                        )

                    trwCB_RECDATE.vt(enmUB.contents, Now.ToString)
                End If
            End Function 'Apply(ur_from_messagebox
        End Class 'sUserBowl
    End Class 'Have

    Public Class Use
        Public Shared Sub Compile_SplitRecords(ur_tempfield_table As Have.sTempField, ur_text As Have.rUserBowl)
            Call prv.wRecurse_CodeSplit(ur_tempfield_table, ur_text.v(enmUB.contents))
        End Sub 'Compile_SplitRecords

        Private Class prv
            Public Class enmCONTEXT
                Inherits bitBASE
                Public Shared comment As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
                Public Shared cur_continue As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
                Public Shared cur_close As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
                Public Shared default_search As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
                Public Shared dquote As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
                Public Shared quote As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
                Public Shared text_data As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
                Public Shared wspace As enmCONTEXT = TRow(Of enmCONTEXT).glbl.NewBitBase()
            End Class 'enmCONTEXT

            Public Class enmTL
                Inherits bitBASE
                Public Shared chunk_text As enmTL = TRow(Of enmTL).glbl.NewBitBase()
                Public Shared flag_operator_found As enmTL = TRow(Of enmTL).glbl.NewBitBase()
                Public Shared char_next_sprtr As enmTL = TRow(Of enmTL).glbl.NewBitBase()
                Public Shared chunk_type As enmTL = TRow(Of enmTL).glbl.NewBitBase()
            End Class

            Public Class rTempLine
                Inherits TRow(Of enmTL)

                Public rem_text_index As Integer

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New()
                    Call MyBase.New()
                    Me.rem_text_index = 0
                End Sub

                <System.Diagnostics.DebuggerHidden()>
                Public Function vt(ur_enm As enmTL, ur_val As String) As rTempLine
                    vt = Me
                    Me.v(ur_enm) = ur_val
                End Function
            End Class 'rTempLine

            Public Class boxChunk_State
                Public gContext As enmCONTEXT
                Public gChunk_End As Integer
                Public gSplit_Char As String
                Public gStart_Index As Integer
                Public gText_Orig As String

                Public Sub New(ur_context As enmCONTEXT, ur_chunk_end As Integer, ur_split_char As String, ur_start_index As Integer, ur_text_orig As String)
                    Me.gContext = ur_context
                    Me.gChunk_End = ur_chunk_end
                    Me.gSplit_Char = ur_split_char
                    Me.gStart_Index = ur_start_index
                    Me.gText_Orig = ur_text_orig
                End Sub
            End Class 'boxChunk_State

            Public Class boxChunk_Loop
                Public gChar_Entry As Char
                Public gSkip_Context As enmCONTEXT

                Public Sub New(ur_char_entry As Char, ur_skip_context As enmCONTEXT)
                    Me.gChar_Entry = ur_char_entry
                    Me.gSkip_Context = ur_skip_context
                End Sub
            End Class 'boxChunk_Loop

            Public Class boxSplit_State
                Public gContext As enmCONTEXT
                Public gRow As rTempLine
                Public gSplit As Integer
                Public gText_Orig As String

                Public Sub New(ur_context As enmCONTEXT, ur_row As rTempLine, ur_split As Integer, ur_text_orig As String)
                    Me.gContext = ur_context
                    Me.gRow = ur_row
                    Me.gSplit = ur_split
                    Me.gText_Orig = ur_text_orig
                End Sub
            End Class 'boxSplit_State

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub gChunk_Next(ret_split_state As boxSplit_State, ur_large_text As String, ur_start_index As Integer, ur_context As enmCONTEXT)
                Dim boxCHUNK = New boxChunk_State(
                        ur_context:=ur_context,
                        ur_chunk_end:=0,
                        ur_split_char:=mt,
                        ur_start_index:=ur_start_index,
                        ur_text_orig:=ur_large_text
                        )

                If boxCHUNK.gContext Is enmCONTEXT.default_search Then
                    boxCHUNK.gContext = prvChunkContext.gChunk_Type(boxCHUNK.gText_Orig, boxCHUNK.gStart_Index)
                End If

                ret_split_state.gRow.v(enmTL.chunk_type) = "WS"
                If boxCHUNK.gContext Is enmCONTEXT.text_data Then
                    ret_split_state.gRow.v(enmTL.chunk_type) = "TX"
                End If

                For CHRCTR = boxCHUNK.gStart_Index To boxCHUNK.gText_Orig.Length
                    Dim boxLOOP = New boxChunk_Loop(
                            ur_char_entry:=boxCHUNK.gText_Orig(b0(CHRCTR)),
                            ur_skip_context:=enmCONTEXT.cur_continue
                            )

                    boxLOOP.gChar_Entry = boxCHUNK.gText_Orig(b0(CHRCTR))
                    If boxCHUNK.gContext Is enmCONTEXT.comment Then
                        boxLOOP.gSkip_Context = prvChunkContext.CommentClose(boxCHUNK.gText_Orig, boxLOOP.gChar_Entry, CHRCTR)

                    ElseIf boxCHUNK.gContext Is enmCONTEXT.dquote OrElse boxCHUNK.gContext Is enmCONTEXT.quote Then
                        boxLOOP.gSkip_Context = prvChunkContext.QuoteSkip(boxCHUNK.gContext, boxCHUNK.gText_Orig, boxLOOP.gChar_Entry, CHRCTR)

                    ElseIf boxCHUNK.gContext Is enmCONTEXT.text_data Then
                        boxLOOP.gSkip_Context = prvChunkContext.TDataCombine(boxCHUNK.gText_Orig, boxLOOP.gChar_Entry, CHRCTR)

                    ElseIf boxCHUNK.gContext Is enmCONTEXT.wspace Then
                        boxLOOP.gSkip_Context = prvChunkContext.WSpaceCombine(boxCHUNK.gText_Orig, boxLOOP.gChar_Entry, CHRCTR)
                    End If 'enmCONTEXT

                    If boxLOOP.gSkip_Context Is enmCONTEXT.cur_close Then
                        boxCHUNK.gChunk_End = CHRCTR
                        Exit For

                    ElseIf boxLOOP.gSkip_Context IsNot enmCONTEXT.cur_continue AndAlso
                          boxLOOP.gSkip_Context IsNot enmCONTEXT.text_data AndAlso
                          boxLOOP.gSkip_Context IsNot enmCONTEXT.wspace Then
                        CHRCTR = gChunk_Skip(boxCHUNK.gText_Orig, CHRCTR, boxLOOP.gSkip_Context)
                    End If 'boxLOOP
                Next CHRCTR

                If boxCHUNK.gChunk_End = 0 Then
                    ret_split_state.gRow.v(enmTL.chunk_text) = Mid(boxCHUNK.gText_Orig, boxCHUNK.gStart_Index)
                    ret_split_state.gRow.rem_text_index = boxCHUNK.gText_Orig.Length + 1

                Else
                    ret_split_state.gRow.v(enmTL.chunk_text) = Mid(boxCHUNK.gText_Orig, boxCHUNK.gStart_Index, boxCHUNK.gChunk_End - boxCHUNK.gStart_Index + 1)
                    ret_split_state.gRow.rem_text_index = boxCHUNK.gChunk_End + 1
                End If 'intCHUNK_END
            End Sub 'gChunk_Next

            Private Class prvChunkContext
                Const chrGR_THAN = ">"c
                Const chrCR = vbCr
                Const chrLF = vbLf
                Const chrLS_THAN = "<"c
                Const chrSP = Chr(32)
                Const chrQS = Chr(34)
                Const chrQT = "'"c
                Const chrTAB = vbTab
                Const lit_ls_than_excl_hyph_hyph = "<!--"

                Public Shared Function gChunk_Type(ur_large_text As String, ur_start_index As Integer) As enmCONTEXT
                    gChunk_Type = enmCONTEXT.wspace
                    Dim chrNEXT = ur_large_text(b0(ur_start_index))
                    If (
                            chrNEXT <> chrSP AndAlso
                            chrNEXT <> chrCR AndAlso
                            chrNEXT <> chrLF AndAlso
                            chrNEXT <> chrTAB
                          ) Then
                        gChunk_Type = enmCONTEXT.text_data
                    End If
                End Function 'gChunk_Type

                <System.Diagnostics.DebuggerHidden()>
                Public Shared Function CommentClose(ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As enmCONTEXT
                    CommentClose = enmCONTEXT.cur_continue
                    Select Case ur_chr_entry
                        Case chrGR_THAN
                            If ur_chrctr >= 3 AndAlso
                                  Mid(ur_large_text, ur_chrctr - 2, 3) = "-->" Then
                                CommentClose = enmCONTEXT.cur_close
                            End If
                    End Select 'ur_chr
                End Function 'CommentClose

                <System.Diagnostics.DebuggerHidden()>
                Public Shared Function QuoteSkip(ur_context As enmCONTEXT, ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As enmCONTEXT
                    QuoteSkip = enmCONTEXT.cur_continue
                    Dim objEXIT_CHAR = chrQS
                    If ur_context Is enmCONTEXT.quote Then
                        objEXIT_CHAR = chrQT
                    End If

                    Select Case ur_chr_entry
                        Case objEXIT_CHAR
                            If ur_chrctr < ur_large_text.Length Then
                                If Mid(ur_large_text, ur_chrctr + 1, 1) <> objEXIT_CHAR Then
                                    QuoteSkip = enmCONTEXT.cur_close
                                End If

                            Else 'CHRCTR
                                QuoteSkip = enmCONTEXT.cur_close
                            End If
                    End Select 'chrENTRY
                End Function 'QuoteSkip

                <System.Diagnostics.DebuggerHidden()>
                Public Shared Function TDataCombine(ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As enmCONTEXT
                    TDataCombine = enmCONTEXT.cur_continue
                    Select Case ur_chr_entry
                        Case chrSP, chrLF, chrCR, chrTAB
                            TDataCombine = enmCONTEXT.wspace

                        Case chrQS
                            TDataCombine = enmCONTEXT.dquote

                        Case chrQT
                            TDataCombine = enmCONTEXT.quote

                        Case chrLS_THAN
                            Dim strCOMMENT_START = mt
                            If ur_chrctr + 3 <= ur_large_text.Length Then
                                strCOMMENT_START = Mid(ur_large_text, ur_chrctr, 4)
                            End If

                            If strCOMMENT_START = lit_ls_than_excl_hyph_hyph Then
                                TDataCombine = enmCONTEXT.comment
                            End If

                        Case Else
                            Dim chrNEXT = chrSP
                            If ur_chrctr < ur_large_text.Length Then
                                chrNEXT = Mid(ur_large_text, ur_chrctr + 1, 1)
                                If (
                                        chrNEXT = chrSP OrElse
                                        chrNEXT = chrCR OrElse
                                        chrNEXT = chrLF OrElse
                                        chrNEXT = chrTAB
                                      ) Then
                                    TDataCombine = enmCONTEXT.cur_close
                                End If

                            Else 'ur_chrctr
                                TDataCombine = enmCONTEXT.cur_close
                            End If
                    End Select 'ur_chr_entry
                End Function 'TDataCombine

                <System.Diagnostics.DebuggerHidden()>
                Public Shared Function WSpaceCombine(ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As enmCONTEXT
                    WSpaceCombine = enmCONTEXT.cur_continue
                    Select Case ur_chr_entry
                        Case chrSP, chrLF, chrCR, chrTAB
                            Dim chrNEXT = chrSP
                            If ur_chrctr < ur_large_text.Length Then
                                chrNEXT = Mid(ur_large_text, ur_chrctr + 1, 1)
                                If (
                                        chrNEXT <> chrSP AndAlso
                                        chrNEXT <> chrCR AndAlso
                                        chrNEXT <> chrLF AndAlso
                                        chrNEXT <> chrTAB
                                      ) Then
                                    WSpaceCombine = enmCONTEXT.cur_close
                                End If

                            Else 'ur_chrctr
                                WSpaceCombine = enmCONTEXT.cur_close
                            End If

                        Case Else
                            WSpaceCombine = enmCONTEXT.text_data
                    End Select 'ur_chr_entry
                End Function 'WSpaceCombine
            End Class 'prvChunkContext

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function gChunk_Skip(ur_large_text As String, ur_char_index As Integer, ur_context As enmCONTEXT) As Integer
                Dim boxSPLIT = New boxSplit_State(
                        ur_context:=ur_context,
                        ur_row:=New rTempLine,
                        ur_split:=ur_char_index + 1,
                        ur_text_orig:=ur_large_text
                        )

                Call gChunk_Next(
                        ret_split_state:=boxSPLIT,
                        ur_large_text:=boxSPLIT.gText_Orig,
                        ur_start_index:=boxSPLIT.gSplit,
                        ur_context:=boxSPLIT.gContext
                        )

                gChunk_Skip = ur_char_index + boxSPLIT.gRow.v(enmTL.chunk_text).Length
            End Function 'gChunk_Skip

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub wRecurse_CodeSplit(ret_chunk_table As Have.sTempField, ur_source_text As String)
                Dim boxSPLIT = New boxSplit_State(
                        ur_context:=enmCONTEXT.default_search,
                        ur_row:=New rTempLine,
                        ur_split:=1,
                        ur_text_orig:=ur_source_text
                        )

                'Dim intPREV_LINECTR = -100
                'Dim dtePREV_DISP = System.DateTime.Now().AddSeconds(-10)
                While boxSPLIT.gSplit <= boxSPLIT.gText_Orig.Length
                    Call prv.gChunk_Next(
                            ret_split_state:=boxSPLIT,
                            ur_large_text:=boxSPLIT.gText_Orig,
                            ur_start_index:=boxSPLIT.gSplit,
                            ur_context:=boxSPLIT.gContext
                            )

                    Dim trwCODE = ret_chunk_table.Ins(
                            New Have.rTempField().
                            vt(enmTF.code_text, boxSPLIT.gRow.v(enmTL.chunk_text)).
                            vt(enmTF.code_type, boxSPLIT.gRow.v(enmTL.chunk_type))
                            )

                    boxSPLIT.gSplit = boxSPLIT.gRow.rem_text_index
                    boxSPLIT.gContext = enmCONTEXT.wspace
                    If boxSPLIT.gRow.v(enmTL.chunk_type) = "WS" Then
                        boxSPLIT.gContext = enmCONTEXT.text_data
                    End If
                End While 'intCUR_SPLIT
            End Sub 'wRecurse_CodeSplit
        End Class 'prv
    End Class 'Use


    Partial Public Class Have
        Private Shared tblTempField As sTempField
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.tblTempField = New sTempField
                Have.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have

    Public Class enmTF
        Inherits bitBASE
        Public Shared code_type As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared code_text As enmTF = TRow(Of enmTF).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempField() As sTempField
            Call Have.Connect()
            TempField = Have.tblTempField
        End Function

        Public Class rTempField
            Inherits TRow(Of enmTF)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmTF, ur_val As String) As rTempField
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rTempField

        Public Class sTempField
            Private ttb As Objlist(Of rTempField)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rTempField)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Del(ur_col As enmTF, ur_val As String)
                For ROWCTR = Me.ttb.Count To 1 Step -1
                    If AreEqual(Me.ttb.tr_b1(ROWCTR).v(ur_col), ur_val) Then
                        Me.ttb.RemoveAt(b0(ROWCTR))
                    End If
                Next ROWCTR
            End Sub 'Del

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rTempField) As rTempField
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            Public ReadOnly Property SelAll() As Objlist(Of rTempField)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rTempField
                If Me.ttb.Count = 0 Then
                    SelFirst = New rTempField()
                Else
                    SelFirst = Me.ttb.tr_b1(1)
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean) As String
                Dim stpRET = Strapd() : For Each kvpREC In Me.ttb.kvp
                    stpRET.d(kvpREC.row.ToString((kvpREC.Indexb1 = 1) And ur_hdr))
                Next kvpREC : ToString = stpRET
            End Function 'ToString
            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sTempField
    End Class 'TF

    Public Class enmUB
        Inherits bitBASE
        Public Shared bowl_name As enmUB = TRow(Of enmUB).glbl.NewBitBase()
        Public Shared contents As enmUB = TRow(Of enmUB).glbl.NewBitBase()
    End Class

    Public Class enmUN
        Inherits bitBASE
        Public Shared app_name As zapp_name = TRow(Of enmUN).glbl.Trbase(Of zapp_name).NewBitBase() : Public Class zapp_name : Inherits enmUN : End Class
        Public Shared clipboard_recdate As zclipboard_recdate = TRow(Of enmUN).glbl.Trbase(Of zclipboard_recdate).NewBitBase() : Public Class zclipboard_recdate : Inherits enmUN : End Class
        Public Shared cmdline_audit As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared from_clipboard As zfrom_clipboard = TRow(Of enmUN).glbl.Trbase(Of zfrom_clipboard).NewBitBase() : Public Class zfrom_clipboard : Inherits enmUN : End Class
        Public Shared from_messagebox As enmUN = TRow(Of enmUN).glbl.Trbase(Of zfrom_messagebox).NewBitBase() : Public Class zfrom_messagebox : Inherits enmUN : End Class
        Public Shared report_output As zreport_output = TRow(Of enmUN).glbl.Trbase(Of zreport_output).NewBitBase() : Public Class zreport_output : Inherits enmUN : End Class
    End Class

    Public Class enmUR
        Inherits bitBASE
        Public Shared Ok As enmUR = TRow(Of enmUR).glbl.NewBitBase()
        Public Shared Cancel As enmUR = TRow(Of enmUR).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Dim bolFIRST_INIT = (Have.tblUserBowl Is Nothing)
            Call Have.Connect()
            UserBowl = Have.tblUserBowl
            If bolFIRST_INIT Then
                Call Have.tblUserBowl.InsFrom_Application()
                'Have.tblUserBowl.InsKey(enmUN.cmdline_audit, "1")
                Call Have.tblUserBowl.Cboard_CmdlineAudit()
            End If
        End Function

        Public Class rUserBowl
            Inherits TRow(Of enmUB)

            <System.Diagnostics.DebuggerHidden()>
            Public Function v_is(ur_enm As enmUB, ur_cmp As enmUR) As Boolean
                v_is = AreEqual(Me.v(ur_enm), ur_cmp.name)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmUB, ur_val As String) As rUserBowl
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rUserBowl

        Public Class sUserBowl
            Private ttb As Objlist(Of rUserBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rUserBowl)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Cboard_CmdlineAudit()
                If HasText(Me.SelKey(enmUN.cmdline_audit).v(enmUB.contents)) Then
                    Dim strAUDIT = Me.ToString(True)
                    Dim ins_msg = Have.MessageBox.Ins(
                        New Have.rMessageBox().
                            vt(enmMB.title, Me.SelKey(enmUN.app_name).v(enmUB.contents)).
                            vt(enmMB.text, strAUDIT),
                        MsgBoxStyle.OkCancel
                        )
                    If ins_msg.vUserResponse = MsgBoxResult.Ok Then
                        Have.Clipboard.Ins(
                            New Have.rClipboard().
                            vt(enmCB.text, strAUDIT)
                            )
                    End If
                End If
            End Sub 'Cboard_CmdlineAudit

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rUserBowl) As rUserBowl
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsKey(ur_key As enmUN, ur_val As String) As rUserBowl
                Dim ret = Me.SelKey(ur_key)
                InsKey = ret
                If HasText(ret.v(enmUB.contents)) Then
                    Throw New System.Exception("Cannot insert duplicate key for key: " & ur_key.name)
                Else
                    ret.vt(enmUB.contents, ur_val)
                End If
            End Function 'InsKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsKey(ur_key As enmUN, ur_val As enmUR) As rUserBowl
                InsKey = Me.InsKey(ur_key, ur_val.name)
            End Function 'InsKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFrom_Application() As rUserBowl
                Dim ret = New rUserBowl
                InsFrom_Application = ret
                Me.InsKey(enmUN.app_name, New MxText.FileName().d(Mx.Class1.SourcePath).FileGroup)

                Dim arlCMD_RET = MxText.Cmdline_UB(Of enmUN, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, System.Environment.CommandLine)
                Me.InsKey(enmUN.cmdline_orig, qs & System.Environment.CommandLine.Replace(qs, qs & qs) & qs)
                Me.InsKey(enmUN.cmdline_table, qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(qs, qs & qs) & qs)
                For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                    Me.Ins(
                        New Have.rUserBowl().
                        vt(enmUB.bowl_name, rowFOUND.v(enmUB.bowl_name)).
                        vt(enmUB.contents, rowFOUND.v(enmUB.contents))
                        )
                Next
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmUB, ur_value As String) As sUserBowl
                Dim retUB = New sUserBowl
                Sel = retUB
                For Each rowUB In Me.ttb
                    If AreEqual(rowUB.v(ur_col), ur_value) Then
                        retUB.Ins(rowUB)
                    End If
                Next
            End Function 'Sel

            Public ReadOnly Property SelAll() As Objlist(Of rUserBowl)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelKey(ur_key As enmUN) As rUserBowl
                Dim ret As rUserBowl = Nothing
                Dim strUN = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmUB.bowl_name), strUN) Then
                        ret = row
                        Exit For
                    End If
                Next row

                If ret Is Nothing Then
                    ret = New rUserBowl
                    Me.ttb.Add(
                        ret.
                        vt(enmUB.bowl_name, ur_key.name)
                        )
                End If

                SelKey = ret
            End Function 'SelKey

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean) As String
                Dim stpRET = Strapd() : For Each kvpREC In Me.ttb.kvp
                    stpRET.d(kvpREC.row.ToString((kvpREC.Indexb1 = 1) And ur_hdr))
                Next kvpREC : ToString = stpRET
            End Function 'ToString
            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sUserBowl
    End Class 'UB, UN
End Namespace 'Mx
