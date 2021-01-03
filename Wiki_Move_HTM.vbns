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
@START "" "%mx_dir%\MxClasses\VBNetScript.exe" /path=%0

:mx_end
@EXIT
MxClasses\DLL_WinForm2019m09d13\System.Drawing.dll
MxClasses\DLL_WinForm2019m09d13\System.Windows.Forms.dll
MxClasses\MxBaseEc13.vb
RetVal = Mx.Want.UITimer_to_Poll_FileDir_And_Move_errhnd(System.Environment.CommandLine)
End Function '2021m01d03
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.UITimer_to_Poll_FileDir_And_Move_errhnd(System.Environment.CommandLine)
'            If Mx.AreEqual(RetVal, "QUIT") = False Then MsgBox(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "")
'        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
'    End Class
'End Namespace 'Mx

Namespace Mx
    Public Class Want
        Const strLIT_WIKI_MOVE_5_SEC = "Wiki Move 5-Sec"
        Const strvLIT_USER_PROFILE_DOWNLOADS = "%USERPROFILE%\downloads"
        Const strLIT_STAR_DOT_HTM = "WikiMove_*-stamp-*.htm"
        Const strLIT_STAR_DOT_HTML = "WikiMove_*-stamp-*.html"
        Const strLIT_STAR_DOT_CARDTXT = "WikiMove_*-stamp-*.card*.txt"

        Public Shared Sub UITimer_to_Poll_FileDir_And_Move(ur_ret As Strap)
            ur_ret.d("QUIT")
            Dim userbowl_cart = Have.UserBowl
            Dim uiform_title_bowlname = enmUN.uiform_title
            Dim strUI_FORM_TITLE = strLIT_WIKI_MOVE_5_SEC
            userbowl_cart.SelKey(uiform_title_bowlname).Contents = strUI_FORM_TITLE
            userbowl_cart.SelKey(enmUN.poll_folder).Contents = glbl.gEnvironment.ExpandEnvironmentVariables(strvLIT_USER_PROFILE_DOWNLOADS)
            If glbl.gDiagnostics.IsRunningWindow(strUI_FORM_TITLE) Then
                glbl.gInteraction.AppActivate(strUI_FORM_TITLE)

            Else
                Dim objFORM = New Mx.WikiMove_Form()
                objFORM.Text = userbowl_cart.SelKey(enmUN.uiform_title).v(enmUB.contents)
                Call objFORM.Display_FolderList()
                objFORM.tmrFIVE_SEC.Start()
                Dim strNEW_DATA_FOLDER = glbl.gEnvironment.ExpandEnvironmentVariables(strvLIT_USER_PROFILE_DOWNLOADS)

                'System.Windows.Forms.Application.Run(objFORM) only works from a command line program with no forms open. VBNetScript already has a form open.
                Call objFORM.ShowDialog()
            End If 'gDiagnostics
        End Sub 'UITimer_to_Poll_FileDir_And_Move

        Public Shared Function UITimer_to_Poll_FileDir_And_Move_errhnd(ur_commandline_text As String) As Strap
            Have.CmdLineText = ur_commandline_text
            Dim stpRET = Strapd()
            UITimer_to_Poll_FileDir_And_Move_errhnd = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Call UITimer_to_Poll_FileDir_And_Move(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'UITimer_to_Poll_FileDir_And_Move_errhnd

        Public Shared Sub Poll_FileDir_And_Move(ur_ui_form As WikiMove_Form)
            Dim userbowl_cart = Have.UserBowl
            Dim pollfolder_bowlname = enmUN.poll_folder
            Dim windows_fs_cart = Have.WindowsFS
            Dim filemove_cart = Have.FileMove
            Call ur_ui_form.Display_FolderList()
            filemove_cart.DelAll()
            filemove_cart.Apply(windows_fs_cart, userbowl_cart, pollfolder_bowlname, strLIT_STAR_DOT_HTM)
            filemove_cart.Apply(windows_fs_cart, userbowl_cart, pollfolder_bowlname, strLIT_STAR_DOT_HTML)
            filemove_cart.Apply(windows_fs_cart, userbowl_cart, pollfolder_bowlname, strLIT_STAR_DOT_CARDTXT)
            filemove_cart.Move_Files()
            Dim report_output_bowlname = userbowl_cart.Apply(filemove_cart)
            Dim strREPORT_OUTPUT = userbowl_cart.SelKey(report_output_bowlname).v(enmUB.contents)
            If HasText(strREPORT_OUTPUT) Then
                ur_ui_form.txtOUTPUT.Text = strREPORT_OUTPUT & vbCrLf & ur_ui_form.txtOUTPUT.Text
                ur_ui_form.intSHOW_FORM = 1
            End If
        End Sub 'Poll_FileDir_And_Move

        Public Shared Sub Poll_FileDir_And_Move_errhnd(ur_ui_form As WikiMove_Form)
            Dim objERR_LIST = New ErrListBase : Try
                Call Poll_FileDir_And_Move(ur_ui_form:=ur_ui_form)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                ur_ui_form.txtOUTPUT.Text = objERR_LIST.ToString & vbCrLf & ur_ui_form.txtOUTPUT.Text
                ur_ui_form.intSHOW_FORM = 1
            End If
        End Sub 'Poll_FileDir_And_Move_errhnd

        Public Class CombineTextOutput
            Public gText As Strap
            Public gFolderList As Sdata

            Public Sub New(ur_text As Strap, ur_folder_list As Sdata)
                Me.gText = ur_text
                Me.gFolderList = ur_folder_list
            End Sub
        End Class 'CombineTextOutput
    End Class 'sub_main

    Public Class WikiMove_Form
        Inherits System.Windows.Forms.Form

        Public tmrFIVE_SEC As System.Windows.Forms.Timer
        Public txtOUTPUT As System.Windows.Forms.TextBox
        Public intSHOW_FORM As Integer

        Public Sub New()
            Me.intSHOW_FORM = 1

            Me.Name = "Wiki_Move"
            Me.Size = New System.Drawing.Size(400, 400)
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen

            Me.txtOUTPUT = New System.Windows.Forms.TextBox()
            Me.txtOUTPUT.Name = "txtOUTPUT"
            Me.txtOUTPUT.Multiline = True
            Me.txtOUTPUT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtOUTPUT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Controls.Add(Me.txtOUTPUT)

            Me.tmrFIVE_SEC = New System.Windows.Forms.Timer()
            Me.tmrFIVE_SEC.Interval = 50
            AddHandler Me.tmrFIVE_SEC.Tick, AddressOf Timer1_Tick
        End Sub 'New

        Sub Display_FolderList()
            Dim userbowl_cart = Have.UserBowl
            Dim pollfolder_bowlname = enmUN.poll_folder
            If HasText(Me.txtOUTPUT.Text) = False Then
                Me.txtOUTPUT.Text = Strapd().dLine("Listening:").dS(userbowl_cart.SelKey(pollfolder_bowlname).v(enmUB.contents)).dLine().d(Me.txtOUTPUT.Text)
            End If
        End Sub 'Display_FolderList

        Sub Look_For_Files()
            Call Mx.Want.Poll_FileDir_And_Move_errhnd(Me)
        End Sub

        Sub Show_Saved()
            Dim userbowl_cart = Have.UserBowl
            Dim uiform_title_bowlname = enmUN.uiform_title
            If Me.intSHOW_FORM = 1 Then
                Me.WindowState = System.Windows.Forms.FormWindowState.Normal
                Call AppActivate(userbowl_cart.SelKey(uiform_title_bowlname).v(enmUB.contents))
                Me.tmrFIVE_SEC.Interval = 800
                Me.intSHOW_FORM = 2

            ElseIf Me.intSHOW_FORM = 2 Then
                Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
                Me.intSHOW_FORM = 0

            ElseIf Me.intSHOW_FORM = 0 Then
                If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
                    Me.intSHOW_FORM = 6
                End If

            Else
                Me.intSHOW_FORM -= 1
            End If
        End Sub 'Show_Saved

        Sub Timer1_Tick(sender As Object, e As System.EventArgs)
            Me.tmrFIVE_SEC.Stop()
            Me.tmrFIVE_SEC.Interval = 5000
            Call Look_For_Files()
            Call Show_Saved()
            Me.tmrFIVE_SEC.Start()
        End Sub 'Timer1_Tick
    End Class 'WikiMove_Form




    Public Class Have
        Partial Class sFileMove
            Public Sub Apply(ur_windows_fs_cart As Have.glblWindowsFS, ur_userbowl_cart As Have.sUserBowl, ur_poll_folder_bowlname As enmUN, ur_filespec As String)
                Dim strPOLL_FOLDER = ur_userbowl_cart.SelKey(ur_poll_folder_bowlname).Contents
                For Each strFILE_PATH In ur_windows_fs_cart.GetFiles(strPOLL_FOLDER, ur_filespec, System.IO.SearchOption.TopDirectoryOnly)
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
                    Me.Ins(
                        New rFileMove().
                        vt(enmFM.src_file_path, strFILE_PATH).
                        vt(enmFM.dest_file_path, strDEST_PATH)
                        )
                Next strFILE_PATH
            End Sub 'Apply ur_windows_fs_cart

            Public Sub Move_Files()
                Dim lstLEN = New System.Collections.Generic.List(Of Integer)
                For Each rowFILE In Me.SelAll
                    Dim strFILE_PATH = rowFILE.v(enmFM.src_file_path)
                    Dim intLEN = strFILE_PATH.Length
                    If lstLEN.Contains(intLEN) = False Then
                        lstLEN.Add(intLEN)
                    End If
                Next rowFILE

                Call lstLEN.Sort()
                For Each intLEN In lstLEN
                    For Each rowFILE In Me.SelAll
                        Dim strFILE_PATH = rowFILE.v(enmFM.src_file_path)
                        If strFILE_PATH.Length = intLEN Then
                            Dim strDEST_PATH = rowFILE.v(enmFM.dest_file_path)
                            glbl.gWindowsFS.Move(strFILE_PATH, strFILE_PATH & ".TDLY")
                            glbl.gWindowsFS.Delete(strDEST_PATH)
                            glbl.gWindowsFS.Move(strFILE_PATH & ".TDLY", strDEST_PATH)
                        End If 'strFILE_PATH
                    Next rowFILE
                Next intLEN
            End Sub 'Move_Files
        End Class 'sFileMove

        Partial Class sUserBowl
            Public Function Apply(ur_filemove_cart As sFileMove) As enmUN.zreport_output
                Dim retKEY = enmUN.report_output
                Apply = retKEY
                Dim stpREPORT = Strapd()
                For Each rowFILE In ur_filemove_cart.SelAll
                    stpREPORT.dLine(Now.ToString("tt hh:mm:ss")).d(":").dS(rowFILE.v(enmFM.dest_file_path))
                Next rowFILE

                Me.SelKey(retKEY).v(enmUB.contents) = stpREPORT
            End Function 'Apply(ur_filemove_cart
        End Class 'sUserBowl
    End Class 'Have



    Partial Public Class Have
        Private Shared envWindowsCboard As glblWindowsCboard
        Private Shared envWindowsMsgBox As glblWindowsMsgBox
        Private Shared envWindowsFS As glblWindowsFS
        Private Shared tblFileMove As sFileMove
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.envWindowsCboard = New glblWindowsCboard
                Have.envWindowsMsgBox = New glblWindowsMsgBox
                Have.envWindowsFS = New glblWindowsFS
                Have.tblFileMove = New sFileMove
                Have.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsCboard() As glblWindowsCboard
            Call Have.Connect()
            WindowsCboard = Have.envWindowsCboard
        End Function

        Public Class glblWindowsCboard
            Public Function SetText(ur_text As String) As Integer
                SetText = Mx.glbl.gCboard.SetText(ur_text)
            End Function
        End Class 'glblWindowsCboard
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsMsgBox() As glblWindowsMsgBox
            Call Have.Connect()
            WindowsMsgBox = Have.envWindowsMsgBox
        End Function

        Public Class glblWindowsMsgBox
            Public Function GetResult(ur_message As String, ur_title As String, ur_style As MsgBoxStyle) As MsgBoxResult
                GetResult = Mx.glbl.gMsgBox.GetResult(ur_message, ur_style, ur_title)
            End Function
        End Class 'glblWindowsMsgBox
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsFS() As glblWindowsFS
            Call Have.Connect()
            WindowsFS = Have.envWindowsFS
        End Function

        Public Class glblWindowsFS
            Public Function GetFiles(ur_search_folder As String, ur_filespec As String, ur_recurse_option As System.IO.SearchOption) As String()
                GetFiles = Mx.glbl.gWindowsFS.GetFiles(ur_search_folder, ur_filespec, ur_recurse_option)
            End Function

            Public Function ReadAllText(ur_file_path As String) As String
                ReadAllText = Mx.glbl.gWindowsFS.ReadAllText(ur_file_path)
            End Function
        End Class 'glblWindowsFS
    End Class 'Have

    Public Class enmFM
        Inherits bitBASE
        Public Shared src_file_path As enmFM = TRow(Of enmFM).glbl.NewBitBase()
        Public Shared dest_file_path As enmFM = TRow(Of enmFM).glbl.NewBitBase()
    End Class

    Partial Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function FileMove() As sFileMove
            Call Have.Connect()
            FileMove = Have.tblFileMove
        End Function

        Public Class rFileMove
            Inherits TRow(Of enmFM)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmFM, ur_val As String) As rFileMove
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rFileMove

        Public Class sFileMove
            Private ttb As Objlist(Of rFileMove)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rFileMove)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub DelAll()
                Me.ttb.Clear()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rFileMove) As rFileMove
                Ins = ur_from
                Dim ttbSEL = Me
                Me.ttb.Add(ur_from)
            End Function 'Ins

            Public ReadOnly Property SelAll() As Objlist(Of rFileMove)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

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
        End Class 'sFileMove
    End Class 'Have

    Public Class enmUB
        Inherits bitBASE
        Public Shared bowl_name As enmUB = TRow(Of enmUB).glbl.NewBitBase()
        Public Shared contents As enmUB = TRow(Of enmUB).glbl.NewBitBase()
    End Class

    Public Class enmUN
        Inherits bitBASE
        Public Shared app_folder As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared app_name As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared app_path As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_audit As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared in_subfolder As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared from_messagebox As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared poll_folder As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared report_output As zreport_output = TRow(Of enmUN).glbl.Trbase(Of zreport_output).NewBitBase() : Public Class zreport_output : Inherits enmUN : End Class
        Public Shared uiform_title As enmUN = TRow(Of enmUN).glbl.NewBitBase()
    End Class

    Public Class enmUR
        Inherits bitBASE
        Public Shared Ok As enmUR = TRow(Of enmUR).glbl.NewBitBase()
        Public Shared Cancel As enmUR = TRow(Of enmUR).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        Public Shared FirstConnect As Object
        Public Shared CmdLineText As String

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Dim bolFIRST_INIT = (Have.FirstConnect Is Nothing)
            Call Have.Connect()
            UserBowl = Have.tblUserBowl
            If bolFIRST_INIT Then
                Have.FirstConnect = "Done"
                Call Have.tblUserBowl.InsFrom_Application()
                'Have.tblUserBowl.InsKey(enmUN.cmdline_audit, "1")
                Call Have.tblUserBowl.Cboard_CmdlineAudit()
            End If
        End Function 'UserBowl

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

            Public Property Contents As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enmUB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.v(enmUB.contents) = value
                End Set
            End Property 'Contents
        End Class 'rUserBowl

        Public Class sUserBowl
            Inherits Mx.TablePKEnum(Of enmUB, enmUN, rUserBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Cboard_CmdlineAudit()
                If HasText(Me.SelKey(enmUN.cmdline_audit).v(enmUB.contents)) Then
                    Dim strAUDIT = Me.ToString(True)
                    Dim enrUSER_INPUT = Have.WindowsMsgBox.GetResult(
                        ur_title:=Me.SelKey(enmUN.app_name).v(enmUB.contents),
                        ur_message:=strAUDIT,
                        ur_style:=MsgBoxStyle.OkCancel
                        )
                    If enrUSER_INPUT = MsgBoxResult.Ok Then
                        Have.WindowsCboard.SetText(strAUDIT)
                    End If
                End If
            End Sub 'Cboard_CmdlineAudit

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFrom_Application() As rUserBowl
                Dim ret = New rUserBowl
                InsFrom_Application = ret
                Me.SelKey(enmUN.app_name).Contents = Mx.FileNamed().d(Mx.Class1.SourcePath).FileGroup
                Me.SelKey(enmUN.app_path).Contents = Mx.Class1.SourcePath
                Me.SelKey(enmUN.app_folder).Contents = Mx.Class1.SourceFolder

                Dim arlCMD_RET = MxText.Cmdline_UB(Of enmUN, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, Have.CmdLineText)
                Me.SelKey(enmUN.cmdline_orig).Contents = qs & Have.CmdLineText.Replace(qs, qs & qs) & qs
                Me.SelKey(enmUN.cmdline_table).Contents = qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(qs, qs & qs) & qs
                For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                    Me.Sel(enmUB.bowl_name, rowFOUND.v(enmUB.bowl_name)).SelFirst.Contents = rowFOUND.v(enmUB.contents)
                Next
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sUserBowl
    End Class 'UB, UN
End Namespace 'Mx
