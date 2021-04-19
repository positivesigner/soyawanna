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
MxClasses\DLL_WinForm2019m09d13\System.Drawing.dll
MxClasses\DLL_WinForm2019m09d13\System.Windows.Forms.dll
MxClasses\MxBaseEc13.vb
RetVal = Mx.UserAction.UITimer_to_Poll_FileDir_And_Move_errhnd(System.Environment.CommandLine)
End Function '2021m01d03
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.UserAction.UITimer_to_Poll_FileDir_And_Move_errhnd(System.Environment.CommandLine)
'            If Mx.AreEqual(RetVal, "QUIT") = False Then MsgBox(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "")
'        Public Shared SourcePath As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "") & "\Wiki_Move_HTM.vbns.cmd"
'    End Class
'End Namespace 'Mx

Namespace Mx
    Public Class UserAction
        Public Shared Sub UITimer_to_Poll_FileDir_And_Move(ur_ret As Strap, ur_commandline_text As String)
            Have.CurExeFilePath = Mx.Class1.SourcePath
            Have.CmdLineText = ur_commandline_text
            Dim message_written = False
            Dim fn_expand_var = enfWV.ExpandVariable
            Dim fn_is_running = enfWD.IsRunningWindow
            Dim fn_app_activate = enfWI.AppActivate
            Dim fn_resize_form = enfMV.Resize_Form
            Dim fn_show_form = enfMV.Show_FormAsDialog
            Dim fn_start_timer = enfMV.Start_Timer
            Dim log_text_bowlname = enrMV.LogText
            Dim uiform_title_bowlname = Assistant.Store_UIForm_Title(message_written)
            Dim poll_folder_bowlname = Assistant.Store_Poll_Folder(message_written, fn_expand_var)
            message_written = Assistant.Validate_NewAppNeeded(message_written, uiform_title_bowlname, fn_is_running, fn_app_activate)
            Dim title_text_bowlname = Assistant.Assign_FormTitle(message_written, uiform_title_bowlname)
            Dim start_position_bowlname = Assistant.Assign_FormPosition(message_written)
            Dim form_resized_count = Assistant.Resize_Form(message_written, fn_resize_form)
            Dim timer_interval_bowlname = Assistant.Assign_TimerIntervalInitial(message_written)
            Dim logtext_written_count = Assistant.DisplayPollFolderWhenEmptyLog(message_written, log_text_bowlname, poll_folder_bowlname)
            Dim timer_started_count = Assistant.Start_Timer(message_written, fn_start_timer)
            Dim form_shown_count = Assistant.Show_FormVisible(message_written, fn_show_form)
        End Sub 'UITimer_to_Poll_FileDir_And_Move

        Public Shared Function UITimer_to_Poll_FileDir_And_Move_errhnd(ur_commandline_text As String) As Strap
            Dim stpRET = Strapd()
            UITimer_to_Poll_FileDir_And_Move_errhnd = stpRET
            Dim objERR_LIST = New ErrListBase : Try
                Call UITimer_to_Poll_FileDir_And_Move(stpRET, ur_commandline_text)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If

            If stpRET.HasText = False Then
                stpRET.d("QUIT")
            End If
        End Function 'UITimer_to_Poll_FileDir_And_Move_errhnd

        Public Shared Sub Poll_FileDir_And_Move(ur_ret As Strap)
            Dim message_written = False
            Dim form_popupseq_bowlname = enrMV.FormPopupSeq
            Dim form_windowstate_bowlname = enrMV.WindowState
            Dim form_timerinterval_bowlname = enrMV.TimerInterval
            Dim log_text_bowlname = enrMV.LogText
            Dim poll_folder_bowlname = enmTB.poll_folder
            Dim uiform_title_bowlname = enmTB.uiform_title
            Dim fn_fm_delall = enfFM.del_all
            Dim fn_fm_ins = enfFM.ins
            Dim fn_fmselect_all = enfFM.select_all
            Dim fn_start_timer = enfMV.Start_Timer
            Dim fn_stop_timer = enfMV.Stop_Timer
            Dim fn_file_search = enfWF.FileSearch
            Dim fn_file_delete = enfWF.FileDelete
            Dim fn_file_move = enfWF.FileMove
            Dim fn_appactivate = enfWI.AppActivate
            Dim timer_stopped_count = Assistant.Stop_Timer(message_written, fn_stop_timer)
            Dim timer_interval_bowlname = Assistant.Assign_TimerIntervalRepeating(message_written)
            Dim logtext_written_count = Assistant.DisplayPollFolderWhenEmptyLog(message_written, log_text_bowlname, poll_folder_bowlname)
            Dim removed_rec_count = Assistant.Clear_FileMoveRecs(message_written, fn_fm_delall)
            Dim inserted_rec_count = Assistant.Insert_FMRecs_from_FS(message_written, fn_file_search, poll_folder_bowlname, fn_fm_ins)
            Dim moved_rec_count = Assistant.Overwrite_FileDestination(message_written, fn_fmselect_all, fn_file_delete, fn_file_move)
            Dim report_output_bowlname = Assistant.Store_FileMove_Report(message_written, fn_fmselect_all)
            Dim displayed_rec_count = Assistant.Display_FileMove_Report(message_written, log_text_bowlname, form_popupseq_bowlname, report_output_bowlname)
            Dim form_cycle_count = Assistant.Popup_FormNextCycle(message_written, form_popupseq_bowlname, form_windowstate_bowlname, form_timerinterval_bowlname, uiform_title_bowlname, fn_appactivate)
            Dim timer_started_count = Assistant.Start_Timer(message_written, fn_start_timer)
        End Sub 'Poll_FileDir_And_Move

        Public Shared Sub Poll_FileDir_And_Move_errhnd(ur_ui_form As Have.rWikiMoveForm)
            Dim stpRET = Strapd()
            Dim objERR_LIST = New ErrListBase : Try
                Call Poll_FileDir_And_Move(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If

            If stpRET.HasText Then
                ur_ui_form.txtLOG.Text = stpRET.ToString & vbCrLf & ur_ui_form.txtLOG.Text
                ur_ui_form.intFORM_POPUP_SEQ = 1
            End If
        End Sub 'Poll_FileDir_And_Move_errhnd
    End Class 'sub_main


    Class Assistant
        Const strLIT_STAR_DOT_HTM = "WikiMove_*-stamp-*.htm"
        Const strLIT_STAR_DOT_HTML = "WikiMove_*-stamp-*.html"
        Const strLIT_STAR_DOT_CARDTXT = "WikiMove_*-stamp-*.card*.txt"
        Const strLIT_USER_PROFILE_DOWNLOADS = "%USERPROFILE%\downloads"
        Const strLIT_WIKI_MOVE_5_SEC = "Wiki Move 5-Sec"
        Const intLIT_POPUP_SEQ_0 = 0
        Const intLIT_POPUP_SEQ_1 = 1
        Const intLIT_POPUP_SEQ_2 = 3
        Const intLIT_POPUP_SEQ_3 = 3
        Const intLIT_POPUP_SEQ_6 = 6
        Const intLIT_SIZE_400_TWIP = 400
        Const intLIT_INTERVAL_0p05_SEC = 50
        Const intLIT_INTERVAL_5_SEC = 5000
        Const intLIT_INTERVAL_0p8_SEC = 800

        Public Shared Function Assign_FormTitle(ur_message_written As Boolean, ur_uiform_title_bowlname As enmTB.zuiform_title) As enrMV.zTitleText
            Dim bwnRET = enrMV.TitleText
            Assign_FormTitle = bwnRET
            If ur_message_written = False Then
                Dim strTITLE = Have.TempBowl.SelKey(ur_uiform_title_bowlname).Contents
                Have.WikiMoveForm.v(bwnRET) = strTITLE
            End If
        End Function 'Assign_FormTitle

        Public Shared Function Assign_FormPosition(ur_message_written As Boolean) As enrMV.zStartPosition
            Dim bwnRET = enrMV.StartPosition
            Assign_FormPosition = bwnRET
            If ur_message_written = False Then
                Have.WikiMoveForm.v(bwnRET) = System.Windows.Forms.FormStartPosition.CenterScreen
            End If
        End Function 'Assign_FormPosition

        Public Shared Function Assign_TimerIntervalInitial(ur_message_written As Boolean) As enrMV.zTimerInterval
            Dim bwnRET = enrMV.TimerInterval
            Assign_TimerIntervalInitial = bwnRET
            If ur_message_written = False Then
                Have.WikiMoveForm.v(bwnRET) = intLIT_INTERVAL_0p05_SEC
            End If
        End Function 'Assign_TimerIntervalInitial

        Public Shared Function Assign_TimerIntervalRepeating(ur_message_written As Boolean) As enrMV.zTimerInterval
            Dim bwnRET = enrMV.TimerInterval
            Assign_TimerIntervalRepeating = bwnRET
            If ur_message_written = False Then
                Have.WikiMoveForm.v(bwnRET) = intLIT_INTERVAL_5_SEC
            End If
        End Function 'Assign_TimerIntervalRepeating

        Public Shared Function Clear_FileMoveRecs(ur_message_written As Boolean, ur_fn_delall As enfFM.zdel_all) As Integer
            Clear_FileMoveRecs = 0
            If ur_message_written = False Then
                Clear_FileMoveRecs = Have.FileMove.Result(ur_fn_delall)
            End If
        End Function 'Clear_FileMoveRecs

        Public Shared Function Display_FileMove_Report(ur_message_written As Boolean, ur_log_text_bowlname As enrMV.zLogText, ur_form_popupseq_bowlname As enrMV.zFormPopupSeq, ur_report_output_bowlname As enmTB.zreport_output) As Integer
            Display_FileMove_Report = 0
            If ur_message_written = False Then
                Dim strREPORT_OUTPUT = Have.TempBowl.SelKey(ur_report_output_bowlname).Contents
                If HasText(strREPORT_OUTPUT) Then
                    Display_FileMove_Report = 1
                    Have.WikiMoveForm.v(ur_log_text_bowlname) = strREPORT_OUTPUT & vbCrLf & Have.WikiMoveForm.v(ur_log_text_bowlname)
                    Have.WikiMoveForm.v(ur_form_popupseq_bowlname) = 1
                End If
            End If
        End Function 'Display_FileMove_Report

        Public Shared Function DisplayPollFolderWhenEmptyLog(ur_message_written As Boolean, ur_logtext_bowlname As enrMV.zLogText, ur_poll_folder_bowlname As enmTB.zpoll_folder) As Integer
            DisplayPollFolderWhenEmptyLog = 0
            If ur_message_written = False AndAlso
              HasText(Have.WikiMoveForm.v(ur_logtext_bowlname)) = False Then
                DisplayPollFolderWhenEmptyLog = 1
                Have.WikiMoveForm.v(ur_logtext_bowlname) = Strapd().dLine("Listening:").dS(Have.TempBowl.SelKey(ur_poll_folder_bowlname).Contents)
            End If
        End Function 'DisplayPollFolderWhenEmptyLog

        Public Shared Function Insert_FMRecs_from_FS(ur_message_written As Boolean, ur_fn_file_search As enfWF.zFileSearch, ur_poll_folder_bowlname As enmTB.zpoll_folder, ur_fn_fmins As enfFM.zins) As Integer
            Insert_FMRecs_from_FS = 0
            If ur_message_written = False Then
                For Each strFILE_SPEC In {strLIT_STAR_DOT_HTM, strLIT_STAR_DOT_HTML, strLIT_STAR_DOT_CARDTXT}
                    Dim flnPOLL_FOLDER = FileNamed().d(Have.TempBowl.SelKey(ur_poll_folder_bowlname).Contents).d(strFILE_SPEC)
                    For Each strFILE_PATH In Have.WindowsFSEnv.Result(ur_fn_file_search, flnPOLL_FOLDER)
                        Dim flnFILE_NAME = FileNamed().d(strFILE_PATH)
                        Dim sdaEXT_LIST = flnFILE_NAME.ExtList
                        Dim strDEST_PATH_ENCODED_IN_NAME = flnFILE_NAME.Name.Substring("WikiMove_".Length)
                        Dim strDEST_PATH = strDEST_PATH_ENCODED_IN_NAME.Replace("-colon-", ":").Replace("-fslash-", "\")
                        Dim intSTAMP = InStr(strDEST_PATH, "-stamp-")
                        strDEST_PATH = Left(strDEST_PATH, intSTAMP - 1)
                        Dim strEXT = sdaEXT_LIST.Item(b0(sdaEXT_LIST.Count))
                        If sdaEXT_LIST.Count > 1 Then
                            Dim strEXT2 = sdaEXT_LIST.Item(b0(sdaEXT_LIST.Count - 1))
                            If AreEqual(strEXT, ".txt") AndAlso
                              StartingWithText(strEXT2, ".card") Then
                                strEXT = ".card.txt"
                            End If
                        End If 'sdaEXT_LIST

                        strDEST_PATH &= strEXT
                        Have.FileMove.Result(ur_fn_fmins, strFILE_PATH, strDEST_PATH)
                    Next strFILE_PATH
                Next strFILE_SPEC

                Insert_FMRecs_from_FS = Have.FileMove.Count
            End If
        End Function 'Insert_FMRecs_from_FS

        Public Shared Function Overwrite_FileDestination(ur_message_written As Boolean, ur_fn_fmselect_all As enfFM.zselect_all, ur_fn_file_delete As enfWF.zFileDelete, ur_fn_file_move As enfWF.zFileMove) As Integer
            Overwrite_FileDestination = 0
            If ur_message_written = False Then
                Dim lstLEN = New System.Collections.Generic.List(Of Integer)
                For Each strFILE_PATH In Have.FileMove.SelDistinct(enrFM.src_file_path)
                    Dim intLEN = strFILE_PATH.Length
                    If lstLEN.Contains(intLEN) = False Then
                        lstLEN.Add(intLEN)
                    End If
                Next strFILE_PATH

                Call lstLEN.Sort()
                For Each intLEN In lstLEN
                    For Each rowFILE In Have.FileMove.Result(ur_fn_fmselect_all)
                        Dim strFILE_PATH = rowFILE.v(enrFM.src_file_path)
                        If strFILE_PATH.Length = intLEN Then
                            Dim strTEMP_RENAME = strFILE_PATH & ".TDLY"
                            Dim strDEST_PATH = rowFILE.v(enrFM.dest_file_path)
                            Have.WindowsFSEnv.Result(ur_fn_file_move, strFILE_PATH, strTEMP_RENAME)
                            Have.WindowsFSEnv.Result(ur_fn_file_delete, strDEST_PATH)
                            Have.WindowsFSEnv.Result(ur_fn_file_move, strTEMP_RENAME, strDEST_PATH)
                        End If 'strFILE_PATH
                    Next rowFILE
                Next intLEN

                Overwrite_FileDestination = Have.FileMove.Count
            End If
        End Function 'Overwrite_FileDestination

        Public Shared Function Popup_FormNextCycle(
            ur_message_written As Boolean,
            ur_form_popupseq_bowlname As enrMV.zFormPopupSeq,
            ur_form_windowstate_bowlname As enrMV.zWindowState,
            ur_timer_interval_bowlname As enrMV.zTimerInterval,
            ur_uiform_title_bowlname As enmTB.zuiform_title,
            ur_fn_app_activate As enfWI.zAppActivate
            )

            Popup_FormNextCycle = 1
            If ur_message_written = False Then
                Dim intPOPUP_SEQ = Have.WikiMoveForm.v(ur_form_popupseq_bowlname)
                If intPOPUP_SEQ = intLIT_POPUP_SEQ_1 Then
                    Have.WikiMoveForm.v(ur_form_windowstate_bowlname) = System.Windows.Forms.FormWindowState.Normal
                    Dim strTITLE = Have.TempBowl.SelKey(ur_uiform_title_bowlname).Contents
                    Have.WindowsIntrActEnv.Result(ur_fn_app_activate, strTITLE)
                    Have.WikiMoveForm.v(ur_timer_interval_bowlname) = intLIT_INTERVAL_0p8_SEC
                    Have.WikiMoveForm.v(ur_form_popupseq_bowlname) = intLIT_POPUP_SEQ_2

                ElseIf intPOPUP_SEQ = intLIT_POPUP_SEQ_2 Then
                    Have.WikiMoveForm.v(ur_form_windowstate_bowlname) = System.Windows.Forms.FormWindowState.Minimized
                    Have.WikiMoveForm.v(ur_form_popupseq_bowlname) = intLIT_POPUP_SEQ_0

                ElseIf intPOPUP_SEQ = intLIT_POPUP_SEQ_0 Then
                    If Have.WikiMoveForm.v(ur_form_windowstate_bowlname) <> System.Windows.Forms.FormWindowState.Minimized Then
                        Have.WikiMoveForm.v(ur_form_popupseq_bowlname) = intLIT_POPUP_SEQ_6
                    End If

                Else
                    Have.WikiMoveForm.v(ur_form_popupseq_bowlname) -= 1
                End If
            End If
        End Function 'Popup_FormNextCycle

        Public Shared Function Resize_Form(ur_message_written As Boolean, ur_fn_resize_form As enfMV.zResize_Form) As Integer
            Resize_Form = 0
            If ur_message_written = False Then
                Resize_Form = Have.WikiMoveForm.Result(ur_fn_resize_form, intLIT_SIZE_400_TWIP, intLIT_SIZE_400_TWIP)
            End If
        End Function 'Resize_Form

        Public Shared Function Show_FormVisible(ur_message_written As Boolean, ur_fn_show_form As enfMV.zShow_FormAsDialog) As Integer
            Show_FormVisible = 0
            If ur_message_written = False Then
                Show_FormVisible = Have.WikiMoveForm.Result(ur_fn_show_form)
            End If
        End Function 'Show_FormVisible

        Public Shared Function Start_Timer(ur_message_written As Boolean, ur_fn_start_timer As enfMV.zStart_Timer) As Integer
            Start_Timer = 0
            If ur_message_written = False Then
                Start_Timer = Have.WikiMoveForm.Result(ur_fn_start_timer)
            End If
        End Function 'Start_Timer

        Public Shared Function Stop_Timer(ur_message_written As Boolean, ur_fn_stop_timer As enfMV.zStop_Timer) As Integer
            Stop_Timer = 0
            If ur_message_written = False Then
                Stop_Timer = Have.WikiMoveForm.Result(ur_fn_stop_timer)
            End If
        End Function 'Stop_Timer

        Public Shared Function Store_FileMove_Report(ur_message_written As Boolean, ur_fn_fmselect_all As enfFM.zselect_all) As enmTB.zreport_output
            Dim retKEY = enmTB.report_output
            Store_FileMove_Report = retKEY
            If ur_message_written = False Then
                Dim stpREPORT = Strapd()
                For Each rowFILE In Have.FileMove.Result(ur_fn_fmselect_all)
                    stpREPORT.dLine(Now.ToString("tt hh:mm:ss")).d(":").dS(rowFILE.v(enrFM.dest_file_path))
                Next rowFILE

                Have.TempBowl.SelKey(retKEY).Contents = stpREPORT
            End If
        End Function 'Store_FileMove_Report

        Public Shared Function Store_Poll_Folder(ur_message_written As Boolean, ur_fn_expand_var As enfWV.zExpandVariable) As enmTB.zpoll_folder
            Dim bwnRET = enmTB.poll_folder
            Store_Poll_Folder = bwnRET
            If ur_message_written = False Then
                Have.TempBowl.SelKey(bwnRET).Contents = Have.WindowsVarEnv.Result(ur_fn_expand_var, strLIT_USER_PROFILE_DOWNLOADS)
            End If
        End Function 'Store_Poll_Folder

        Public Shared Function Store_UIForm_Title(ur_message_written As Boolean) As enmTB.zuiform_title
            Dim bwnRET = enmTB.uiform_title
            Store_UIForm_Title = bwnRET
            If ur_message_written = False Then
                Have.TempBowl.SelKey(bwnRET).Contents = strLIT_WIKI_MOVE_5_SEC
            End If
        End Function 'Store_UIForm_Title

        Public Shared Function Validate_NewAppNeeded(ur_message_written As Boolean, ur_uiform_title_bowlname As enmTB.zuiform_title, ur_fn_is_running As enfWD.zIsRunningWindow, ur_fn_app_activate As enfWI.zAppActivate) As Boolean
            Validate_NewAppNeeded = ur_message_written
            If ur_message_written = False Then
                Dim strTITLE = Have.TempBowl.SelKey(ur_uiform_title_bowlname).Contents
                If Have.WindowsDiagEnv.Result(ur_fn_is_running, strTITLE) Then
                    Validate_NewAppNeeded = True
                    Have.WindowsIntrActEnv.Result(ur_fn_app_activate, strTITLE)
                End If
            End If
        End Function 'Validate_NewAppNeeded
    End Class 'Assistant



    Partial Public Class Have
        Private Shared prv_frmWikiMove As rWikiMoveForm
        Private Shared prv_envWindowsCboard As glblWindowsCboard
        Private Shared prv_envWindowsDiag As glblWindowsDiag
        Private Shared prv_envWindowsIntrAct As glblWindowsIntrAct
        Private Shared prv_envWindowsMsgBox As glblWindowsMsgBox
        Private Shared prv_envWindowsFS As glblWindowsFS
        Private Shared prv_envWindowsVar As glblWindowsVar
        Private Shared prv_tblFileMove As sFileMove
        Private Shared prv_tblSessionBowl As sSessionBowl
        Private Shared prv_tblTempBowl As sTempBowl
        Private Shared prv_tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.prv_tblUserBowl Is Nothing Then
                prv_frmWikiMove = New rWikiMoveForm()
                Have.prv_envWindowsCboard = New glblWindowsCboard
                Have.prv_envWindowsDiag = New glblWindowsDiag
                Have.prv_envWindowsIntrAct = New glblWindowsIntrAct
                Have.prv_envWindowsMsgBox = New glblWindowsMsgBox
                Have.prv_envWindowsFS = New glblWindowsFS
                Have.prv_envWindowsVar = New glblWindowsVar
                Have.prv_tblFileMove = New sFileMove
                Have.prv_tblSessionBowl = New sSessionBowl
                Have.prv_tblTempBowl = New sTempBowl
                Have.prv_tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have


    Public Class enrMV
        Inherits bitBASE
        Public Shared FormPopupSeq As zFormPopupSeq = TRow(Of enrMV).glbl.Trbase(Of zFormPopupSeq).NewBitBase() : Public Class zFormPopupSeq : Inherits enrMV : End Class
        Public Shared LogText As zLogText = TRow(Of enrMV).glbl.Trbase(Of zLogText).NewBitBase() : Public Class zLogText : Inherits enrMV : End Class
        Public Shared StartPosition As zStartPosition = TRow(Of enrMV).glbl.Trbase(Of zStartPosition).NewBitBase() : Public Class zStartPosition : Inherits enrMV : End Class
        Public Shared TimerInterval As zTimerInterval = TRow(Of enrMV).glbl.Trbase(Of zTimerInterval).NewBitBase() : Public Class zTimerInterval : Inherits enrMV : End Class
        Public Shared TitleText As zTitleText = TRow(Of enrMV).glbl.Trbase(Of zTitleText).NewBitBase() : Public Class zTitleText : Inherits enrMV : End Class
        Public Shared WindowState As zWindowState = TRow(Of enrMV).glbl.Trbase(Of zWindowState).NewBitBase() : Public Class zWindowState : Inherits enrMV : End Class
    End Class

    Public Class enfMV
        Inherits bitBASE
        Public Shared Resize_Form As zResize_Form = TRow(Of enfMV).glbl.Trbase(Of zResize_Form).NewBitBase() : Public Class zResize_Form : Inherits enfMV : End Class
        Public Shared Show_FormAsDialog As zShow_FormAsDialog = TRow(Of enfMV).glbl.Trbase(Of zShow_FormAsDialog).NewBitBase() : Public Class zShow_FormAsDialog : Inherits enfMV : End Class
        Public Shared Start_Timer As zStart_Timer = TRow(Of enfMV).glbl.Trbase(Of zStart_Timer).NewBitBase() : Public Class zStart_Timer : Inherits enfMV : End Class
        Public Shared Stop_Timer As zStop_Timer = TRow(Of enfMV).glbl.Trbase(Of zStop_Timer).NewBitBase() : Public Class zStop_Timer : Inherits enfMV : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WikiMoveForm() As rWikiMoveForm
            Call Have.Connect()
            WikiMoveForm = Have.prv_frmWikiMove
        End Function

        Public Class rWikiMoveForm
            Inherits System.Windows.Forms.Form

            Public tmrFIVE_SEC As System.Windows.Forms.Timer
            Public txtLOG As System.Windows.Forms.TextBox
            Public intFORM_POPUP_SEQ As Integer

            Const strLIT_TXT_LOG = "txtLOG"
            Const strLIT_WIKI_MOVE = "Wiki_Move"

            Public Sub New()
                Me.intFORM_POPUP_SEQ = 1
                Me.Name = strLIT_WIKI_MOVE

                Me.txtLOG = New System.Windows.Forms.TextBox()
                Me.txtLOG.Name = strLIT_TXT_LOG

                Me.txtLOG.Multiline = True
                Me.txtLOG.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtLOG.Dock = System.Windows.Forms.DockStyle.Fill
                Me.Controls.Add(Me.txtLOG)

                Me.tmrFIVE_SEC = New System.Windows.Forms.Timer()
                AddHandler Me.tmrFIVE_SEC.Tick, AddressOf Timer1_Tick
            End Sub 'New

            Public Function Result(ur_fn_resize_form As enfMV.zResize_Form, ur_width As Integer, ur_height As Integer) As Integer
                Result = 1
                Me.Size = New System.Drawing.Size(ur_width, ur_height)
            End Function

            Public Function Result(ur_fn_start_timer As enfMV.zStart_Timer) As Integer
                Result = 1
                Me.tmrFIVE_SEC.Start()
            End Function

            Public Function Result(ur_fn_stop_timer As enfMV.zStop_Timer) As Integer
                Result = 1
                Me.tmrFIVE_SEC.Stop()
            End Function

            Public Function Result(ur_fn_show_form As enfMV.zShow_FormAsDialog) As Integer
                Result = 1
                'Note: System.Windows.Forms.Application.Run(objFORM) only works from a command line program with no forms open. VBNetScript already has a form open.
                Call Me.ShowDialog()
            End Function

            Public Property v(ur_brw As enrMV.zFormPopupSeq) As Integer
                <System.Diagnostics.DebuggerHidden()>
                Get
                    v = Me.intFORM_POPUP_SEQ
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As Integer)
                    Me.intFORM_POPUP_SEQ = value
                End Set
            End Property 'v

            Public Property v(ur_brw As enrMV.zLogText) As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    v = Me.txtLOG.Text
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.txtLOG.Text = value
                End Set
            End Property 'v

            Public Property v(ur_brw As enrMV.zStartPosition) As System.Windows.Forms.FormStartPosition
                <System.Diagnostics.DebuggerHidden()>
                Get
                    v = Me.StartPosition
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As System.Windows.Forms.FormStartPosition)
                    Me.StartPosition = value
                End Set
            End Property 'v

            Public Property v(ur_brw As enrMV.zTimerInterval) As Integer
                <System.Diagnostics.DebuggerHidden()>
                Get
                    v = Me.tmrFIVE_SEC.Interval
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As Integer)
                    Me.tmrFIVE_SEC.Interval = value
                End Set
            End Property 'v

            Public Property v(ur_brw As enrMV.zTitleText) As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    v = Me.Text
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.Text = value
                End Set
            End Property 'v

            Public Property v(ur_brw As enrMV.zWindowState) As System.Windows.Forms.FormWindowState
                <System.Diagnostics.DebuggerHidden()>
                Get
                    v = Me.WindowState
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As System.Windows.Forms.FormWindowState)
                    Me.WindowState = value
                End Set
            End Property 'v

            Sub Timer1_Tick(sender As Object, e As System.EventArgs)
                Call Mx.UserAction.Poll_FileDir_And_Move_errhnd(Me)
            End Sub 'Timer1_Tick
        End Class 'WikiMove_Form
    End Class 'MV


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
            Public Function Result(ur_fn As enfWC.zStoreText, ur_text As String) As Integer
                Result = glbl.gCboard.SetText(ur_text)
            End Function
        End Class 'glblWindowsCboard
    End Class 'Have

    Public Class enfWD
        Inherits bitBASE
        Public Shared IsRunningWindow As zIsRunningWindow = TRow(Of enfWD).glbl.Trbase(Of zIsRunningWindow).NewBitBase() : Public Class zIsRunningWindow : Inherits enfWD : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsDiagEnv() As glblWindowsDiag
            Call Have.Connect()
            WindowsDiagEnv = Have.prv_envWindowsDiag
        End Function

        Public Class glblWindowsDiag
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWD.zIsRunningWindow, ur_title As String) As Boolean
                Result = glbl.gDiagnostics.IsRunningWindow(ur_title)
            End Function
        End Class 'glblWindowsDiag
    End Class 'Have

    Public Class enfWI
        Inherits bitBASE
        Public Shared AppActivate As zAppActivate = TRow(Of enfWI).glbl.Trbase(Of zAppActivate).NewBitBase() : Public Class zAppActivate : Inherits enfWI : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsIntrActEnv() As glblWindowsIntrAct
            Call Have.Connect()
            WindowsIntrActEnv = Have.prv_envWindowsIntrAct
        End Function

        Public Class glblWindowsIntrAct
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWI.zAppActivate, ur_title As String) As Integer
                Result = 1
                Call glbl.gInteraction.AppActivate(ur_title)
            End Function
        End Class 'glblWindowsIntrAct
    End Class 'Have

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
            Public Function Result(ur_fn As enfWM.zUserInputDialog, ur_message As String, Optional ur_style As MsgBoxStyle = MsgBoxStyle.OkOnly) As MsgBoxResult
                Dim strAPP_NAME = Have.SessionBowl.SelKey(enmSB.app_exetitle).Contents
                Result = glbl.gMsgBox.GetResult(ur_message, ur_style, strAPP_NAME)
            End Function
        End Class 'glblWindowsMsgBox
    End Class 'Have

    Public Class enfWF
        Inherits bitBASE
        Public Shared FileDelete As zFileDelete = TRow(Of enfWF).glbl.Trbase(Of zFileDelete).NewBitBase() : Public Class zFileDelete : Inherits enfWF : End Class
        Public Shared FileMove As zFileMove = TRow(Of enfWF).glbl.Trbase(Of zFileMove).NewBitBase() : Public Class zFileMove : Inherits enfWF : End Class
        Public Shared FileSearch As zFileSearch = TRow(Of enfWF).glbl.Trbase(Of zFileSearch).NewBitBase() : Public Class zFileSearch : Inherits enfWF : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsFSEnv() As glblWindowsFS
            Call Have.Connect()
            WindowsFSEnv = Have.prv_envWindowsFS
        End Function

        Public Class glblWindowsFS
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWF.zFileSearch, ur_search_filespec As MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As Sdata
                Try
                    Result = New Sdata().dList(glbl.gWindowsFS.GetFiles(ur_search_filespec.gParentDir, ur_search_filespec.Name, ur_recurse_option))
                Catch ex As System.Exception
                    Result = New Sdata
                End Try
            End Function
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWF.zFileDelete, ur_file_path As String) As Integer
                Result = 0
                Try
                    Call glbl.gWindowsFS.Delete(ur_file_path)
                    Result = 1
                Catch ex As System.Exception
                End Try
            End Function
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWF.zFileMove, ur_source_path As String, ur_dest_path As String) As Integer
                Result = 0
                Try
                    Call glbl.gWindowsFS.Move(ur_source_path, ur_dest_path)
                    Result = 1
                Catch ex As System.Exception
                End Try
            End Function
        End Class 'glblWindowsFS
    End Class 'Have

    Public Class enfWV
        Inherits bitBASE
        Public Shared ExpandVariable As zExpandVariable = TRow(Of enfWV).glbl.Trbase(Of zExpandVariable).NewBitBase() : Public Class zExpandVariable : Inherits enfWV : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsVarEnv() As glblWindowsVar
            Call Have.Connect()
            WindowsVarEnv = Have.prv_envWindowsVar
        End Function

        Public Class glblWindowsVar
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfWV.zExpandVariable, ur_path As String) As String
                Result = glbl.gEnvironment.ExpandEnvironmentVariables(ur_path)
            End Function
        End Class 'glblWindowsCboard
    End Class 'Have


    Public Class enrFM
        Inherits bitBASE
        Public Shared src_file_path As zsrc_file_path = TRow(Of enrFM).glbl.Trbase(Of zsrc_file_path).NewBitBase() : Public Class zsrc_file_path : Inherits enrFM : End Class
        Public Shared dest_file_path As zdest_file_path = TRow(Of enrFM).glbl.Trbase(Of zdest_file_path).NewBitBase() : Public Class zdest_file_path : Inherits enrFM : End Class
    End Class

    Public Class enfFM
        Inherits bitBASE
        Public Shared del_all As zdel_all = TRow(Of enfFM).glbl.Trbase(Of zdel_all).NewBitBase() : Public Class zdel_all : Inherits enfFM : End Class
        Public Shared ins As zins = TRow(Of enfFM).glbl.Trbase(Of zins).NewBitBase() : Public Class zins : Inherits enfFM : End Class
        Public Shared select_all As zselect_all = TRow(Of enfFM).glbl.Trbase(Of zselect_all).NewBitBase() : Public Class zselect_all : Inherits enfFM : End Class
    End Class

    Partial Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function FileMove() As sFileMove
            Call Have.Connect()
            FileMove = Have.prv_tblFileMove
        End Function

        Public Class rFileMove
            Inherits TRow(Of enrFM)
        End Class 'rFileMove

        Public Class sFileMove
            Inherits TablePKStr(Of enrFM, rFileMove)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                MyBase.New(1)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfFM.zdel_all) As Integer
                Result = Me.Count
                Me.DelAll()
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfFM.zins, ur_src_file As String, ur_dest_file As String) As Integer
                Dim trwNEW = Me.InsKey(ur_src_file)
                trwNEW.v(enrFM.dest_file_path) = ur_dest_file
                Result = 1
            End Function 'zins

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enfFM.zselect_all) As Objlist(Of rFileMove)
                Result = Me.SelAll
            End Function 'zselect_all
        End Class 'sFileMove
    End Class 'Have


    Public Class enrSB
        Inherits bitBASE
        Public Shared bowl_name As enrSB = TRow(Of enrSB).glbl.NewBitBase()
        Public Shared contents As enrSB = TRow(Of enrSB).glbl.NewBitBase()
    End Class

    Public Class enmSB
        Inherits bitBASE
        Public Shared application_exe_filepath As zapplication_exe_filepath = TRow(Of enmSB).glbl.Trbase(Of zapplication_exe_filepath).NewBitBase() : Public Class zapplication_exe_filepath : Inherits enmSB : End Class
        Public Shared app_folder As zapp_folder = TRow(Of enmSB).glbl.Trbase(Of zapp_folder).NewBitBase() : Public Class zapp_folder : Inherits enmSB : End Class
        Public Shared app_exetitle As zapp_exetitle = TRow(Of enmSB).glbl.Trbase(Of zapp_exetitle).NewBitBase() : Public Class zapp_exetitle : Inherits enmSB : End Class
        Public Shared cmdline_orig As zcmdline_orig = TRow(Of enmSB).glbl.Trbase(Of zcmdline_orig).NewBitBase() : Public Class zcmdline_orig : Inherits enmSB : End Class
    End Class

    Partial Public Class Have
        Public Shared FirstConnect As Object
        Public Shared CmdLineText As String
        Public Shared CurAssembly As System.Reflection.Assembly
        Public Shared CurExeFilePath As String

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function SessionBowl() As sSessionBowl
            Dim bolFIRST_INIT = (Have.FirstConnect Is Nothing)
            Call Have.Connect()
            SessionBowl = Have.prv_tblSessionBowl
            If bolFIRST_INIT Then
                Have.FirstConnect = "Done"
                Dim first_unnamed_parm_compiler_exe_bowlname = enmUB.script_compiler_exe_filepath
                Dim second_unnamed_parm_path_vbns_bowlname = enmUB.unnamed_parm_path01

                Dim application_exe_folder_bowlname = SessionBowl.Store_ApplicationEXEFilePath(Have.CurAssembly, Have.CurExeFilePath)
                Dim app_exetitle_bowlname = SessionBowl.Store_ApplicationEXETitle(application_exe_folder_bowlname)
                Dim app_folder_bowlname = SessionBowl.Store_ApplicationEXENonDebugFolder(application_exe_folder_bowlname)
                Dim cmdline_orig_bowlname = SessionBowl.Store_Original_CommandLine(Have.CmdLineText)
                Dim cmdline_table_bowlname = Have.UserBowl.Store_CommandLine_ParseTable(cmdline_orig_bowlname, first_unnamed_parm_compiler_exe_bowlname, second_unnamed_parm_path_vbns_bowlname)

                'When from Command Line parameters to show user a dialog box: SessionBowl.SelKey(cmdexport_audit_bowlname).Contents = "1"
                Dim cmdexport_audit_bowlname = enmUB.audit_parms_to_cboard
                Dim fn_user_input_dialog = enfWM.UserInputDialog
                Dim fn_store_text = enfWC.StoreText
                Call Have.UserBowl.Show_UserAuditMessage(cmdexport_audit_bowlname, fn_user_input_dialog, fn_store_text)
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
            Inherits Mx.TablePKEnum(Of enrSB, enmSB, rSessionBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXEFilePath(
                ur_cur_assembly As System.Reflection.Assembly,
                ur_curexe_path As String
                ) As enmSB.zapplication_exe_filepath

                Dim retSB = enmSB.application_exe_filepath
                Store_ApplicationEXEFilePath = retSB
                Dim strCUR_EXE_PATH = mt
                If ur_cur_assembly IsNot Nothing Then
                    strCUR_EXE_PATH = ur_cur_assembly.Location
                End If

                If Mx.HasText(strCUR_EXE_PATH) = False Then
                    strCUR_EXE_PATH = ur_curexe_path
                End If

                Me.SelKey(retSB).Contents = strCUR_EXE_PATH
            End Function 'Store_ApplicationEXEFilePath

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXETitle(ur_application_exe_filepath As enmSB.zapplication_exe_filepath) As enmSB.zapp_exetitle
                Dim retSB = enmSB.app_exetitle
                Store_ApplicationEXETitle = retSB
                Dim flnAPP_EXEPATH = Mx.FileNamed().d(Me.SelKey(ur_application_exe_filepath).Contents)
                Me.SelKey(retSB).Contents = flnAPP_EXEPATH.FileGroup
            End Function 'Store_ApplicationEXETitle

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXENonDebugFolder(ur_application_exe_filepath As enmSB.zapplication_exe_filepath) As enmSB.zapp_folder
                Dim retSB = enmSB.app_folder
                Store_ApplicationEXENonDebugFolder = retSB
                Dim strCUR_EXE_PATH = Me.SelKey(ur_application_exe_filepath).Contents
                Dim flnAPP_PATH = Mx.FileNamed().d(strCUR_EXE_PATH.Replace("\bin\Debug", Mx.mt))
                Me.SelKey(retSB).Contents = flnAPP_PATH.ParentDir
            End Function 'Store_ApplicationEXENonDebugFolder

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_Original_CommandLine(ur_cmdline_text As String) As enmSB.zcmdline_orig
                Dim retSB = enmSB.cmdline_orig
                Store_Original_CommandLine = retSB
                Me.SelKey(retSB).Contents = ur_cmdline_text
            End Function 'Store_Original_CommandLine
        End Class 'sSessionBowl
    End Class 'SB


    Public Class enrTB
        Inherits bitBASE
        Public Shared bowl_name As enrTB = TRow(Of enrTB).glbl.NewBitBase()
        Public Shared contents As enrTB = TRow(Of enrTB).glbl.NewBitBase()
    End Class


    Public Class enmTB
        Inherits bitBASE
        Public Shared poll_folder As zpoll_folder = TRow(Of enmTB).glbl.Trbase(Of zpoll_folder).NewBitBase() : Public Class zpoll_folder : Inherits enmTB : End Class
        Public Shared report_output As zreport_output = TRow(Of enmTB).glbl.Trbase(Of zreport_output).NewBitBase() : Public Class zreport_output : Inherits enmTB : End Class
        Public Shared uiform_title As zuiform_title = TRow(Of enmTB).glbl.Trbase(Of zuiform_title).NewBitBase() : Public Class zuiform_title : Inherits enmTB : End Class
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
            Inherits Mx.TablePKEnum(Of enrTB, enmTB, rTempBowl)
        End Class 'sTempBowl
    End Class 'TB


    Public Class enrUB
        Inherits bitBASE
        Public Shared bowl_name As enrUB = TRow(Of enrUB).glbl.NewBitBase()
        Public Shared contents As enrUB = TRow(Of enrUB).glbl.NewBitBase()
    End Class

    Public Class enmUB
        Inherits bitBASE
        Public Shared audit_parms_to_cboard As zaudit_parms_to_cboard = TRow(Of enmUB).glbl.Trbase(Of zaudit_parms_to_cboard).NewBitBase() : Public Class zaudit_parms_to_cboard : Inherits enmUB : End Class
        Public Shared cmdline_table As zcmdline_table = TRow(Of enmUB).glbl.Trbase(Of zcmdline_table).NewBitBase() : Public Class zcmdline_table : Inherits enmUB : End Class
        Public Shared export_project_code As zexport_project_code = TRow(Of enmUB).glbl.Trbase(Of zexport_project_code).NewBitBase() : Public Class zexport_project_code : Inherits enmUB : End Class
        Public Shared script_compiler_exe_filepath As zscript_compiler_exe_filepath = TRow(Of enmUB).glbl.Trbase(Of zscript_compiler_exe_filepath).NewBitBase() : Public Class zscript_compiler_exe_filepath : Inherits enmUB : End Class
        Public Shared unnamed_parm_path01 As zunnamed_parm_path01 = TRow(Of enmUB).glbl.Trbase(Of zunnamed_parm_path01).NewBitBase() : Public Class zunnamed_parm_path01 : Inherits enmUB : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Call Have.Connect()
            UserBowl = Have.prv_tblUserBowl
        End Function 'UserBowl

        Public Class rUserBowl
            Inherits TRow(Of enrUB)

            Public Property Contents As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enrUB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.v(enrUB.contents) = value
                End Set
            End Property 'Contents
        End Class 'rUserBowl

        Public Class sUserBowl
            Inherits Mx.TablePKEnum(Of enrUB, enmUB, rUserBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Function Show_UserAuditMessage(
                cmdexport_audit_bowlname As enmUB.zaudit_parms_to_cboard,
                ur_fn_user_input_dialog As enfWM.zUserInputDialog,
                ur_store_text As enfWC.zStoreText
                ) As Integer

                Show_UserAuditMessage = 0
                If Mx.HasText(Me.SelKey(cmdexport_audit_bowlname).Contents) Then
                    Show_UserAuditMessage = 1
                    Dim strAUDIT = Me.ToString(True)
                    Dim ins_msg = Have.WindowsMsgBoxEnv.Result(ur_fn_user_input_dialog,
                        ur_message:=strAUDIT,
                        ur_style:=MsgBoxStyle.OkCancel
                        )
                    If ins_msg = MsgBoxResult.Ok Then
                        Have.WindowsCboardEnv.Result(ur_store_text,
                            strAUDIT
                            )
                    End If 'ins_msg
                End If 'me
            End Function 'Show_UserAuditMessage

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_CommandLine_ParseTable(
                ur_cmdline_orig As enmSB.zcmdline_orig,
                ur_first_unnamed_parm_compiler_exe_bowlname As enmUB.zscript_compiler_exe_filepath,
                ur_second_unnamed_parm_path_vbns_bowlname As enmUB.zunnamed_parm_path01
                ) As enmUB.zcmdline_table

                Dim retSB = enmUB.cmdline_table
                Store_CommandLine_ParseTable = retSB
                Dim strCMD_LINE = Have.SessionBowl.SelKey(ur_cmdline_orig).Contents
                Dim arlCMD_RET = Mx.MxText.Cmdline_UB(Of enmUB, enrUB).CommandLine_UBParm(enrUB.bowl_name, enrUB.contents, strCMD_LINE, ur_first_unnamed_parm_compiler_exe_bowlname, ur_second_unnamed_parm_path_vbns_bowlname)
                Me.SelKey(retSB).Contents = Mx.qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(Mx.qs, Mx.qs & Mx.qs) & Mx.qs
                For Each trwPARM In arlCMD_RET.ttbUB_PARM
                    For Each trwBOWL In Me.Sel(enrUB.bowl_name, trwPARM.v(enrUB.bowl_name)).SelAll
                        If Mx.HasText(trwBOWL.Contents) = False Then
                            trwBOWL.Contents = trwPARM.v(enrUB.contents)
                        End If
                    Next trwBOWL
                Next trwPARM
            End Function 'Store_CommandLine_ParseTable
        End Class 'sUserBowl
    End Class 'UB
End Namespace 'Mx
