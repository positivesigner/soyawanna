start "" MxClasses\VBNetScript.exe /path=%0
exit
MxClasses\MxBaseEc10.vb
RetVal = Mx.Want.Clipboard_FileList
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.Clipboard_FileList
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
        Public Shared Function Clipboard_FileList() As Strap
            Dim stpRET = Strapd()
            Clipboard_FileList = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Have.InFolder_Search_FromGlobal(db.UserBowl, Mx.Class1.SourceFolder)
                Have.File_Search_UnderPath(db.UserBowl.SelKey(enmUN.in_subfolder), db.TempFile)
                Use.Compile_FileReport(db.UserBowl, db.TempFile)
                Use.Ask_to_UpdateClipboard(
                    ur_cboard_text:=db.UserBowl.SelKey(enmUN.to_clipboard),
                    ur_app_name:=db.UserBowl.SelKey(enmUN.app_name)
                    )

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'Clipboard_SplitAt_AS_Skip_Comma
    End Class 'Want

    Public Class Have
        Public Shared Sub File_Search_UnderPath(ur_in_folder As db.rUserBowl, ur_tempfile_table As db.sTempFile)
            Dim sdaSKIP_EXTENSION = New Sdata().d(".cmd").d(".exe")
            Dim strSEARCH_FOLDER = ur_in_folder.v(enmUB.contents)
            For Each strPATH In System.IO.Directory.GetFiles(strSEARCH_FOLDER, "*.*", System.IO.SearchOption.AllDirectories)
                Dim flnPATH = New MxText.FileName(strPATH)
                If AreEqual(flnPATH.gParentDir.Name, "MxClasses") = False AndAlso
                  sdaSKIP_EXTENSION.Contains(flnPATH.Ext.ToLower) = False Then
                    ur_tempfile_table.Ins(
                        New db.rTempFile().
                        vt(enmTF.in_filepath, strPATH)
                        )
                End If
            Next strPATH

            If ur_tempfile_table.SelAll.Count = 0 Then
                Throw New System.Exception("No files found in directory: " & strSEARCH_FOLDER)
            End If
        End Sub 'File_Search_UnderPath

        Public Shared Sub InFolder_Search_FromGlobal(ur_bowl_table As db.sUserBowl, ur_infolder As String)
            If System.IO.Directory.Exists(ur_infolder) Then
                ur_bowl_table.InsKey(enmUN.in_subfolder, ur_infolder)

            Else
                Throw New System.Exception("In-Folder not found: " & ur_infolder)
            End If
        End Sub 'InFolder_Search_FromGlobal
    End Class 'Have

    Public Class Use
        Public Shared Sub Ask_to_UpdateClipboard(ur_cboard_text As db.rUserBowl, ur_app_name As db.rUserBowl)
            Dim ins_msgbox = db.MessageBox.Ins(
                New db.rMessageBox().
                    vt(enmMB.text, ur_cboard_text.v(enmUB.contents)).
                    vt(enmMB.title, ur_app_name.v(enmUB.contents)),
                MsgBoxStyle.OkCancel
                )

            If ins_msgbox.vUserResponse = MsgBoxResult.Ok Then
                db.Clipboard.Ins(
                    New db.rClipboard().
                    vt(enmCB.text, ur_cboard_text.v(enmUB.contents))
                    )
            End If
        End Sub 'Ask_to_UpdateClipboard

        Public Shared Sub Compile_FileReport(ur_bowl_table As db.sUserBowl, ur_tempfile_table As db.sTempFile)
            Dim ins_bowl = ur_bowl_table.InsKey(enmUN.to_clipboard, ur_tempfile_table.ToString(True))
        End Sub 'Compile_FileReport
    End Class 'Use



    Partial Public Class db
        Private Shared tblTempFile As sTempFile
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If db.tblUserBowl Is Nothing Then
                db.tblTempFile = New sTempFile
                db.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'db

    Public Class bitTF
        Inherits bitBASE
    End Class

    Public Class enmTF
        Public Shared in_filepath As bitTF = TRow(Of bitTF, enmTF).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempFile() As sTempFile
            Call db.Connect()
            TempFile = db.tblTempFile
        End Function

        Public Class rTempFile
            Inherits TRow(Of bitTF, enmTF)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitTF, ur_val As String) As rTempFile
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rTempFile

        Public Class sTempFile
            Private ttb As Objlist(Of rTempFile)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rTempFile)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rTempFile) As rTempFile
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As bitTF, ur_value As String) As sTempFile
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

    Public Class bitUB
        Inherits bitBASE
    End Class

    Public Class enmUB
        Public Shared bowl_name As bitUB = TRow(Of bitUB, enmUB).glbl.NewBitBase()
        Public Shared contents As bitUB = TRow(Of bitUB, enmUB).glbl.NewBitBase()
    End Class

    Public Class bitUN
        Inherits bitBASE
    End Class

    Public Class enmUN
        Public Shared app_name As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared cmdline_audit As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared in_subfolder As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared to_clipboard As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Dim bolFIRST_INIT = (db.tblUserBowl Is Nothing)
            Call db.Connect()
            UserBowl = db.tblUserBowl
            If bolFIRST_INIT Then
                Call db.tblUserBowl.InsFrom_Application()
                'db.tblUserBowl.InsKey(enmUN.cmdline_audit, "1")
                Call db.tblUserBowl.Cboard_CmdlineAudit()
            End If
        End Function 'UserBowl

        Public Class rUserBowl
            Inherits TRow(Of bitUB, enmUB)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As bitUB, ur_val As String) As rUserBowl
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
                If HasText(Me.SelKey(enmUN.cmdline_audit).v(enmUB.contents)) Then
                    Dim strAUDIT = Me.ToString(True)
                    Dim ins_msg = db.MessageBox.Ins(
                        New db.rMessageBox().
                            vt(enmMB.title, Me.SelKey(enmUN.app_name).v(enmUB.contents)).
                            vt(enmMB.text, strAUDIT),
                        MsgBoxStyle.OkCancel
                        )
                    If ins_msg.vUserResponse = MsgBoxResult.Ok Then
                        db.Clipboard.Ins(
                            New db.rClipboard().
                            vt(enmCB.text, strAUDIT)
                            )
                    End If
                End If
            End Sub 'Cboard_CmdlineAudit

            <System.Diagnostics.DebuggerHidden()>
            Public Function ExistsKey(ur_key As bitUN) As Boolean
                ExistsKey = False
                Dim strUN = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmUB.bowl_name), strUN) Then
                        ExistsKey = True
                        Exit For
                    End If
                Next
            End Function 'ExistsKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rUserBowl) As rUserBowl
                Ins = ur_from
                Me.ttb.Add(ur_from)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsKey(ur_key As bitUN, ur_val As String) As rUserBowl
                Dim ret = New rUserBowl
                InsKey = ret
                Me.ttb.Add(
                    ret.
                    vt(enmUB.bowl_name, ur_key.name).
                    vt(enmUB.contents, ur_val)
                    )
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFrom_Application() As rUserBowl
                Dim ret = New rUserBowl
                InsFrom_Application = ret
                Me.InsKey(enmUN.app_name, New MxText.FileName().d(Mx.Class1.SourcePath).FileGroup)

                Dim arlCMD_RET = MxText.Cmdline_UB(Of bitUN, enmUN, bitUB, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, System.Environment.CommandLine)
                Me.InsKey(enmUN.cmdline_orig, qs & System.Environment.CommandLine.Replace(qs, qs & qs) & qs)
                Me.InsKey(enmUN.cmdline_table, qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(qs, qs & qs) & qs)
                For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                    Me.Ins(
                        New db.rUserBowl().
                        vt(enmUB.bowl_name, rowFOUND.v(enmUB.bowl_name)).
                        vt(enmUB.contents, rowFOUND.v(enmUB.contents))
                        )
                Next
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As bitUB, ur_value As String) As sUserBowl
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
            Public Function SelKey(ur_key As bitUN) As rUserBowl
                Dim ret As rUserBowl = Nothing
                Mx.TRow(Of bitUN, enmUN).glbl.RefKeys()
                Dim strUN = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmUB.bowl_name), strUN) Then
                        ret = row
                        Exit For
                    End If
                Next
                If ret Is Nothing Then ret = New rUserBowl
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
