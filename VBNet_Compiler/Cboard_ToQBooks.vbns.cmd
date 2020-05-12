start "" MxClasses\VBNetScript.exe /path=%0
exit
MxClasses\MxBaseEc10.vb
RetVal = Mx.Want.Clipboard_TimesheetReport
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.Clipboard_TimesheetReport
'            If RetVal <> "" Then MsgBox(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = "UrFolder"
'        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
'    End Class
'End Namespace 'Mx

Namespace Mx
    Public Class Want
        Public Shared Function Clipboard_TimesheetReport() As Strap
            Dim stpRET = Strapd()
            Clipboard_TimesheetReport = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Have.InText_Search_FromGlobal(db.UserBowl, db.Clipboard)
                Have.ClientTable_Populate(db.ClientAbbrv)
                Have.TempField_Search_FromText(db.UserBowl.SelKey(enmUN.from_clipboard), db.TempField)
                Use.Clear_UnusedTimes(db.TempField)
                Use.Compile_QuickBooksRecords(db.ClientAbbrv, db.TempField, db.Report_Hdr, db.Report_Detl)
                Use.Compile_QuickBooksReport(db.UserBowl, db.Report_Hdr, db.Report_Detl)
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
        End Function 'Clipboard_SplitAt_AS_Skip_Comma
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

        Public Shared Sub ClientTable_Populate(ur_client_table As db.sClientAbbrv)
            ur_client_table.InsField(
                "ADVRNT",
                Strapd().
                dLine("BBTS:Rental Demo").
                dLine("BBTS  - Internal")
                )
            ur_client_table.InsField(
                "ASP",
                Strapd().
                dLine("Advance Storage Products").
                dLine("ASP - Technical Services")
                )
            ur_client_table.InsField(
                "BBTS",
                Strapd().
                dLine("BBTS").
                dLine("BBTS  - Internal")
                )
            ur_client_table.InsField(
                "RNT BBTS",
                Strapd().
                dLine("BBTS:Rental Demo").
                dLine("BBTS  - Internal")
                )
            ur_client_table.InsField(
                "BPool",
                Strapd().
                dLine("QuadKor:BluPool").
                dLine("QK/BluPool - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "CleanRooms",
                Strapd().
                dLine("QuadKor:Clean Rooms International").
                dLine("QK/Clean Rooms International - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "CSCF",
                Strapd().
                dLine("QuadKor:Chicago Scaffolding").
                dLine("QK/Chicago Scaffolding - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "FLM",
                Strapd().
                dLine("QuadKor:Filmwerks").
                dLine("QK/Filmwerks - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "Freund",
                Strapd().
                dLine("QuadKor:Freund Bakery").
                dLine("QK/Freund Bakery - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "MKDoor",
                Strapd().
                dLine("QuadKor:MacKenzie Door").
                dLine("QK/MacKenzie Door - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "TPR",
                Strapd().
                dLine("QuadKor:Trench Plate").
                dLine("QK/Trench Plate Rental - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "TGN",
                Strapd().
                dLine("QuadKor:Tecogen").
                dLine("QK/Tecogen - Professional and Technical Services")
                )
            ur_client_table.InsField(
                "UNKNOWN",
                Strapd().
                dLine("*** UNKOWN")
                )

            For Each strCLIENT In {"BBTS", "ADVRNT"}
                For Each rowENTRY In ur_client_table.Sel(enmCA.client_abbrv, strCLIENT).SelAll
                    rowENTRY.v(enmCA.suffix_line) = "*** Billable = clear"
                Next
            Next strCLIENT
        End Sub 'ClientTable_Populate

        Public Shared Sub TempField_Search_FromText(ur_in_text As db.rUserBowl, ur_tempfield_table As db.sTempField)
            Dim strFOUND_TEXT = ur_in_text.v(enmUB.contents)
            Dim intCUR_COL = 1
            Dim trwLINE = New db.rTempField()
            ur_tempfield_table.Ins(trwLINE)

            For Each strLINE In Sdata.Split(strFOUND_TEXT.Replace(vbCr, mt), vbLf)
                Dim intQS = InStr(strLINE, qs)
                Dim strCHUNK_TYPE = mt
                Dim strCHUNK_DATA = Mid(strLINE, intQS + 4)
                If intQS > 0 Then
                    strCHUNK_TYPE = Mid(strLINE, intQS + 1, 2)
                    If strCHUNK_TYPE = "WS" Then
                        If Left(strCHUNK_DATA, 1) = vbTab Then
                            strCHUNK_TYPE = "TB"
                        End If
                    End If
                End If

                Select Case strCHUNK_TYPE
                    Case "TX"
                        If intCUR_COL <= trwLINE.Count Then
                            trwLINE.v_b1(intCUR_COL) &= strCHUNK_DATA
                        End If

                    Case "TB"
                        intCUR_COL += strCHUNK_DATA.Length - strCHUNK_DATA.Replace(vbTab, mt).Length
                    Case "WS"
                        If intCUR_COL <= trwLINE.Count Then
                            trwLINE.v_b1(intCUR_COL) &= s
                        End If

                    Case mt
                        intCUR_COL = 1
                        trwLINE = New db.rTempField()
                        ur_tempfield_table.Ins(trwLINE)
                End Select 'strCHUNK_TYPE
            Next strLINE

            If ur_tempfield_table.SelAll.Count = 1 Then
                Throw New System.Exception("Temp Field table not found: Length = " & strFOUND_TEXT.Length)

            Else
                Dim trwFIRST = ur_tempfield_table.SelFirst
                Dim trwTITLE = New db.rTempField
                trwTITLE.v(enmTF.date_entry) = "Date"
                trwTITLE.v(enmTF.start_time) = "Start"
                trwTITLE.v(enmTF.hrs_dec) = "Hrs"
                trwTITLE.v(enmTF.tkt_num) = "Tkt #"
                trwTITLE.v(enmTF.tkt_num) = "Tkt #"
                trwTITLE.v(enmTF.client_abbrv) = "Client"
                trwTITLE.v(enmTF.task_num) = "Task #"
                trwTITLE.v(enmTF.task_title) = "Task"
                trwTITLE.v(enmTF.notes_text) = "Notes"
                For Each kvpCOL In trwTITLE.kvp
                    If AreEqual(trwTITLE.RefColNames.v_enm(kvpCOL.Indexenm), "blank_col") = False AndAlso
                      AreEqual(trwFIRST.v_enm(kvpCOL.Indexenm), trwTITLE.v_enm(kvpCOL.Indexenm)) Then
                        Throw New System.Exception(Strapd().d("Temp Field cannot inclued the title row:").dS(trwFIRST.ToString(ur_hdr:=True)))
                        Exit For
                    End If
                Next 'kvpCOL

                For Each trwENTRY In ur_tempfield_table.SelAll
                    trwENTRY.vt(enmTF.notes_text, Trim(trwENTRY.v(enmTF.notes_text)))
                Next
            End If 'ur_tempfield_table
        End Sub 'TempField_Search_FromText
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

        Public Shared Sub Clear_UnusedTimes(ur_tempfield_table As db.sTempField)
            ur_tempfield_table.Del(enmTF.hrs_dec, mt)
        End Sub 'Clear_UnusedTimes

        Public Shared Sub Compile_QuickBooksRecords(ur_client_table As db.sClientAbbrv, ur_tempfield_table As db.sTempField, ur_reporthdr_table As db.sReport_Hdr, ur_reportdetl_table As db.sReport_Detl)
            Const lit_unknown = "UNKNOWN"
            Dim stpCOMPILE_MSG = Strapd()
            Dim strDISP_UNKNOWN = ur_client_table.Sel(enmCA.client_abbrv, lit_unknown).SelFirst.v(enmCA.disp_lines)
            For Each kvpDATE In db.TempField.Index_TableSplit(enmTF.date_entry).kvp
                Dim strDATE_GRP = kvpDATE.v
                Dim lstDATE = kvpDATE.l
                For Each kvpDCLIENT In lstDATE.Index_TableSplit(enmTF.client_abbrv).kvp
                    Dim strCLIENT_GRP = kvpDCLIENT.v
                    Dim lstDCLIENT = kvpDCLIENT.l
                    'unique date/client combination
                    'task list
                    'total hours tabbed-over by date
                    Dim recABBRV = db.ClientAbbrv.Sel(enmCA.client_abbrv, strCLIENT_GRP).SelFirst
                    Dim strDISP_LINE = recABBRV.v(enmCA.disp_lines)
                    If HasText(strDISP_LINE) = False Then
                        strDISP_LINE = strDISP_UNKNOWN & ": " & strCLIENT_GRP
                    End If

                    Dim trwHDR = ur_reporthdr_table.InsNew()
                    trwHDR.vt(enmRP.date_grp, kvpDATE.v)
                    trwHDR.vt(enmRP.client_grp, strCLIENT_GRP)
                    trwHDR.v(enmRP.disp_lines) = strDISP_LINE
                    trwHDR.v(enmRP.suffix_line) = recABBRV.v(enmCA.suffix_line)

                    Dim sdpTASK_CHUNK = lstDCLIENT.Index_TableSplit(enmTF.task_title)
                    For Each kvpTASK_CHUNK In sdpTASK_CHUNK.kvp
                        Dim trwTASK_ROW = kvpTASK_CHUNK.l.SelFirst
                        Dim trwDETL = ur_reportdetl_table.InsNew()
                        trwDETL.vt(enmRT.date_grp, kvpDATE.v)
                        trwDETL.vt(enmRT.client_grp, strCLIENT_GRP)
                        trwDETL.vt(enmRT.task_entry, trwTASK_ROW.v(enmTF.task_title).Trim())
                        With 1 : Dim lstCTASK = trwTASK_ROW.Link_Table(ur_tempfield_table.SelAll, enmTF.client_abbrv, enmTF.task_title)
                            Dim sdpDATE_TASK = lstCTASK.Distinct(enmTF.date_entry)
                            If sdpDATE_TASK.Count > 1 Then
                                Dim strDAY_SUFFIX = " - day " & b1(sdpDATE_TASK.IndexOf(strDATE_GRP)).ToString
                                trwDETL.vt(enmRT.task_daysuffix, strDAY_SUFFIX)
                            End If
                        End With

                        Dim stpNOTE_LINES = Strapd()
                        For Each strNOTE In kvpTASK_CHUNK.l.Distinct(enmTF.notes_text)
                            stpNOTE_LINES.dLine("-" & strNOTE)
                        Next

                        trwDETL.vt(enmRT.task_notes, stpNOTE_LINES.ToString)
                    Next kvpTASK_CHUNK

                    With 1 : Dim strDATE_ENTRY = lstDCLIENT.SelFirst.v(enmTF.date_entry).ToUpper
                        strDATE_ENTRY = strDATE_ENTRY.Replace("SUN", "1-Sun")
                        strDATE_ENTRY = strDATE_ENTRY.Replace("MON", "2-Mon")
                        strDATE_ENTRY = strDATE_ENTRY.Replace("TUE", "3-Tue")
                        strDATE_ENTRY = strDATE_ENTRY.Replace("WED", "4-Wed")
                        strDATE_ENTRY = strDATE_ENTRY.Replace("THU", "5-Thu")
                        strDATE_ENTRY = strDATE_ENTRY.Replace("FRI", "6-Fri")
                        strDATE_ENTRY = strDATE_ENTRY.Replace("SAT", "7-Sat")
                        trwHDR.vt(enmRP.weekday_entry, strDATE_ENTRY)
                    End With

                    With 1 : Dim decHRS = 0.0
                        For Each rowDCLIENT_CHUNK In lstDCLIENT.SelAll
                            Dim decFOUND = 0.0
                            If System.Decimal.TryParse(rowDCLIENT_CHUNK.v(enmTF.hrs_dec), decFOUND) Then
                                decHRS += decFOUND
                            End If
                        Next rowDCLIENT_CHUNK

                        trwHDR.vt(enmRP.total_hours, decHRS.ToString)
                    End With
                Next kvpDCLIENT
            Next kvpDATE
        End Sub 'Compile_QuickBooksRecords

        Public Shared Sub Compile_QuickBooksReport(ur_bowl_table As db.sUserBowl, ur_reporthdr_table As db.sReport_Hdr, ur_reportdetl_table As db.sReport_Detl)
            Dim stpRET = Strapd()
            For Each kvpREPORT In ur_reporthdr_table.SelAll.kvp
                Dim trwREPORT = kvpREPORT.row
                stpRET.dLine(trwREPORT.v(enmRP.disp_lines))

                For Each kvpDETL In trwREPORT.Link_Table(ur_reportdetl_table, enmRP.date_grp, enmRP.client_grp).SelAll
                    stpRET.dLine(kvpDETL.v(enmRT.task_entry) & kvpDETL.v(enmRT.task_daysuffix))
                    stpRET.d(kvpDETL.v(enmRT.task_notes))
                Next kvpDETL

                Dim strSUFFIX_LINE = trwREPORT.v(enmRP.suffix_line)
                If HasText(strSUFFIX_LINE) Then
                    stpRET.dLine(strSUFFIX_LINE)
                End If

                stpRET.dLine(trwREPORT.v(enmRP.weekday_entry))
                stpRET.dLine(trwREPORT.v(enmRP.total_hours))

                stpRET.dLine()
                stpRET.dLine()
            Next kvpREPORT

            ur_bowl_table.InsKey(enmUN.to_clipboard, stpRET.ToString)
        End Sub 'Compile_QuickBooksReport
    End Class 'Use


    Partial Public Class db
        Private Shared tblClientAbbrv As sClientAbbrv
        Private Shared tblReport_Hdr As sReport_Hdr
        Private Shared tblReport_Detl As sReport_Detl
        Private Shared tblTempField As sTempField
        Private Shared tblTempLine As sTempLine
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If db.tblUserBowl Is Nothing Then
                db.tblClientAbbrv = New sClientAbbrv
                db.tblReport_Hdr = New sReport_Hdr
                db.tblReport_Detl = New sReport_Detl
                db.tblTempField = New sTempField
                db.tblTempLine = New sTempLine
                db.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'db


    Public Class bitCA
        Inherits bitBASE
    End Class

    Public Class enmCA
        Public Shared client_abbrv As bitCA = TRow(Of bitCA, enmCA).glbl.NewBitBase()
        Public Shared disp_lines As bitCA = TRow(Of bitCA, enmCA).glbl.NewBitBase()
        Public Shared suffix_line As bitCA = TRow(Of bitCA, enmCA).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function ClientAbbrv() As sClientAbbrv
            Call db.Connect()
            ClientAbbrv = db.tblClientAbbrv
        End Function

        Public Class rClientAbbrv
            Inherits TRow(Of bitCA, enmCA)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitCA, ur_val As String) As rClientAbbrv
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rClientAbbrv

        Public Class sClientAbbrv
            Private ttb As Objlist(Of rClientAbbrv)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rClientAbbrv)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rClientAbbrv) As rClientAbbrv
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsField(ur_client_abbrv As String, ur_disp_lines As String) As rClientAbbrv
                Dim retREC = New rClientAbbrv
                InsField = retREC
                Me.ttb.Add(retREC)
                retREC.v(enmCA.client_abbrv) = ur_client_abbrv
                retREC.v(enmCA.disp_lines) = ur_disp_lines
            End Function 'InsField

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As bitCA, ur_value As String) As sClientAbbrv
                Dim retUB = New sClientAbbrv
                Sel = retUB
                For Each rowUB In Me.ttb
                    If AreEqual(rowUB.v(ur_col), ur_value) Then
                        retUB.Ins(rowUB)
                    End If
                Next
            End Function 'Sel

            Public ReadOnly Property SelAll() As Objlist(Of rClientAbbrv)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rClientAbbrv
                If Me.ttb.Count = 0 Then
                    SelFirst = New rClientAbbrv()
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
        End Class 'sClientAbbrv
    End Class 'CA

    Public Class bitRP
        Inherits bitBASE
    End Class

    Public Class enmRP
        Public Shared date_grp As bitRP = TRow(Of bitRP, enmRP).glbl.NewBitBase()
        Public Shared client_grp As bitRP = TRow(Of bitRP, enmRP).glbl.NewBitBase()
        Public Shared disp_lines As bitRP = TRow(Of bitRP, enmRP).glbl.NewBitBase()
        Public Shared suffix_line As bitRP = TRow(Of bitRP, enmRP).glbl.NewBitBase()
        Public Shared weekday_entry As bitRP = TRow(Of bitRP, enmRP).glbl.NewBitBase()
        Public Shared total_hours As bitRP = TRow(Of bitRP, enmRP).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function Report_Hdr() As sReport_Hdr
            Call db.Connect()
            Report_Hdr = db.tblReport_Hdr
        End Function

        Public Class rReport_Hdr
            Inherits TRow(Of bitRP, enmRP)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitRP, ur_val As String) As rReport_Hdr
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Link_Table(ur_table As Objlist(Of db.rReport_Hdr), ParamArray ur_key_list() As bitRP) As sReport_Hdr
                Dim tblRET = New sReport_Hdr
                Link_Table = tblRET
                For Each rowCHUNK In ur_table
                    Dim bolFOUND = True
                    For Each intKEY In ur_key_list
                        If AreEqual(rowCHUNK.v(intKEY), Me.v(intKEY)) = False Then
                            bolFOUND = False
                            Exit For
                        End If
                    Next intKEY

                    If bolFOUND Then
                        tblRET.Ins(rowCHUNK)
                    End If
                Next rowCHUNK
            End Function 'Link_Table

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Link_Table(ur_child_table As db.sReport_Detl, ParamArray ur_key_list() As bitRP) As sReport_Detl
                Dim tblRET = New sReport_Detl
                Link_Table = tblRET
                Dim sdpXLAT = New SdPair(Of bitRT)
                Dim stpKEY_BINDINGS = Strapd()
                For Each keyRP In ur_key_list
                    stpKEY_BINDINGS.dS(keyRP.name)
                    For Each keyRT In ur_child_table.SelFirst.RefColKeys
                        If AreEqual(keyRT.name, keyRP.name) Then
                            sdpXLAT.d(keyRP.name, keyRT)
                        End If
                    Next keyRT
                Next keyRP

                If sdpXLAT.Count <> ur_key_list.Length Then
                    Throw New System.Exception("Key bindings not found for RT/RP join: " & stpKEY_BINDINGS.ToString)

                Else
                    For Each trwJOIN In ur_child_table.SelAll
                        Dim bolFOUND = True
                        For Each keyRP In ur_key_list
                            Dim keyRT = sdpXLAT.l(keyRP.name)
                            If AreEqual(trwJOIN.v(keyRT), Me.v(keyRP)) = False Then
                                bolFOUND = False
                                Exit For
                            End If
                        Next keyRP

                        If bolFOUND Then
                            tblRET.Ins(trwJOIN)
                        End If
                    Next trwJOIN
                End If 'sdpXLAT
            End Function 'Link_Table
        End Class 'rReport_Hdr

        Public Class sReport_Hdr
            Private ttb As Objlist(Of rReport_Hdr)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rReport_Hdr)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Del(ur_col As bitRP, ur_val As String)
                For ROWCTR = Me.ttb.Count To 1 Step -1
                    If AreEqual(Me.ttb.tr_b1(ROWCTR).v(ur_col), ur_val) Then
                        Me.ttb.RemoveAt(b0(ROWCTR))
                    End If
                Next ROWCTR
            End Sub 'Del

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Index_TableSplit(ur_key As bitRP) As SdPair(Of sReport_Hdr)
                Dim sdpRET = New SdPair(Of sReport_Hdr)
                Index_TableSplit = sdpRET
                Dim tblCHUNK As sReport_Hdr = Nothing
                For Each rowCHUNK In Me.ttb
                    Dim strINDEX = rowCHUNK.v(ur_key)
                    Dim intFOUND = sdpRET.IndexOf(strINDEX)
                    If intFOUND < 0 Then
                        tblCHUNK = New sReport_Hdr
                        sdpRET.d(strINDEX, tblCHUNK)
                    Else
                        tblCHUNK = sdpRET.l_enm(intFOUND)
                    End If

                    tblCHUNK.Ins(rowCHUNK)
                Next rowCHUNK

                Call sdpRET.Sort()
            End Function 'Index_TableSplit

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rReport_Hdr) As rReport_Hdr
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsNew() As rReport_Hdr
                Dim trwNEW = New rReport_Hdr
                InsNew = trwNEW
                Me.ttb.Add(trwNEW)
            End Function

            Public ReadOnly Property SelAll() As Objlist(Of rReport_Hdr)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rReport_Hdr
                If Me.ttb.Count = 0 Then
                    SelFirst = New rReport_Hdr()
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
        End Class 'sReport_Hdr
    End Class 'RP


    Public Class bitRT
        Inherits bitBASE
    End Class

    Public Class enmRT
        Public Shared date_grp As bitRT = TRow(Of bitRT, enmRT).glbl.NewBitBase()
        Public Shared client_grp As bitRT = TRow(Of bitRT, enmRT).glbl.NewBitBase()
        Public Shared task_entry As bitRT = TRow(Of bitRT, enmRT).glbl.NewBitBase()
        Public Shared task_daysuffix As bitRT = TRow(Of bitRT, enmRT).glbl.NewBitBase()
        Public Shared task_notes As bitRT = TRow(Of bitRT, enmRT).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function Report_Detl() As sReport_Detl
            Call db.Connect()
            Report_Detl = db.tblReport_Detl
        End Function

        Public Class rReport_Detl
            Inherits TRow(Of bitRT, enmRT)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitRT, ur_val As String) As rReport_Detl
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Link_Table(ur_table As Objlist(Of db.rReport_Detl), ParamArray ur_key_list() As bitRT) As sReport_Detl
                Dim tblRET = New sReport_Detl
                Link_Table = tblRET
                For Each rowCHUNK In ur_table
                    Dim bolFOUND = True
                    For Each intKEY In ur_key_list
                        If AreEqual(rowCHUNK.v(intKEY), Me.v(intKEY)) = False Then
                            bolFOUND = False
                            Exit For
                        End If
                    Next intKEY

                    If bolFOUND Then
                        tblRET.Ins(rowCHUNK)
                    End If
                Next rowCHUNK
            End Function 'Link_Table
        End Class 'rReport_Detl

        Public Class sReport_Detl
            Private ttb As Objlist(Of rReport_Detl)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rReport_Detl)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Del(ur_col As bitRT, ur_val As String)
                For ROWCTR = Me.ttb.Count To 1 Step -1
                    If AreEqual(Me.ttb.tr_b1(ROWCTR).v(ur_col), ur_val) Then
                        Me.ttb.RemoveAt(b0(ROWCTR))
                    End If
                Next ROWCTR
            End Sub 'Del

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Index_TableSplit(ur_key As bitRT) As SdPair(Of sReport_Detl)
                Dim sdpRET = New SdPair(Of sReport_Detl)
                Index_TableSplit = sdpRET
                Dim tblCHUNK As sReport_Detl = Nothing
                For Each rowCHUNK In Me.ttb
                    Dim strINDEX = rowCHUNK.v(ur_key)
                    Dim intFOUND = sdpRET.IndexOf(strINDEX)
                    If intFOUND < 0 Then
                        tblCHUNK = New sReport_Detl
                        sdpRET.d(strINDEX, tblCHUNK)
                    Else
                        tblCHUNK = sdpRET.l_enm(intFOUND)
                    End If

                    tblCHUNK.Ins(rowCHUNK)
                Next rowCHUNK

                Call sdpRET.Sort()
            End Function 'Index_TableSplit

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rReport_Detl) As rReport_Detl
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsNew() As rReport_Detl
                Dim trwNEW = New rReport_Detl
                InsNew = trwNEW
                Me.ttb.Add(trwNEW)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As bitRT, ur_value As String) As sReport_Detl
                Dim retRT = New sReport_Detl
                Sel = retRT
                For Each rowUB In Me.ttb
                    If AreEqual(rowUB.v(ur_col), ur_value) Then
                        retRT.Ins(rowUB)
                    End If
                Next
            End Function 'Sel

            Public ReadOnly Property SelAll() As Objlist(Of rReport_Detl)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rReport_Detl
                If Me.ttb.Count = 0 Then
                    SelFirst = New rReport_Detl()
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
        End Class 'sReport_Detl
    End Class 'RT


    Public Class bitTF
        Inherits bitBASE
    End Class

    Public Class enmTF
        Public Shared date_entry As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared start_time As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared hrs_dec As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared tkt_num As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared blank_col As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared client_abbrv As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared task_num As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared task_title As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
        Public Shared notes_text As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
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

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Link_Table(ur_table As Objlist(Of db.rTempField), ParamArray ur_key_list() As bitTF) As sTempField
                Dim tblRET = New sTempField
                Link_Table = tblRET
                For Each rowCHUNK In ur_table
                    Dim bolFOUND = True
                    For Each intKEY In ur_key_list
                        If AreEqual(rowCHUNK.v(intKEY), Me.v(intKEY)) = False Then
                            bolFOUND = False
                            Exit For
                        End If
                    Next intKEY

                    If bolFOUND Then
                        tblRET.Ins(rowCHUNK)
                    End If
                Next rowCHUNK
            End Function 'Link_Table
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
            Public Shadows Function Distinct(ur_key As bitTF) As Sdata
                Dim sdaRET = New Sdata
                Distinct = sdaRET
                For Each rowCHUNK In Me.ttb
                    Dim strINDEX = rowCHUNK.v(ur_key)
                    Dim intFOUND = sdaRET.IndexOf(strINDEX)
                    If intFOUND < 0 Then
                        sdaRET.d(strINDEX)
                    End If
                Next rowCHUNK

                Call sdaRET.Sort()
            End Function 'Distinct

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Index_TableSplit(ur_key As bitTF) As SdPair(Of sTempField)
                Dim sdpRET = New SdPair(Of sTempField)
                Index_TableSplit = sdpRET
                Dim tblCHUNK As sTempField = Nothing
                For Each rowCHUNK In Me.ttb
                    Dim strINDEX = rowCHUNK.v(ur_key)
                    Dim intFOUND = sdpRET.IndexOf(strINDEX)
                    If intFOUND < 0 Then
                        tblCHUNK = New sTempField
                        sdpRET.d(strINDEX, tblCHUNK)
                    Else
                        tblCHUNK = sdpRET.l_enm(intFOUND)
                    End If

                    tblCHUNK.Ins(rowCHUNK)
                Next rowCHUNK

                Call sdpRET.Sort()
            End Function 'Index_TableSplit

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

    Public Class bitTL
        Inherits bitBASE
    End Class

    Public Class enmTL
        Public Shared line_seq As bitTL = TRow(Of bitTL, enmTL).glbl.NewBitBase()
        Public Shared text As bitTL = TRow(Of bitTL, enmTL).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempLine() As sTempLine
            Call db.Connect()
            TempLine = db.tblTempLine
        End Function

        Public Class rTempLine
            Inherits TRow(Of bitTL, enmTL)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitTL, ur_val As String) As rTempLine
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rTempLine

        Public Class sTempLine
            Private ttb As Objlist(Of rTempLine)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rTempLine)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rTempLine) As rTempLine
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Sub InsSplitAt_Keyword_Skip_Comma(ur_text As String, ur_keyword As String)
                For Each kvpROW In Sdata.Split(ur_text.Replace(vbCr, mt), vbLf).kvp
                    Dim strTEXT = kvpROW.v
                    Dim intFOUND = InStr(strTEXT, ur_keyword)
                    If intFOUND > 0 Then
                        strTEXT = Mid(strTEXT, intFOUND + Len(ur_keyword)).Replace(",", mt)
                        If Right(strTEXT, 1) = "," Then
                            strTEXT = Left(strTEXT, Len(strTEXT) - 1)
                        End If

                        Me.ttb.Add(New rTempLine().vt(enmTL.line_seq, kvpROW.Indexb1).vt(enmTL.text, strTEXT))
                    End If 'intFOUND
                Next kvpROW
            End Sub 'InsSplitAt_Keyword_Skip_Comma

            Public ReadOnly Property SelAll() As Objlist(Of rTempLine)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelCombine_Lines() As Strap
                Dim ret = New Strap
                SelCombine_Lines = ret
                For Each row In Me.ttb
                    ret.dLine(row.v(enmTL.text))
                Next 'strLINE
            End Function 'SelCombine_Lines

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
        End Class 'sTempLine
    End Class 'TL

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
