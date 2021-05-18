@echo off
@SET mx_dir=%CD%
@SET countloops=20
:mx_search
@set /a countloops-=1
@FOR %%i IN ("%mx_dir%") DO IF EXIST %%~si\src\MxClasses\VBNetScript.exe GOTO mx_found
@FOR %%i IN ("%mx_dir%\..") DO SET mx_dir=%%~si
@IF %countloops%==0 GOTO mx_max_loops
@GOTO mx_search

:mx_max_loops
@ECHO Cannot find src\MxClasses\VBNetScript.exe within 20 parent directories
@PAUSE
@GOTO mx_end

:mx_found
@cd %mx_dir%
@START "" "%mx_dir%\src\MxClasses\VBNetScript.exe" /path=%0

:mx_end
@EXIT
DLL_WinForm2019m09d13\System.Drawing.dll
DLL_WinForm2019m09d13\System.Windows.Forms.dll
./../MxBaseEc13.vb
RetVal = Mx.UserAction.BGTask_Status_Form_errhnd()
End Function '2021m05d09
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.UserAction.BGTask_Status_Form_errhnd()
'            If Mx.AreEqual(RetVal, "QUIT") = False AndAlso MsgBox(RetVal,, "") = MsgBoxResult.Ok Then Mx.glbl.gCboard.SetText(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = My.Application.Info.DirectoryPath
'        Public Shared SourcePath As String = My.Application.Info.DirectoryPath & "\TestCode.vbns.cmd"
'    End Class
'End Namespace 'Mx

