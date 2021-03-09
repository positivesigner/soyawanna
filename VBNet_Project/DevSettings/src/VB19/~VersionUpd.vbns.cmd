@echo off
@rem 2021m01d25
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
MxClasses\MxBaseEc13.vb
RetVal = Mx.Want.FileUpdate_Report_errhnd
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.FileUpdate_Report_errhnd
'            If RetVal <> "QUIT" Then MsgBox(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = "UrFolder"
'        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
'    End Class
'End Namespace 'Mx

Namespace Mx
    Public Class Want
        Public Shared Sub FileUpdate_Report(ur_ret As Strap)
            Dim userbowl_cart = Have.UserBowl
            Dim appname_bowlname = enmUN.app_name
            Dim appfolder_bowlname = enmUN.app_folder
            Dim windows_msgbox_env = Have.WindowsMsgBox
            Dim windows_fs_env = Have.WindowsFS
            Dim tempfile_cart = Have.TempFile
            tempfile_cart.Apply(userbowl_cart.SelKey(appfolder_bowlname), windows_fs_env)
            Dim report_output_bowlname = userbowl_cart.Apply(tempfile_cart)
            Dim from_messagebox_bowlname = userbowl_cart.Apply(
                ur_userbowl_text:=userbowl_cart.SelKey(report_output_bowlname),
                ur_userbowl_appname:=userbowl_cart.SelKey(appname_bowlname),
                ur_messagebox_env:=windows_msgbox_env
                )
        End Sub 'FileUpdate_Report

        Public Shared Function FileUpdate_Report_errhnd() As Strap
            Dim stpRET = Strapd()
            FileUpdate_Report_errhnd = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Call FileUpdate_Report(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'FileUpdate_Report_errhnd
    End Class 'Want

    Partial Public Class Have
        Partial Class sTempFile
            Public Sub Apply(ur_app_folder As Have.rUserBowl, ur_windows_fs_env As Have.glblWindowsFS)
                Dim flnROOT_FOLDER = FileNamed().d(ur_app_folder.v(enmUB.contents))
                Dim flnMY_PROJECT = flnROOT_FOLDER.gCopy.d("My Project")
                For Each strFILE_PATH In ur_windows_fs_env.GetFiles(flnROOT_FOLDER, "*.vbproj", System.IO.SearchOption.TopDirectoryOnly)
                    Me.InsKey(enmTN.VBProj, strFILE_PATH)
                    Exit For
                Next

                If HasText(Me.SelKey(enmTN.VBProj).v(enmTF.filename)) = False Then
                    Throw New System.Exception(Strapd().d("Could not find").dS(enmTN.VBProj.name).dS("file in folder"))
                End If

                For Each strFILE_PATH In ur_windows_fs_env.GetFiles(flnMY_PROJECT, "AssemblyInfo.vb", System.IO.SearchOption.TopDirectoryOnly)
                    Me.InsKey(enmTN.AssemblyInfo, strFILE_PATH)
                    Exit For
                Next

                If HasText(Me.SelKey(enmTN.AssemblyInfo).v(enmTF.filename)) = False Then
                    Throw New System.Exception(Strapd().d("Could not find").dS(enmTN.AssemblyInfo.name).dS("file in folder"))
                End If
            End Sub 'Apply(ur_app_folder
        End Class 'sTempFile

        Partial Public Class sUserBowl
            Public Function Apply(ur_userbowl_text As Have.rUserBowl, ur_userbowl_appname As Have.rUserBowl, ur_messagebox_env As Have.glblWindowsMsgBox) As bitBASE
                Dim retUN = enmUN.from_messagebox
                Apply = retUN
                Dim enrUSER_INPUT = ur_messagebox_env.GetResult(
                    ur_message:=ur_userbowl_text.v(enmUB.contents),
                    ur_title:=ur_userbowl_appname.v(enmUB.contents),
                    ur_style:=MsgBoxStyle.OkOnly
                    )

                If enrUSER_INPUT = MsgBoxResult.Ok Then
                    Me.InsKey(retUN, enmUR.Ok)
                End If
            End Function 'Apply ur_messagebox_cart

            Public Function Apply(ur_tempfile_cart As Have.sTempFile) As enmUN
                Dim retUN = enmUN.report_output
                Apply = retUN
                Dim strDATE = Today.ToString("yyyy.MM.dd")
                Dim intCURRENT = 1
				Dim intUPDATED_ROWS = 0

                For Each trwFILE In ur_tempfile_cart.Sel(enmTF.file_search, enmTN.VBProj.name).SelAll
                    Dim flnSOURCE = FileNamed().d(trwFILE.v(enmTF.filename))
                    Dim flnTEMP = flnSOURCE.gCopy.dAppendEXT("tmp")
                    Using stmSECOND = New System.IO.StreamWriter(flnTEMP, False, gUTF8_FileEncoding)
                        Using stmORIG = New System.IO.StreamReader(flnSOURCE, gUTF8_FileEncoding)
                            While Not stmORIG.EndOfStream
                                Dim strLINE = stmORIG.ReadLine
                                If StartingWithText(strLINE.TrimStart, "<ApplicationRevision>") Then
                                    Dim strPARSE = Mid(strLINE.TrimStart, "<ApplicationRevision>".Length + 1)
                                    strPARSE = Mid(strPARSE, 1, InStr(strPARSE, "<") - 1)
                                    If System.Int32.TryParse(strPARSE, intCURRENT) Then
                                        intCURRENT += 1
                                    End If

                                ElseIf StartingWithText(strLINE.TrimStart, "<ApplicationVersion>") Then
                                    Dim strPARSE = Mid(strLINE.TrimStart, "<ApplicationVersion>".Length + 1)
                                    strPARSE = Mid(strPARSE, 1, 10)
                                    If AreEqual(strPARSE, strDATE) = False Then
                                        intCURRENT = 1
                                    End If 'strPARSE

									intUPDATED_ROWS += 1
                                    stmSECOND.Write(Mid(strLINE, 1, InStr(strLINE, "<") - 1))
                                    stmSECOND.Write("<ApplicationRevision>")
                                    stmSECOND.Write(intCURRENT)
                                    stmSECOND.WriteLine("</ApplicationRevision>")

                                    stmSECOND.Write(Mid(strLINE, 1, InStr(strLINE, "<") - 1))
                                    stmSECOND.Write("<ApplicationVersion>")
                                    stmSECOND.Write(Strapd().d(strDATE).d(intCURRENT.ToString("00")).d(".%2a"))
                                    stmSECOND.WriteLine("</ApplicationVersion>")

                                Else 'strLINE
                                    stmSECOND.WriteLine(strLINE)
                                End If 'strLINE
                            End While 'stmORIG
                        End Using 'stmORIG
                    End Using 'stmSECOND

                    Call glbl.gWindowsFS.Delete(flnSOURCE)
                    Call glbl.gWindowsFS.Move(flnTEMP, flnSOURCE)
                Next trwFILE
				
				If intUPDATED_ROWS >= 1 Then
					For Each trwFILE In ur_tempfile_cart.Sel(enmTF.file_search, enmTN.AssemblyInfo.name).SelAll
						Dim flnSOURCE = FileNamed().d(trwFILE.v(enmTF.filename))
						Dim flnTEMP = flnSOURCE.gCopy.dAppendEXT("tmp")
						Using stmSECOND = New System.IO.StreamWriter(flnTEMP, False, gUTF8_FileEncoding)
							Using stmORIG = New System.IO.StreamReader(flnSOURCE, gUTF8_FileEncoding)
								While Not stmORIG.EndOfStream
									Dim strLINE = stmORIG.ReadLine
									If StartingWithText(strLINE.TrimStart, "<Assembly: AssemblyVersion(") Then
										intUPDATED_ROWS += 1
										stmSECOND.WriteLine(Strapd().d("<").d("Assembly: AssemblyVersion").d("(").d(qs).d(Today.ToString("yyyy.MM.dd")).d(intCURRENT.ToString("00")).d(".00").d(qs).d(")").d(">"))

									ElseIf StartingWithText(strLINE.TrimStart, "<Assembly: AssemblyFileVersion") Then
										intUPDATED_ROWS += 1
										stmSECOND.WriteLine(Strapd().d("<").d("Assembly: AssemblyFileVersion").d("(").d(qs).d(strDATE).d(intCURRENT.ToString("00")).d(".00").d(qs).d(")").d(">"))

									Else 'strLINE
										stmSECOND.WriteLine(strLINE)
									End If 'strLINE
								End While 'stmORIG
							End Using 'stmORIG
						End Using 'stmSECOND

						Call glbl.gWindowsFS.Delete(flnSOURCE)
						Call glbl.gWindowsFS.Move(flnTEMP, flnSOURCE)
					Next trwFILE
				End If

                Me.InsKey(retUN, Strapd().d("Configuration File Rows updated:").dS(intUPDATED_ROWS.ToString))
            End Function 'Apply ur_tempfield_cart
        End Class 'sUserBowl
    End Class 'Have


    Partial Public Class Have
        Private Shared envWindowsCboard As glblWindowsCboard
        Private Shared envWindowsMsgBox As glblWindowsMsgBox
        Private Shared envWindowsFS As glblWindowsFS
        Private Shared tblTempFile As sTempFile
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.envWindowsCboard = New glblWindowsCboard
                Have.envWindowsMsgBox = New glblWindowsMsgBox
                Have.envWindowsFS = New glblWindowsFS
                Have.tblTempFile = New sTempFile
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
                SetText = glbl.gCboard.SetText(ur_text)
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
                GetResult = glbl.gMsgBox.GetResult(ur_message, ur_style, ur_title)
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
                GetFiles = glbl.gWindowsFS.GetFiles(ur_search_folder, ur_filespec, ur_recurse_option)
            End Function
        End Class 'glblWindowsFS
    End Class 'Have

    Public Class enmTF
        Inherits bitBASE
        Public Shared file_search As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared filename As enmTF = TRow(Of enmTF).glbl.NewBitBase()
    End Class

    Public Class enmTN
        Inherits bitBASE
        Public Shared AssemblyInfo As enmTN = TRow(Of enmTN).glbl.NewBitBase()
        Public Shared VBProj As enmTN = TRow(Of enmTN).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempFile() As sTempFile
            Call Have.Connect()
            TempFile = Have.tblTempFile
        End Function

        Public Class rTempFile
            Inherits TRow(Of enmTF)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmTF, ur_val As String) As rTempFile
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rTempFile

        Public Class sTempFile
            Private ttb As Objlist(Of rTempFile)
            Private PK As Objlist(Of enmTF)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rTempFile)
                Me.PK = New Objlist(Of enmTF)
                Me.PK.Add(enmTF.file_search)
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
            Public Sub Ins(ur_from As rTempFile)
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
            End Sub 'Ins

            <System.Diagnostics.DebuggerHidden()>
            Public Sub InsKey(ur_key As enmTN, ur_val As String)
                Dim ret = Me.SelKey(ur_key)
                If HasText(ret.v(enmTF.filename)) Then
                    Throw New System.Exception("Cannot insert duplicate key for key: " & ur_key.name)
                Else
                    ret.vt(enmTF.filename, ur_val)
                End If
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmTF, ur_value As String) As sTempFile
                Dim retUB = New sTempFile
                Sel = retUB
                For Each rowUB In Me.ttb
                    If AreEqual(rowUB.v(ur_col), ur_value) Then
                        retUB.Ins(rowUB)
                    End If
                Next
            End Function 'Sel

            Public ReadOnly Property SelAll() As Objlist(Of rTempFile)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelKey(ur_key As enmTN) As rTempFile
                Dim ret As rTempFile = Nothing
                Dim strKEY = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmTF.file_search), strKEY) Then
                        ret = row
                        Exit For
                    End If
                Next row

                If ret Is Nothing Then
                    ret = New rTempFile
                    Me.ttb.Add(
                        ret.
                        vt(enmTF.file_search, ur_key.name)
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
        End Class 'sTempFile
    End Class 'TF

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
        Public Shared cmd_export_cmdline_audit As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared from_messagebox As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared report_output As enmUN = TRow(Of enmUN).glbl.NewBitBase()
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

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rUserBowl)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Cboard_CmdlineAudit()
                If HasText(Me.SelKey(enmUN.cmd_export_cmdline_audit).v(enmUB.contents)) Then
                    Dim strAUDIT = Me.ToString(True)
                    Dim ins_msg = Have.WindowsMsgBox.GetResult(
                        ur_message:=Me.SelKey(enmUN.app_name).v(enmUB.contents),
                        ur_title:=strAUDIT,
                        ur_style:=MsgBoxStyle.OkCancel
                        )
                    If ins_msg = MsgBoxResult.Ok Then
                        Have.WindowsCboard.SetText(
                            strAUDIT
                            )
                    End If
                End If
            End Sub 'Cboard_CmdlineAudit

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rUserBowl) As rUserBowl
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsKey(ur_key As enmUN, ur_val As String) As rUserBowl
                Dim ret = Me.SelKey(ur_key)
                InsKey = ret
                If HasText(ret.v(enmUB.contents)) Then
                    Throw New System.Exception("Cannot insert duplicate key for key: " & ur_key.name)
                Else
                    ret.vt(enmUB.contents, ur_val)
                End If
            End Function 'InsKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsKey(ur_key As enmUN, ur_val As enmUR) As rUserBowl
                InsKey = Me.InsKey(ur_key, ur_val.name)
            End Function 'InsKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFrom_Application() As rUserBowl
                Dim ret = New rUserBowl
                InsFrom_Application = ret
                Me.InsKey(enmUN.app_name, FileNamed().d(Mx.Class1.SourcePath).FileGroup)
                Me.InsKey(enmUN.app_path, Mx.Class1.SourcePath)
                Me.InsKey(enmUN.app_folder, Mx.Class1.SourceFolder)

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
                Next row

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
