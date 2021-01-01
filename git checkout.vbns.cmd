start "" "%~dp0\MxClasses\VBNetScript.exe" /path=%0
exit
MxClasses\MxBaseEc13.vb
RetVal = Mx.Want.BranchCheckout_errhnd()
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.BranchCheckout_errhnd()
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
        Public Shared Sub BranchCheckout(ret_msg As Strap)
            Dim flnPERSIST = FileNamed().d(Strapd().d(glbl.gEnvironment.ExpandEnvironmentVariables("%APPDATA%")).dSprtr("\", "DeveloperCustomApp_settings").dSprtr("\", "user.config.tsv"))
            Have.UserBowl.SelKey(enmUN.persist_path).Contents = flnPERSIST
            Call Have.CustomApp.PersistRead(flnPERSIST)

            Dim strAPP_NAME = Have.UserBowl.SelKey(enmUN.app_name).Contents
            Dim strGIT_PATH = mt
            Dim strPREFIX = mt
            Dim strBRANCH = mt
            For Each rowENTRY In Have.CustomApp.Sel(enmSG.setting_name, "git_path").SelAll
                strGIT_PATH = rowENTRY.v(enmSG.setting_value)
            Next

            If HasText(strGIT_PATH) = False Then
                ret_msg.d("Cannot find DeveloperCustomApp_settings for git_path")
            End If

            If ret_msg.HasText = False Then
                Dim flnROOT_PATH = FileNamed().d(Have.UserBowl.SelKey(enmUN.script_folder).Contents)
                If glbl.gWindowsFS.HasDir(flnROOT_PATH.gCopy.d(".git")) Then
                    flnROOT_PATH = flnROOT_PATH
                    strPREFIX = flnROOT_PATH.Name

                Else
                    strPREFIX = InputBox("Enter a folder prefix (like tpr):", "Git Export Log")
					If HasText(strPREFIX) Then
						flnROOT_PATH.d(strPREFIX & "_gotoassisttickets")
					End If
                End If 'flnROOT_PATH

                If HasText(strPREFIX) Then
                    strBRANCH = InputBox("Enter a branch hash:", "Git Export Log")
                End If

                If HasText(strBRANCH) Then
                    'Dim strGIT_PATH = FileNamed().d(Have.UserBowl.SelKey(enmUN.script_folder).Contents).d("PortableGit\cmd\git.exe").gFullPath
                    Dim prcGIT = glbl.gDiagnostics.Start_CommandLine_Program(strGIT_PATH, Strapd().d("-C").dS(qs).d(flnROOT_PATH).d(qs).dS("checkout").dS("-b").dS("test-branch").dS(strBRANCH))
                    Dim strLINES = prcGIT.StandardError.ReadToEnd
                    Call prcGIT.WaitForExit()
                    glbl.gMsgBox.GetResult(strLINES, MsgBoxStyle.OkOnly, strAPP_NAME)
                End If 'strPREFIX
            End If
        End Sub 'BranchCheckout

        Public Shared Function BranchCheckout_errhnd() As Strap
            Dim stpRET = Strapd()
            BranchCheckout_errhnd = stpRET
            Dim objERR_LIST = New ErrListBase : Try
                Call BranchCheckout(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
			
			If stpRET.HasText = False Then
				stpRET.d("QUIT")
			End If
        End Function 'BranchCheckout_errhnd
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
End Namespace 'Mx