Namespace Mx
    Public Class UserAction
        Public Class enfBG
            Inherits bitBASE
            Public Shared run_tests As zrun_tests = TRow(Of enfBG).glbl.Trbase(Of zrun_tests).NewBitBase() : Public Class zrun_tests : Inherits enfBG : End Class
        End Class

        Public Shared Sub BGTask_Status_Form(stpERROR As Strap)
            Dim fn_st_show_form = enfST.Show_Form
            Have.StatusForm.Result(enfST.Show_Form)
        End Sub
        Public Shared Function BGTask_Status_Form_errhnd() As Strap
            Dim stpRET = Strapd() : BGTask_Status_Form_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try
                Call BGTask_Status_Form(stpRET) : Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try
            If objERR_LIST.Found Then stpRET.Clear().d(objERR_LIST.ToString)
            If stpRET.HasText = False Then stpRET.d("QUIT")
        End Function 'BGTask_Status_Form_errhnd

        Public Shared Sub Result(ur_fn As enfBG.zrun_tests, ret_msg As Strap, worker As System.ComponentModel.BackgroundWorker)
            Dim message_written = False
            Dim test_list = TRow(Of Assistant.enfTEST).glbl.RefKeys
            Dim test_count = test_list.Count / 1.0
            Dim expected_result_bowlname = enkTB.expected_result
            Dim expected_table_bowlname = enkTB.expected_table
            Dim file_comment_bowlname = enkTB.file_comment
            Dim file_text_bowlname = enkTB.file_text
            For Each enfENTRY In test_list
                worker.ReportProgress(enfENTRY.seq / test_count, enfENTRY.name)
                Dim setup_value_count = Assistant.Store_CLine_Setup(
                    message_written,
                    enfENTRY,
                    file_comment_bowlname,
                    file_text_bowlname,
                    expected_result_bowlname,
                    expected_table_bowlname)

                message_written = Assistant.Test_CLine(
                    message_written,
                    ret_msg,
                    enfENTRY,
                    file_comment_bowlname,
                    file_text_bowlname,
                    expected_result_bowlname,
                    expected_table_bowlname)
            Next enfENTRY
        End Sub
        Public Shared Function Result_errhnd(ur_fn As enfBG.zrun_tests, worker As System.ComponentModel.BackgroundWorker) As enkTB.ztest_result
            Dim stpRET = Strapd() : Dim enkRET = enkTB.test_result : Result_errhnd = enkRET : Dim objERR_LIST = New ErrListBase : Try
                Call Result(ur_fn, stpRET, worker) : Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try
            If objERR_LIST.Found Then stpRET.Clear().d(objERR_LIST.ToString)
            Have.TempBowl.SelKey(enkRET).Contents = stpRET.ToString
        End Function 'Result(zrun_tests
    End Class 'UserAction


    Public Class Assistant
        Public Class enfTEST
            Inherits bitBASE
            Public Shared key_unquoted_value = TRow(Of enfTEST).glbl.NewBitBase()
            Public Shared key_quoted_value = TRow(Of enfTEST).glbl.NewBitBase()
            Public Shared two_keys = TRow(Of enfTEST).glbl.NewBitBase()
            Public Shared key_without_value = TRow(Of enfTEST).glbl.NewBitBase()
        End Class

        Public Shared Function Store_CLine_Setup(
            ur_message_written As Boolean,
            ur_fn As enfTEST,
            ur_file_comment_bowlname As enkTB.zfile_comment,
            ur_file_text_bowlname As enkTB.zfile_text,
            ur_exp_result_bowlname As enkTB.zexpected_result,
            ur_exp_table_bowlname As enkTB.zexpected_table
            ) As Integer
            Store_CLine_Setup = 0
            If ur_message_written = False Then
                Store_CLine_Setup = 3
                If ur_fn Is enfTEST.key_unquoted_value Then
                    Have.TempBowl.SelKey(ur_file_comment_bowlname).Contents = "There is a key with an unquoted value."

                    Dim stpFILE = Strapd().d(
                    "/key_e1=val_e1")
                    Have.TempBowl.SelKey(ur_file_text_bowlname).Contents = stpFILE

                    Dim stpEXPECTED_MESSAGE = Strapd()
                    Have.TempBowl.SelKey(ur_exp_result_bowlname).Contents = stpEXPECTED_MESSAGE

                    Dim stpEXPECTED_TABLE = Strapd().d(
                    "key-value".Replace("-", vbTab)).dLine().d(
                    "key_e1-val_e1".Replace("-", vbTab).Replace(qt, qs)).dLine
                    Have.TempBowl.SelKey(ur_exp_table_bowlname).Contents = stpEXPECTED_TABLE.ToString

                ElseIf ur_fn Is enfTEST.key_quoted_value Then
                    Have.TempBowl.SelKey(ur_file_comment_bowlname).Contents = "There is a key with a quoted value."

                    Dim stpFILE = Strapd().d(
                    "/key_e1='val_e1'".Replace(qt, qs))
                    Have.TempBowl.SelKey(ur_file_text_bowlname).Contents = stpFILE

                    Dim stpEXPECTED_MESSAGE = Strapd()
                    Have.TempBowl.SelKey(ur_exp_result_bowlname).Contents = stpEXPECTED_MESSAGE

                    Dim stpEXPECTED_TABLE = Strapd().d(
                    "key-value".Replace("-", vbTab)).dLine().d(
                    "key_e1-val_e1".Replace("-", vbTab).Replace(qt, qs)).dLine
                    Have.TempBowl.SelKey(ur_exp_table_bowlname).Contents = stpEXPECTED_TABLE.ToString

                ElseIf ur_fn Is enfTEST.two_keys Then
                    Have.TempBowl.SelKey(ur_file_comment_bowlname).Contents = "There are two keys."

                    Dim stpFILE = Strapd().d(
                    "/key_e1=val_e1 /key_e2=val_e2")
                    Have.TempBowl.SelKey(ur_file_text_bowlname).Contents = stpFILE

                    Dim stpEXPECTED_MESSAGE = Strapd()
                    Have.TempBowl.SelKey(ur_exp_result_bowlname).Contents = stpEXPECTED_MESSAGE

                    Dim stpEXPECTED_TABLE = Strapd().d(
                    "key-value".Replace("-", vbTab)).dLine().d(
                    "key_e1-val_e1".Replace("-", vbTab).Replace(qt, qs)).dLine.d(
                    "key_e2-val_e2".Replace("-", vbTab).Replace(qt, qs)).dLine
                    Have.TempBowl.SelKey(ur_exp_table_bowlname).Contents = stpEXPECTED_TABLE.ToString

                ElseIf ur_fn Is enfTEST.key_without_value Then
                    Have.TempBowl.SelKey(ur_file_comment_bowlname).Contents = "There is a key without a value."

                    Dim stpFILE = Strapd().d(
                    "/key_e1")
                    Have.TempBowl.SelKey(ur_file_text_bowlname).Contents = stpFILE

                    Dim stpEXPECTED_MESSAGE = Strapd()
                    Have.TempBowl.SelKey(ur_exp_result_bowlname).Contents = stpEXPECTED_MESSAGE

                    Dim stpEXPECTED_TABLE = Strapd().d(
                    "key-value".Replace("-", vbTab)).dLine()
                    Have.TempBowl.SelKey(ur_exp_table_bowlname).Contents = stpEXPECTED_TABLE.ToString

                Else
                    Throw New System.Exception("Unhandled test setup:" & s & ur_fn.name)
                End If 'ur_fn
            End If
        End Function 'Store_CLine_Setup

        Public Shared Function Test_CLine(
            ur_message_written As Boolean,
            ret_msg As Strap,
            ur_fn As enfTEST,
            ur_file_comment_bowlname As enkTB.zfile_comment,
            ur_file_text_bowlname As enkTB.zfile_text,
            ur_exp_result_bowlname As enkTB.zexpected_result,
            ur_exp_table_bowlname As enkTB.zexpected_table
            ) As Boolean
            Test_CLine = ur_message_written
            If ur_message_written = False Then
                If ur_fn Is enfTEST.key_without_value Then
                    Dim abc = 123
                End If
                Dim strCOMMENT = Have.TempBowl.SelKey(ur_file_comment_bowlname).Contents
                Dim strFILE = Have.TempBowl.SelKey(ur_file_text_bowlname).Contents
                Dim strEXPECTED_RESULT = Have.TempBowl.SelKey(ur_exp_result_bowlname).Contents
                Dim strEXPECTED_TABLE = Have.TempBowl.SelKey(ur_exp_table_bowlname).Contents

                Dim stpERROR = Strapd()
                Dim sdaSHOULD = MxText.ParseCLine(strFILE, stpERROR)
                Dim stpOUTPUT_LINES = Strapd().d("key").dSprtr(vbTab, "value").dLine
                For Each trwENTRY In sdaSHOULD.kvp
                    stpOUTPUT_LINES.d(trwENTRY.row.key).d(vbTab).d(trwENTRY.row.value).dLine()
                Next trwENTRY

                Dim strRESULT_TEXT = stpERROR.ToString
                Dim strRESULT_TABLE = stpOUTPUT_LINES.ToString
                If strRESULT_TEXT <> strEXPECTED_RESULT OrElse
                  AreEqual(strEXPECTED_TABLE, strRESULT_TABLE) = False Then
                    Test_CLine = True
                    ret_msg.d("Test:").dS(ur_fn.name).dLine()
                    ret_msg.d("Expected:").dS(strEXPECTED_RESULT).dLine()
                    ret_msg.d("Result:").dS(strRESULT_TEXT).dLine().dLine()
                    ret_msg.d(strEXPECTED_TABLE).dLine().dLine()
                    ret_msg.d(strRESULT_TABLE).dLine().dLine()
                    ret_msg.d(strCOMMENT).dLine().dLine()
                    ret_msg.d(strFILE)
                End If 'strRESULT_TEXT
            End If
        End Function 'Test_CLine
    End Class 'Assistant


    Partial Public Class Have
        Private Shared prv_envWindowsCboard As glblWindowsCboard
        Private Shared prv_envWindowsVar As glblWindowsVar
        Private Shared prv_envWindowsFS As glblWindowsFS
        Private Shared prv_envWindowsMsgBox As glblWindowsMsgBox
        Private Shared prv_exeAssembly As dbExeAssembly
        Private Shared prv_frmStatus As rStatusForm
        Private Shared prv_stpShellExePath As dbShellExePath
        Private Shared prv_tblSessionBowl As sSessionBowl
        Private Shared prv_tblTempBowl As sTempBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.prv_tblSessionBowl Is Nothing Then
                Have.prv_envWindowsCboard = New glblWindowsCboard
                Have.prv_envWindowsVar = New glblWindowsVar
                Have.prv_envWindowsFS = New glblWindowsFS
                Have.prv_envWindowsMsgBox = New glblWindowsMsgBox
                If Have.prv_exeAssembly Is Nothing Then
                    Have.prv_exeAssembly = New dbExeAssembly(Nothing)
                End If

                Have.prv_frmStatus = New rStatusForm

                If Have.prv_stpShellExePath Is Nothing Then
                    Have.prv_stpShellExePath = New dbShellExePath(Nothing)
                End If

                Have.prv_tblSessionBowl = New sSessionBowl
                Have.prv_tblTempBowl = New sTempBowl
            End If 'prv_tblSessionBowl
        End Sub 'Connect


        <System.Diagnostics.DebuggerHidden()>
        Public Shared Sub Connect(ur_assembly As dbExeAssembly)
            If ur_assembly IsNot Nothing Then
                Have.prv_exeAssembly = ur_assembly
            End If
        End Sub 'Connect(ur_assembly

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Sub Connect(ur_exe_path As dbShellExePath)
            If ur_exe_path IsNot Nothing Then
                Have.prv_stpShellExePath = ur_exe_path
            End If
        End Sub 'Connect(ur_exe_path

        Public Shared ReadOnly Property ExeAssembly() As dbExeAssembly
            <System.Diagnostics.DebuggerHidden()>
            Get
                ExeAssembly = Have.prv_exeAssembly
            End Get
        End Property

        Public Shared ReadOnly Property ShellExePath() As dbShellExePath
            <System.Diagnostics.DebuggerHidden()>
            Get
                ShellExePath = Have.prv_stpShellExePath
            End Get
        End Property
    End Class 'Have

    Public Class enrSB
        Inherits bitBASE
        Public Shared bowl_name As enrSB = TRow(Of enrSB).glbl.NewBitBase()
        Public Shared contents As enrSB = TRow(Of enrSB).glbl.NewBitBase()
    End Class

    Public Class enkSB
        Inherits bitBASE
        Public Shared application_exe_filepath As zapplication_exe_filepath = TRow(Of enkSB).glbl.Trbase(Of zapplication_exe_filepath).NewBitBase() : Public Class zapplication_exe_filepath : Inherits enkSB : End Class
        Public Shared app_folder As zapp_folder = TRow(Of enkSB).glbl.Trbase(Of zapp_folder).NewBitBase() : Public Class zapp_folder : Inherits enkSB : End Class
        Public Shared app_exetitle As zapp_exetitle = TRow(Of enkSB).glbl.Trbase(Of zapp_exetitle).NewBitBase() : Public Class zapp_exetitle : Inherits enkSB : End Class
    End Class

    Partial Public Class Have
        Public Shared FirstConnect As Object

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function SessionBowl() As sSessionBowl
            Dim bolFIRST_INIT = (Have.FirstConnect Is Nothing)
            Call Have.Connect()
            SessionBowl = Have.prv_tblSessionBowl
            If bolFIRST_INIT Then
                Have.FirstConnect = "Done"

                Dim application_exe_folder_bowlname = SessionBowl.Store_ApplicationEXEFilePath(Have.ExeAssembly, Have.ShellExePath)
                Dim app_exetitle_bowlname = SessionBowl.Store_ApplicationEXETitle(application_exe_folder_bowlname)
                Dim app_folder_bowlname = SessionBowl.Store_ApplicationEXENonDebugFolder(application_exe_folder_bowlname)
            End If 'bolFIRST_INIT
        End Function 'SessionBowl

        Public Class rSessionBowl
            Inherits TRow(Of enrSB)

            Public Property Contents As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enrSB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.v(enrSB.contents) = value
                End Set
            End Property 'Contents
        End Class 'rSessionBowl

        Public Class sSessionBowl
            Inherits Mx.TablePKEnum(Of enrSB, enkSB, rSessionBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXEFilePath(
                ur_exe_assembly As dbExeAssembly,
                ur_shellexe_path As dbShellExePath
                ) As enkSB.zapplication_exe_filepath

                Dim retSB = enkSB.application_exe_filepath
                Store_ApplicationEXEFilePath = retSB
                Dim strCUR_EXE_PATH = If(ur_exe_assembly Is Nothing, mt, ur_exe_assembly.Location)
                If Mx.HasText(strCUR_EXE_PATH) = False Then
                    strCUR_EXE_PATH = ur_shellexe_path.ExePath
                End If

                Me.SelKey(retSB).Contents = strCUR_EXE_PATH
            End Function 'Store_ApplicationEXEFilePath

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXETitle(ur_application_exe_filepath As enkSB.zapplication_exe_filepath) As enkSB.zapp_exetitle
                Dim retSB = enkSB.app_exetitle
                Store_ApplicationEXETitle = retSB
                Dim flnAPP_EXEPATH = Mx.FileNamed().d(Me.SelKey(ur_application_exe_filepath).Contents)
                Me.SelKey(retSB).Contents = flnAPP_EXEPATH.FileGroup
            End Function 'Store_ApplicationEXETitle

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXENonDebugFolder(ur_application_exe_filepath As enkSB.zapplication_exe_filepath) As enkSB.zapp_folder
                Dim retSB = enkSB.app_folder
                Store_ApplicationEXENonDebugFolder = retSB
                Dim strCUR_EXE_PATH = Me.SelKey(ur_application_exe_filepath).Contents
                Dim flnAPP_PATH = Mx.FileNamed().d(strCUR_EXE_PATH.Replace("\bin\Debug", Mx.mt))
                Me.SelKey(retSB).Contents = flnAPP_PATH.ParentDir
            End Function 'Store_ApplicationEXENonDebugFolder
        End Class 'sSessionBowl
    End Class 'SB


    Public Class enrST
        Inherits bitBASE
        Public Shared FormPopupSeq As zFormPopupSeq = TRow(Of enrST).glbl.Trbase(Of zFormPopupSeq).NewBitBase() : Public Class zFormPopupSeq : Inherits enrST : End Class
        Public Shared LogText As zLogText = TRow(Of enrST).glbl.Trbase(Of zLogText).NewBitBase() : Public Class zLogText : Inherits enrST : End Class
        Public Shared StartPosition As zStartPosition = TRow(Of enrST).glbl.Trbase(Of zStartPosition).NewBitBase() : Public Class zStartPosition : Inherits enrST : End Class
        Public Shared TimerInterval As zTimerInterval = TRow(Of enrST).glbl.Trbase(Of zTimerInterval).NewBitBase() : Public Class zTimerInterval : Inherits enrST : End Class
        Public Shared TitleText As zTitleText = TRow(Of enrST).glbl.Trbase(Of zTitleText).NewBitBase() : Public Class zTitleText : Inherits enrST : End Class
        Public Shared WindowState As zWindowState = TRow(Of enrST).glbl.Trbase(Of zWindowState).NewBitBase() : Public Class zWindowState : Inherits enrST : End Class
    End Class

    Public Class enfST
        Inherits bitBASE
        Public Shared Close_Form As zClose_Form = TRow(Of enfST).glbl.Trbase(Of zClose_Form).NewBitBase() : Public Class zClose_Form : Inherits enfST : End Class
        Public Shared Resize_Form As zResize_Form = TRow(Of enfST).glbl.Trbase(Of zResize_Form).NewBitBase() : Public Class zResize_Form : Inherits enfST : End Class
        Public Shared Show_Form As zShow_Form = TRow(Of enfST).glbl.Trbase(Of zShow_Form).NewBitBase() : Public Class zShow_Form : Inherits enfST : End Class
        Public Shared Start_Timer As zStart_Timer = TRow(Of enfST).glbl.Trbase(Of zStart_Timer).NewBitBase() : Public Class zStart_Timer : Inherits enfST : End Class
        Public Shared Stop_Timer As zStop_Timer = TRow(Of enfST).glbl.Trbase(Of zStop_Timer).NewBitBase() : Public Class zStop_Timer : Inherits enfST : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function StatusForm() As rStatusForm
            Call Have.Connect()
            StatusForm = Have.prv_frmStatus
        End Function

        Public Class rStatusForm
            Inherits System.Windows.Forms.Form

            Public txtLOG As System.Windows.Forms.TextBox
            Public intFORM_POPUP_SEQ As Integer
            Public bgTHREAD As System.ComponentModel.BackgroundWorker
            Private bg_fn As UserAction.enfBG.zrun_tests
            Private bg_completed As System.ComponentModel.RunWorkerCompletedEventHandler
            Private bg_progress As System.ComponentModel.ProgressChangedEventHandler
            Private bg_work As System.ComponentModel.DoWorkEventHandler

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.intFORM_POPUP_SEQ = 1
                Me.Name = "frmStatus"

                Me.txtLOG = New System.Windows.Forms.TextBox()
                Me.txtLOG.Name = "txtLOG"

                Me.txtLOG.Multiline = True
                Me.txtLOG.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtLOG.Dock = System.Windows.Forms.DockStyle.Fill
                Me.Controls.Add(Me.txtLOG)
                Me.bgTHREAD = New System.ComponentModel.BackgroundWorker()
                Me.bgTHREAD.WorkerReportsProgress = True
                Me.bgTHREAD.WorkerSupportsCancellation = True

                AddHandler Me.txtLOG.KeyDown, AddressOf StopWork
            End Sub 'New

            Private Sub ClearHandlers()
                If Me.bg_work IsNot Nothing Then
                    RemoveHandler Me.bgTHREAD.DoWork, Me.bg_work
                End If
                If Me.bg_progress IsNot Nothing Then
                    RemoveHandler Me.bgTHREAD.ProgressChanged, Me.bg_progress
                End If
                If Me.bg_completed IsNot Nothing Then
                    RemoveHandler Me.bgTHREAD.RunWorkerCompleted, Me.bg_completed
                End If
            End Sub 'ClearHandlers

            Private Sub RunWorkerAsyncStartWork(worker As System.ComponentModel.BackgroundWorker, e As System.ComponentModel.DoWorkEventArgs)
                Mx.UserAction.Result_errhnd(Me.bg_fn, worker)
            End Sub

            Private Sub RunWorkerAsyncProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs)
                Me.txtLOG.Text = "Working: " & e.ProgressPercentage.ToString & vbCrLf & vbCrLf & e.UserState.ToString
            End Sub

            Private Sub RunWorkerAsyncRunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
                Dim test_result_bowlname = enkTB.test_result
                Dim strRESULT = Have.TempBowl.SelKey(test_result_bowlname).Contents
                If e.Cancelled Then
                    Me.txtLOG.Text = "Operation cancelled By User" & vbCrLf & vbCrLf & strRESULT
                Else
                    Me.txtLOG.Text = "Operation completed" & vbCrLf & vbCrLf & strRESULT
                End If
            End Sub 'RunWorkerAsyncRunWorkerCompleted

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfST.zShow_Form) As Integer
                Result = 1
                Me.StartWork(Me.bg_fn)
                'Note: System.Windows.Forms.Application.Run(Me) only works from a command line program with no forms open. VBNetScript already has a form open.
                Call Me.ShowDialog()
            End Function

            Private Sub StartWork(ur_fn As UserAction.enfBG.zrun_tests)
                Me.bg_fn = ur_fn
                Call Me.ClearHandlers()

                Me.bg_work = New System.ComponentModel.DoWorkEventHandler(AddressOf RunWorkerAsyncStartWork)
                AddHandler Me.bgTHREAD.DoWork, Me.bg_work

                Me.bg_progress = New System.ComponentModel.ProgressChangedEventHandler(AddressOf RunWorkerAsyncProgressChanged)
                AddHandler Me.bgTHREAD.ProgressChanged, Me.bg_progress

                Me.bg_completed = New System.ComponentModel.RunWorkerCompletedEventHandler(AddressOf RunWorkerAsyncRunWorkerCompleted)
                AddHandler Me.bgTHREAD.RunWorkerCompleted, Me.bg_completed

                Me.bgTHREAD.RunWorkerAsync()
            End Sub 'StartWork

            Private Sub StopWork(sender As Object, e As System.Windows.Forms.KeyEventArgs)
                If e.KeyCode = 27 Then
                    Call Me.bgTHREAD.CancelAsync()
                End If
            End Sub
        End Class 'rStatusForm
    End Class 'ST


    Public Class enrTB
        Inherits bitBASE
        Public Shared bowl_name As enrTB = TRow(Of enrTB).glbl.NewBitBase()
        Public Shared contents As enrTB = TRow(Of enrTB).glbl.NewBitBase()
    End Class

    Public Class enkTB
        Inherits bitBASE
        Public Shared expected_result As zexpected_result = TRow(Of enkTB).glbl.Trbase(Of zexpected_result).NewBitBase() : Public Class zexpected_result : Inherits enkTB : End Class
        Public Shared expected_table As zexpected_table = TRow(Of enkTB).glbl.Trbase(Of zexpected_table).NewBitBase() : Public Class zexpected_table : Inherits enkTB : End Class
        Public Shared file_comment As zfile_comment = TRow(Of enkTB).glbl.Trbase(Of zfile_comment).NewBitBase() : Public Class zfile_comment : Inherits enkTB : End Class
        Public Shared file_text As zfile_text = TRow(Of enkTB).glbl.Trbase(Of zfile_text).NewBitBase() : Public Class zfile_text : Inherits enkTB : End Class
        Public Shared test_result As ztest_result = TRow(Of enkTB).glbl.Trbase(Of ztest_result).NewBitBase() : Public Class ztest_result : Inherits enkTB : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempBowl() As sTempBowl
            Call Have.Connect()
            TempBowl = Have.prv_tblTempBowl
        End Function 'TempBowl

        Public Class rTempBowl
            Inherits TRow(Of enrTB)

            Public Property Contents As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enrTB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.v(enrTB.contents) = value
                End Set
            End Property 'Contents
        End Class 'rTempBowl

        Public Class sTempBowl
            Inherits Mx.TablePKEnum(Of enrTB, enkTB, rTempBowl)
        End Class 'sTempBowl
    End Class 'TB


    Public Class enfWC
        Inherits bitBASE
        Public Shared StoreText As zStoreText = TRow(Of enfWC).glbl.Trbase(Of zStoreText).NewBitBase() : Public Class zStoreText : Inherits enfWC : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsCboardEnv() As glblWindowsCboard
            Call Have.Connect()
            WindowsCboardEnv = Have.prv_envWindowsCboard
        End Function

        Public Class glblWindowsCboard
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(
                ur_fn As enfWC.zStoreText,
                ur_text As String
                ) As Integer

                Result = glbl.gCboard.SetText(ur_text)
            End Function
        End Class 'glblWindowsCboard
    End Class 'WC


    Public Class enfWE
        Inherits bitBASE
        Public Shared ExpandEnvironmentVariables As zExpandEnvironmentVariables = TRow(Of enfWE).glbl.Trbase(Of zExpandEnvironmentVariables).NewBitBase() : Public Class zExpandEnvironmentVariables : Inherits enfWE : End Class
    End Class

    Partial Public Class Have
        Public Shared ReadOnly Property WindowsEnvVar() As glblWindowsVar
            <System.Diagnostics.DebuggerHidden()>
            Get
                Call Have.Connect()
                WindowsEnvVar = Have.prv_envWindowsVar
            End Get
        End Property

        Public Class glblWindowsVar
            Public Function Result(ur_fn As enfWE.zExpandEnvironmentVariables, ur_path As String) As String
                Result = Mx.glbl.gEnvironment.ExpandEnvironmentVariables(ur_path)
            End Function
        End Class 'glblWindowsVar
    End Class 'WE


    Public Class enfWF
        Inherits bitBASE
        Public Shared file_delete_when_exists As zfile_delete_when_exists = TRow(Of enfWF).glbl.Trbase(Of zfile_delete_when_exists).NewBitBase() : Public Class zfile_delete_when_exists : Inherits enfWF : End Class
        Public Shared folder_create_when_missing As zfolder_create_when_missing = TRow(Of enfWF).glbl.Trbase(Of zfolder_create_when_missing).NewBitBase() : Public Class zfolder_create_when_missing : Inherits enfWF : End Class
        Public Shared open_writestream As zopen_writestream = TRow(Of enfWF).glbl.Trbase(Of zopen_writestream).NewBitBase() : Public Class zopen_writestream : Inherits enfWF : End Class
        Public Shared read_alltext As zread_alltext = TRow(Of enfWF).glbl.Trbase(Of zread_alltext).NewBitBase() : Public Class zread_alltext : Inherits enfWF : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsFSEnv() As glblWindowsFS
            Call Have.Connect()
            WindowsFSEnv = Have.prv_envWindowsFS
        End Function

        Public Class glblWindowsFS
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWF.zfile_delete_when_exists, ur_file_path As String) As Integer
                Result = 0
                Try
                    If glbl.gWindowsFS.HasFile(ur_file_path) Then
                        Result = 1
                        Call glbl.gWindowsFS.Delete(ur_file_path)
                    End If
                Catch ex As System.Exception
                End Try
            End Function 'zfile_delete_when_exists
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWF.zfolder_create_when_missing, ur_dest_path As String) As Integer
                Result = 0
                Try
                    If glbl.gWindowsFS.HasDir(ur_dest_path) = False Then
                        Result = 1
                        Call glbl.gWindowsFS.CreateDirectory(ur_dest_path)
                    End If
                Catch ex As System.Exception
                End Try
            End Function 'zfolder_create_when_missing
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWF.zopen_writestream, ur_dest_path As String) As System.IO.StreamWriter
                Result = glbl.gWindowsFS.WriteStream(ur_dest_path)
            End Function
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWF.zread_alltext, ur_dest_path As String) As String
                Result = mt
                Try
                    If glbl.gWindowsFS.HasFile(ur_dest_path) Then
                        Result = glbl.gWindowsFS.ReadAllText(ur_dest_path)
                    End If
                Catch ex As System.Exception
                End Try
            End Function 'zread_alltext
        End Class 'glblWindowsFS
    End Class 'WF


    Public Class enfWM
        Inherits bitBASE
        Public Shared UserInputDialog As zUserInputDialog = TRow(Of enfWM).glbl.Trbase(Of zUserInputDialog).NewBitBase() : Public Class zUserInputDialog : Inherits enfWM : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsMsgBoxEnv() As glblWindowsMsgBox
            Call Have.Connect()
            WindowsMsgBoxEnv = Have.prv_envWindowsMsgBox
        End Function

        Public Class glblWindowsMsgBox
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(
                ur_fn As enfWM.zUserInputDialog,
                ur_message As String,
                Optional ur_style As MsgBoxStyle = MsgBoxStyle.OkOnly
                ) As MsgBoxResult

                Dim strAPP_NAME = Have.SessionBowl.SelKey(enkSB.app_exetitle).Contents
                Result = glbl.gMsgBox.GetResult(ur_message, ur_style, strAPP_NAME)
            End Function
        End Class 'glblWindowsMsgBox
    End Class 'WM

    Public Class dbExeAssembly
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Widening Operator CType(b As System.Reflection.Assembly) As dbExeAssembly
            Return New dbExeAssembly(b)
        End Operator

        Private exeASSEMBLY As System.Reflection.Assembly
        Public GetName As componentAssemblyName

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_assembly As System.Reflection.Assembly)
            Me.exeASSEMBLY = ur_assembly
            Me.GetName = New componentAssemblyName(If(ur_assembly Is Nothing, Nothing, ur_assembly.GetName))
        End Sub 'New

        Public ReadOnly Property Location As String
            <System.Diagnostics.DebuggerHidden()>
            Get
                Location = If(If(Me.exeASSEMBLY Is Nothing, Nothing, Me.exeASSEMBLY.Location), Mx.mt)
            End Get
        End Property

        Public Class componentAssemblyName
            Private objASM_NAME As System.Reflection.AssemblyName
            Public Version As componentVersion

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As System.Reflection.AssemblyName)
                Me.objASM_NAME = ur_component
                Me.Version = New componentVersion(If(ur_component Is Nothing, Nothing, ur_component.Version))
            End Sub
        End Class 'componentAssemblyName

        Public Class componentVersion
            Private objASM_VER As System.Version

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As System.Version)
                Me.objASM_VER = ur_component
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function ToString() As String
                ToString = If(If(Me.objASM_VER Is Nothing, Nothing, Me.objASM_VER.ToString), Mx.mt)
            End Function 'toString  
        End Class 'componentVersion
    End Class 'dbExeAssembly


    Public Class dbShellExePath
        Public ExePath As String

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_exe_path As String)
            Me.ExePath = If(ur_exe_path, Mx.mt)
        End Sub 'New
    End Class 'dbShellExePath
End Namespace 'Mx
