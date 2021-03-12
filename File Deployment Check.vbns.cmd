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
@cd %mx_dir%
@START "" "%mx_dir%\MxClasses\VBNetScript.exe" /path=%0

:mx_end
@EXIT
MxClasses\DLL_SystemIoCompression2019m12\System.IO.Compression.dll
MxClasses\DLL_SystemIoCompression2019m12\System.IO.Compression.FileSystem.dll
MxClasses\MxBaseEc13.vb
RetVal = Mx.UserAction.Deployment_Report_errhnd()
End Function
End Class
End Namespace

'Namespace Mx
'    Module subs
'        Sub Main()
'            Dim RetVal = Mx.UserAction.Deployment_Report_errhnd()
'            If Mx.AreEqual(RetVal, "QUIT") = False Then MsgBox(RetVal)
'        End Sub
'    End Module 'subs

'    Public Class Class1
'        Public Shared SourceFolder As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "")
'        Public Shared SourcePath As String = My.Application.Info.DirectoryPath.Replace("\bin\Debug", "") & "\File Deployment Check.vbns.cmd"
'    End Class
'End Namespace 'Mx


Namespace Mx
    Public Class UserAction
        Public Shared Sub Deployment_Report(ret_message As Strap)
            Have.CurExeFilePath = Mx.Class1.SourcePath
            Dim app_folder_bowlname = enmSB.app_folder
            Dim app_title_bowlname = enmSB.app_exetitle
            Dim fn_file_exists = enmWF.FileExists
            Dim fn_file_size = enmWF.FileSize
            Dim fn_store_text = enmWC.StoreText
            Dim fn_user_input_dialog = enmWM.UserInputDialog
            Dim sourcefile_list_filepath_bowlname = Assistant.Load_SourceFile_List(app_folder_bowlname, app_title_bowlname)
            Dim destinationfolder_list_filepath_bolwname = Assistant.Load_DestinationFolder_List(app_folder_bowlname, app_title_bowlname)
            Dim report_output_bowlname = Assistant.Compile_FileSize_Report(fn_file_exists, fn_file_size)
            Dim written_message_count = Assistant.Display_UserDialog(report_output_bowlname, fn_user_input_dialog, fn_store_text)
        End Sub 'Duplicates_Report

        Public Shared Function Deployment_Report_errhnd() As Strap
            Dim stpRET = Strapd()
            Deployment_Report_errhnd = stpRET
            Dim objERR_LIST = New ErrListBase : Try
                Call UserAction.Deployment_Report(stpRET)

            Catch ex As System.Exception
                Call objERR_LIST.dError_Stack(ex)
            End Try

            If objERR_LIST.Found Then
                stpRET.Clear().d(objERR_LIST.ToString)
            End If

            If stpRET.HasText = False Then
                stpRET.d("QUIT")
            End If
        End Function 'Duplicates_Report_errhnd
    End Class 'UserAction


    Class Assistant
        Const strLIT_SOURCE_FILES_DOTTXT = "Source Files.txt"
        Const strLIT_DESTINATION_FOLDERS_DOTTXT = "Destination Folders.txt"

        Public Shared Function Compile_FileSize_Report(
            ur_fn_file_exists As enmWF.zFileExists,
            ur_fn_file_size As enmWF.zFileSize
            ) As enmTB.zreport_output
            Dim filesize_report_cart = Have.FilesizeReport
            Dim retTB = enmTB.report_output
            Compile_FileSize_Report = retTB
            For Each trwSOURCE_FILE In Have.SourceFile.SelAll
                Dim strFILE_NAME = trwSOURCE_FILE.v(enmSF.source_filename)
                Dim sdaFOLDERS_WHERE_FILE_EXISTS = New Sdata
                For Each trwDEST_FOLDER In Have.DestinationFolder.SelAll
                    Dim strFOLDER_PATH = trwDEST_FOLDER.v(enmDF.deployment_folderpath)
                    sdaFOLDERS_WHERE_FILE_EXISTS.d(strFOLDER_PATH)
                    Dim flnFILE_SEARCH = FileNamed().d(strFOLDER_PATH).d(strFILE_NAME)
                    If Have.WindowsFSEnv.Result(ur_fn_file_exists, flnFILE_SEARCH) Then
                        Dim strCUR_SIZE = Have.WindowsFSEnv.Result(ur_fn_file_size, flnFILE_SEARCH)
                        Dim trwCUR_GROUP = filesize_report_cart.UseKey(strFILE_NAME, strCUR_SIZE)
                        trwCUR_GROUP.v(enmFSR.folderpath_list) &= "," & strFOLDER_PATH
                    End If 'flnFILE_SEARCH
                Next trwDEST_FOLDER

                'Dim ttbFILE_SIZES = filesize_report_cart.SelOnKey(strFILE_NAME)
                'If ttbFILE_SIZES.Count = 1 Then
                '    filesize_report_cart.Del(ttbFILE_SIZES.SelFirst)
                'End If
            Next trwSOURCE_FILE

            Have.TempBowl.SelKey(retTB).Contents = filesize_report_cart.ToString(True)
        End Function 'Compile_FileSize_Report

        Public Shared Function Display_UserDialog(ur_report_output As enmTB.zreport_output, ur_fn_user_input_dialog As enmWM.zUserInputDialog, ur_fn_store_text As enmWC.zStoreText) As Integer
            Display_UserDialog = 0
            Dim strOUTPUT = Have.TempBowl.SelKey(ur_report_output).Contents
            If Have.WindowsMsgBoxEnv.Result(ur_fn_user_input_dialog, strOUTPUT, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Have.WindowsCboardEnv.Result(ur_fn_store_text, strOUTPUT)
            End If
        End Function 'Display_UserDialog

        Public Shared Function Load_SourceFile_List(
            ur_appfolder_bowlname As enmSB.zapp_folder,
            ur_apptitle_bowlname As enmSB.zapp_exetitle
            ) As enmTB.zsourcefile_list_filepath

            Dim retTB = enmTB.sourcefile_list_filepath
            Load_SourceFile_List = retTB
            Dim strFILE_NAME = FileNamed().d(Have.SessionBowl.SelKey(ur_apptitle_bowlname).Contents).FileGroup & " parm " & strLIT_SOURCE_FILES_DOTTXT
            Dim flnFILE_PATH = FileNamed().d(Have.SessionBowl.SelKey(ur_appfolder_bowlname).Contents).d(strFILE_NAME)
            Have.TempBowl.SelKey(retTB).Contents = flnFILE_PATH
            Have.SourceFile.PersistRead(flnFILE_PATH)
        End Function 'Load_SourceFile_List

        Public Shared Function Load_DestinationFolder_List(
            ur_appfolder_bowlname As enmSB.zapp_folder,
            ur_apptitle_bowlname As enmSB.zapp_exetitle
            ) As enmTB.zdestinationfolder_list_filepath

            Dim retTB = enmTB.destinationfolder_list_filepath
            Load_DestinationFolder_List = retTB
            Dim strFILE_NAME = FileNamed().d(Have.SessionBowl.SelKey(ur_apptitle_bowlname).Contents).FileGroup & " parm " & strLIT_DESTINATION_FOLDERS_DOTTXT
            Dim flnFILE_PATH = FileNamed().d(Have.SessionBowl.SelKey(ur_appfolder_bowlname).Contents).d(strFILE_NAME)
            Have.TempBowl.SelKey(retTB).Contents = flnFILE_PATH
            Have.DestinationFolder.PersistRead(flnFILE_PATH)
        End Function 'Load_DestinationFolder_List

        Private Class prv
        End Class 'prv
    End Class 'Assistant



    Partial Public Class Have
        Private Shared prv_envWindowsCboard As glblWindowsCboard
        Private Shared prv_envWindowsFS As glblWindowsFS
        Private Shared prv_envWindowsMsgBox As glblWindowsMsgBox
        Private Shared prv_tblDestinationFolder As sDestinationFolder
        Private Shared prv_tblFilesizeReport As sFilesizeReport
        Private Shared prv_tblSourceFile As sSourceFile
        Private Shared prv_tblSessionBowl As sSessionBowl
        Private Shared prv_tblTempBowl As sTempBowl
        Private Shared prv_tblUserBowl As sUserBowl
        Private Shared prv_envZipFS As glblZipFS

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.prv_tblUserBowl Is Nothing Then
                Have.prv_envWindowsCboard = New glblWindowsCboard
                Have.prv_envWindowsFS = New glblWindowsFS
                Have.prv_envWindowsMsgBox = New glblWindowsMsgBox
                Have.prv_envZipFS = New glblZipFS
                Have.prv_tblDestinationFolder = New sDestinationFolder
                Have.prv_tblFilesizeReport = New sFilesizeReport
                Have.prv_tblSourceFile = New sSourceFile
                Have.prv_tblSessionBowl = New sSessionBowl
                Have.prv_tblTempBowl = New sTempBowl
                Have.prv_tblUserBowl = New sUserBowl
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have


    Public Class enmWC
        Inherits bitBASE
        Public Shared StoreText As zStoreText = TRow(Of enmWC).glbl.Trbase(Of zStoreText).NewBitBase() : Public Class zStoreText : Inherits enmWC : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsCboardEnv() As glblWindowsCboard
            Call Have.Connect()
            WindowsCboardEnv = Have.prv_envWindowsCboard
        End Function

        Public Class glblWindowsCboard
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWC.zStoreText, ur_text As String) As Integer
                Result = glbl.gCboard.SetText(ur_text)
            End Function
        End Class 'glblWindowsCboard
    End Class 'Have

    Public Class enmWM
        Inherits bitBASE
        Public Shared UserInputDialog As zUserInputDialog = TRow(Of enmWM).glbl.Trbase(Of zUserInputDialog).NewBitBase() : Public Class zUserInputDialog : Inherits enmWM : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsMsgBoxEnv() As glblWindowsMsgBox
            Call Have.Connect()
            WindowsMsgBoxEnv = Have.prv_envWindowsMsgBox
        End Function

        Public Class glblWindowsMsgBox
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWM.zUserInputDialog, ur_message As String, Optional ur_style As MsgBoxStyle = MsgBoxStyle.OkOnly) As MsgBoxResult
                Dim strAPP_NAME = Have.SessionBowl.SelKey(enmSB.app_exetitle).Contents
                Result = glbl.gMsgBox.GetResult(ur_message, ur_style, strAPP_NAME)
            End Function
        End Class 'glblWindowsMsgBox
    End Class 'Have

    Public Class enmWF
        Inherits bitBASE
        Public Shared FileExists As zFileExists = TRow(Of enmWF).glbl.Trbase(Of zFileExists).NewBitBase() : Public Class zFileExists : Inherits enmWF : End Class
        Public Shared FileSearch As zFileSearch = TRow(Of enmWF).glbl.Trbase(Of zFileSearch).NewBitBase() : Public Class zFileSearch : Inherits enmWF : End Class
        Public Shared FileSize As zFileSize = TRow(Of enmWF).glbl.Trbase(Of zFileSize).NewBitBase() : Public Class zFileSize : Inherits enmWF : End Class
        Public Shared FolderSearch As zFolderSearch = TRow(Of enmWF).glbl.Trbase(Of zFolderSearch).NewBitBase() : Public Class zFolderSearch : Inherits enmWF : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsFSEnv() As glblWindowsFS
            Call Have.Connect()
            WindowsFSEnv = Have.prv_envWindowsFS
        End Function

        Public Class glblWindowsFS
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWF.zFileExists, ur_filepath As String) As Boolean
                Result = False
                Dim zrpFILE = New Have.glblZipFS.ZipRelPath(ur_filepath)
                Try
                    If HasText(zrpFILE.ZipFile) = False Then
                        Result = glbl.gWindowsFS.HasFile(ur_filepath)
                    Else
                        Result = Have.ZipFSEnv.Result(enmZF.FileExists, zrpFILE.ZipFile, zrpFILE.RelPath)
                    End If
                Catch ex As System.Exception
                End Try
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWF.zFileSearch, ur_search_filespec As MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As Sdata
                Dim zrpFILE = New Have.glblZipFS.ZipRelPath(ur_search_filespec)
                Try
                    If HasText(zrpFILE.ZipFile) = False Then
                        Result = New Sdata().dList(glbl.gWindowsFS.GetFiles(ur_search_filespec.gParentDir, ur_search_filespec.Name, ur_recurse_option))
                    Else
                        Dim retLIST = New Sdata
                        Result = retLIST
                        For Each strENTRY In Have.ZipFSEnv.Result(enmZF.FileSearch, zrpFILE.ZipFile, zrpFILE.RelPath, ur_recurse_option)
                            retLIST.d(ur_search_filespec.gCopy.d(strENTRY.Replace("/", "\")))
                        Next
                    End If
                Catch ex As System.Exception
                    Result = New Sdata
                End Try
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWF.zFileSize, ur_filepath As String) As String
                Dim zrpFILE = New Have.glblZipFS.ZipRelPath(ur_filepath)
                Try
                    If HasText(zrpFILE.ZipFile) = False Then
                        Result = New System.IO.FileInfo(ur_filepath).Length.ToString
                    Else
                        Result = Have.ZipFSEnv.Result(enmZF.FileSize, zrpFILE.ZipFile, zrpFILE.RelPath)
                    End If
                Catch ex As System.Exception
                    Result = "0"
                End Try
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmWF.zFolderSearch, ur_search_filespec As MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As Sdata
                Dim zrpFILE = New Have.glblZipFS.ZipRelPath(ur_search_filespec)
                Try
                    If HasText(zrpFILE.ZipFile) = False Then
                        Result = New Sdata().dList(glbl.gWindowsFS.GetDirectories(ur_search_filespec.gParentDir, ur_search_filespec.Name, ur_recurse_option))
                    Else
                        Dim retLIST = New Sdata
                        Result = retLIST
                        For Each strENTRY In Have.ZipFSEnv.Result(enmZF.FolderSearch, zrpFILE.ZipFile, zrpFILE.RelPath, ur_recurse_option)
                            retLIST.d(ur_search_filespec.gCopy.d(strENTRY.Replace("/", "\")))
                        Next
                    End If
                Catch ex As System.Exception
                    Result = New Sdata
                End Try
            End Function
        End Class 'glblWindowsFS
    End Class 'Have

    Public Class enmZF
        Inherits bitBASE
        Public Shared FileExists As zFileExists = TRow(Of enmZF).glbl.Trbase(Of zFileExists).NewBitBase() : Public Class zFileExists : Inherits enmZF : End Class
        Public Shared FileSize As zFileSize = TRow(Of enmZF).glbl.Trbase(Of zFileSize).NewBitBase() : Public Class zFileSize : Inherits enmZF : End Class
        Public Shared FileText As zFileText = TRow(Of enmZF).glbl.Trbase(Of zFileText).NewBitBase() : Public Class zFileText : Inherits enmZF : End Class
        Public Shared FileSearch As zFileSearch = TRow(Of enmZF).glbl.Trbase(Of zFileSearch).NewBitBase() : Public Class zFileSearch : Inherits enmZF : End Class
        Public Shared FolderSearch As zFolderSearch = TRow(Of enmZF).glbl.Trbase(Of zFolderSearch).NewBitBase() : Public Class zFolderSearch : Inherits enmZF : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function ZipFSEnv() As glblZipFS
            Call Have.Connect()
            ZipFSEnv = Have.prv_envZipFS
        End Function

        Public Class glblZipFS
            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmZF.zFileExists, ur_zipfile_path As String, ur_filepath As String) As Boolean
                Result = Me.Result(enmZF.FileSearch, ur_zipfile_path, ur_filepath).Count = 1
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmZF.zFileSearch, ur_zipfile_path As String, ur_search_filespec As MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As Sdata
                Dim retLIST = New Sdata
                Result = retLIST
                Try
                    Using zipFS = System.IO.Compression.ZipFile.OpenRead(ur_zipfile_path)
                        Dim strROOT = ur_search_filespec.gParentDir.FilePath.Replace("\", "/")
                        Dim strFILE_SPEC = ur_search_filespec.Name

                        For Each zfnENTRY In zipFS.Entries
                            Dim flnINZIP_PATH = FileNamed().d(zfnENTRY.FullName)
                            Dim strFOUND = flnINZIP_PATH.Name
                            If ur_recurse_option = System.IO.SearchOption.AllDirectories Then
                                If HasText(strROOT) = False AndAlso
                                  prv.MatchesFileSpec(strFOUND, strFILE_SPEC) Then
                                    retLIST.d(flnINZIP_PATH)

                                Else 'ur_recurse_option
                                    If StartingWithText(flnINZIP_PATH.FilePath, strROOT & "/") AndAlso
                                      prv.MatchesFileSpec(strFOUND, strFILE_SPEC) Then
                                        retLIST.d(flnINZIP_PATH)
                                    End If 'flnINZIP_PATH
                                End If

                            Else
                                If AreEqual(flnINZIP_PATH.gParentDir.FilePath, strROOT) AndAlso
                                  prv.MatchesFileSpec(strFOUND, strFILE_SPEC) Then
                                    retLIST.d(flnINZIP_PATH)
                                End If 'flnINZIP_PATH
                            End If
                        Next zfnENTRY
                    End Using 'zipFS

                Catch ex As System.Exception
                    Result = New Sdata
                End Try
            End Function 'zFileSearch

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmZF.zFileText, ur_zipfile_path As String, ur_filepath As String) As String
                Result = mt
                Try
                    Dim sdaFILE_PATH = Me.Result(enmZF.FileSearch, ur_zipfile_path, ur_filepath)
                    If sdaFILE_PATH.Count = 1 Then
                        Using zipFS = System.IO.Compression.ZipFile.OpenRead(ur_zipfile_path)
                            Using stmFILE = New System.IO.StreamReader(zipFS.GetEntry(sdaFILE_PATH.v_b1(1)).Open)
                                Result = stmFILE.ReadToEnd
                            End Using
                        End Using 'zipFS
                    End If
                Catch ex As System.Exception
                End Try
            End Function 'zFileText

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmZF.zFileSize, ur_zipfile_path As String, ur_filepath As String) As String
                Result = "0"
                Try
                    Dim sdaFILE_PATH = Me.Result(enmZF.FileSearch, ur_zipfile_path, ur_filepath)
                    If sdaFILE_PATH.Count = 1 Then
                        Using zipFS = System.IO.Compression.ZipFile.OpenRead(ur_zipfile_path)
                            Result = zipFS.GetEntry(sdaFILE_PATH.v_b1(1)).Length.ToString
                        End Using 'zipFS
                    End If
                Catch ex As System.Exception
                End Try
            End Function 'zFileSize

            <System.Diagnostics.DebuggerHidden()>
            Public Function Result(ur_fn As enmZF.zFolderSearch, ur_zipfile_path As String, ur_search_filespec As MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As Sdata
                Dim retLIST = New Sdata
                Result = retLIST
                Try
                    Using zipFS = System.IO.Compression.ZipFile.OpenRead(ur_zipfile_path)
                        Dim strROOT = ur_search_filespec.gParentDir.FilePath.Replace("\", "/")
                        Dim strFILE_SPEC = ur_search_filespec.Name
                        If HasText(strROOT) = False Then
                            strROOT &= "/"
                        End If

                        For Each zfnENTRY In zipFS.Entries
                            Dim flnINZIP_PATH = FileNamed().d(zfnENTRY.FullName)
                            Dim strFOUND = mt
                            Dim flnREDUCE = flnINZIP_PATH.gParentDir
                            While HasText(flnREDUCE.FilePath)
                                Dim strCOMPARE = flnREDUCE.FilePath & "/"
                                If strCOMPARE.Length > strROOT.Length AndAlso
                                  StartingWithText(strCOMPARE, strROOT) Then
                                    strFOUND = strCOMPARE
                                    If ur_recurse_option = System.IO.SearchOption.AllDirectories Then
                                        If prv.MatchesFileSpec(strFOUND, strFILE_SPEC) AndAlso
                                          retLIST.Contains(strFOUND) = False Then
                                            retLIST.d(strFOUND)
                                        End If
                                    End If 'ur_recurse_option
                                End If 'strCOMPARE

                                Call flnREDUCE.wParentDir()
                            End While 'flnREDUCE

                            If prv.MatchesFileSpec(strFOUND, strFILE_SPEC) AndAlso
                              retLIST.Contains(strFOUND) = False Then
                                retLIST.d(strFOUND)
                            End If
                        Next zfnENTRY
                    End Using 'zipFS

                Catch ex As System.Exception
                    Result = New Sdata
                End Try
            End Function 'zFolderSearch

            Private Class prv
                Public Shared Function MatchesFileSpec(ur_file_name As String, ur_file_spec As String) As Boolean
                    MatchesFileSpec = False
                    If AreEqual(ur_file_name, ur_file_spec) Then
                        MatchesFileSpec = True

                    ElseIf AreEqual(ur_file_spec, "*.*") Then
                        MatchesFileSpec = True

                    ElseIf EndingWithText(ur_file_spec, ".*") AndAlso
                      StartingWithText(ur_file_name, Left(ur_file_spec, ur_file_spec.Length - 2) & "*") Then
                        MatchesFileSpec = True

                    Else
                        MatchesFileSpec = ur_file_name.ToUpper Like ur_file_spec.ToUpper
                    End If
                End Function 'MatchesFileSpec
            End Class 'prv

            Public Class ZipRelPath
                Public Property ZipFile As String
                Public Property RelPath As String

                Public Sub New(ur_filepath As String)
                    Dim intFOUND_INDEX = InStr(ur_filepath.ToUpper, ".ZIP\")
                    If intFOUND_INDEX = 0 Then
                        Me.ZipFile = mt
                        Me.RelPath = ur_filepath

                    Else
                        Me.ZipFile = Left(ur_filepath, intFOUND_INDEX) & "ZIP"
                        Me.RelPath = Mid(ur_filepath, intFOUND_INDEX + 5)
                    End If 'intFOUND_INDEX
                End Sub 'New
            End Class 'ZipRelPath
        End Class 'glblZipFS
    End Class 'Have

    Public Class enmFSR
        Inherits bitBASE
        Public Shared file_name As zfile_name = TRow(Of enmFSR).glbl.Trbase(Of zfile_name).NewBitBase() : Public Class zfile_name : Inherits enmFSR : End Class
        Public Shared file_size As zfile_size = TRow(Of enmFSR).glbl.Trbase(Of zfile_size).NewBitBase() : Public Class zfile_size : Inherits enmFSR : End Class
        Public Shared folderpath_list As zfolderpath_list = TRow(Of enmFSR).glbl.Trbase(Of zfolderpath_list).NewBitBase() : Public Class zfolderpath_list : Inherits enmFSR : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function FilesizeReport() As sFilesizeReport
            Call Have.Connect()
            FilesizeReport = Have.prv_tblFilesizeReport
        End Function

        Public Class rFilesizeReport
            Inherits TRow(Of enmFSR)
        End Class 'rFilesizeReport

        Public Class sFilesizeReport
            Inherits Mx.TablePKStr(Of enmFSR, rFilesizeReport)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Call MyBase.New(2)
            End Sub
        End Class 'sFilesizeReport
    End Class 'FSR


    Public Class enmDF
        Inherits bitBASE
        Public Shared deployment_folderpath As zdeployment_folderpath = TRow(Of enmDF).glbl.Trbase(Of zdeployment_folderpath).NewBitBase() : Public Class zdeployment_folderpath : Inherits enmDF : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function DestinationFolder() As sDestinationFolder
            Call Have.Connect()
            DestinationFolder = Have.prv_tblDestinationFolder
        End Function

        Public Class rDestinationFolder
            Inherits TRow(Of enmDF)
        End Class 'rDestinationFolder

        Public Class sDestinationFolder
            Inherits Mx.TablePKStr(Of enmDF, rDestinationFolder)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Call MyBase.New(1)
            End Sub
        End Class 'sDestinationFolder
    End Class 'DF


    Public Class enmSF
        Inherits bitBASE
        Public Shared source_filename As zsource_filename = TRow(Of enmSF).glbl.Trbase(Of zsource_filename).NewBitBase() : Public Class zsource_filename : Inherits enmSF : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function SourceFile() As sSourceFile
            Call Have.Connect()
            SourceFile = Have.prv_tblSourceFile
        End Function

        Public Class rSourceFile
            Inherits TRow(Of enmSF)
        End Class 'rSourceFile

        Public Class sSourceFile
            Inherits Mx.TablePKStr(Of enmSF, rSourceFile)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Call MyBase.New(1)
            End Sub
        End Class 'sSourceFile
    End Class 'SF


    Public Class enrSB
        Inherits bitBASE
        Public Shared bowl_name As enrSB = TRow(Of enrSB).glbl.NewBitBase()
        Public Shared contents As enrSB = TRow(Of enrSB).glbl.NewBitBase()
    End Class

    Public Class enmSB
        Inherits bitBASE
        Public Shared application_exe_filepath As zapplication_exe_filepath = TRow(Of enmSB).glbl.Trbase(Of zapplication_exe_filepath).NewBitBase() : Public Class zapplication_exe_filepath : Inherits enmSB : End Class
        Public Shared app_folder As zapp_folder = TRow(Of enmSB).glbl.Trbase(Of zapp_folder).NewBitBase() : Public Class zapp_folder : Inherits enmSB : End Class
        Public Shared app_exetitle As zapp_exetitle = TRow(Of enmSB).glbl.Trbase(Of zapp_exetitle).NewBitBase() : Public Class zapp_exetitle : Inherits enmSB : End Class
        Public Shared cmdline_orig As zcmdline_orig = TRow(Of enmSB).glbl.Trbase(Of zcmdline_orig).NewBitBase() : Public Class zcmdline_orig : Inherits enmSB : End Class
    End Class

    Partial Public Class Have
        Public Shared FirstConnect As Object
        Public Shared CmdLineText As String
        Public Shared CurAssembly As System.Reflection.Assembly
        Public Shared CurExeFilePath As String

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function SessionBowl() As sSessionBowl
            Dim bolFIRST_INIT = (Have.FirstConnect Is Nothing)
            Call Have.Connect()
            SessionBowl = Have.prv_tblSessionBowl
            If bolFIRST_INIT Then
                Have.FirstConnect = "Done"
                Dim first_unnamed_parm_compiler_exe_bowlname = enmUB.script_compiler_exe_filepath
                Dim second_unnamed_parm_path_vbns_bowlname = enmUB.unnamed_parm_path01

                Dim application_exe_folder_bowlname = SessionBowl.Store_ApplicationEXEFilePath(Have.CurAssembly, Have.CurExeFilePath)
                Dim app_exetitle_bowlname = SessionBowl.Store_ApplicationEXETitle(application_exe_folder_bowlname)
                Dim app_folder_bowlname = SessionBowl.Store_ApplicationEXENonDebugFolder(application_exe_folder_bowlname)
                Dim cmdline_orig_bowlname = SessionBowl.Store_Original_CommandLine(Have.CmdLineText)
                Dim cmdline_table_bowlname = Have.UserBowl.Store_CommandLine_ParseTable(cmdline_orig_bowlname, first_unnamed_parm_compiler_exe_bowlname, second_unnamed_parm_path_vbns_bowlname)

                'When from Command Line parameters to show user a dialog box: SessionBowl.SelKey(cmdexport_audit_bowlname).Contents = "1"
                Dim cmdexport_audit_bowlname = enmUB.audit_parms_to_cboard
                Dim fn_user_input_dialog = enmWM.UserInputDialog
                Dim fn_store_text = enmWC.StoreText
                Call Have.UserBowl.Show_UserAuditMessage(cmdexport_audit_bowlname, fn_user_input_dialog, fn_store_text)
            End If 'bolFIRST_INIT
        End Function 'SessionBowl

        Public Class rSessionBowl
            Inherits TRow(Of enrSB)

            Public Property Contents As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enrSB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.v(enrSB.contents) = value
                End Set
            End Property 'Contents
        End Class 'rSessionBowl

        Public Class sSessionBowl
            Inherits Mx.TablePKEnum(Of enrSB, enmSB, rSessionBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXEFilePath(
                ur_cur_assembly As System.Reflection.Assembly,
                ur_curexe_path As String
                ) As enmSB.zapplication_exe_filepath

                Dim retSB = enmSB.application_exe_filepath
                Store_ApplicationEXEFilePath = retSB
                Dim strCUR_EXE_PATH = mt
                If ur_cur_assembly IsNot Nothing Then
                    strCUR_EXE_PATH = ur_cur_assembly.Location
                End If

                If Mx.HasText(strCUR_EXE_PATH) = False Then
                    strCUR_EXE_PATH = ur_curexe_path
                End If

                Me.SelKey(retSB).Contents = strCUR_EXE_PATH
            End Function 'Store_ApplicationEXEFilePath

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXETitle(ur_application_exe_filepath As enmSB.zapplication_exe_filepath) As enmSB.zapp_exetitle
                Dim retSB = enmSB.app_exetitle
                Store_ApplicationEXETitle = retSB
                Dim flnAPP_EXEPATH = Mx.FileNamed().d(Me.SelKey(ur_application_exe_filepath).Contents)
                Me.SelKey(retSB).Contents = flnAPP_EXEPATH.FileGroup
            End Function 'Store_ApplicationEXETitle

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_ApplicationEXENonDebugFolder(ur_application_exe_filepath As enmSB.zapplication_exe_filepath) As enmSB.zapp_folder
                Dim retSB = enmSB.app_folder
                Store_ApplicationEXENonDebugFolder = retSB
                Dim strCUR_EXE_PATH = Me.SelKey(ur_application_exe_filepath).Contents
                Dim flnAPP_PATH = Mx.FileNamed().d(strCUR_EXE_PATH.Replace("\bin\Debug", Mx.mt))
                Me.SelKey(retSB).Contents = flnAPP_PATH.ParentDir
            End Function 'Store_ApplicationEXENonDebugFolder

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_Original_CommandLine(ur_cmdline_text As String) As enmSB.zcmdline_orig
                Dim retSB = enmSB.cmdline_orig
                Store_Original_CommandLine = retSB
                Me.SelKey(retSB).Contents = ur_cmdline_text
            End Function 'Store_Original_CommandLine
        End Class 'sUserBowl
    End Class 'SB


    Public Class enrTB
        Inherits bitBASE
        Public Shared bowl_name As enrTB = TRow(Of enrTB).glbl.NewBitBase()
        Public Shared contents As enrTB = TRow(Of enrTB).glbl.NewBitBase()
    End Class

    Public Class enmTB
        Inherits bitBASE
        Public Shared report_output As zreport_output = TRow(Of enmTB).glbl.Trbase(Of zreport_output).NewBitBase() : Public Class zreport_output : Inherits enmTB : End Class
        Public Shared destinationfolder_list_filepath As zdestinationfolder_list_filepath = TRow(Of enmTB).glbl.Trbase(Of zdestinationfolder_list_filepath).NewBitBase() : Public Class zdestinationfolder_list_filepath : Inherits enmTB : End Class
        Public Shared sourcefile_list_filepath As zsourcefile_list_filepath = TRow(Of enmTB).glbl.Trbase(Of zsourcefile_list_filepath).NewBitBase() : Public Class zsourcefile_list_filepath : Inherits enmTB : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function TempBowl() As sTempBowl
            Call Have.Connect()
            TempBowl = Have.prv_tblTempBowl
        End Function 'TempBowl

        Public Class rTempBowl
            Inherits TRow(Of enrTB)

            Public Property Contents As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enrTB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.v(enrTB.contents) = value
                End Set
            End Property 'Contents
        End Class 'rTempBowl

        Public Class sTempBowl
            Inherits Mx.TablePKEnum(Of enrTB, enmTB, rTempBowl)
        End Class 'sTempBowl
    End Class 'TB


    Public Class enrUB
        Inherits bitBASE
        Public Shared bowl_name As enrUB = TRow(Of enrUB).glbl.NewBitBase()
        Public Shared contents As enrUB = TRow(Of enrUB).glbl.NewBitBase()
    End Class

    Public Class enmUB
        Inherits bitBASE
        Public Shared audit_parms_to_cboard As zaudit_parms_to_cboard = TRow(Of enmUB).glbl.Trbase(Of zaudit_parms_to_cboard).NewBitBase() : Public Class zaudit_parms_to_cboard : Inherits enmUB : End Class
        Public Shared cmdline_table As zcmdline_table = TRow(Of enmUB).glbl.Trbase(Of zcmdline_table).NewBitBase() : Public Class zcmdline_table : Inherits enmUB : End Class
        Public Shared export_project_code As zexport_project_code = TRow(Of enmUB).glbl.Trbase(Of zexport_project_code).NewBitBase() : Public Class zexport_project_code : Inherits enmUB : End Class
        Public Shared script_compiler_exe_filepath As zscript_compiler_exe_filepath = TRow(Of enmUB).glbl.Trbase(Of zscript_compiler_exe_filepath).NewBitBase() : Public Class zscript_compiler_exe_filepath : Inherits enmUB : End Class
        Public Shared unnamed_parm_path01 As zunnamed_parm_path01 = TRow(Of enmUB).glbl.Trbase(Of zunnamed_parm_path01).NewBitBase() : Public Class zunnamed_parm_path01 : Inherits enmUB : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Call Have.Connect()
            UserBowl = Have.prv_tblUserBowl
        End Function 'UserBowl

        Public Class rUserBowl
            Inherits TRow(Of enrUB)

            Public Property Contents As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enrUB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.v(enrUB.contents) = value
                End Set
            End Property 'Contents
        End Class 'rUserBowl

        Public Class sUserBowl
            Inherits Mx.TablePKEnum(Of enrUB, enmUB, rUserBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Function Show_UserAuditMessage(
                cmdexport_audit_bowlname As enmUB.zaudit_parms_to_cboard,
                ur_fn_user_input_dialog As enmWM.zUserInputDialog,
                ur_store_text As enmWC.zStoreText
                ) As Integer

                Show_UserAuditMessage = 0
                If Mx.HasText(Me.SelKey(cmdexport_audit_bowlname).Contents) Then
                    Show_UserAuditMessage = 1
                    Dim strAUDIT = Me.ToString(True)
                    Dim ins_msg = Have.WindowsMsgBoxEnv.Result(ur_fn_user_input_dialog,
                        ur_message:=strAUDIT,
                        ur_style:=MsgBoxStyle.OkCancel
                        )
                    If ins_msg = MsgBoxResult.Ok Then
                        Have.WindowsCboardEnv.Result(ur_store_text,
                            strAUDIT
                            )
                    End If 'ins_msg
                End If 'me
            End Function 'Show_UserAuditMessage

            <System.Diagnostics.DebuggerHidden()>
            Public Function Store_CommandLine_ParseTable(
                ur_cmdline_orig As enmSB.zcmdline_orig,
                ur_first_unnamed_parm_compiler_exe_bowlname As enmUB.zscript_compiler_exe_filepath,
                ur_second_unnamed_parm_path_vbns_bowlname As enmUB.zunnamed_parm_path01
                ) As enmUB.zcmdline_table

                Dim retSB = enmUB.cmdline_table
                Store_CommandLine_ParseTable = retSB
                Dim strCMD_LINE = Have.SessionBowl.SelKey(ur_cmdline_orig).Contents
                Dim arlCMD_RET = Mx.MxText.Cmdline_UB(Of enmUB, enrUB).CommandLine_UBParm(enrUB.bowl_name, enrUB.contents, strCMD_LINE, ur_first_unnamed_parm_compiler_exe_bowlname, ur_second_unnamed_parm_path_vbns_bowlname)
                Me.SelKey(retSB).Contents = Mx.qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(Mx.qs, Mx.qs & Mx.qs) & Mx.qs
                For Each trwPARM In arlCMD_RET.ttbUB_PARM
                    For Each trwBOWL In Me.Sel(enrUB.bowl_name, trwPARM.v(enrUB.bowl_name)).SelAll
                        If Mx.HasText(trwBOWL.Contents) = False Then
                            trwBOWL.Contents = trwPARM.v(enrUB.contents)
                        End If
                    Next trwBOWL
                Next trwPARM
            End Function 'Store_CommandLine_ParseTable
        End Class 'sUserBowl
    End Class 'UB
End Namespace 'Mx
