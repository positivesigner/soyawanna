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
                If kvpVAL.Indexb1 > 1 Then
                    stpRET.d(Constants.vbTab)
                End If

                stpRET.d(kvpVAL.v.Replace(Constants.vbTab, mt).Replace(Constants.vbCr, mt).Replace(Constants.vbLf, mt))
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
            Dim strNOTICE_MSG = ""
			Dim strROOT_GITFOLDER = System.IO.Path.Combine(prv.Ret_RootPath(Environment.GetCommandLineArgs), ur_subfolder)
            Dim strLOGQ_FOLDER = prv.Get_LogQFolder(strROOT_GITFOLDER)
            If AreEqual(System.IO.Path.GetFilename(strROOT_GITFOLDER), System.IO.Path.GetFilename(Mx.Class1.SourceFolder)) Then
                strROOT_GITFOLDER = ""
               End If
            
            If strROOT_GITFOLDER = "" Then
                strNOTICE_MSG = "Please select a Git folder"
               End If
            
           If strNOTICE_MSG = "" Then
			Dim strUSER_FILENAME = InputBox("Enter whole or partial file name", "Old Code for Filename")
            If HasText(strUSER_FILENAME) Then
                Dim strGIT_PATH = System.Environment.ExpandEnvironmentVariables("PortableGit\cmd\git.exe")
                Dim strLINES = MxText.CmdOutput(strGIT_PATH, String.Format("-C ""{0}"" --no-pager log --full-history --name-only", strROOT_GITFOLDER))
                Dim sdaPLIST = prv.Parse_CommitFiles(strLINES, strUSER_FILENAME)
                If sdaPLIST.Count > 0 Then
                    Call GLog.Export_DailyCommits(sdaPLIST, strGIT_PATH, strROOT_GITFOLDER, strLOGQ_FOLDER)
                   End If
                
                strNOTICE_MSG = String.Format("{0} files in log", sdaPLIST.Count.ToString)
               End If
           End If
           
           If strNOTICE_MSG <> "" Then
            MsgBox(strNOTICE_MSG, , "Old Code of Filename")
           End If
        End Sub 'LoadLog

        Public Shared Sub Export_DailyCommits(ur_cprop_list As Snlist(Of Commit_Prop), ur_git_path As String, ur_root_path As String, ur_logq_path As String)
            For Each kvpCPROP In ur_cprop_list.kvp
				Dim sdnCPROP = kvpCPROP.row
				Dim strFULL_PATH = sdnCPROP.v(eprCMT.full_win_path)
				Dim strCUR_DATE = sdnCPROP.v(eprCMT.commit_date)
				Dim strOUTPUT_PATH = System.IO.Path.Combine(ur_logq_path, "Old Code" & s & strCUR_DATE & "\" & strFULL_PATH)
				Dim strPARENT_DIR = System.IO.Path.GetDirectoryName(strOUTPUT_PATH)
				If System.IO.Directory.Exists(strPARENT_DIR) = False Then
					System.IO.Directory.CreateDirectory(strPARENT_DIR)
				End If

				Dim P As New System.Diagnostics.Process()
				With 1 : Dim prcINFO = P.StartInfo
					prcINFO.FileName = "cmd"
					prcINFO.Arguments = String.Format("/c {0} -C ""{1}"" show {2}:""{3}"" > ""{4}""", ur_git_path, ur_root_path, sdnCPROP.v(eprCMT.commit_sha), sdnCPROP.v(eprCMT.full_unix_path), strOUTPUT_PATH)
					prcINFO.UseShellExecute = False
					prcINFO.RedirectStandardOutput = False
					prcINFO.CreateNoWindow = True
				End With 'prcINFO

				P.Start()
				P.WaitForExit()
				Call System.IO.File.SetLastWriteTime(strOUTPUT_PATH, sdnCPROP.dteTIME_STAMP)
            Next kvpCPROP
        End Sub 'Export_DailyCommits

        Private Class prv
            Public Shared Function Get_LogQFolder(ur_git_folder As String) As String
                Get_LogQFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ur_git_folder), "LogQ_" & System.IO.Path.GetFileName(ur_git_folder) & "_" & Now.ToString("yyyyMMddhhmmss") )
            End Function
            
            Public Shared Function Parse_CommitFiles(ur_commit_log_text As String, ur_file_name As String) As Snlist(Of Commit_Prop)
                Dim lstFILE_PROP = New Snlist(Of Commit_Prop)
                Parse_CommitFiles = lstFILE_PROP
				Dim strEXPORT_ONE_DAY = mt
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
                                        Dim intPOS = InStr(strLINE.ToUpper, ur_file_name.ToUpper)
										If intPOS > 0 AndAlso
                                           ContainingText(Mid(strLINE, intPOS), "/") = False Then
											sdnFILE_PROP = sdnCOMMIT_PROP.ToCopy
											sdnFILE_PROP.w(eprCMT.full_unix_path) = strLINE
											sdnFILE_PROP.w(eprCMT.full_win_path) = strLINE.Replace("/", "\")
											lstFILE_PROP.Add(sdnFILE_PROP)
										End If
                                    End If 'sdnCOMMIT_PROP

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

            Public Shared Function Ret_RootDateTime(ur_root_path As String) As String
				Dim strRET_ROOT_DATETIME = mt
				Dim strOLD_CODE = " Old Code "
				Dim idxOLD_CODE = InStr(ur_root_path, strOLD_CODE)
				If idxOLD_CODE > 0 Then
					strRET_ROOT_DATETIME = Mid(ur_root_path, idxOLD_CODE + strOLD_CODE.Length).Replace("\", mt)
                End If
				
                Ret_RootDateTime = strRET_ROOT_DATETIME
            End Function 'Ret_RootDateTime

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
