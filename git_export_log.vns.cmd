start "" "%~dp0\MxClasses\VBNetScript.exe" /path=%0
exit
MxClasses\MxBaseEc13.vb
Call Mx.Want.LoadLog_errhnd()
RetVal = "QUIT"
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.LoadLog_errhnd()
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
        Public Shared Sub LoadLog(ret_msg As Strap)
            Dim flnPERSIST = FileNamed().d(Strapd().d(glbl.gEnvironment.ExpandEnvironmentVariables("%APPDATA%")).dSprtr("\", "DeveloperCustomApp_settings").dSprtr("\", "user.config.tsv"))
            Have.UserBowl.SelKey(enmUN.persist_path).Contents = flnPERSIST
            Call Have.CustomApp.PersistRead(flnPERSIST)

            Dim strAPP_NAME = Have.UserBowl.SelKey(enmUN.app_name).Contents
            Dim strGIT_PATH = mt
            Dim strPREFIX = mt
            For Each rowENTRY In Have.CustomApp.Sel(enmSG.setting_name, "git_path").SelAll
                strGIT_PATH = rowENTRY.v(enmSG.setting_value)
            Next

            If HasText(strGIT_PATH) = False Then
                ret_msg.d("Cannot find DeveloperCustomApp_settings for git_path")
            End If

            If ret_msg.HasText = False Then
                Dim flnROOT_PATH = FileNamed().d(Have.UserBowl.SelKey(enmUN.script_folder).Contents)
				Dim flnOUT_PATH = flnROOT_PATH.gCopy
                If glbl.gWindowsFS.HasDir(flnROOT_PATH.gCopy.d(".git")) Then
                    flnROOT_PATH = flnROOT_PATH
                    strPREFIX = flnROOT_PATH.Name
					flnOUT_PATH.d(flnOUT_PATH.Name & s & "log.tsv")

                Else
                    strPREFIX = InputBox("Enter a folder prefix (like tpr):", "Git Export Log")
					If HasText(strPREFIX) Then
						flnROOT_PATH.d(strPREFIX & "_gotoassisttickets")
						flnOUT_PATH.d(flnROOT_PATH.Name & s & "log.tsv")
					End If
                End If 'flnROOT_PATH

                If HasText(strPREFIX) Then
                    Dim strDATE_AFTER = Today.AddDays(-1).ToString("MM-dd-yyyy")
                    strDATE_AFTER = InputBox("Enter date after, or leave blank for all dates", strAPP_NAME, strDATE_AFTER)
                    If strDATE_AFTER <> "" Then
                        Dim intINDEX = InStr(strDATE_AFTER, s)
                        If intINDEX > 0 Then
                            strDATE_AFTER = Mid(strDATE_AFTER, 1, intINDEX - 1)
                        End If

                        strDATE_AFTER = " --after=" & strDATE_AFTER
                    End If

                    Dim strLINES = glbl.gDiagnostics.Read_CommandLineText(strGIT_PATH, Strapd().d("-C").dS(qs).d(flnROOT_PATH).d(qs).dS("--no-pager log").dS("--full-history").dS("--name-only").d(strDATE_AFTER))
                    Dim sdaPLIST = prv.Parse_CommitFiles(strLINES)
                    Using stmOUT_FILE = glbl.gWindowsFS.WriteStream(flnOUT_PATH)
                        For Each sdnCPROP In sdaPLIST
                            stmOUT_FILE.WriteLine(sdnCPROP.ToTSV)
                        Next sdnCPROP
                    End Using

                    Call glbl.gMsgBox.GetResult(Strapd().d(sdaPLIST.Count).dS("files in log"), MsgBoxStyle.OkOnly, strAPP_NAME)
                End If 'strPREFIX
            End If 'ret_msg
        End Sub 'LoadLog

        Public Shared Function LoadLog_errhnd() As Strap
            Dim stpRET = Strapd()
            LoadLog_errhnd = stpRET
            Dim objERR_LIST = New ErrListBase : Try
                Call LoadLog(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If

            If stpRET.HasText = False Then
                stpRET.d("QUIT")
            End If
        End Function 'LoadLog_errhnd

        Private Class prv
            Public Shared Function Parse_CommitFiles(ur_commit_log_text As String) As Objlist(Of Commit_Prop)
                Dim lstFILE_PROP = New Objlist(Of Commit_Prop)
                Parse_CommitFiles = lstFILE_PROP
                Dim sdaTEXT = Sdata.Split(ur_commit_log_text.Replace(Chr(13), mt), Chr(10))
                Dim sdnCOMMIT_PROP As Commit_Prop = Nothing
                Dim sdnFILE_PROP As Commit_Prop = Nothing
                Dim intSTATE = enmST.open
                For Each strLINE In sdaTEXT
                    If strLINE <> "" Then
                        Select Case intSTATE
                            Case enmST.commit
                                If strLINE.ToLower.StartsWith("author:") Then
                                    sdnCOMMIT_PROP.v(eprCMT.author) = Mid(strLINE, Len("author:") + 1)
                                    intSTATE = enmST.author
                                End If 'strLINE

                            Case enmST.author
                                If strLINE.ToLower.StartsWith("date:") Then
                                    sdnCOMMIT_PROP.v(eprCMT.commit_date) = Mid(strLINE, Len("Date:") + 1)
                                    If prv.Ret_Date(sdnCOMMIT_PROP.v(eprCMT.commit_date), sdnCOMMIT_PROP.dteTIME_STAMP) Then
                                        sdnCOMMIT_PROP.v(eprCMT.commit_date) = sdnCOMMIT_PROP.dteTIME_STAMP.ToString("yyyy'm'MM'd'dd HH'm'mm's'ss")
                                    Else
                                        sdnCOMMIT_PROP.v(eprCMT.commit_date) = mt
                                    End If

                                    intSTATE = enmST.commit_date
                                End If 'strLINE

                            Case enmST.commit_date, enmST.comment, enmST.file, enmST.open
                                If strLINE.ToLower.StartsWith("commit") Then
                                    intSTATE = enmST.commit
                                    sdnCOMMIT_PROP = New Commit_Prop
                                    sdnCOMMIT_PROP.v(eprCMT.commit_sha) = LTrim(Mid(strLINE, Len("commit:") + 1))


                                ElseIf Left(strLINE, 1) = Space(1) Then
                                    If HasText(sdnCOMMIT_PROP.v(eprCMT.comment_line)) = False Then
                                        sdnCOMMIT_PROP.v(eprCMT.comment_line) = LTrim(strLINE)
                                    End If

                                    intSTATE = enmST.comment

                                ElseIf strLINE <> "" Then
                                    If sdnCOMMIT_PROP IsNot Nothing Then
                                        sdnFILE_PROP = sdnCOMMIT_PROP.ToCopy
                                        sdnFILE_PROP.v(eprCMT.full_unix_path) = strLINE
                                        sdnFILE_PROP.v(eprCMT.full_win_path) = strLINE.Replace("/", "\")
                                        lstFILE_PROP.Add(sdnFILE_PROP)
                                    End If

                                    intSTATE = enmST.file
                                End If 'strLINE
                        End Select 'intSTATE
                    End If 'strLINE
                Next strLINE
            End Function 'Parse_CommitFiles

            Public Shared Function Ret_Date(ur_date_wmdhmsytz As String, ByRef ur_date As System.DateTime) As Boolean
                ur_date_wmdhmsytz = LTrim(ur_date_wmdhmsytz)
                Dim strMONTH = Ret_Month(Mid(ur_date_wmdhmsytz, 5, 3))
                Dim strDAY = Mid(ur_date_wmdhmsytz, 9, 2)
                Dim intOFFSET = 0
                If Mid(strDAY, 2) = s Then
                    intOFFSET = -1
                    strDAY = Trim(strDAY)
                End If

                Dim strHMS = Mid(ur_date_wmdhmsytz, 12 + intOFFSET, 8)
                Dim strYEAR = Mid(ur_date_wmdhmsytz, 21 + intOFFSET, 4)
                Dim strTZ = Mid(ur_date_wmdhmsytz, 26 + intOFFSET, 5)
                Dim strDATE_MDYHMSTZ = strMONTH & "/" & strDAY & "/" & strYEAR & Space(1) & strHMS & Space(1) & strTZ
                Ret_Date = System.DateTime.TryParse(strDATE_MDYHMSTZ, ur_date)
            End Function 'Ret_Date

            Public Shared Function Ret_Month(ur_month_3char As String) As String
                Ret_Month = "1"
                Select Case ur_month_3char
                    Case "Jan"
                        Ret_Month = "1"
                    Case "Feb"
                        Ret_Month = "2"
                    Case "Mar"
                        Ret_Month = "3"
                    Case "Apr"
                        Ret_Month = "4"
                    Case "May"
                        Ret_Month = "5"
                    Case "Jun"
                        Ret_Month = "6"
                    Case "Jul"
                        Ret_Month = "7"
                    Case "Aug"
                        Ret_Month = "8"
                    Case "Sep"
                        Ret_Month = "9"
                    Case "Oct"
                        Ret_Month = "10"
                    Case "Nov"
                        Ret_Month = "11"
                    Case "Dec"
                        Ret_Month = "12"
                End Select
            End Function 'Ret_Month

            Public Shared Function Ret_RootPath(ur_cmd_line As String) As String
                Dim agr_list = MxText.CSVSplit(ur_cmd_line, s)
                Dim strROOT_PATH = agr_list.Item(agr_list.Count - 1).Trim.Replace(qs, mt)
                If strROOT_PATH.ToLower.EndsWith(".cmd") OrElse
                   strROOT_PATH.ToLower.EndsWith(".exe") Then
                    strROOT_PATH = System.IO.Path.GetDirectoryName(strROOT_PATH)
                End If

                If strROOT_PATH.ToLower.EndsWith("\bin\debug") Then
                    strROOT_PATH = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(strROOT_PATH))
                End If

                Ret_RootPath = strROOT_PATH
            End Function 'Ret_RootPath
        End Class 'prv  
    End Class 'Want

    Partial Public Class Have
        Private Shared tblCustomApp As sCustomApp
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.tblUserBowl = New sUserBowl
                Have.tblCustomApp = New sCustomApp
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have

    Public Enum enmST
        open
        commit
        author
        commit_date
        comment
        file
    End Enum

    Public Class eprCMT
        Inherits bitBASE
        Public Shared full_win_path As eprCMT = TRow(Of eprCMT).glbl.NewBitBase()
        Public Shared full_unix_path As eprCMT = TRow(Of eprCMT).glbl.NewBitBase()
        Public Shared author As eprCMT = TRow(Of eprCMT).glbl.NewBitBase()
        Public Shared commit_sha As eprCMT = TRow(Of eprCMT).glbl.NewBitBase()
        Public Shared commit_date As eprCMT = TRow(Of eprCMT).glbl.NewBitBase()
        Public Shared comment_line As eprCMT = TRow(Of eprCMT).glbl.NewBitBase()
    End Class

    Public Class Commit_Prop
        Inherits TRow(Of eprCMT)

        Public dteTIME_STAMP As System.DateTime

        <System.Diagnostics.DebuggerHidden()> Public Sub New()
            Me.dteTIME_STAMP = System.DateTime.MinValue
        End Sub

        <System.Diagnostics.DebuggerHidden()> Public Function ToCopy() As Commit_Prop
            Dim sdnRET = New Commit_Prop
            ToCopy = sdnRET
            For Each kvpVAL In Me.kvp
                sdnRET.v_enm(kvpVAL.Indexenm) = kvpVAL.v
            Next

            sdnRET.dteTIME_STAMP = Me.dteTIME_STAMP
        End Function 'ToCopy

        <System.Diagnostics.DebuggerHidden()> Public Function ToTSV() As String
            Dim stpRET = Strapd()
            For Each kvpVAL In Me.RefColKeys.kvp
                If kvpVAL.row IsNot eprCMT.full_unix_path Then
                    If kvpVAL.Indexb1 > 1 Then
                        stpRET.d(Constants.vbTab)
                    End If

                    stpRET.d(Me.Item(kvpVAL.row.seq).Replace(Constants.vbTab, mt).Replace(Constants.vbCr, mt).Replace(Constants.vbLf, mt))
                End If
            Next kvpVAL

            ToTSV = stpRET
        End Function 'ToTSV
    End Class 'Commit_Prop

    Public Class eprFLD
        Inherits bitBASE
        Public Shared folder_name As eprFLD = TRow(Of eprFLD).glbl.NewBitBase()
        Public Shared commit_date As eprFLD = TRow(Of eprFLD).glbl.NewBitBase()
    End Class

    Public Class Folder_Prop
        Inherits TRow(Of eprFLD)

        Public dteTIME_STAMP As System.DateTime

        <System.Diagnostics.DebuggerHidden()> Public Sub New()
            Me.dteTIME_STAMP = System.DateTime.MinValue
        End Sub
    End Class 'Folder_Prop

    Public Class enmSG
        Inherits bitBASE
        Public Shared setting_name As enmSG = TRow(Of enmSG).glbl.NewBitBase()
        Public Shared setting_value As enmSG = TRow(Of enmSG).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function CustomApp() As sCustomApp
            Call Have.Connect()
            CustomApp = Have.tblCustomApp
        End Function 'CustomApp

        Public Class rCustomApp
            Inherits TRow(Of enmSG)
        End Class

        Public Class sCustomApp
            Inherits TablePKStr(Of enmSG, rCustomApp)

            Public Sub New()
                Call MyBase.New(1)
            End Sub
        End Class
    End Class 'enmSG

    Public Class enmUB
        Inherits bitBASE
        Public Shared bowl_name As enmUB = TRow(Of enmUB).glbl.NewBitBase()
        Public Shared contents As enmUB = TRow(Of enmUB).glbl.NewBitBase()
    End Class

    Public Class enmUN
        Inherits bitBASE
        Public Shared app_folder As zapp_folder = TRow(Of enmUN).glbl.Trbase(Of zapp_folder).NewBitBase() : Public Class zapp_folder : Inherits enmUN : End Class
        Public Shared app_name As zapp_name = TRow(Of enmUN).glbl.Trbase(Of zapp_name).NewBitBase() : Public Class zapp_name : Inherits enmUN : End Class
        Public Shared app_path As zapp_path = TRow(Of enmUN).glbl.Trbase(Of zapp_path).NewBitBase() : Public Class zapp_path : Inherits enmUN : End Class
        Public Shared cmdline_audit As zcmdline_audit = TRow(Of enmUN).glbl.Trbase(Of zcmdline_audit).NewBitBase() : Public Class zcmdline_audit : Inherits enmUN : End Class
        Public Shared cmdline_orig As zcmdline_orig = TRow(Of enmUN).glbl.Trbase(Of zcmdline_orig).NewBitBase() : Public Class zcmdline_orig : Inherits enmUN : End Class
        Public Shared cmdline_table As zcmdline_table = TRow(Of enmUN).glbl.Trbase(Of zcmdline_table).NewBitBase() : Public Class zcmdline_table : Inherits enmUN : End Class
        Public Shared script_folder As zscript_folder = TRow(Of enmUN).glbl.Trbase(Of zscript_folder).NewBitBase() : Public Class zscript_folder : Inherits enmUN : End Class
        Public Shared script_path As zscript_path = TRow(Of enmUN).glbl.Trbase(Of zscript_path).NewBitBase() : Public Class zscript_path : Inherits enmUN : End Class
        Public Shared persist_path As zpersist_path = TRow(Of enmUN).glbl.Trbase(Of zpersist_path).NewBitBase() : Public Class zpersist_path : Inherits enmUN : End Class
    End Class 'enmUN

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Dim bolFIRST_INIT = (Have.tblUserBowl Is Nothing)
            Call Have.Connect()
            UserBowl = Have.tblUserBowl
            If bolFIRST_INIT Then
                Call Have.tblUserBowl.Ins_CommandLine(System.Reflection.Assembly.GetExecutingAssembly, System.Environment.CommandLine)
                'Have.tblUserBowl.InsKey(enmUN.cmdline_audit, "1")
                Call Have.tblUserBowl.Cboard_CmdlineAudit()
            End If
        End Function 'UserBowl

        Public Class rUserBowl
            Inherits TRow(Of enmUB)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmUB, ur_val As String) As rUserBowl
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Property Contents() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enmUB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(ur_val As String)
                    Me.v(enmUB.contents) = ur_val
                End Set
            End Property
        End Class 'rUserBowl

        Public Class sUserBowl
            Inherits TablePKEnum(Of enmUB, enmUN, rUserBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Cboard_CmdlineAudit()
                If HasText(Me.SelKey(enmUN.cmdline_audit).Contents) Then
                    Dim strAUDIT = Me.ToString(True)
                    If MsgBox(strAUDIT, MsgBoxStyle.OkCancel, Me.SelKey(enmUN.app_name).Contents) = MsgBoxResult.Ok Then
                        My.Computer.Clipboard.SetText(strAUDIT)
                    End If
                End If
            End Sub 'Cboard_CmdlineAudit

            '<System.Diagnostics.DebuggerHidden()>
            Public Function Ins_CommandLine(ur_assembly_path As System.Reflection.Assembly, ur_command_line As String) As sUserBowl
                Ins_CommandLine = Me
                Dim flnAPP_PATH = FileNamed().d(ur_assembly_path.Location.Replace("\bin\Debug", ""))
                Me.SelKey(enmUN.app_path).vt(enmUB.contents, flnAPP_PATH)
                Me.SelKey(enmUN.app_name).vt(enmUB.contents, flnAPP_PATH.FileGroup)
                Me.SelKey(enmUN.app_folder).vt(enmUB.contents, flnAPP_PATH.gParentDir)

                Dim arlCMD_RET = MxText.Cmdline_UB(Of enmUN, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, ur_command_line)
                Me.SelKey(enmUN.cmdline_orig).vt(enmUB.contents, qs & System.Environment.CommandLine.Replace(qs, qs & qs) & qs)
                Me.SelKey(enmUN.cmdline_table).vt(enmUB.contents, qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(qs, qs & qs) & qs)
                For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                    For Each enmKEY In TRow(Of enmUN).glbl.RefKeySearch(rowFOUND.v(enmUB.bowl_name))
                        Me.SelKey(enmKEY).Contents = rowFOUND.v(enmUB.contents)
                    Next
                Next rowFOUND

                Me.SelKey(enmUN.script_path).Contents = Mx.Class1.SourcePath
                Dim flnSCRIPT_PATH = FileNamed().d(Mx.Class1.SourceFolder).gFullPath
                If AreEqual(flnSCRIPT_PATH.Name, "Debug") Then
                    flnSCRIPT_PATH = flnSCRIPT_PATH.gParentDir
                End If
                If AreEqual(flnSCRIPT_PATH.Name, "bin") Then
                    flnSCRIPT_PATH = flnSCRIPT_PATH.gParentDir
                End If

                Me.SelKey(enmUN.script_folder).Contents = flnSCRIPT_PATH
            End Function 'Ins_CommandLine

            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sUserBowl
    End Class 'UB, UN
End Namespace
