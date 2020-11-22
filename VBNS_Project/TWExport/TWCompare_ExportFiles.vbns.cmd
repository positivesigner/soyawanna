start "" "%~dp0\MxClasses\VBNetScript.exe" /path=%0
exit
MxClasses\MxBaseEc12.vb
RetVal = Mx.Want.TWCompare_ExportFiles_errhnd
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.TWCompare_ExportFiles_errhnd
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
        Public Shared Sub TWCompare_ExportFiles(ret_message As Strap)
            Dim userbowl_cart = Have.UserBowl
            Dim appname_bowlname = enmUN.app_name
            Dim appfolder_bowlname = enmUN.app_folder
            Dim windows_fs_cart = Have.WindowsFS
            Dim tempfile_cart = Have.TempFile
            Dim messagebox_cart = Have.MessageBox
            Dim windows_clipboard_cart = Have.Clipboard
            Dim infolder_bowlname = userbowl_cart.Apply(appfolder_bowlname, windows_fs_cart)
            tempfile_cart.Apply(infolder_bowlname, windows_fs_cart)
            Dim report_output_bowlname = userbowl_cart.Apply(tempfile_cart)
            Dim from_messagebox_bowlname = userbowl_cart.Apply(report_output_bowlname, appfolder_bowlname, messagebox_cart)
            Dim clipboard_recdate_bowlname = userbowl_cart.Apply(from_messagebox_bowlname, report_output_bowlname, windows_clipboard_cart)
        End Sub 'TWCompare_ExportFiles

        Public Shared Function TWCompare_ExportFiles_errhnd() As Strap
            Dim stpRET = Strapd()
            TWCompare_ExportFiles_errhnd = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Call TWCompare_ExportFiles(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'TWCompare_ExportFiles_errhnd
    End Class 'Want

    Public Class Have
        Partial Class sTempFile
            Public Sub Apply(ur_in_folder As enmUN.zin_subfolder, ur_windows_fs_cart As Have.sWindowsFS)
                Dim sdaSKIP_EXTENSION = New Sdata().d(".cmd").d(".exe")
                Dim strSEARCH_FOLDER = Have.UserBowl.SelKey(ur_in_folder).v(enmUB.contents)
                For Each rowPATH In ur_windows_fs_cart.InsList_From_Windows(strSEARCH_FOLDER, "*.*", System.IO.SearchOption.AllDirectories).SelAll
                    Dim flnPATH = New MxText.FileName(rowPATH.v(enmWF.file_path))
                    If AreEqual(flnPATH.gParentDir.Name, "MxClasses") = False AndAlso
                      sdaSKIP_EXTENSION.Contains(flnPATH.Ext.ToLower) = False Then
                        Me.Ins(
                            New Have.rTempFile().
                            vt(enmTF.in_filepath, rowPATH.v(enmWF.file_path))
                            )
                    End If
                Next rowPATH

                If Me.SelAll.Count = 0 Then
                    Throw New System.Exception("No files found in directory: " & strSEARCH_FOLDER)
                End If
            End Sub 'Apply(ur_in_folder
        End Class 'sTempFile

        Partial Class sUserBowl
            Public Function Apply(ur_report_output As enmUN.zreport_output, ur_userbowl_appname As enmUN.zapp_folder, ur_messagebox_cart As Have.sMessageBox) As enmUN.zfrom_messagebox
                Dim retKEY = enmUN.from_messagebox
                Apply = retKEY
                Dim ins_msgbox = ur_messagebox_cart.Ins(
                    New Have.rMessageBox().
                        vt(enmMB.text, Me.SelKey(ur_report_output).v(enmUB.contents)).
                        vt(enmMB.title, Me.SelKey(ur_userbowl_appname).v(enmUB.contents)),
                    MsgBoxStyle.OkCancel
                    )

                If ins_msgbox.vUserResponse = MsgBoxResult.Ok Then
                    Me.InsKey(retKEY, enmUR.Ok)
                End If
            End Function 'Apply(ur_report_output

            Public Function Apply(ur_tempfile_cart As Have.sTempFile) As enmUN.zreport_output
                Dim retKEY = enmUN.report_output
                Apply = retKEY
                Dim ins_bowl = Me.InsKey(retKEY, ur_tempfile_cart.ToString(True))
            End Function 'Apply(ur_tempfile_cart

            Public Function Apply(ur_infolder As enmUN.zapp_folder, ur_windows_fs_cart As Have.sWindowsFS) As enmUN.zin_subfolder
                Dim retKEY = enmUN.in_subfolder
                Apply = retKEY
                Dim strPATH = Me.SelKey(ur_infolder).v(enmUB.contents)
                If ur_windows_fs_cart.InsDir_From_Windows(strPATH).SelAll.Count > 0 Then
                    Me.InsKey(retKEY, strPATH)

                Else
                    Throw New System.Exception("In-Folder not found: " & strPATH)
                End If
            End Function 'Apply(ur_infolder

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
        Private Shared tblTempFile As sTempFile
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.tblTempFile = New sTempFile
                Have.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have

    Public Class enmTF
        Inherits bitBASE
        Public Shared in_filepath As enmTF = TRow(Of enmTF).glbl.NewBitBase()
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
                Me.PK.Add(enmTF.in_filepath)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rTempFile) As rTempFile
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
            Public Function Sel(ur_col As enmTF, ur_value As String) As sTempFile
                Dim ret = New sTempFile
                Sel = ret
                For Each row In Me.ttb
                    If AreEqual(row.v(ur_col), ur_value) Then
                        ret.Ins(row)
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
            Public Function SelFirst() As rTempFile
                If Me.ttb.Count = 0 Then
                    SelFirst = New rTempFile()
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
        End Class 'sTempFile
    End Class 'TF

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
        Public Shared clipboard_recdate As zclipboard_recdate = TRow(Of enmUN).glbl.Trbase(Of zclipboard_recdate).NewBitBase() : Public Class zclipboard_recdate : Inherits enmUN : End Class
        Public Shared cmdline_audit As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared in_subfolder As zin_subfolder = TRow(Of enmUN).glbl.Trbase(Of zin_subfolder).NewBitBase() : Public Class zin_subfolder : Inherits enmUN : End Class
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
            Public Function SelFirst() As rUserBowl
                If Me.ttb.Count = 0 Then
                    SelFirst = New rUserBowl()
                Else
                    SelFirst = Me.ttb.tr_b1(1)
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelKey(ur_key As enmUN) As rUserBowl
                Dim ret As rUserBowl = Nothing
                Mx.TRow(Of enmUN).glbl.RefKeys()
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
