start "" PortableGit\VBNetScript.exe %0 %1
exit
PortableGit\MxBaseE3.vb
Dim strPREFIX = InputBox("Enter a folder name:")
Call Mx.GLog.LoadLog(strPREFIX)
RetVal = "QUIT"
End Function
End Class
End Namespace

Namespace Mx
    Public Enum eprCL
        server
        appdb
        formdb
        objdb
        reportdb
        report_basepath
        nointerface
        root_dir
        out_subdir
        flag_sort_files
    End Enum

    Public Class CommandInput
        Inherits Snrow(Of eprCL)
    End Class 'CommandInput


    Public Enum enmST
        open
        commit
        author
        commit_date
        comment
        file
    End Enum

    Public Enum eprCMT
        full_win_path
        full_unix_path
        author
        commit_sha
        commit_date
        comment_line
    End Enum

    Public Class Commit_Prop
        Inherits Snrow(Of eprCMT)

        Public dteTIME_STAMP As DateTime

        <System.Diagnostics.DebuggerHidden()> Public Sub New()
            Me.dteTIME_STAMP = System.DateTime.MinValue
        End Sub

        <System.Diagnostics.DebuggerHidden()> Public Function ToCopy() As Commit_Prop
            Dim sdnRET = New Commit_Prop
            ToCopy = sdnRET
            For Each kvpVAL In Me.kvp
                sdnRET.w(kvpVAL.Indexenm) = kvpVAL.v
            Next
			
			sdnRET.dteTIME_STAMP = Me.dteTIME_STAMP
        End Function 'ToCopy

        <System.Diagnostics.DebuggerHidden()> Public Function ToTSV() As String
            Dim stpRET = Strapd()
            For Each kvpVAL In Me.kvp
                If kvpVAL.Indexenm <> eprCMT.full_unix_path Then
                    If kvpVAL.Indexb1 > 1 Then
                        stpRET.d(Constants.vbTab)
                    End If
                    
                    stpRET.d(kvpVAL.v.Replace(Constants.vbTab, mt).Replace(Constants.vbCr, mt).Replace(Constants.vbLf, mt))
                   End If
            Next kvpVAL

            ToTSV = stpRET
        End Function 'ToTSV
    End Class 'Commit_Prop

    Public Enum eprFLD
        folder_name
        commit_date
    End Enum

    Public Class Folder_Prop
        Inherits Snrow(Of eprFLD)

        Public dteTIME_STAMP As DateTime

        <System.Diagnostics.DebuggerHidden()> Public Sub New()
            Me.dteTIME_STAMP = System.DateTime.MinValue
        End Sub
    End Class 'Folder_Prop

    Public Class GLog
        Public Shared Sub LoadLog(ur_subfolder As String)
            Dim strROOT_PATH = System.IO.Path.Combine(prv.Ret_RootPath(Environment.GetCommandLineArgs), ur_subfolder)
            Dim strGIT_PATH = System.Environment.ExpandEnvironmentVariables("PortableGit\cmd\git.exe")
            Dim strLINES = MxText.CmdOutput(strGIT_PATH, String.Format("-C ""{0}"" --no-pager log --full-history --name-only", strROOT_PATH))
            Dim sdaPLIST = prv.Parse_CommitFiles(strLINES)
            Call GLog.Write_FileTable(sdaPLIST, strROOT_PATH)
            MsgBox(String.Format("{0} files in log", sdaPLIST.Count.ToString), , "Git Log Export")
        End Sub 'LoadLog
	
        Public Shared Sub Write_FileTable(ur_cprop_list As Snlist(Of Commit_Prop), ur_root_path As String)
			Dim strLOG_PATH = ur_root_path & s & "log.tsv"
            Dim stmLOG_TSV = New System.IO.StreamWriter(strLOG_PATH, False, MxText.Std_FileEncoding)
            For Each sdnCPROP In ur_cprop_list
                stmLOG_TSV.WriteLine(sdnCPROP.ToTSV)
            Next sdnCPROP

            stmLOG_TSV.Close()
        End Sub 'Write_FileTable

        Private Class prv
            Public Shared Function Parse_CommitFiles(ur_commit_log_text As String) As Snlist(Of Commit_Prop)
                Dim lstFILE_PROP = New Snlist(Of Commit_Prop)
                Parse_CommitFiles = lstFILE_PROP
                Dim sdaTEXT = Sdata.Split(ur_commit_log_text.Replace(Chr(13), mt), Chr(10))
                Dim sdnCOMMIT_PROP As Commit_Prop = Nothing
                Dim sdnFILE_PROP As Commit_Prop = Nothing
                Dim intSTATE = enmST.open
                For Each strLINE In sdaTEXT
                    If strLINE <> "" Then
                        Select Case intSTATE
                            Case enmST.commit
                                If strLINE.ToLower.StartsWith("author:") Then
                                    sdnCOMMIT_PROP.w(eprCMT.author) = Mid(strLINE, Len("author:") + 1)
                                    intSTATE = enmST.author
                                End If 'strLINE

                            Case enmST.author
                                If strLINE.ToLower.StartsWith("date:") Then
                                    sdnCOMMIT_PROP.w(eprCMT.commit_date) = Mid(strLINE, Len("Date:") + 1)
                                    If prv.Ret_Date(sdnCOMMIT_PROP.v(eprCMT.commit_date), sdnCOMMIT_PROP.dteTIME_STAMP) Then
                                        sdnCOMMIT_PROP.w(eprCMT.commit_date) = sdnCOMMIT_PROP.dteTIME_STAMP.ToString("yyyy'm'MM'd'dd HH'm'mm's'ss")
                                    Else
                                        sdnCOMMIT_PROP.w(eprCMT.commit_date) = mt
                                    End If

                                    intSTATE = enmST.commit_date
                                End If 'strLINE

                            Case enmST.commit_date, enmST.comment, enmST.file, enmST.open
                                If strLINE.ToLower.StartsWith("commit") Then
                                    intSTATE = enmST.commit
                                    sdnCOMMIT_PROP = New Commit_Prop
                                    sdnCOMMIT_PROP.w(eprCMT.commit_sha) = LTrim(Mid(strLINE, Len("commit:") + 1))


                                ElseIf Left(strLINE, 1) = Space(1) Then
                                    If HasText(sdnCOMMIT_PROP.v(eprCMT.comment_line)) = False Then
                                        sdnCOMMIT_PROP.w(eprCMT.comment_line) = LTrim(strLINE)
                                    End If

                                    intSTATE = enmST.comment

                                ElseIf strLINE <> "" Then
                                    If sdnCOMMIT_PROP IsNot Nothing Then
                                        sdnFILE_PROP = sdnCOMMIT_PROP.ToCopy
                                        sdnFILE_PROP.w(eprCMT.full_unix_path) = strLINE
                                        sdnFILE_PROP.w(eprCMT.full_win_path) = strLINE.Replace("/", "\")
                                        lstFILE_PROP.Add(sdnFILE_PROP)
                                    End If

                                    intSTATE = enmST.file
                                End If 'strLINE
                        End Select 'intSTATE
                    End If 'strLINE
                Next strLINE
            End Function 'Parse_CommitFiles

            Public Shared Function Ret_Date(ur_date_wmdhmsytz As String, ByRef ur_date As DateTime) As Boolean
                ur_date_wmdhmsytz = LTrim(ur_date_wmdhmsytz)
                Dim strMONTH = Ret_Month(Mid(ur_date_wmdhmsytz, 5, 3))
                Dim strDAY = Mid(ur_date_wmdhmsytz, 9, 2)
                Dim intOFFSET = 0
                If Mid(strDAY, 2) = s Then
                    intOFFSET = -1
                    strDAY = Trim(strDAY)
                End If

                Dim strHMS = Mid(ur_date_wmdhmsytz, 12 + intOFFSET, 8)
                Dim strYEAR = Mid(ur_date_wmdhmsytz, 21 + intOFFSET, 4)
                Dim strTZ = Mid(ur_date_wmdhmsytz, 26 + intOFFSET, 5)
                Dim strDATE_MDYHMSTZ = strMONTH & "/" & strDAY & "/" & strYEAR & Space(1) & strHMS & Space(1) & strTZ
                Ret_Date = System.DateTime.TryParse(strDATE_MDYHMSTZ, ur_date)
            End Function 'Ret_Date

            Public Shared Function Ret_Month(ur_month_3char As String) As String
                Ret_Month = "1"
                Select Case ur_month_3char
                    Case "Jan"
                        Ret_Month = "1"
                    Case "Feb"
                        Ret_Month = "2"
                    Case "Mar"
                        Ret_Month = "3"
                    Case "Apr"
                        Ret_Month = "4"
                    Case "May"
                        Ret_Month = "5"
                    Case "Jun"
                        Ret_Month = "6"
                    Case "Jul"
                        Ret_Month = "7"
                    Case "Aug"
                        Ret_Month = "8"
                    Case "Sep"
                        Ret_Month = "9"
                    Case "Oct"
                        Ret_Month = "10"
                    Case "Nov"
                        Ret_Month = "11"
                    Case "Dec"
                        Ret_Month = "12"
                End Select
            End Function 'Ret_Month

            Public Shared Function Ret_RootPath(ur_agr_list As String()) As String
                Dim strROOT_PATH = ur_agr_list(ur_agr_list.Length - 1).Trim.Replace(qs, mt)
                If strROOT_PATH.ToLower.EndsWith(".cmd") OrElse
                   strROOT_PATH.ToLower.EndsWith(".exe") Then
                    strROOT_PATH = System.IO.Path.GetDirectoryName(strROOT_PATH)
                End If

                If strROOT_PATH.ToLower.EndsWith("\bin\debug") Then
                    strROOT_PATH = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(strROOT_PATH))
                End If

                Ret_RootPath = strROOT_PATH
            End Function 'Ret_RootPath
        End Class 'prv  
    End Class 'GLog
End Namespace
