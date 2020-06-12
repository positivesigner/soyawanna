Option Strict On
Namespace My
    Partial Friend Class MyApplication
        Sub Main(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Mx.Want.TestReferencedAssemblies_errhnd(e)
            'Next goes to frmInput_1_Load, then Want.Compile_And_Run_Script
        End Sub
    End Class
End Namespace

Namespace Mx
    Public Class Want
        Public Shared Sub Compile_And_Run_Script(ur_form As frmInput_1, ur_command_text As String)
            Dim userbowl_cart = Have.UserBowl
            Dim strEXPORT_PROJECT_CODE_PATH = userbowl_cart.SelKey(enmUN.cmd_export_project_code).v(enmUB.contents)
            If HasText(strEXPORT_PROJECT_CODE_PATH) Then
                Call Want.Export_Project_Code(ur_form, userbowl_cart)

            Else
                Dim input_path_bowlname = enmUN.path
                Dim app_folder_bowlname = enmUN.app_folder
                Dim script_path_bowlname = userbowl_cart.Apply(input_path_bowlname, app_folder_bowlname)
                Dim form_title_bowlname = userbowl_cart.Apply(script_path_bowlname, ur_form)
                Dim report_output_bowlname = userbowl_cart.Apply(script_path_bowlname, app_folder_bowlname)
                Dim window_color_bowlname = userbowl_cart.Apply(report_output_bowlname)
                Dim form_close_bowlname = userbowl_cart.Apply(report_output_bowlname, window_color_bowlname, ur_form)
            End If
        End Sub 'Compile_And_Run_Script

        Public Shared Sub TestReferencedAssemblies()
            'Will error if DLL does is not available
            Dim objV1 = System.Data.SqlClient.SortOrder.Ascending
        End Sub 'TestReferencedAssemblies

        Public Shared Sub Export_Project_Code(ur_form As frmInput_1, ur_userbowl_cart As Have.sUserBowl)
            Dim strPROJECT_FILE = "VBNetScript_Project.zip"
            Dim strRESOURCE_FILE = mt
            Dim objASSEMBLY = System.Reflection.Assembly.GetExecutingAssembly
            For Each strENTRY In objASSEMBLY.GetManifestResourceNames()
                If EndingWithText(strENTRY, strPROJECT_FILE) Then
                    strRESOURCE_FILE = strENTRY
                    Exit For
                End If
            Next strENTRY

            If HasText(strRESOURCE_FILE) = False Then
                Throw New System.Exception(Strapd().d("Cannot find resource:").dS(strPROJECT_FILE))
            Else
                Dim flnDEST_PATH = FileNamed().d(ur_userbowl_cart.SelKey(enmUN.app_folder).v(enmUB.contents)).d(strPROJECT_FILE)
                Dim ins_msg = Have.MessageBox.Ins(
                New Have.rMessageBox().
                    vt(enmMB.title, ur_userbowl_cart.SelKey(enmUN.app_name).v(enmUB.contents)).
                    vt(enmMB.text, Strapd().d("Export VBNetScript_Project.zip file to").dS(qs).d(flnDEST_PATH).d(qs).d("?")),
                    MsgBoxStyle.OkCancel
                    )
                If ins_msg.vUserResponse = MsgBoxResult.Ok Then
                    Using objRESOURCE = objASSEMBLY.GetManifestResourceStream(strRESOURCE_FILE)
                        Using objFILE = New System.IO.FileStream(flnDEST_PATH, System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            objRESOURCE.CopyTo(objFILE)
                        End Using
                    End Using
                End If

                ur_form.Close()
            End If
        End Sub 'Export_Project_Code

        Public Shared Sub Show_CommandLine_Options()
            Dim stpHELP = Strapd()
            stpHELP.d(
"VBNetScript.exe /path=""UrCodeFile.vbns""

/path specifies a code file in this format:

Skipped- start "" VBNetScript.exe /path script.vbns
Skipped exit
Optional - UrRef.dll
Optional - UrInclude.vb
Optional - Option Strict On, or some other Option On/Off
Optional - Imports UrNamespace
UrCodeLines

Add UrCodeLines in the following format:

The program automatically adds NameSpace Mx
The program automatically adds Class Class1
The program automatically adds Function RetVal() As String
UrFunctionLines
Optional - RetVal = UrReturnValue
Optional - End Function
Optional - More Functions and Subs
Optional - End Class
Optional - More Classes
Optional - End Namespace
Optional - More Namespaces

VBNetScript.exe /cmd_export_cmdline_audit=True

/cmd_export_cmdline_audit will show the command-line audit table in a message box, and then allow you to copy the table to the clipboard

VBNetScript.exe /cmd_export_project_code=True

/cmd_export_project_code will ask for a folder in which to save the VBNetScript Project .ZIP file

* Note: Command-line parameters may not work in .lnk Windows Shortcut files. Please call from a .cmd file to test.
")
            Dim ins_msg = Have.MessageBox.Ins(
                New Have.rMessageBox().
                    vt(enmMB.title, "VBNetScript").
                    vt(enmMB.text, stpHELP),
                MsgBoxStyle.OkCancel
                )
            If ins_msg.vUserResponse = MsgBoxResult.Ok Then
                Have.Clipboard.Ins(
                    New Have.rClipboard().
                    vt(enmCB.text, stpHELP)
                    )
            End If
        End Sub 'Show_CommandLine_Options


        Public Shared Sub Compile_And_Run_Script_errhnd(ur_form As frmInput_1, ur_command_text As String)
            Dim objERR_LIST = New ErrListBase : Try
                Call Compile_And_Run_Script(ur_form, ur_command_text)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                MsgBox(objERR_LIST.ToString, , My.Application.Info.Title)
                Call Want.Show_CommandLine_Options()
                Call ur_form.Close()
            End If
        End Sub 'Compile_And_Run_Script_errhnd

        Public Shared Sub TestReferencedAssemblies_errhnd(ur_startup_event_args As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs)
            Dim objERR_LIST = New ErrListBase : Try
                Call Want.TestReferencedAssemblies()

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                MsgBox(objERR_LIST.ToString, , My.Application.Info.Title)
                ur_startup_event_args.Cancel = True
            End If
        End Sub 'TestReferencedAssemblies_errhnd
    End Class 'Want


    Public Class Have
        Partial Class sUserBowl
            Public Function Apply(ur_in_path As enmUN.zpath, ur_app_folder As enmUN.zapp_folder) As enmUN.zscript_path
                Dim retKEY = enmUN.script_path
                Apply = retKEY
                Dim script_path = Me.SelKey(retKEY)
                Dim strFOUND_PATH = Me.SelKey(ur_in_path).v(enmUB.contents)
                If HasText(strFOUND_PATH) = False Then
                    Throw New System.Exception(Strapd().d("Script file not provided."))

                ElseIf glbl.gWindowsFS.HasFile(strFOUND_PATH) = False Then
                    Dim flnAPP_RELATIVE = FileNamed().d(Me.SelKey(ur_app_folder).v(enmUB.contents)).d(strFOUND_PATH)
                    If glbl.gWindowsFS.HasFile(flnAPP_RELATIVE) = False Then
                        Throw New System.Exception(Strapd().d("Script file not found:").dS(strFOUND_PATH))

                    Else
                        script_path.vt(enmUB.contents, flnAPP_RELATIVE)
                    End If

                Else
                    script_path.vt(enmUB.contents, strFOUND_PATH)
                End If
            End Function 'Apply(ur_in_path

            Public Function Apply(ur_script_path As enmUN.zscript_path, ur_form1 As frmInput_1) As enmUN.zform_title
                Dim retKEY = enmUN.form_title
                Apply = retKEY
                Dim strFORM_TITLE = Me.SelKey(ur_script_path).v(enmUB.contents)
                ur_form1.Text = strFORM_TITLE
                Me.InsKey(retKEY, strFORM_TITLE)
            End Function

            Public Function Apply(ur_script_path As enmUN.zscript_path, ur_app_folder As enmUN.zapp_folder) As enmUN.zreport_output
                Dim retKEY = enmUN.report_output
                Apply = retKEY
                retKEY = enmUN.report_output
                Dim report_output = Me.SelKey(retKEY)
                Dim strSCRIPT_PATH = Me.SelKey(ur_script_path).v(enmUB.contents)
                If HasText(strSCRIPT_PATH) Then
                    Dim strREPORT_OUTPUT = Mx.ScriptRun.RunCode(Mx.ScriptRun.enmC_V_J.VisualBasic, strSCRIPT_PATH, Me.SelKey(ur_app_folder).v(enmUB.contents))
                    report_output.vt(enmUB.contents, strREPORT_OUTPUT)
                End If
            End Function 'Apply(ur_script_path

            Public Function Apply(ur_report_output As enmUN.zreport_output, ur_background_color As enmUN.zbackground_color, ur_form1 As frmInput_1) As enmUN.zwindow_status
                Dim retKEY = enmUN.window_status
                Apply = retKEY
                Dim bitSTATUS = enmUR_Status.Show
                Dim flgSHOW_FORM = True
                Dim strNOTICE_MSG = Me.SelKey(ur_report_output).v(enmUB.contents).Trim
                If StartingWithText(strNOTICE_MSG, "Quit") = False Then
                    Dim trwBG_COLOR = Me.SelKey(ur_background_color)
                    If trwBG_COLOR.v_is(enmUR_Color.Clear) = False Then
                        ur_form1.BackColor = trwBG_COLOR.Background_Color
                    End If

                    ur_form1.txtNotice_Message.Text = strNOTICE_MSG

                Else
                    bitSTATUS = enmUR_Status.Close
                    Call ur_form1.Close()
                End If

                Me.InsKey(retKEY, bitSTATUS.name)
            End Function 'Apply(ur_report_output, ur_background_color

            Public Function Apply(ur_report_output As enmUN.zreport_output) As enmUN.zbackground_color
                Dim retKEY = enmUN.background_color
                Apply = retKEY
                retKEY = enmUN.background_color
                Dim bitCOLOR = enmUR_Color.Clear
                Dim strNOTICE_MSG = Me.SelKey(ur_report_output).v(enmUB.contents).Trim
                If StartingWithText(strNOTICE_MSG, "RED") Then
                    bitCOLOR = enmUR_Color.Red

                ElseIf StartingWithText(strNOTICE_MSG, "GREEN") Then
                    bitCOLOR = enmUR_Color.Green
                End If

                Me.InsKey(retKEY, bitCOLOR)
            End Function 'Apply(ur_bowl_key
        End Class 'sUserBowl
    End Class 'Have


    Partial Public Class Have
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect

        Partial Private Class prv
            Public Class enmCT
                Inherits bitBASE
                Public Shared eq As enmCT = TRow(Of enmCT).glbl.NewBitBase()
                Public Shared fslash As enmCT = TRow(Of enmCT).glbl.NewBitBase()
                Public Shared qs As enmCT = TRow(Of enmCT).glbl.NewBitBase()
                Public Shared txt As enmCT = TRow(Of enmCT).glbl.NewBitBase()
                Public Shared ws As enmCT = TRow(Of enmCT).glbl.NewBitBase()
            End Class

            Public Shared Function CommandLine_Chunk(ur_source_text As String) As Sdata
                Dim retCHUNK = New Sdata
                CommandLine_Chunk = retCHUNK
                For CHRCTR = 0 To ur_source_text.Length - 1
                    Dim chrC = ur_source_text(CHRCTR)
                    Dim intCHUNK_C = gChunkType(chrC)
                    Dim intSPAN = 1
                    For SPNCTR = CHRCTR + 1 To ur_source_text.Length - 1
                        Dim chrS = ur_source_text(SPNCTR)
                        If gChunkType(chrS) IsNot intCHUNK_C Then
                            intSPAN = SPNCTR - CHRCTR
                            Exit For
                        End If

                        If SPNCTR = ur_source_text.Length Then
                            intSPAN = SPNCTR + 1 - CHRCTR
                            Exit For
                        End If
                    Next SPNCTR

                    retCHUNK.d(ur_source_text.Substring(CHRCTR, intSPAN))
                    CHRCTR += intSPAN - 1
                Next CHRCTR
            End Function 'CommandLine_Chunk

            Public Shared Function gChunkType(ur_char As Char) As enmCT
                If ur_char = vbCr OrElse
                  ur_char = vbLf OrElse
                  ur_char = " "c OrElse
                  ur_char = vbTab Then
                    gChunkType = enmCT.ws

                ElseIf ur_char = "="c OrElse
                  ur_char = ":"c Then
                    gChunkType = enmCT.eq

                ElseIf ur_char = "/"c Then
                    gChunkType = enmCT.fslash

                ElseIf ur_char = """"c Then
                    gChunkType = enmCT.qs

                Else
                    gChunkType = enmCT.txt
                End If
            End Function 'gChunkType
        End Class 'prv
    End Class 'Have

    Public Class enmUB
        Inherits bitBASE
        Public Shared bowl_name As enmUB = TRow(Of enmUB).glbl.NewBitBase()
        Public Shared contents As enmUB = TRow(Of enmUB).glbl.NewBitBase()
    End Class

    Public Class enmUN
        Inherits bitBASE
        Public Shared app_folder As zapp_folder = TRow(Of enmUN).glbl.Trbase(Of zapp_folder).NewBitBase() : Public Class zapp_folder : Inherits enmUN : End Class
        Public Shared app_name As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared app_path As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared background_color As zbackground_color = TRow(Of enmUN).glbl.Trbase(Of zbackground_color).NewBitBase() : Public Class zbackground_color : Inherits enmUN : End Class
        Public Shared cmdline_orig As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmd_export_cmdline_audit As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmd_export_project_code As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared compiler_exe As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared form_title As zform_title = TRow(Of enmUN).glbl.Trbase(Of zform_title).NewBitBase() : Public Class zform_title : Inherits enmUN : End Class
        Public Shared path As zpath = TRow(Of enmUN).glbl.Trbase(Of zpath).NewBitBase() : Public Class zpath : Inherits enmUN : End Class
        Public Shared script_path As zscript_path = TRow(Of enmUN).glbl.Trbase(Of zscript_path).NewBitBase() : Public Class zscript_path : Inherits enmUN : End Class
        Public Shared report_output As zreport_output = TRow(Of enmUN).glbl.Trbase(Of zreport_output).NewBitBase() : Public Class zreport_output : Inherits enmUN : End Class
        Public Shared window_status As zwindow_status = TRow(Of enmUN).glbl.Trbase(Of zwindow_status).NewBitBase() : Public Class zwindow_status : Inherits enmUN : End Class
    End Class

    Public Class enmUR_Color
        Inherits bitBASE
        Public Shared Clear As enmUR_Color = TRow(Of enmUR_Color).glbl.NewBitBase()
        Public Shared Green As enmUR_Color = TRow(Of enmUR_Color).glbl.NewBitBase()
        Public Shared Red As enmUR_Color = TRow(Of enmUR_Color).glbl.NewBitBase()
    End Class

    Public Class enmUR_Status
        Inherits bitBASE
        Public Shared Show As enmUR_Status = TRow(Of enmUR_Status).glbl.NewBitBase()
        Public Shared Close As enmUR_Status = TRow(Of enmUR_Status).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Dim bolFIRST_INIT = (Have.tblUserBowl Is Nothing)
            Call Have.Connect()
            UserBowl = Have.tblUserBowl
            If bolFIRST_INIT Then
                Call Have.tblUserBowl.InsFrom_Application()
                'db.tblUserBowl.InsKey(enmUN.cmdline_audit, "1")
                Call Have.tblUserBowl.Cboard_CmdlineAudit()
            End If
        End Function

        Public Class rUserBowl
            Inherits TRow(Of enmUB)
            Public Background_Color As System.Drawing.Color

            <System.Diagnostics.DebuggerHidden()>
            Public Function v_bowlname_is(ur_cmp As enmUN) As Boolean
                v_bowlname_is = AreEqual(Me.v(enmUB.bowl_name), ur_cmp.name)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function v_is(ur_cmp As enmUR_Color) As Boolean
                v_is = AreEqual(Me.v(enmUB.contents), ur_cmp.name)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmUB, ur_val As String) As rUserBowl
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rUserBowl

        Public Class sUserBowl
            Private ttb As Objlist(Of rUserBowl)
            Private PK As Objlist(Of enmUB)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rUserBowl)
                Me.PK = New Objlist(Of enmUB)
                Me.PK.Add(enmUB.bowl_name)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Cboard_CmdlineAudit()
                If HasText(Me.SelKey(enmUN.cmd_export_cmdline_audit).v(enmUB.contents)) Then
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
            Public Sub Ins(ur_from As rUserBowl)
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
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFrom_Application() As rUserBowl
                Dim ret = New rUserBowl
                InsFrom_Application = ret
                Dim strASSEMBLY_EXE = My.Application.Info.Title
                Dim flnASSEMBLY_FOLDER = FileNamed().wAssemblyDir(My.Application.Info)
                Dim flnASSEMBLY_PATH = flnASSEMBLY_FOLDER.gCopy.d(strASSEMBLY_EXE)
                Me.InsKey(enmUN.app_name, flnASSEMBLY_PATH.FileGroup)
                Me.InsKey(enmUN.app_path, flnASSEMBLY_PATH)
                Me.InsKey(enmUN.app_folder, flnASSEMBLY_FOLDER)

                'enmUN.compiler_exe, enmUN.path take the first non-prefixed parameters; they are just file paths but without /parameter_name=
                Dim arlCMD_RET = MxText.Cmdline_UB(Of enmUN, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, System.Environment.CommandLine, enmUN.compiler_exe, enmUN.path)
                Me.InsKey(enmUN.cmdline_orig, qs & System.Environment.CommandLine.Replace(qs, qs & qs) & qs)
                Me.InsKey(enmUN.cmdline_table, qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(qs, qs & qs) & qs)
                For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                    Me.Ins(
                        New Have.rUserBowl().
                        vt(enmUB.bowl_name, rowFOUND.v(enmUB.bowl_name)).
                        vt(enmUB.contents, rowFOUND.v(enmUB.contents))
                        )
                Next
            End Function 'InsFrom_Application

            <System.Diagnostics.DebuggerHidden()>
            Public Sub InsKey(ur_key As enmUN, ur_val As String)
                Dim ret = Me.SelKey(ur_key)
                If HasText(ret.v(enmUB.contents)) Then
                    Throw New System.Exception("Cannot insert duplicate key for key: " & ur_key.name)
                Else
                    ret.vt(enmUB.contents, ur_val)
                End If
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub InsKey(ur_key As enmUN, ur_val As enmUR_Color)
                Dim ret = Me.SelKey(ur_key)
                Dim intCOLOR = System.Drawing.Color.Transparent
                If ur_val Is enmUR_Color.Red Then
                    intCOLOR = System.Drawing.Color.Red

                ElseIf ur_val Is enmUR_Color.Green Then
                    intCOLOR = System.Drawing.Color.Green
                End If

                If ur_key Is enmUN.background_color Then
                    ret.Background_Color = intCOLOR
                End If

                ret.vt(enmUB.contents, ur_val.name)
            End Sub 'InsKey

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
                Next

                If ret Is Nothing Then
                    ret = New rUserBowl
                    Me.Ins(
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