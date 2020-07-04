start "" MxClasses\VBNetScript.exe /path=%0
exit
MxClasses\MxBaseEc10.vb
RetVal = Mx.Want.Clipboard_TextChuck_Table
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.Clipboard_TextChuck_Table
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
        Public Shared Function Clipboard_TextChuck_Table() As Strap
            Dim stpRET = Strapd()
            Clipboard_TextChuck_Table = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Have.InText_Search_FromGlobal(db.UserBowl, db.Clipboard)
                Use.Compile_SplitRecords(db.UserBowl.SelKey(enmUN.from_clipboard), db.TempField)
                Use.Compile_RecordReport(db.UserBowl, db.TempField)
                Use.Ask_to_UpdateClipboard(
                    ur_cboard_text:=db.UserBowl.SelKey(enmUN.to_clipboard),
                    ur_app_name:=db.UserBowl.SelKey(enmUN.app_name)
                    )

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'Clipboard_TextChuck_Table
    End Class 'Want

    Public Class Have
        Public Shared Sub InText_Search_FromGlobal(ur_bowl_table As db.sUserBowl, ur_cboard_table As db.sClipboard)
            Dim ins_rec = db.Clipboard.InsFrom_Windows()
            Dim strFOUND_TEXT = ins_rec.v(enmCB.text)
            If HasText(strFOUND_TEXT) Then
                ur_bowl_table.InsKey(enmUN.from_clipboard, strFOUND_TEXT)

            Else
                Throw New System.Exception("Text not found on clipboard: Length = " & strFOUND_TEXT.Length)
            End If
        End Sub 'InText_Search_FromGlobal
    End Class 'Have

    Public Class Use
        Public Shared Sub Ask_to_UpdateClipboard(ur_cboard_text As db.rUserBowl, ur_app_name As db.rUserBowl)
            Dim ins_msgbox = db.MessageBox.Ins(
                New db.rMessageBox().
                    vt(enmMB.text, ur_cboard_text.v(enmUB.contents)).
                    vt(enmMB.title, ur_app_name.v(enmUB.contents)),
                MsgBoxStyle.OkCancel
                )

            If ins_msgbox.vUserResponse = MsgBoxResult.Ok Then
                db.Clipboard.Ins(
                    New db.rClipboard().
                    vt(enmCB.text, ur_cboard_text.v(enmUB.contents))
                    )
            End If
        End Sub 'Ask_to_UpdateClipboard

        Public Shared Sub Compile_RecordReport(ur_bowl_table As db.sUserBowl, ur_tempfield_table As db.sTempField)
            Dim stpREPORT = New Strap
            For Each krw In ur_tempfield_table.SelAll.kvp
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

            ur_bowl_table.InsKey(enmUN.to_clipboard, stpREPORT.ToString)
        End Sub 'Compile_RecordReport


        Public Shared Sub Compile_SplitRecords(ur_text As db.rUserBowl, ur_tempfield_table As db.sTempField)
            Call prv.wRecurse_CodeSplit(ur_tempfield_table, ur_text.v(enmUB.contents))
        End Sub 'InsSplitAt_TAB

        Private Class prv
            Public Class bitCONTEXT
                Inherits bitBASE
            End Class

            Public Class enmCONTEXT
                Public Shared comment As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
                Public Shared cur_continue As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
                Public Shared cur_close As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
                Public Shared default_search As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
                Public Shared dquote As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
                Public Shared quote As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
                Public Shared text_data As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
                Public Shared wspace As bitCONTEXT = TRow(Of bitCONTEXT, enmCONTEXT).glbl.NewBitBase()
            End Class 'enmCONTEXT

            Public Class bitTL
                Inherits bitBASE
            End Class

            Public Class enmTL
                Public Shared chunk_text As bitTL = TRow(Of bitTL, enmTL).glbl.NewBitBase()
                Public Shared flag_operator_found As bitTL = TRow(Of bitTL, enmTL).glbl.NewBitBase()
                Public Shared char_next_sprtr As bitTL = TRow(Of bitTL, enmTL).glbl.NewBitBase()
                Public Shared chunk_type As bitTL = TRow(Of bitTL, enmTL).glbl.NewBitBase()
            End Class

            Public Class rTempLine
                Inherits TRow(Of bitTL, enmTL)

                Public rem_text_index As Integer

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New()
                    Call MyBase.New()
                    Me.rem_text_index = 0
                End Sub

                <System.Diagnostics.DebuggerHidden()>
                Public Function vt(ur_enm As bitTL, ur_val As String) As rTempLine
                    vt = Me
                    Me.v(ur_enm) = ur_val
                End Function
            End Class 'rTempLine

            Public Class boxChunk_State
                Public gContext As bitCONTEXT
                Public gChunk_End As Integer
                Public gSplit_Char As String
                Public gStart_Index As Integer
                Public gText_Orig As String

                Public Sub New(ur_context As bitCONTEXT, ur_chunk_end As Integer, ur_split_char As String, ur_start_index As Integer, ur_text_orig As String)
                    Me.gContext = ur_context
                    Me.gChunk_End = ur_chunk_end
                    Me.gSplit_Char = ur_split_char
                    Me.gStart_Index = ur_start_index
                    Me.gText_Orig = ur_text_orig
                End Sub
            End Class 'boxChunk_State

            Public Class boxChunk_Loop
                Public gChar_Entry As Char
                Public gSkip_Context As bitCONTEXT

                Public Sub New(ur_char_entry As Char, ur_skip_context As bitCONTEXT)
                    Me.gChar_Entry = ur_char_entry
                    Me.gSkip_Context = ur_skip_context
                End Sub
            End Class 'boxChunk_Loop

            Public Class boxSplit_State
                Public gContext As bitCONTEXT
                Public gRow As rTempLine
                Public gSplit As Integer
                Public gText_Orig As String

                Public Sub New(ur_context As bitCONTEXT, ur_row As rTempLine, ur_split As Integer, ur_text_orig As String)
                    Me.gContext = ur_context
                    Me.gRow = ur_row
                    Me.gSplit = ur_split
                    Me.gText_Orig = ur_text_orig
                End Sub
            End Class 'boxSplit_State

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub gChunk_Next(ret_split_state As boxSplit_State, ur_large_text As String, ur_start_index As Integer, ur_context As bitCONTEXT)
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

                Public Shared Function gChunk_Type(ur_large_text As String, ur_start_index As Integer) As bitCONTEXT
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
                Public Shared Function CommentClose(ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As bitCONTEXT
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
                Public Shared Function QuoteSkip(ur_context As bitCONTEXT, ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As bitCONTEXT
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
                Public Shared Function TDataCombine(ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As bitCONTEXT
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
                Public Shared Function WSpaceCombine(ur_large_text As String, ur_chr_entry As Char, ur_chrctr As Integer) As bitCONTEXT
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
            Public Shared Function gChunk_Skip(ur_large_text As String, ur_char_index As Integer, ur_context As bitCONTEXT) As Integer
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
            Public Shared Sub wRecurse_CodeSplit(ret_chunk_table As db.sTempField, ur_source_text As String)
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
                            New db.rTempField().
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


    Partial Public Class db
        Private Shared tblTempField As sTempField
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If db.tblUserBowl Is Nothing Then
                db.tblTempField = New sTempField
                db.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'db

    Public Class bitTF
        Inherits bitBASE
    End Class

    Public Class enmTF
        Public Shared code_type As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared code_text As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempField() As sTempField
            Call db.Connect()
            TempField = db.tblTempField
        End Function

        Public Class rTempField
            Inherits TRow(Of bitTF, enmTF)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitTF, ur_val As String) As rTempField
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
            Public Sub Del(ur_col As bitTF, ur_val As String)
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

    Public Class bitUB
        Inherits bitBASE
    End Class

    Public Class enmUB
        Public Shared bowl_name As bitUB = TRow(Of bitUB, enmUB).glbl.NewBitBase()
        Public Shared contents As bitUB = TRow(Of bitUB, enmUB).glbl.NewBitBase()
    End Class

    Public Class bitUN
        Inherits bitBASE
    End Class

    Public Class enmUN
        Public Shared app_name As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared cmdline_audit As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared from_clipboard As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared to_clipboard As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Dim bolFIRST_INIT = (db.tblUserBowl Is Nothing)
            Call db.Connect()
            UserBowl = db.tblUserBowl
            If bolFIRST_INIT Then
                Call db.tblUserBowl.InsFrom_Application()
                'db.tblUserBowl.InsKey(enmUN.cmdline_audit, "1")
                Call db.tblUserBowl.Cboard_CmdlineAudit()
            End If
        End Function

        Public Class rUserBowl
            Inherits TRow(Of bitUB, enmUB)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitUB, ur_val As String) As rUserBowl
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
                    Dim ins_msg = db.MessageBox.Ins(
                        New db.rMessageBox().
                            vt(enmMB.title, Me.SelKey(enmUN.app_name).v(enmUB.contents)).
                            vt(enmMB.text, strAUDIT),
                        MsgBoxStyle.OkCancel
                        )
                    If ins_msg.vUserResponse = MsgBoxResult.Ok Then
                        db.Clipboard.Ins(
                            New db.rClipboard().
                            vt(enmCB.text, strAUDIT)
                            )
                    End If
                End If
            End Sub 'Cboard_CmdlineAudit

            <System.Diagnostics.DebuggerHidden()>
            Public Function ExistsKey(ur_key As bitUN) As Boolean
                ExistsKey = False
                Dim strUN = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmUB.bowl_name), strUN) Then
                        ExistsKey = True
                        Exit For
                    End If
                Next
            End Function 'ExistsKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rUserBowl) As rUserBowl
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsKey(ur_key As bitUN, ur_val As String) As rUserBowl
                Dim ret = New rUserBowl
                InsKey = ret
                Me.ttb.Add(
                    ret.
                    vt(enmUB.bowl_name, ur_key.name).
                    vt(enmUB.contents, ur_val)
                    )
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFrom_Application() As rUserBowl
                Dim ret = New rUserBowl
                InsFrom_Application = ret
                Me.InsKey(enmUN.app_name, New MxText.FileName().d(Mx.Class1.SourcePath).FileGroup)

                Dim arlCMD_RET = MxText.Cmdline_UB(Of bitUN, enmUN, bitUB, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, System.Environment.CommandLine)
                Me.InsKey(enmUN.cmdline_orig, qs & System.Environment.CommandLine.Replace(qs, qs & qs) & qs)
                Me.InsKey(enmUN.cmdline_table, qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(qs, qs & qs) & qs)
                For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                    Me.Ins(
                        New db.rUserBowl().
                        vt(enmUB.bowl_name, rowFOUND.v(enmUB.bowl_name)).
                        vt(enmUB.contents, rowFOUND.v(enmUB.contents))
                        )
                Next
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As bitUB, ur_value As String) As sUserBowl
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
            Public Function SelFirst() As rUserBowl
                If Me.ttb.Count = 0 Then
                    SelFirst = New rUserBowl()
                Else
                    SelFirst = Me.ttb.tr_b1(1)
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelKey(ur_key As bitUN) As rUserBowl
                Dim ret As rUserBowl = Nothing
                Dim strUN = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmUB.bowl_name), strUN) Then
                        ret = row
                        Exit For
                    End If
                Next
                If ret Is Nothing Then ret = New rUserBowl
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
