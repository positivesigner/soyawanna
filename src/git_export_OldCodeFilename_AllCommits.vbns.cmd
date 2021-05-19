@echo off
@SET mx_dir=%CD%
@SET countloops=20
:mx_search
@set /a countloops-=1
@FOR %%i IN ("%mx_dir%") DO IF EXIST %%~si\MxClasses\VBNetScript.exe GOTO mx_found
@FOR %%i IN ("%mx_dir%\..") DO SET mx_dir=%%~si
@IF %countloops%==0 GOTO mx_max_loops
@GOTO mx_search

:mx_max_loops
@ECHO Cannot find MxClasses\VBNetScript.exe within 20 parent directories
@PAUSE
@GOTO mx_end

:mx_found
@cd %mx_dir%
@START "" "%mx_dir%\MxClasses\VBNetScript.exe" /path=%0

:mx_end
@EXIT
MxClasses\MxBaseEc13.vb
Call Mx.GLog.LoadLog()
RetVal = "QUIT"
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = ""
'            Call Mx.GLog.LoadLog()
'            If RetVal <> "" Then MsgBox(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = "UrFolder"
'        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
'    End Class
'End Namespace 'Mx

Namespace Mx
    Public Class eprCL
        Inherits bitBASE
        Public Shared server As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared appdb As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared formdb As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared objdb As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared reportdb As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared report_basepath As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared nointerface As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared root_dir As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared out_subdir As eprCL = TRow(Of eprCL).glbl.NewBitBase()
        Public Shared flag_sort_files As eprCL = TRow(Of eprCL).glbl.NewBitBase()
    End Class

    Public Class CommandInput
        Inherits TRow(Of eprCL)
    End Class 'CommandInput


    Public Class eprST
        Inherits bitBASE
        Public Shared open As eprST = TRow(Of eprST).glbl.NewBitBase()
        Public Shared commit As eprST = TRow(Of eprST).glbl.NewBitBase()
        Public Shared author As eprST = TRow(Of eprST).glbl.NewBitBase()
        Public Shared commit_date As eprST = TRow(Of eprST).glbl.NewBitBase()
        Public Shared comment As eprST = TRow(Of eprST).glbl.NewBitBase()
        Public Shared file As eprST = TRow(Of eprST).glbl.NewBitBase()
    End Class

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

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New()
            Me.dteTIME_STAMP = System.DateTime.MinValue
        End Sub

        <System.Diagnostics.DebuggerHidden()>
        Public Function ToCopy() As Commit_Prop
            Dim sdnRET = New Commit_Prop
            ToCopy = sdnRET
            For Each enrVAL In Me.RefColKeys
                sdnRET.v(enrVAL) = Me.v(enrVAL)
            Next

            sdnRET.dteTIME_STAMP = Me.dteTIME_STAMP
        End Function 'ToCopy

        <System.Diagnostics.DebuggerHidden()>
        Public Function ToTSV() As String
            Dim stpRET = Strapd()
            For Each kvpVAL In Me.kvp
                If kvpVAL.Indexb1 > 1 Then
                    stpRET.d(Constants.vbTab)
                End If

                stpRET.d(kvpVAL.v.Replace(Constants.vbTab, mt).Replace(Constants.vbCr, mt).Replace(Constants.vbLf, mt))
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

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New()
            Me.dteTIME_STAMP = System.DateTime.MinValue
        End Sub
    End Class 'Folder_Prop

    Public Class GLog
        Public Shared Sub LoadLog()
            Dim ret_msg = Strapd()
            Dim flnPERSIST = FileNamed().d(Strapd().d(glbl.gEnvironment.ExpandEnvironmentVariables("%APPDATA%")).dSprtr("\", "DevCustomApp_settings").dSprtr("\", "user.config.tsv"))
            Have.UserBowl.SelKey(enmUN.persist_path).Contents = flnPERSIST
            Call Have.CustomApp.PersistRead(flnPERSIST)

            Dim strAPP_NAME = Have.UserBowl.SelKey(enmUN.app_name).Contents
            Dim strGIT_PATH = mt
            Dim strPREFIX = mt
            Dim strLOGQ_FOLDER = mt
            Dim strDATE_AFTER = mt
            Dim flnROOT_PATH = FileNamed()
            Dim flnOUT_PATH = FileNamed()

            For Each rowENTRY In Have.CustomApp.Sel(enmSG.setting_name, "git_path").SelAll
                strGIT_PATH = rowENTRY.v(enmSG.setting_value)
            Next

            If HasText(strGIT_PATH) = False Then
                ret_msg.d("Cannot find DeveloperCustomApp_settings for git_path")
            End If

            If ret_msg.HasText = False Then
                flnROOT_PATH.d(Have.UserBowl.SelKey(enmUN.script_folder).Contents)
                flnOUT_PATH = flnROOT_PATH.gCopy
                If glbl.gWindowsFS.HasDir(flnROOT_PATH.gCopy.d(".git")) Then
                    flnROOT_PATH = flnROOT_PATH
                    strPREFIX = flnROOT_PATH.Name
                    flnOUT_PATH.dNowTextYMDHMS("git_export_log").dAppendEXT("tsv")

                ElseIf glbl.gWindowsFS.HasDir(flnROOT_PATH.gParentDir.d(".git")) Then
                    flnROOT_PATH = flnROOT_PATH.gParentDir
                    strPREFIX = flnROOT_PATH.Name
                    flnOUT_PATH.dNowTextYMDHMS("git_export_log").dAppendEXT("tsv")

                Else
                    strPREFIX = InputBox("Enter a folder prefix (like tpr):", strAPP_NAME)
                    If HasText(strPREFIX) Then
                        flnROOT_PATH.d(strPREFIX & "_gotoassisttickets")
                        flnOUT_PATH.dNowTextYMDHMS("git_export_log").dAppendEXT("tsv")
                    End If
                End If 'flnROOT_PATH
                If HasText(strPREFIX) = False Then
                    ret_msg.d("Folder prefix not found")
                End If
            End If

            If ret_msg.HasText = False Then
                strDATE_AFTER = Today.AddDays(-1).ToString("MM-dd-yyyy")
                strDATE_AFTER = InputBox("Enter date after, or leave blank for all dates", strAPP_NAME, strDATE_AFTER)
                If strDATE_AFTER <> "" Then
                    Dim intINDEX = InStr(strDATE_AFTER, s)
                    If intINDEX > 0 Then
                        strDATE_AFTER = Mid(strDATE_AFTER, 1, intINDEX - 1)
                    End If

                    strDATE_AFTER = "--after=" & strDATE_AFTER
                End If

                strLOGQ_FOLDER = prv.Get_LogQFolder(flnOUT_PATH)
                If HasText(flnROOT_PATH.FilePath) = False OrElse AreEqual(flnROOT_PATH.Name, FileNamed().d(Mx.Class1.SourceFolder).Name) Then
                    ret_msg.d("Repository folder not found")
                End If
            End If

            If ret_msg.HasText = False Then
                Dim strUSER_FILENAME = InputBox("Enter whole or partial file name", strAPP_NAME)
                If HasText(strUSER_FILENAME) Then
                    Dim strLINES = glbl.gDiagnostics.Read_CommandLineText(strGIT_PATH, Strapd().d("-C").dS().dSprtr(qs, flnROOT_PATH).d(qs).dS("--no-pager log --full-history --name-only").dS(strDATE_AFTER))
                    Dim sdaPLIST = prv.Parse_CommitFiles(strLINES, strUSER_FILENAME)
                    If sdaPLIST.Count > 0 Then
                        Call GLog.Export_DailyCommits(sdaPLIST, strGIT_PATH, flnROOT_PATH, strLOGQ_FOLDER)
                    End If

                    ret_msg.d(sdaPLIST.Count.ToString).dS(" files in log")
                End If
            End If

            If ret_msg.HasText Then
                MsgBox(ret_msg, , strAPP_NAME)
            End If
        End Sub 'LoadLog

        Public Shared Sub Export_DailyCommits(ur_cprop_list As Objlist(Of Commit_Prop), ur_git_path As String, ur_root_path As String, ur_logq_path As String)
            For Each kvpCPROP In ur_cprop_list.kvp
                Dim sdnCPROP = kvpCPROP.row
                Dim strFULL_PATH = sdnCPROP.v(eprCMT.full_win_path)
                Dim strCUR_DATE = sdnCPROP.v(eprCMT.commit_date)
                Dim strOUTPUT_PATH = System.IO.Path.Combine(ur_logq_path, "Old Code" & s & strCUR_DATE & "\" & strFULL_PATH)
                Dim strPARENT_DIR = System.IO.Path.GetDirectoryName(strOUTPUT_PATH)
                If System.IO.Directory.Exists(strPARENT_DIR) = False Then
                    System.IO.Directory.CreateDirectory(strPARENT_DIR)
                End If

                Dim P As New System.Diagnostics.Process()
                With 1 : Dim prcINFO = P.StartInfo
                    prcINFO.FileName = "cmd"
                    prcINFO.Arguments = String.Format("/c {0} -C ""{1}"" show {2}:""{3}"" > ""{4}""", ur_git_path, ur_root_path, sdnCPROP.v(eprCMT.commit_sha), sdnCPROP.v(eprCMT.full_unix_path), strOUTPUT_PATH)
                    prcINFO.UseShellExecute = False
                    prcINFO.RedirectStandardOutput = False
                    prcINFO.CreateNoWindow = True
                End With 'prcINFO

                P.Start()
                P.WaitForExit()
                Call System.IO.File.SetLastWriteTime(strOUTPUT_PATH, sdnCPROP.dteTIME_STAMP)
            Next kvpCPROP
        End Sub 'Export_DailyCommits

        Private Class prv
            Public Shared Function Get_LogQFolder(ur_out_file As MxText.FileName) As MxText.FileName
                Get_LogQFolder = ur_out_file.gParentDir.dNowTextYMDHMS("LogQ")
            End Function

            Public Shared Function Parse_CommitFiles(ur_commit_log_text As String, ur_file_name As String) As Objlist(Of Commit_Prop)
                Dim lstFILE_PROP = New Objlist(Of Commit_Prop)
                Parse_CommitFiles = lstFILE_PROP
                Dim strEXPORT_ONE_DAY = mt
                Dim sdaTEXT = Sdata.Split(ur_commit_log_text.Replace(Chr(13), mt), Chr(10))
                Dim sdnCOMMIT_PROP As Commit_Prop = Nothing
                Dim sdnFILE_PROP As Commit_Prop = Nothing
                Dim objSTATE = eprST.open
                For Each strLINE In sdaTEXT
                    If strLINE <> "" Then
                        If objSTATE Is eprST.commit Then
                            If strLINE.ToLower.StartsWith("author:") Then
                                sdnCOMMIT_PROP.v(eprCMT.author) = Mid(strLINE, Len("author:") + 1)
                                objSTATE = eprST.author
                            End If 'strLINE

                        ElseIf objSTATE Is eprST.author Then
                            If strLINE.ToLower.StartsWith("date:") Then
                                sdnCOMMIT_PROP.v(eprCMT.commit_date) = Mid(strLINE, Len("Date:") + 1)
                                If prv.Ret_Date(sdnCOMMIT_PROP.v(eprCMT.commit_date), sdnCOMMIT_PROP.dteTIME_STAMP) Then
                                    sdnCOMMIT_PROP.v(eprCMT.commit_date) = sdnCOMMIT_PROP.dteTIME_STAMP.ToString("yyyy'm'MM'd'dd HH'm'mm's'ss")
                                Else
                                    sdnCOMMIT_PROP.v(eprCMT.commit_date) = mt
                                End If

                                objSTATE = eprST.commit_date
                            End If 'strLINE

                        ElseIf objSTATE Is eprST.commit_date OrElse objSTATE Is eprST.comment OrElse objSTATE Is eprST.file OrElse objSTATE Is eprST.open Then
                            If strLINE.ToLower.StartsWith("commit") Then
                                objSTATE = eprST.commit
                                sdnCOMMIT_PROP = New Commit_Prop
                                sdnCOMMIT_PROP.v(eprCMT.commit_sha) = LTrim(Mid(strLINE, Len("commit:") + 1))


                            ElseIf Left(strLINE, 1) = Space(1) Then
                                If HasText(sdnCOMMIT_PROP.v(eprCMT.comment_line)) = False Then
                                    sdnCOMMIT_PROP.v(eprCMT.comment_line) = LTrim(strLINE)
                                End If

                                objSTATE = eprST.comment

                            ElseIf strLINE <> "" Then
                                If sdnCOMMIT_PROP IsNot Nothing Then
                                    Dim intPOS = InStr(strLINE.ToUpper, ur_file_name.ToUpper)
                                    If intPOS > 0 AndAlso
                                           ContainingText(Mid(strLINE, intPOS), "/") = False Then
                                        sdnFILE_PROP = sdnCOMMIT_PROP.ToCopy
                                        sdnFILE_PROP.v(eprCMT.full_unix_path) = strLINE
                                        sdnFILE_PROP.v(eprCMT.full_win_path) = strLINE.Replace("/", "\")
                                        lstFILE_PROP.Add(sdnFILE_PROP)
                                    End If
                                End If 'sdnCOMMIT_PROP

                                objSTATE = eprST.file
                            End If 'strLINE
                        End If 'objSTATE
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

            Public Shared Function Ret_RootDateTime(ur_root_path As String) As String
                Dim strRET_ROOT_DATETIME = mt
                Dim strOLD_CODE = " Old Code "
                Dim idxOLD_CODE = InStr(ur_root_path, strOLD_CODE)
                If idxOLD_CODE > 0 Then
                    strRET_ROOT_DATETIME = Mid(ur_root_path, idxOLD_CODE + strOLD_CODE.Length).Replace("\", mt)
                End If

                Ret_RootDateTime = strRET_ROOT_DATETIME
            End Function 'Ret_RootDateTime

            Public Shared Function Ret_RootPath(ur_agr_list As String()) As String
                Dim strROOT_PATH = ur_agr_list(ur_agr_list.Length - 1).Trim.Replace(qs, mt)
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
    End Class 'GLog


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

            <System.Diagnostics.DebuggerHidden()>
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
