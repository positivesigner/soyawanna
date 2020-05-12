start "" "Z:\My Documents\GitHub\asp_gotoassisttickets\SpProcDef\VBNetScript.exe" %0 %*
exit
Z:\My Documents\GitHub\asp_gotoassisttickets\SpProcDef\VBClass\MxBaseEc10.vb
RetVal = Mx.Want.NPP_Timer()
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.Want.NPP_Timer()
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
        Public Shared Function NPP_Timer() As Strap
            Dim stpRET = Strapd()
            NPP_Timer = stpRET
            stpRET.d("QUIT")
            Dim objERR_LIST = New ErrListBase : Try
                Call Have.NPP_Path_Search(db.UserBowl, Mx.Class1.SourceFolder)
                Call Have.CommandLine_Search_For_TextFile(db.UserBowl, System.Environment.GetCommandLineArgs)

                Call Use.Seconds_Delay(
                    ur_seconds_count:=1
                    )
                Call Use.Open_Program(
                     ur_app_path:=db.UserBowl.SelKey(enmUN.npp_path),
                     ur_file_path_to_pass_in:=db.UserBowl.SelKey(enmUN.in_textfile_path)
                     )

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If
        End Function 'NPP_Timer
    End Class 'Want

    Public Class Have
        Public Shared Sub CommandLine_Search_For_TextFile(ur_bowl_table As db.sUserBowl, ur_commandline_args() As String)
            Dim strNOTICE_MSG = mt
            Dim strFOUND_PATH = mt
            If ur_commandline_args.Length < 3 Then
                strNOTICE_MSG = "There must be at least three command-line parameters. Current length = " & ur_commandline_args.Length.ToString

            End If

            If HasText(strNOTICE_MSG) = False Then
                strFOUND_PATH = ur_commandline_args(b0(3))
                If System.IO.File.Exists(strFOUND_PATH) = False Then
                    strNOTICE_MSG = "Text file path not found: " & strFOUND_PATH
                End If
            End If

            If HasText(strNOTICE_MSG) = False Then
                ur_bowl_table.InsKey(enmUN.in_textfile_path, strFOUND_PATH)

            Else
                Throw New System.Exception(strNOTICE_MSG)
            End If
        End Sub 'CommandLine_Input_Search

        Public Shared Sub NPP_Path_Search(ur_bowl_table As db.sUserBowl, ur_root_path As String)
            Dim strPATH = "C:\Program Files\Notepad++\Notepad++.exe"
            Dim strSEARCH = vbCrLf & vbCrLf & strPATH
            If System.IO.File.Exists(strPATH) = False Then
                strPATH = "C:\TJBF\zPortableInstalls\NotePadPP\Notepad++Portable.exe"
                strSEARCH &= vbCrLf & vbCrLf & strPATH
            End If

            If System.IO.File.Exists(strPATH) = False Then
                strPATH = System.IO.Path.Combine(ur_root_path, "..\..\NotepadPP\Notepad++Portable.exe")
                strSEARCH &= vbCrLf & vbCrLf & strPATH
            End If

            If System.IO.File.Exists(strPATH) Then
                ur_bowl_table.InsKey(enmUN.npp_path, strPATH)

            Else
                Throw New System.Exception("NPP Path not found: " & strSEARCH)
            End If
        End Sub 'NPP_Path_Search
    End Class 'Have

    Public Class Use
        Public Shared Sub Open_Program(ur_app_path As db.rUserBowl, ur_file_path_to_pass_in As db.rUserBowl)
            Dim strPARAMETERS = ur_file_path_to_pass_in.v(enmUB.contents)
            If AreEqual(Left(strPARAMETERS, 1), qs) = False Then
                strPARAMETERS = qs & strPARAMETERS.Replace(qs, qs & qs) & qs
            End If

            With 1 : Dim p As New System.Diagnostics.ProcessStartInfo
                p.Verb = "Open"
                p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                p.UseShellExecute = True
                System.Diagnostics.Process.Start(ur_app_path.v(enmUB.contents), strPARAMETERS)
            End With
        End Sub 'Run_Program

        Public Shared Sub Seconds_Delay(ur_seconds_count As Integer)
            System.Threading.Thread.Sleep(ur_seconds_count * 1000)
        End Sub 'Seconds_Delay
    End Class 'Use


    Partial Public Class db
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If db.tblUserBowl Is Nothing Then
                db.tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'db

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
        Public Shared in_textfile_path As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared npp_path As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
        Public Shared to_npp_parm As bitUN = TRow(Of bitUN, enmUN).glbl.NewBitBase()
    End Class

    Partial Public Class db
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Call db.Connect()
            UserBowl = db.tblUserBowl
        End Function

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
