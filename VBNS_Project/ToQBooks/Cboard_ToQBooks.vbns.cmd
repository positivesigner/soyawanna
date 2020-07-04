start "" "%~dp0\MxClasses\VBNetScript.exe" /path=%0
exit
MxClasses\MxBaseEc12.vb
RetVal = Mx.Want.Clipboard_TimesheetReport_errhnd
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.Clipboard_TimesheetReport_errhnd
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
        Public Shared Sub Clipboard_TimesheetReport(ur_ret As Strap)
            Dim userbowl_cart = Have.UserBowl
            Dim appname_bowlname = enmUN.app_name
            Dim clientabbrv_cart = Have.ClientAbbrv
            Dim tempfield_cart = Have.TempField
            Dim messagebox_cart = Have.MessageBox
            Dim windows_clipboard_cart = Have.Clipboard
            Dim from_clipboard_bowlname = userbowl_cart.Apply(Have.Clipboard)
            Dim reporthdr_cart = Have.Report_Hdr
            Dim reportdtl_cart = Have.Report_Detl
            clientabbrv_cart.Apply_Defaults()
            tempfield_cart.Apply(from_clipboard_bowlname)
            tempfield_cart.Clear_UnusedTimes()
            reporthdr_cart.Apply(clientabbrv_cart, tempfield_cart)
            reportdtl_cart.Apply(clientabbrv_cart, tempfield_cart, reporthdr_cart)
            Dim report_output_bowlname = userbowl_cart.Apply(reporthdr_cart, reportdtl_cart)
            Dim from_messagebox_bowlname = userbowl_cart.Apply(report_output_bowlname, appname_bowlname, messagebox_cart)
            Dim clipboard_recdate_bowlname = userbowl_cart.Apply(from_messagebox_bowlname, report_output_bowlname, windows_clipboard_cart)
        End Sub 'Clipboard_TimesheetReport

        Public Shared Function Clipboard_TimesheetReport_errhnd() As Strap
            Dim stpRET = Strapd()
            Clipboard_TimesheetReport_errhnd = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Call Clipboard_TimesheetReport(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'Clipboard_TimesheetReport_errhnd
    End Class 'Want

    Public Class Have
        Partial Class sClientAbbrv
            <System.Diagnostics.DebuggerHidden()>
            Public Sub Apply_Defaults()
                Me.InsField(
                    "ADVRNT",
                    Strapd().
                    dLine("BBTS:Rental Demo").
                    dLine("BBTS  - Internal")
                    )
                Me.InsField(
                    "ASP",
                    Strapd().
                    dLine("Advance Storage Products").
                    dLine("ASP - Technical Services")
                    )
                Me.InsField(
                    "BBTS",
                    Strapd().
                    dLine("BBTS").
                    dLine("BBTS  - Internal")
                    )
                Me.InsField(
                    "RNT BBTS",
                    Strapd().
                    dLine("BBTS:Rental Demo").
                    dLine("BBTS  - Internal")
                    )
                Me.InsField(
                    "BPool",
                    Strapd().
                    dLine("QuadKor:BluPool").
                    dLine("QK/BluPool - Professional and Technical Services")
                    )
                Me.InsField(
                    "CleanRooms",
                    Strapd().
                    dLine("QuadKor:Clean Rooms International").
                    dLine("QK/Clean Rooms International - Professional and Technical Services")
                    )
                Me.InsField(
                    "CSCF",
                    Strapd().
                    dLine("QuadKor:Chicago Scaffolding").
                    dLine("QK/Chicago Scaffolding - Professional and Technical Services")
                    )
                Me.InsField(
                    "FLM",
                    Strapd().
                    dLine("QuadKor:Filmwerks").
                    dLine("QK/Filmwerks - Professional and Technical Services")
                    )
                Me.InsField(
                    "Freund",
                    Strapd().
                    dLine("QuadKor:Freund Bakery").
                    dLine("QK/Freund Bakery - Professional and Technical Services")
                    )
                Me.InsField(
                    "MKDoor",
                    Strapd().
                    dLine("QuadKor:MacKenzie Door").
                    dLine("QK/MacKenzie Door - Professional and Technical Services")
                    )
                Me.InsField(
                    "NANT",
                    Strapd().
                    dLine("QuadKor:NantEnergy").
                    dLine("QK/NantEnergy - Professional and Technical Services")
                    )
                Me.InsField(
                    "TeDan",
                    Strapd().
                    dLine("QuadKor:TeDan Surgical Innovations (TSI)").
                    dLine("QK/TeDan Surgical Innovations - Professional and Technical Services")
                    )
                Me.InsField(
                    "TPR",
                    Strapd().
                    dLine("QuadKor:Trench Plate").
                    dLine("QK/Trench Plate Rental - Professional and Technical Services")
                    )
                Me.InsField(
                    "TGN",
                    Strapd().
                    dLine("QuadKor:Tecogen").
                    dLine("QK/Tecogen - Professional and Technical Services")
                    )
                Me.InsField(
                    enmCN.unknown.name,
                    Strapd().
                    dLine("*** UNKOWN")
                    )

                For Each strCLIENT In {"BBTS", "ADVRNT"}
                    For Each rowENTRY In Me.Sel(enmCA.client_abbrv, strCLIENT).SelAll
                        rowENTRY.v(enmCA.suffix_line) = "*** Billable = clear"
                    Next
                Next strCLIENT
            End Sub 'Apply_Defaults
        End Class 'sClientAbbrv

        Partial Class sReport_Detl
            Public Sub Apply(ur_client_table As Have.sClientAbbrv, ur_tempfield_cart As Have.sTempField, ur_reporthdr_table As Have.sReport_Hdr)
                For Each trwHDR In ur_reporthdr_table.SelAll
                    Dim strDATE_GRP = trwHDR.v(enmRP.date_grp)
                    Dim strCLIENT_GRP = trwHDR.v(enmRP.client_grp)
                    For Each kvpTASK_CHUNK In ur_tempfield_cart.Sel(enmTF.date_entry, strDATE_GRP).Sel(enmTF.client_abbrv, strCLIENT_GRP).Index_TableSplit(enmTF.task_title).kvp
                        Dim trwTASK_ROW = kvpTASK_CHUNK.l.SelFirst
                        Dim trwDETL = New Have.rReport_Detl
                        trwDETL.vt(enmRT.date_grp, strDATE_GRP)
                        trwDETL.vt(enmRT.client_grp, strCLIENT_GRP)
                        trwDETL.vt(enmRT.task_entry, trwTASK_ROW.v(enmTF.task_title).Trim())
                        Me.Ins(trwDETL)
                        With 1 : Dim lstCTASK = trwTASK_ROW.Link_Table(ur_tempfield_cart.SelAll, enmTF.client_abbrv, enmTF.task_title)
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
                Next trwHDR
            End Sub 'Apply
        End Class 'sReport_Detl

        Partial Class sReport_Hdr
            Public Sub Apply(ur_clientabbrv_cart As Have.sClientAbbrv, ur_tempfield_cart As Have.sTempField)
                Dim strDISP_UNKNOWN = ur_clientabbrv_cart.SelKey(enmCN.unknown).v(enmCA.disp_lines)
                For Each kvpDATE In ur_tempfield_cart.SelNE(enmTF.date_entry, mt).Index_TableSplit(enmTF.date_entry).kvp
                    Dim strDATE_GRP = kvpDATE.v
                    Dim lstDATE = kvpDATE.l
                    For Each kvpDCLIENT In lstDATE.SelNE(enmTF.client_abbrv, mt).Index_TableSplit(enmTF.client_abbrv).kvp
                        Dim strCLIENT_GRP = kvpDCLIENT.v
                        Dim lstDCLIENT = kvpDCLIENT.l
                        'unique date/client combination
                        'task list
                        'total hours tabbed-over by date
                        Dim recABBRV = ur_clientabbrv_cart.Sel(enmCA.client_abbrv, strCLIENT_GRP).SelFirst
                        Dim strDISP_LINE = recABBRV.v(enmCA.disp_lines)
                        If HasText(strDISP_LINE) = False Then
                            strDISP_LINE = strDISP_UNKNOWN & ": " & strCLIENT_GRP
                        End If

                        Dim trwHDR = New Have.rReport_Hdr
                        trwHDR.vt(enmRP.date_grp, strDATE_GRP)
                        trwHDR.vt(enmRP.client_grp, strCLIENT_GRP)
                        trwHDR.v(enmRP.disp_lines) = strDISP_LINE
                        trwHDR.v(enmRP.suffix_line) = recABBRV.v(enmCA.suffix_line)
                        Me.Ins(trwHDR)

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
            End Sub 'ur_tempfield_table
        End Class 'sReport_Hdr

        Partial Class sTempField
            Public Sub Apply(ur_in_text As enmUN.zfrom_clipboard)
                Dim strFOUND_TEXT = Have.UserBowl.SelKey(ur_in_text).v(enmUB.contents)
                Dim intCUR_COL = 1
                Dim trwLINE = New Have.rTempField()
                Me.Ins(trwLINE)

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
                            trwLINE = New Have.rTempField()
                            Me.Ins(trwLINE)
                    End Select 'strCHUNK_TYPE
                Next strLINE

                If Me.SelAll.Count = 1 Then
                    Throw New System.Exception("Temp Field table not found: Length = " & strFOUND_TEXT.Length)

                Else
                    Dim trwFIRST = Me.SelFirst
                    Dim trwTITLE = New Have.rTempField
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

                    For Each trwENTRY In Me.SelAll
                        trwENTRY.vt(enmTF.notes_text, Trim(trwENTRY.v(enmTF.notes_text)))
                    Next
                End If 'Me
            End Sub 'Apply ur_in_text

            Public Sub Clear_UnusedTimes()
                Me.Del(enmTF.hrs_dec, mt)
            End Sub 'Clear_UnusedTimes
        End Class 'sTempField

        Partial Class sUserBowl
            Public Function Apply(ur_userbowl_text As enmUN.zreport_output, ur_userbowl_appname As enmUN.zapp_name, ur_messagebox_cart As Have.sMessageBox) As enmUN.zfrom_messagebox
                Dim retKEY = enmUN.from_messagebox
                Apply = retKEY
                Dim ins_msgbox = ur_messagebox_cart.Ins(
                    New Have.rMessageBox().
                        vt(enmMB.text, Me.SelKey(ur_userbowl_text).v(enmUB.contents)).
                        vt(enmMB.title, Me.SelKey(ur_userbowl_appname).v(enmUB.contents)),
                    MsgBoxStyle.OkCancel
                    )

                If ins_msgbox.vUserResponse = MsgBoxResult.Ok Then
                    Me.InsKey(retKEY, enmUR.Ok)
                End If
            End Function 'Apply(ur_messagebox_cart

            Public Function Apply(ur_reporthdr_cart As Have.sReport_Hdr, ur_reportdetl_cart As Have.sReport_Detl) As enmUN.zreport_output
                Dim retKEY = enmUN.report_output
                Apply = retKEY
                Dim stpRET = Strapd()
                For Each kvpREPORT In ur_reporthdr_cart.SelAll.kvp
                    Dim trwREPORT = kvpREPORT.row
                    stpRET.dLine(trwREPORT.v(enmRP.disp_lines))

                    For Each kvpDETL In trwREPORT.Link_Table(ur_reportdetl_cart, enmRP.date_grp, enmRP.client_grp).SelAll
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

                Me.InsKey(enmUN.report_output, stpRET.ToString)
            End Function 'Apply(ur_reporthdr_cart

            Public Function Apply(ur_windows_clipboard_cart As Have.sClipboard) As enmUN.zfrom_clipboard
                Dim retKEY = enmUN.from_clipboard
                Apply = retKEY
                Dim ins_rec = ur_windows_clipboard_cart.InsFrom_Windows()
                Dim strFOUND_TEXT = ins_rec.v(enmCB.text)
                If HasText(strFOUND_TEXT) Then
                    Me.InsKey(retKEY, strFOUND_TEXT)

                Else
                    Throw New System.Exception("Text not found on clipboard: Length = " & strFOUND_TEXT.Length)
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


    Partial Public Class Have
        Private Shared tblClientAbbrv As sClientAbbrv
        Private Shared tblReport_Hdr As sReport_Hdr
        Private Shared tblReport_Detl As sReport_Detl
        Private Shared tblTempField As sTempField
        Private Shared tblTempLine As sTempLine
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.tblClientAbbrv = New sClientAbbrv
                Have.tblReport_Hdr = New sReport_Hdr
                Have.tblReport_Detl = New sReport_Detl
                Have.tblTempField = New sTempField
                Have.tblTempLine = New sTempLine
                Have.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have


    Public Class enmCA
        Inherits bitBASE
        Public Shared client_abbrv As enmCA = TRow(Of enmCA).glbl.NewBitBase()
        Public Shared disp_lines As enmCA = TRow(Of enmCA).glbl.NewBitBase()
        Public Shared suffix_line As enmCA = TRow(Of enmCA).glbl.NewBitBase()
    End Class

    Public Class enmCN
        Inherits bitBASE
        Public Shared unknown As enmCN = TRow(Of enmCN).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function ClientAbbrv() As sClientAbbrv
            Call Have.Connect()
            ClientAbbrv = Have.tblClientAbbrv
        End Function

        Public Class rClientAbbrv
            Inherits TRow(Of enmCA)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmCA, ur_val As String) As rClientAbbrv
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rClientAbbrv

        Public Class sClientAbbrv
            Private ttb As Objlist(Of rClientAbbrv)
            Public PK As Objlist(Of enmCA)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rClientAbbrv)
                Me.PK = New Objlist(Of enmCA)
                Me.PK.Add(enmCA.client_abbrv)
            End Sub 'New

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rClientAbbrv) As rClientAbbrv
                Ins = ur_from
                Dim ttbSEL = Me
                Dim stpPK_LIST = Strapd()
                For Each keyPK In Me.PK
                    Dim strCUR_KEY = ur_from.v(keyPK)
                    If stpPK_LIST.Length > 0 Then stpPK_LIST.d(", ")
                    stpPK_LIST.d(strCUR_KEY)
                    If HasText(strCUR_KEY) = False Then
                        Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("PK value must have data:").dS(keyPK.name))
                    Else
                        ttbSEL = ttbSEL.Sel(keyPK, ur_from.v(keyPK))
                    End If
                Next keyPK

                If ttbSEL.SelAll.Count > 0 Then
                    Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("must have unique PK values:").dS(stpPK_LIST.ToString))
                Else
                    Me.ttb.Add(ur_from)
                End If
            End Function 'Ins

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsField(ur_client_abbrv As String, ur_disp_lines As String) As rClientAbbrv
                Dim retREC = New rClientAbbrv
                InsField = retREC
                retREC.v(enmCA.client_abbrv) = ur_client_abbrv
                retREC.v(enmCA.disp_lines) = ur_disp_lines
                Me.Ins(retREC)
            End Function 'InsField

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmCA, ur_value As String) As sClientAbbrv
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
            Public Function SelKey(ur_key As enmCN) As rClientAbbrv
                Dim ret As rClientAbbrv = Nothing
                Dim strKEY = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmCA.client_abbrv), strKEY) Then
                        ret = row
                        Exit For
                    End If
                Next

                If ret Is Nothing Then
                    ret = New rClientAbbrv
                    ret.vt(enmCA.client_abbrv, strKEY)
                    Me.ttb.Add(ret)
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
        End Class 'sClientAbbrv
    End Class 'CA, CN

    Public Class enmRP
        Inherits bitBASE
        Public Shared date_grp As enmRP = TRow(Of enmRP).glbl.NewBitBase()
        Public Shared client_grp As enmRP = TRow(Of enmRP).glbl.NewBitBase()
        Public Shared disp_lines As enmRP = TRow(Of enmRP).glbl.NewBitBase()
        Public Shared suffix_line As enmRP = TRow(Of enmRP).glbl.NewBitBase()
        Public Shared weekday_entry As enmRP = TRow(Of enmRP).glbl.NewBitBase()
        Public Shared total_hours As enmRP = TRow(Of enmRP).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function Report_Hdr() As sReport_Hdr
            Call Have.Connect()
            Report_Hdr = Have.tblReport_Hdr
        End Function

        Public Class rReport_Hdr
            Inherits TRow(Of enmRP)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmRP, ur_val As String) As rReport_Hdr
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Link_Table(ur_table As Objlist(Of Have.rReport_Hdr), ParamArray ur_key_list() As enmRP) As sReport_Hdr
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
            Public Shadows Function Link_Table(ur_child_table As Have.sReport_Detl, ParamArray ur_key_list() As enmRP) As sReport_Detl
                Dim tblRET = New sReport_Detl
                Link_Table = tblRET
                Dim sdpXLAT = New SdPair(Of enmRT)
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
            Public pk As Objlist(Of enmRP)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rReport_Hdr)
                Me.pk = New Objlist(Of enmRP)
                pk.Add(enmRP.client_grp)
                pk.Add(enmRP.date_grp)
            End Sub 'New

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Del(ur_col As enmRP, ur_val As String)
                For ROWCTR = Me.ttb.Count To 1 Step -1
                    If AreEqual(Me.ttb.tr_b1(ROWCTR).v(ur_col), ur_val) Then
                        Me.ttb.RemoveAt(b0(ROWCTR))
                    End If
                Next ROWCTR
            End Sub 'Del

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Index_TableSplit(ur_key As enmRP) As SdPair(Of sReport_Hdr)
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
                Dim ttbSEL = Me
                Dim stpPK_LIST = Strapd()
                For Each keyPK In Me.pk
                    Dim strCUR_KEY = ur_from.v(keyPK)
                    If stpPK_LIST.Length > 0 Then stpPK_LIST.d(", ")
                    stpPK_LIST.d(strCUR_KEY)
                    If HasText(strCUR_KEY) = False Then
                        Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("PK value must have data:").dS(keyPK.name))
                    Else
                        ttbSEL = ttbSEL.Sel(keyPK, ur_from.v(keyPK))
                    End If
                Next keyPK

                If ttbSEL.SelAll.Count > 0 Then
                    Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("must have unique PK values:").dS(stpPK_LIST.ToString))
                Else
                    Me.ttb.Add(ur_from)
                End If
            End Function 'Ins

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmRP, ur_value As String) As sReport_Hdr
                Dim retROW = New sReport_Hdr
                Sel = retROW
                For Each rowCART In Me.ttb
                    If AreEqual(rowCART.v(ur_col), ur_value) Then
                        retROW.Ins(rowCART)
                    End If
                Next
            End Function 'Sel

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


    Public Class enmRT
        Inherits bitBASE
        Public Shared date_grp As enmRT = TRow(Of enmRT).glbl.NewBitBase()
        Public Shared client_grp As enmRT = TRow(Of enmRT).glbl.NewBitBase()
        Public Shared task_entry As enmRT = TRow(Of enmRT).glbl.NewBitBase()
        Public Shared task_daysuffix As enmRT = TRow(Of enmRT).glbl.NewBitBase()
        Public Shared task_notes As enmRT = TRow(Of enmRT).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function Report_Detl() As sReport_Detl
            Call Have.Connect()
            Report_Detl = Have.tblReport_Detl
        End Function

        Public Class rReport_Detl
            Inherits TRow(Of enmRT)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmRT, ur_val As String) As rReport_Detl
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Link_Table(ur_table As Objlist(Of Have.rReport_Detl), ParamArray ur_key_list() As enmRT) As sReport_Detl
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
            Public PK As Objlist(Of enmRT)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rReport_Detl)
                Me.PK = New Objlist(Of enmRT)
                Me.PK.Add(enmRT.client_grp)
                Me.PK.Add(enmRT.date_grp)
                Me.PK.Add(enmRT.task_entry)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Del(ur_col As enmRT, ur_val As String)
                For ROWCTR = Me.ttb.Count To 1 Step -1
                    If AreEqual(Me.ttb.tr_b1(ROWCTR).v(ur_col), ur_val) Then
                        Me.ttb.RemoveAt(b0(ROWCTR))
                    End If
                Next ROWCTR
            End Sub 'Del

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Index_TableSplit(ur_key As enmRT) As SdPair(Of sReport_Detl)
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
                Dim ttbSEL = Me
                Dim stpPK_LIST = Strapd()
                For Each keyPK In Me.PK
                    Dim strCUR_KEY = ur_from.v(keyPK)
                    If stpPK_LIST.Length > 0 Then stpPK_LIST.d(", ")
                    stpPK_LIST.d(strCUR_KEY)
                    If HasText(strCUR_KEY) = False Then
                        Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("PK value must have data:").dS(keyPK.name))
                    Else
                        ttbSEL = ttbSEL.Sel(keyPK, ur_from.v(keyPK))
                    End If
                Next keyPK

                If ttbSEL.SelAll.Count > 0 Then
                    Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("must have unique PK values:").dS(stpPK_LIST.ToString))
                Else
                    Me.ttb.Add(ur_from)
                End If
            End Function 'Ins

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsNew() As rReport_Detl
                Dim trwNEW = New rReport_Detl
                InsNew = trwNEW
                Me.Ins(trwNEW)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmRT, ur_value As String) As sReport_Detl
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


    Public Enum Abc
        date_entry
        start_time
    End Enum

    Public Enum Def
        hrs_dec
        txt_num
    End Enum

    Public Class enmTF
        Inherits bitBASE
        Public Shared date_entry As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared start_time As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared hrs_dec As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared tkt_num As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared blank_col As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared client_abbrv As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared task_num As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared task_title As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared notes_text As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared row_num As enmTF = TRow(Of enmTF).glbl.NewBitBase()
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

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function Link_Table(ur_table As Objlist(Of Have.rTempField), ParamArray ur_key_list() As enmTF) As sTempField
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
            Public PK As Objlist(Of enmTF)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rTempField)
                Me.PK = New Objlist(Of enmTF)
                Me.PK.Add(enmTF.row_num)
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
            Public Shadows Function Distinct(ur_key As enmTF) As Sdata
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
            Public Shadows Function Index_TableSplit(ur_key As enmTF) As SdPair(Of sTempField)
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
                Dim ttbSEL = Me
                Dim stpPK_LIST = Strapd()
                Me.ttb.Add(ur_from.vt(enmTF.row_num, Me.ttb.Count + 1))
            End Function 'Ins

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmTF, ur_value As String) As sTempField
                Dim retROW = New sTempField
                Sel = retROW
                For Each rowCART In Me.ttb
                    If AreEqual(rowCART.v(ur_col), ur_value) Then
                        retROW.Ins(rowCART)
                    End If
                Next
            End Function 'Sel

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
            Public Function SelNE(ur_col As enmTF, ur_value As String) As sTempField
                Dim retROW = New sTempField
                SelNE = retROW
                For Each rowCART In Me.ttb
                    If AreEqual(rowCART.v(ur_col), ur_value) = False Then
                        retROW.Ins(rowCART)
                    End If
                Next
            End Function 'SelNE

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

    Public Class enmTL
        Inherits bitBASE
        Public Shared line_seq As enmTL = TRow(Of enmTL).glbl.NewBitBase()
        Public Shared text As enmTL = TRow(Of enmTL).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempLine() As sTempLine
            Call Have.Connect()
            TempLine = Have.tblTempLine
        End Function

        Public Class rTempLine
            Inherits TRow(Of enmTL)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmTL, ur_val As String) As rTempLine
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rTempLine

        Public Class sTempLine
            Private ttb As Objlist(Of rTempLine)
            Public PK As Objlist(Of enmTL)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rTempLine)
                Me.PK = New Objlist(Of enmTL)
                Me.PK.Add(enmTL.line_seq)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rTempLine) As rTempLine
                Ins = ur_from
                Dim ttbSEL = Me
                Dim stpPK_LIST = Strapd()
                For Each keyPK In Me.PK
                    Dim strCUR_KEY = ur_from.v(keyPK)
                    If stpPK_LIST.Length > 0 Then stpPK_LIST.d(", ")
                    stpPK_LIST.d(strCUR_KEY)
                    If HasText(strCUR_KEY) = False Then
                        Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("PK value must have data:").dS(keyPK.name))
                    Else
                        ttbSEL = ttbSEL.Sel(keyPK, ur_from.v(keyPK))
                    End If
                Next keyPK

                If ttbSEL.SelAll.Count > 0 Then
                    Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("must have unique PK values:").dS(stpPK_LIST.ToString))
                Else
                    Me.ttb.Add(ur_from)
                End If
            End Function 'Ins

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

                        Me.Ins(New rTempLine().vt(enmTL.line_seq, kvpROW.Indexb1).vt(enmTL.text, strTEXT))
                    End If 'intFOUND
                Next kvpROW
            End Sub 'InsSplitAt_Keyword_Skip_Comma

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmTL, ur_value As String) As sTempLine
                Dim retROW = New sTempLine
                Sel = retROW
                For Each rowCART In Me.ttb
                    If AreEqual(rowCART.v(ur_col), ur_value) Then
                        retROW.Ins(rowCART)
                    End If
                Next
            End Function 'Sel

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
        Public Shared from_messagebox As zfrom_messagebox = TRow(Of enmUN).glbl.Trbase(Of zfrom_messagebox).NewBitBase() : Public Class zfrom_messagebox : Inherits enmUN : End Class
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
            Public PK As Objlist(Of enmUB)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rUserBowl)
                Me.PK = New Objlist(Of enmUB)
                Me.PK.Add(enmUB.bowl_name)
            End Sub 'New

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
                Dim ttbSEL = Me
                Dim stpPK_LIST = Strapd()
                For Each keyPK In Me.PK
                    Dim strCUR_KEY = ur_from.v(keyPK)
                    If stpPK_LIST.Length > 0 Then stpPK_LIST.d(", ")
                    stpPK_LIST.d(strCUR_KEY)
                    If HasText(strCUR_KEY) = False Then
                        Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("PK value must have data:").dS(keyPK.name))
                    Else
                        ttbSEL = ttbSEL.Sel(keyPK, ur_from.v(keyPK))
                    End If
                Next keyPK

                If ttbSEL.SelAll.Count > 0 Then
                    Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("must have unique PK values:").dS(stpPK_LIST.ToString))
                Else
                    Me.ttb.Add(ur_from)
                End If
            End Function 'Ins

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsKey(ur_key As enmUN, ur_val As String) As rUserBowl
                Dim ret = Me.SelKey(ur_key)
                InsKey = ret
                If HasText(ret.v(enmUB.contents)) Then
                    Throw New System.Exception("Cannot insert duplicate key for key: " & ur_key.name)
                Else
                    ret.vt(enmUB.contents, ur_val)
                End If
            End Function

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
                Dim retROW = New sUserBowl
                Sel = retROW
                For Each rowCART In Me.ttb
                    If AreEqual(rowCART.v(ur_col), ur_value) Then
                        retROW.Ins(rowCART)
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
                Next

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
