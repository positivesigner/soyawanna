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
RetVal = Mx.UserAction.Deployment_Report_errhnd()
End Function
End Class
End Namespace
'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.UserAction.Deployment_Report_errhnd()
'
'            If RetVal <> "QUIT" Then If MsgBox(RetVal, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then My.Computer.Clipboard.SetText(RetVal)
'        End Sub
'    End Module 'subs
'
'    Public Class Class1
'        Public Shared SourceFolder As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "")
'        Public Shared SourcePath As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "") & "\test.vbns.cmd"
'    End Class
'End Namespace 'Mx


Namespace Mx
    Public Class UserAction
        Public Shared Sub Deployment_Report(ret_message As Strap)
            For Each strFILE In glbl.gWindowsFS.GetFiles("C:\Gthb", "vbnetscript.exe", IO.SearchOption.AllDirectories)
                ret_message.dLine(strFILE)
                Dim sha512 = New System.Security.Cryptography.SHA512Managed
                Using stmFILE = System.IO.File.OpenRead(strFILE)
                    Dim hash As Byte() = sha512.ComputeHash(stmFILE)
                    ret_message.dLine("-").d(s)
                    ret_message.d(Convert.ToBase64String(hash))
                End Using
            Next strFILE
        End Sub 'Deployment_Report
        Public Shared Function Deployment_Report_errhnd() As Strap
            Dim stpRET = Strapd() : Deployment_Report_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try
                Call UserAction.Deployment_Report(stpRET)
            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try
            If objERR_LIST.Found Then : stpRET.Clear().d(objERR_LIST.ToString) : End If
            If stpRET.HasText = False Then : stpRET.d("QUIT") : End If
        End Function 'Deployment_Report_errhnd
    End Class 'UserAction


    Class Assistant

    End Class 'Assistant



    Partial Public Class Have
        Private Shared prv_envWindowsCboard As glblWindowsCboard
        Private Shared prv_envWindowsMsgBox As glblWindowsMsgBox
        Private Shared prv_tblSessionBowl As sSessionBowl
        Private Shared prv_tblTempBowl As sTempBowl
        Private Shared prv_tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.prv_tblUserBowl Is Nothing Then
                Have.prv_envWindowsCboard = New glblWindowsCboard
                Have.prv_envWindowsMsgBox = New glblWindowsMsgBox
                Have.prv_tblSessionBowl = New sSessionBowl
                Have.prv_tblTempBowl = New sTempBowl
                Have.prv_tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have


    Public Class enmWC
        Inherits bitBASE
        Public Shared StoreText As zStoreText = TRow(Of enmWC).glbl.Trbase(Of zStoreText).NewBitBase() : Public Class zStoreText : Inherits enmWC : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsCboardEnv() As glblWindowsCboard
            Call Have.Connect()
            WindowsCboardEnv = Have.prv_envWindowsCboard
        End Function

        Public Class glblWindowsCboard
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWC.zStoreText, ur_text As String) As Integer
                Result = glbl.gCboard.SetText(ur_text)
            End Function
        End Class 'glblWindowsCboard
    End Class 'Have

    Public Class enmWM
        Inherits bitBASE
        Public Shared UserInputDialog As zUserInputDialog = TRow(Of enmWM).glbl.Trbase(Of zUserInputDialog).NewBitBase() : Public Class zUserInputDialog : Inherits enmWM : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsMsgBoxEnv() As glblWindowsMsgBox
            Call Have.Connect()
            WindowsMsgBoxEnv = Have.prv_envWindowsMsgBox
        End Function

        Public Class glblWindowsMsgBox
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWM.zUserInputDialog, ur_message As String, Optional ur_style As MsgBoxStyle = MsgBoxStyle.OkOnly) As MsgBoxResult
                Dim strAPP_NAME = Have.SessionBowl.SelKey(enmSB.app_exetitle).Contents
                Result = glbl.gMsgBox.GetResult(ur_message, ur_style, strAPP_NAME)
            End Function
        End Class 'glblWindowsMsgBox
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

                'When from Command Line parameters to show user a dialog box: SessionBowl.SelKey(cmdexport_audit_bowlname).Contents = "1"
                Dim cmdexport_audit_bowlname = enmUB.audit_parms_to_cboard
                Dim fn_user_input_dialog = enmWM.UserInputDialog
                Dim fn_store_text = enmWC.StoreText
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
        End Class 'sUserBowl
    End Class 'SB


    Public Class enrTB
        Inherits bitBASE
        Public Shared bowl_name As enrTB = TRow(Of enrTB).glbl.NewBitBase()
        Public Shared contents As enrTB = TRow(Of enrTB).glbl.NewBitBase()
    End Class

    Public Class enmTB
        Inherits bitBASE
        Public Shared report_output As zreport_output = TRow(Of enmTB).glbl.Trbase(Of zreport_output).NewBitBase() : Public Class zreport_output : Inherits enmTB : End Class
        Public Shared destinationfolder_list_filepath As zdestinationfolder_list_filepath = TRow(Of enmTB).glbl.Trbase(Of zdestinationfolder_list_filepath).NewBitBase() : Public Class zdestinationfolder_list_filepath : Inherits enmTB : End Class
        Public Shared sourcefile_list_filepath As zsourcefile_list_filepath = TRow(Of enmTB).glbl.Trbase(Of zsourcefile_list_filepath).NewBitBase() : Public Class zsourcefile_list_filepath : Inherits enmTB : End Class
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

        End Class 'sUserBowl
    End Class 'UB
End Namespace 'Mx
