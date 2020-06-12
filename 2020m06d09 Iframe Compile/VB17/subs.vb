Namespace Mx
    Module subs
        Sub Main()
            Dim RetVal = Mx.Want.TWCompare_ExportFiles_errhnd
            If Mx.AreEqual(RetVal, "QUIT") = False Then MsgBox(RetVal)
        End Sub
    End Module 'subs

    Public Class Class1
        Public Shared SourceFolder As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "")
        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
    End Class
End Namespace 'Mx

Namespace Mx
    Public Class Want
        Public Shared Sub TWCompare_ExportFiles(ret_message As Strap)
            Dim userbowl_cart = Have.UserBowl
            Dim appname_bowlname = enmUN.app_name
            Dim appfolder_bowlname = enmUN.app_folder
            Dim windows_fs_cart = Have.WindowsFS
            Dim tempfile_cart = Have.TempFile
            Dim subdir_cart = Have.SubDir
            Dim tf_txt_html = enmTN.txt_html
            Dim tf_folder_remove = enmTN.remove_indexpath
            Dim messagebox_cart = Have.MessageBox
            Dim skipfolder_bowlname = userbowl_cart.Apply(appfolder_bowlname, windows_fs_cart)
            Dim rootfolder_bowlname = userbowl_cart.Apply(skipfolder_bowlname, windows_fs_cart)
            tempfile_cart.Apply(rootfolder_bowlname, skipfolder_bowlname, windows_fs_cart)
            subdir_cart.Apply(tf_txt_html, rootfolder_bowlname, tempfile_cart)
            tempfile_cart.Apply(rootfolder_bowlname, skipfolder_bowlname, subdir_cart, windows_fs_cart)
            windows_fs_cart.Apply(rootfolder_bowlname, subdir_cart)
            windows_fs_cart.Apply(tf_folder_remove, rootfolder_bowlname, tempfile_cart)
            Dim report_output_bowlname = userbowl_cart.Apply(tempfile_cart, subdir_cart)
            Dim from_messagebox_bowlname = userbowl_cart.Apply(report_output_bowlname, appfolder_bowlname, messagebox_cart)
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
        Const lit_subdir_html = "subdir.html"
        Const lit_txt_html = "txt.html"

        Partial Class sSubDir
            Public Sub Apply(ur_tf_txt_html As enmTN.ztxt_html, ur_in_folder As enmUN.zroot_subfolder, ur_tempfile As Have.sTempFile)
                Dim flnROOT_FOLDER = FileNamed().d(Have.UserBowl.SelKey(ur_in_folder).v(enmUB.contents))
                For Each rowTF In ur_tempfile.SelSection(ur_tf_txt_html).SelAll
                    'sample1 C:\...\subdir\...\TiddlyWiki\TW_Notes\iframe.txt.html
                    Dim strLINK_PATH = rowTF.v(enmTF.in_filepath)
                    'part1 TiddlyWiki\TW_Notes\iframe.txt.html
                    Dim flnLINK_FILE = FileNamed().d(Mid(strLINK_PATH, flnROOT_FOLDER.ToString.Length + 2))
                    'part1_parent_dir TiddlyWiki\TW_Notes
                    'part1_parent_dir_append_ext TiddlyWiki\TW_Notes.subdir.html
                    Dim flnREF_TABLE = flnROOT_FOLDER.gCopy.d(flnLINK_FILE.gParentDir.dAppendEXT(lit_subdir_html))
                    Me.Ins(
                        New rSubDir().
                        vt(enmSD.section, flnREF_TABLE).
                        vt(enmSD.create_indexpath, flnLINK_FILE)
                        )
                Next rowTF
                glbl.gWindowsFS.WriteAllText(flnROOT_FOLDER.gCopy.d("subdir.txt"), Me.ToString(True))
            End Sub 'Apply(ur_tf_folder_name
        End Class 'sSubDir

        Partial Class sTempFile
            Public Sub Apply(ur_root_folder As enmUN.zroot_subfolder, ur_skip_folder As enmUN.zskip_subfolder, ur_windows_fs_cart As Have.sWindowsFS)
                Dim strROOT_FOLDER = Have.UserBowl.SelKey(ur_root_folder).v(enmUB.contents)
                Dim strSKIP_FOLDER = Have.UserBowl.SelKey(ur_skip_folder).v(enmUB.contents)
                'sample1 C:\...\subdir\...\TiddlyWiki\TW_Notes\iframe.txt.html
                'sample2 C:\...\subdir\...\TiddlyWiki\TW_Notes\Image.txt.html
                For Each rowPATH In ur_windows_fs_cart.InsList_From_Windows(strROOT_FOLDER, FileNamed().d("*").dAppendEXT(lit_txt_html), System.IO.SearchOption.AllDirectories).SelAll
                    Dim flnFILE = FileNamed().d(rowPATH.v(enmWF.file_path))
                    If StartingWithText(flnFILE, strSKIP_FOLDER) = False Then
                        Me.Ins(
                            New Have.rTempFile().
                            vt(enmTF.section, enmTN.txt_html.name).
                            vt(enmTF.in_filepath, flnFILE)
                            )
                    End If
                Next rowPATH
                glbl.gWindowsFS.WriteAllText(FileNamed().d(strROOT_FOLDER).d("tf_txt_html.txt"), Me.ToString(True))
            End Sub 'Apply(ur_in_folder

            Public Sub Apply(ur_root_folder As enmUN.zroot_subfolder, ur_skip_folder As enmUN.zskip_subfolder, ur_subdir_cart As Have.sSubDir, ur_windows_fs_cart As Have.sWindowsFS)
                Dim strROOT_FOLDER = Have.UserBowl.SelKey(ur_root_folder).v(enmUB.contents)
                Dim strSKIP_FOLDER = Have.UserBowl.SelKey(ur_skip_folder).v(enmUB.contents)
                'sample1 C:\...\subdir\...\Windows\t1.subdir.html
                For Each rowPATH In ur_windows_fs_cart.InsList_From_Windows(strROOT_FOLDER, FileNamed().d("*").dAppendEXT(lit_subdir_html), System.IO.SearchOption.AllDirectories).SelAll
                    Dim flnFILE = FileNamed().d(rowPATH.v(enmWF.file_path))
                    If StartingWithText(flnFILE, strSKIP_FOLDER) = False AndAlso
                      ur_subdir_cart.SelEachSection.Contains(flnFILE) = False Then
                        Me.Ins(
                                New Have.rTempFile().
                                vt(enmTF.section, enmTN.remove_indexpath.name).
                                vt(enmTF.in_filepath, flnFILE)
                                )
                    End If
                Next rowPATH
                glbl.gWindowsFS.WriteAllText(FileNamed().d(strROOT_FOLDER).d("tf_remove_index.txt"), Me.ToString(True))
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
                    MsgBoxStyle.OkOnly
                    )

                If ins_msgbox.vUserResponse = MsgBoxResult.Ok Then
                    Me.InsKey(retKEY, enmUR.Ok)
                End If
            End Function 'Apply(ur_report_output

            Public Function Apply(ur_tempfile_cart As Have.sTempFile, ur_subdir_cart As Have.sSubDir) As enmUN.zreport_output
                Dim retKEY = enmUN.report_output
                Apply = retKEY
                Dim stpMSG = Strapd().d("Inedxes created:").dS(ur_subdir_cart.SelEachSection.Count)
                stpMSG.dLine("Indexes removed:").dS(ur_tempfile_cart.SelSection(enmTN.remove_indexpath).SelAll.Count)
                Dim ins_bowl = Me.InsKey(retKEY, stpMSG)
            End Function 'Apply(ur_tempfile_cart

            Public Function Apply(ur_app_folder As enmUN.zapp_folder, ur_windows_fs_cart As Have.sWindowsFS) As enmUN.zskip_subfolder
                Dim retKEY = enmUN.skip_subfolder
                Apply = retKEY
                Dim flnSKIP_PATH = FileNamed().d(Me.SelKey(ur_app_folder).v(enmUB.contents))
                If ur_windows_fs_cart.InsDir_From_Windows(flnSKIP_PATH).SelAll.Count > 0 Then
                    Me.InsKey(retKEY, ur_windows_fs_cart.SelFirst.v(enmWF.file_path))

                Else
                    Throw New System.Exception(Strapd().d("Skip Folder not found:").dS(flnSKIP_PATH))
                End If
            End Function 'Apply(ur_infolder

            Public Function Apply(ur_skip_folder As enmUN.zskip_subfolder, ur_windows_fs_cart As Have.sWindowsFS) As enmUN.zroot_subfolder
                Dim retKEY = enmUN.root_subfolder
                Apply = retKEY
                Dim flnROOT_PATH = FileNamed().d(Me.SelKey(ur_skip_folder).v(enmUB.contents)).gParentDir
                If ur_windows_fs_cart.InsDir_From_Windows(flnROOT_PATH).SelAll.Count > 0 Then
                    Me.InsKey(retKEY, ur_windows_fs_cart.SelFirst.v(enmWF.file_path))

                Else
                    Throw New System.Exception(Strapd().d("Root Folder not found:").dS(flnROOT_PATH))
                End If
            End Function 'Apply(ur_infolder
        End Class 'sUserBowl

        Partial Public Class sWindowsFS
            Public Sub Apply(ur_in_folder As enmUN.zroot_subfolder, ur_subdir As Have.sSubDir)
                For Each kvpSD In ur_subdir.SelEachSection.kvp
                    'Dim strLINK_PATH = kvpSD.v(enmTF.in_filepath)
                    'strLINK_PATH = Mid(strLINK_PATH, flnROOT_FOLDER.ToString.Length + 2)
                    'Dim flnLINK_FILE = FileNamed().d(strLINK_PATH)
                    'Dim flnREF_TABLE = flnROOT_FOLDER.gCopy.d(flnLINK_FILE.gParentDir.dAppendEXT("subdir.html"))
                    'Dim strSECTION = flnREF_TABLE.ToString
                Next kvpSD
                'glbl.gWindowsFS.WriteAllText(flnROOT_FOLDER, ur_tempfile.SelSection(ur_tf_folder_name).ToString(True))
            End Sub
            Public Sub Apply(ur_tf_folder_remove As enmTN.zremove_indexpath, ur_in_folder As enmUN.zroot_subfolder, ur_tempfile As Have.sTempFile)
                Dim flnOUTPUT = FileNamed().d(Have.UserBowl.SelKey(ur_in_folder).v(enmUB.contents)).d("tempfile.remove.txt")
                glbl.gWindowsFS.WriteAllText(flnOUTPUT, ur_tempfile.SelSection(ur_tf_folder_remove).ToString(True))
            End Sub
        End Class 'sWindowsFS
    End Class 'Have



    Partial Public Class Have
        Private Shared tblSubDir As sSubDir
        Private Shared tblTempFile As sTempFile
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.tblSubDir = New sSubDir
                Have.tblTempFile = New sTempFile
                Have.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have

    Public Class enmSD
        Inherits bitBASE
        Public Shared section As enmSD = TRow(Of enmSD).glbl.NewBitBase()
        Public Shared create_indexpath As enmSD = TRow(Of enmSD).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function SubDir() As sSubDir
            Call Have.Connect()
            SubDir = Have.tblSubDir
        End Function

        Public Class rSubDir
            Inherits TRow(Of enmSD)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmSD, ur_val As String) As rSubDir
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rSubDir

        Public Class sSubDir
            Private ttb As Objlist(Of rSubDir)
            Private sdttb As SdPair(Of sSubDir)
            Private PK As Objlist(Of enmSD)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rSubDir)
                Me.sdttb = New SdPair(Of sSubDir)
                Me.PK = New Objlist(Of enmSD)
                Me.PK.Add(enmSD.create_indexpath)
            End Sub 'New

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rSubDir) As rSubDir
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
                    Dim strSECTION = ur_from.v(enmSD.section)
                    If Me.sdttb.Contains(strSECTION) = False Then
                        Dim trwPAIR = New sSubDir
                        trwPAIR.ttb.Add(ur_from)
                        Me.sdttb.d(strSECTION, trwPAIR)
                    Else
                        Me.sdttb.l(strSECTION).ttb.Add(ur_from)
                    End If 'Me.sdttb
                End If 'ttbSEL
            End Function 'Ins

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmSD, ur_value As String) As sSubDir
                Dim ret = New sSubDir
                Sel = ret
                For Each row In Me.ttb
                    If AreEqual(row.v(ur_col), ur_value) Then
                        ret.Ins(row)
                    End If
                Next
            End Function 'Sel

            Public ReadOnly Property SelAll() As Objlist(Of rSubDir)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rSubDir
                If Me.ttb.Count = 0 Then
                    SelFirst = New rSubDir()
                Else
                    SelFirst = Me.ttb.tr_b1(1)
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelEachSection() As SdPair(Of sSubDir)
                SelEachSection = Me.sdttb
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
        End Class 'sSubDir
    End Class 'SD

    Public Class enmTF
        Inherits bitBASE
        Public Shared section As enmTF = TRow(Of enmTF).glbl.NewBitBase()
        Public Shared in_filepath As enmTF = TRow(Of enmTF).glbl.NewBitBase()
    End Class

    Public Class enmTN
        Inherits bitBASE
        Public Shared remove_indexpath As zremove_indexpath = TRow(Of enmTN).glbl.Trbase(Of zremove_indexpath).NewBitBase() : Public Class zremove_indexpath : Inherits enmTN : End Class
        Public Shared txt_html As ztxt_html = TRow(Of enmTN).glbl.Trbase(Of ztxt_html).NewBitBase() : Public Class ztxt_html : Inherits enmTN : End Class
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
            Private sdttb As SdPair(Of sTempFile)
            Private PK As Objlist(Of enmTF)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Call Me.New(True)
            End Sub 'New
            Public Sub New(ur_grouping_flag As Boolean)
                Me.ttb = New Objlist(Of rTempFile)
                Me.sdttb = New SdPair(Of sTempFile)
                If ur_grouping_flag Then
                    For Each strNAME In TRow(Of enmTN).glbl.RefNames
                        Me.sdttb.d(strNAME, New sTempFile(False))
                    Next
                End If

                Me.PK = New Objlist(Of enmTF)
                Me.PK.Add(enmTF.in_filepath)
            End Sub 'New

            '<System.Diagnostics.DebuggerHidden()>
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
                    Dim strSECTION = ur_from.v(enmTF.section)
                    Me.sdttb.l(strSECTION).ttb.Add(ur_from)
                End If 'ttbSEL
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
            Public Function SelSection(ur_col As enmTN) As sTempFile
                Dim retTTB = New sTempFile
                Dim strSECTION = ur_col.name
                If Me.sdttb.Contains(strSECTION) Then
                    retTTB = Me.sdttb.l(strSECTION)
                End If 'Me.sdttb

                SelSection = retTTB
            End Function 'SelSection

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
        Public Shared cmdline_audit As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As enmUN = TRow(Of enmUN).glbl.NewBitBase()
        Public Shared skip_subfolder As zskip_subfolder = TRow(Of enmUN).glbl.Trbase(Of zskip_subfolder).NewBitBase() : Public Class zskip_subfolder : Inherits enmUN : End Class
        Public Shared root_subfolder As zroot_subfolder = TRow(Of enmUN).glbl.Trbase(Of zroot_subfolder).NewBitBase() : Public Class zroot_subfolder : Inherits enmUN : End Class
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
