Namespace Mx2
    Partial Public Class Mock
        Public Class NameValueCollection
            Public Keys As NameObjectCollectionBase.KeysCollection
            Public sdaKEY_NAME As Mx.Sdata
            Public sdaKEY_VAL As Mx.Sdata

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Keys = New NameObjectCollectionBase.KeysCollection
                Me.sdaKEY_NAME = New Mx.Sdata
                Me.sdaKEY_VAL = New Mx.Sdata
                Me.Keys.sdaKEY_VAL = Me.sdaKEY_VAL
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Item(name As String) As String
                Item = Mx.mt
                For Each kvpENTRY In Me.sdaKEY_NAME.kvp
                    If Mx.AreEqual(kvpENTRY.v, name) Then
                        Item = Me.sdaKEY_VAL.Item(kvpENTRY.Indexenm)
                    End If
                Next kvpENTRY
            End Function 'Item
        End Class 'NameValueCollection

        Partial Public Class NameObjectCollectionBase
            Public Class KeysCollection
                Public Count As Integer
                Public sdaKEY_VAL As Mx.Sdata

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New()
                    Me.Count = 0
                End Sub

                <System.Diagnostics.DebuggerHidden()>
                Public Function Item(index As Integer) As String
                    Item = Me.sdaKEY_VAL.Item(index)
                End Function
            End Class 'KeysCollection
        End Class 'NameObjectCollectionBase

        Public Class DataGridView
            Public CurrentCell As DataGridViewCell
            Public CurrentRow As DataGridViewRow
            Public Columns As DataGridViewColumnCollection
            Public Rows As DataGridViewRowCollection

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Columns = New DataGridViewColumnCollection
                Me.Rows = New DataGridViewRowCollection
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub [Select]()
            End Sub
        End Class 'DataGridView

        Public Class DataGridViewColumnCollection
            Public Count As Integer
            Public sdaCOLUMN_LIST As Mx.Sdata

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Count = 0
                Me.sdaCOLUMN_LIST = New Mx.Sdata
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Add() As Integer
                Add = 0
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Clear()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Item(index As Integer) As DataGridViewColumn
                Item = New DataGridViewColumn(index, Me.sdaCOLUMN_LIST.Item(index))
            End Function
        End Class 'DataGridViewRowCollection

        Public Class DataGridViewRowCollection
            Public Count As Integer

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Count = 0
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Add() As Integer
                Add = 0
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Clear()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Item(index As Integer) As DataGridViewRow
                Item = New DataGridViewRow
            End Function
        End Class 'DataGridViewRowCollection

        Public Class DataGridViewColumn
            Public Cells As DataGridViewCellCollection
            Public Index As Integer
            Public Name As String
            Public Visible As Boolean

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_index As Integer, ur_name As String)
                Me.Cells = New DataGridViewCellCollection
                Me.Index = ur_index
                Me.Name = ur_name
                Me.Visible = True
            End Sub
        End Class 'DataGridViewColumn

        Public Class DataGridViewRow
            Public Cells As DataGridViewCellCollection

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Cells = New DataGridViewCellCollection
            End Sub
        End Class 'DataGridViewRow

        Public Class DataGridViewCellCollection
            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Item(index As Integer) As DataGridViewCell
                Item = New DataGridViewCell
            End Function
        End Class 'DataGridViewCellCollection

        Public Class DataGridViewCell
            Public Style As DataGridViewCellStyle
            Public Value As String

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Style = New DataGridViewCellStyle
                Me.Value = Mx.mt
            End Sub
        End Class 'DataGridViewCell

        Public Class DataGridViewCellStyle
            Public BackColor As System.Drawing.Color

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.BackColor = System.Drawing.Color.Empty
            End Sub
        End Class 'DataGridViewCellStyle

        Public Class Button
            Public Enabled As Boolean

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Enabled = False
            End Sub
        End Class 'Button

        Public Class TextBox
            Public Enabled As Boolean
            Public Text As String

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Enabled = False
                Me.Text = Mx.mt
            End Sub
        End Class 'TextBox

        Public Class Form
            Public Text As String

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.Text = Mx.mt
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Close()
            End Sub
        End Class 'Form
    End Class 'Mock

    Public Class UserAction
        Public Shared Function CompileCode_Report_errhnd(ur_cur_assembly As System.Reflection.Assembly, ur_commandline_text As String, Optional ur_flag_msgbox As Boolean = True, Optional ur_curexe As String = "") As Mx.Strap
            Have.CmdLineText = ur_commandline_text
            Have.CurAssembly = ur_cur_assembly
            Have.CurExe = ur_curexe

            Dim stpRET = Mx.Strapd() : CompileCode_Report_errhnd = stpRET : Dim objERR_LIST = New Mx.ErrListBase : Try

                stpRET.d("RED").dLine()
                Dim windowsfs_env = Have.WindowsFSEnv
                Dim mocktest_cart = Have.MockTest
                Dim mockexpect_file_bowlname = enmMT.mock_expect_file
                Dim mocktest_file_bowlname = enmMT.mock_test_file
                Dim testcode_file_bowlname = enmMT.test_code_file
                Dim testpass_count_bowlname = enmMT.test_pass_count
                Dim testresult_text_bowlname = enmMT.test_result_text
                Dim userbowl_cart = Have.UserBowl
                Dim appfolder_bowlname = enmUN.app_folder

                Dim intTEST_COUNT = Assistant.Load_MockTest_test_Files(mocktest_cart, userbowl_cart, appfolder_bowlname, windowsfs_env)
                intTEST_COUNT = Assistant.Load_MockTest_action_Files(mocktest_cart, mocktest_file_bowlname, testcode_file_bowlname, windowsfs_env)
                intTEST_COUNT = Assistant.Load_MockTest_expected_result_Files(mocktest_cart, mockexpect_file_bowlname, testcode_file_bowlname, windowsfs_env)
                intTEST_COUNT = Assistant.Gather_MockTest_run_Results(mocktest_cart, mocktest_file_bowlname, mockexpect_file_bowlname, testcode_file_bowlname, testresult_text_bowlname, testpass_count_bowlname, userbowl_cart, appfolder_bowlname, windowsfs_env)
                Call Assistant.Compile_MockTest_results_Report(mocktest_cart, testresult_text_bowlname, testpass_count_bowlname, stpRET)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'CompileCode_Report_errhnd

        Private Class prv
            Public Shared Sub Handle_MsgBox(ur_errlst As Mx.ErrListBase, ur_strap As Mx.Strap, ur_flag_msgbox As Boolean)
                If ur_errlst.Found Then
                    ur_strap.Clear().d(ur_errlst.ToString)
                End If

                If ur_flag_msgbox AndAlso
                  ur_strap.HasText Then
                    Call Mx.glbl.gMsgBox.GetResult(ur_strap, MsgBoxStyle.OkOnly, Have.UserBowl.SelKey(enmUN.app_name).Contents)
                End If
            End Sub 'Handle_MsgBox
        End Class 'prv
    End Class 'UserAction

    Public Class Assistant
        Public Shared strlit_MOCKTEST_FOLDER_STAR_DOT_MOCK_TEST_TXT = "MockTest\*.mock_test.txt"
        Public Shared strlit_DOT_MOCK_EXPECT_TXT = ".mock_expect.txt"

        Public Shared Sub Compile_MockTest_results_Report(ur_mocktest_cart As Have.sMockTest, ur_testresult_text_bowlname As enmMT.ztest_result_text, ur_testpass_count_bowlname As enmMT.ztest_pass_count, ur_report As Mx.Strap)
            Dim intTEST_PASS = 0
            Dim intTEST_FAIL = 0
            For Each trwMOCK_TEST In ur_mocktest_cart.SelAll
                If trwMOCK_TEST.v(ur_testpass_count_bowlname) = "1" Then
                    intTEST_PASS += 1

                Else
                    intTEST_FAIL += 1
                End If
            Next trwMOCK_TEST

            If intTEST_PASS = ur_mocktest_cart.SelAll.Count Then
                ur_report.Clear().dLineNB("GREEN").dLine()
            End If

            ur_report.dLineNB(intTEST_PASS.ToString).dS("Tests passed")
            ur_report.dLine(intTEST_FAIL.ToString).dS("Tests failed")
            ur_report.dLine()
            For Each rowMOCK_TEST In ur_mocktest_cart.SelAll
                If rowMOCK_TEST.v(ur_testpass_count_bowlname) = "0" Then
                    ur_report.dLine(rowMOCK_TEST.v(ur_testresult_text_bowlname))
                    ur_report.dLine()
                End If
            Next rowMOCK_TEST
        End Sub 'Compile_MockTest_results_Report

        Public Shared Function Gather_MockTest_run_Results(ur_mocktest_cart As Have.sMockTest, ur_mocktest_file_bowlname As enmMT.zmock_test_file, ur_mockexpect_file_bowlname As enmMT.zmock_expect_file, ur_testcode_file_bowlname As enmMT.ztest_code_file, ur_testresult_text_bowlname As enmMT.ztest_result_text, ur_testpass_count_bowlname As enmMT.ztest_pass_count, ur_userbowl_cart As Have.sUserBowl, ur_appfolder_bowlname As enmUN.zapp_folder, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_TESTED = 0
            For Each rowMOCK_TEST In ur_mocktest_cart.SelAll
                retREC_TESTED += 1
                If Mx.HasText(rowMOCK_TEST.v(ur_testcode_file_bowlname)) = False Then
                    rowMOCK_TEST.vt(ur_testpass_count_bowlname, "0")
                    rowMOCK_TEST.vt(ur_testresult_text_bowlname, Mx.Strapd().d("Test Code File not found for").dS("").dSprtr("`", rowMOCK_TEST.v(ur_mocktest_file_bowlname)).d("`"))

                ElseIf Mx.HasText(rowMOCK_TEST.v(ur_mockexpect_file_bowlname)) = False Then
                    rowMOCK_TEST.vt(ur_testpass_count_bowlname, "0")
                    rowMOCK_TEST.vt(ur_testresult_text_bowlname, Mx.Strapd().d("Mock Expect File not found for").dS("").dSprtr("`", rowMOCK_TEST.v(ur_mocktest_file_bowlname)).d("`"))

                Else
                    Dim flnTEST_CODEFILE_PATH = Mx.FileNamed().d(rowMOCK_TEST.v(ur_testcode_file_bowlname))
                    Dim strTEST_CALL_NAME = ur_windowsfs_env.ReadAllText(flnTEST_CODEFILE_PATH)
                    Dim strTEST_PARMS = ur_windowsfs_env.ReadAllText(rowMOCK_TEST.v(ur_mocktest_file_bowlname))
                    Dim tblCOMMANDLINE_PARMS = New Mx.Objlist(Of Mx.Sdata)
                    For Each strLINE In strTEST_PARMS.Replace(vbCr, Mx.mt).Split(vbLf)
                        Dim sdaCOL = New Mx.Sdata().dList(strLINE.Split(vbTab))
                        tblCOMMANDLINE_PARMS.Add(sdaCOL)
                        'first column = sAppConfigSettings, sDevSettings, or strPARM_NAME
                        'second column = appconfig key name, customapp key name, parm name
                        'third column = appconfig key value, customapp key value
                    Next

                    Dim strEXPECTED_RESULT = ur_windowsfs_env.ReadAllText(rowMOCK_TEST.v(ur_mockexpect_file_bowlname))

                    Dim mockAppConfig = New Mock.NameValueCollection

                    Dim mockGrid = New Mock.DataGridView
                    Dim mockSaveAllBtn = New Mock.Button
                    Dim mockRowKeyTbox = New Mock.TextBox
                    Dim mockNewValTbox = New Mock.TextBox
                    Dim mockUserFileTbox = New Mock.TextBox
                    Dim mockForm = New Mock.Form
                    Dim mockUserInput = New Mx.dbUserInput(mockForm, mockGrid, mockSaveAllBtn, mockRowKeyTbox, mockNewValTbox, mockUserFileTbox)

                    Mx.Have.objAPP_CONFIG = mockAppConfig
                    Dim windows_inputbox_env = New Mx.Have.glblWindowsInputBox
                    Dim windows_msgbox_env = New Mx.Have.glblWindowsMsgBox
                    Dim windowsfs_env = New Mx.Have.glblWindowsFS
                    Dim appconfig_cart = New Mx.Have.sAppConfigSettings
                    Dim akv_settingval_bowlname = Mx.enmAKV.setting_value
                    For Each sdaCOL In tblCOMMANDLINE_PARMS
                        If Mx.AreEqual(sdaCOL.v_b1(1), "sAppConfigSettings") Then
                            If sdaCOL.Count > 1 Then
                                Dim trwADDED = appconfig_cart.InsKey(sdaCOL.v_b1(2))
                                If sdaCOL.Count > 2 Then
                                    trwADDED.v_b1(2) = sdaCOL.v_b1(3)
                                End If
                            End If
                        End If
                    Next sdaCOL

                    Dim customapp_parm_cart = New Mx.Have.sDevSettings
                    Dim cap_settingval_bowlname = Mx.enmDNV.setting_value
                    For Each sdaCOL In tblCOMMANDLINE_PARMS
                        If Mx.AreEqual(sdaCOL.v_b1(1), "sDevSettings") Then
                            If sdaCOL.Count > 1 Then
                                Dim trwADDED = customapp_parm_cart.InsKey(sdaCOL.v_b1(2))
                                If sdaCOL.Count > 2 Then
                                    trwADDED.v_b1(2) = sdaCOL.v_b1(3)
                                End If
                            End If
                        End If
                    Next sdaCOL

                    Dim strPARM_NAME = Mx.mt
                    For Each sdaCOL In tblCOMMANDLINE_PARMS
                        If Mx.AreEqual(sdaCOL.v_b1(1), "strPARM_NAME") Then
                            strPARM_NAME = sdaCOL.v_b1(2)
                        End If
                    Next

                    Dim mx_userbowl_cart = New Mx.Have.sUserBowl
                    Dim mx_appfolder_bowlname = Mx.enmUN.app_folder
                    mx_userbowl_cart.SelKey(mx_appfolder_bowlname).Contents = ur_userbowl_cart.SelKey(ur_appfolder_bowlname).Contents
                    'assign default values

                    Dim strFOUND_RESULT = Mx.mt
                    Dim sdaFOUND_RESULT = New Mx.Sdata
                    If Mx.AreEqual(strTEST_CALL_NAME, "Find_extra_AppConfig_Parms") Then
                        Dim sdaRESULT = Mx.Assistant.Find_extra_AppConfig_Parms(appconfig_cart, customapp_parm_cart, akv_settingval_bowlname)
                        For Each strENTRY In sdaRESULT
                            sdaFOUND_RESULT.d(strENTRY)
                        Next

                        strFOUND_RESULT = String.Join("; ", sdaFOUND_RESULT.ToArray)

                    ElseIf Mx.AreEqual(strTEST_CALL_NAME, "Search_for_AppConfig_Path") Then
                        strFOUND_RESULT = Mx.Assistant.Search_for_AppConfig_Path(appconfig_cart, akv_settingval_bowlname, strPARM_NAME, mx_userbowl_cart, mx_appfolder_bowlname, windowsfs_env)
                        Dim flnFOUND_RESULT = Mx.FileNamed().d(strFOUND_RESULT)
                        strFOUND_RESULT = Mx.FileNamed().d(flnFOUND_RESULT.gParentDir.Name).d(flnFOUND_RESULT.Name).ToString
                    End If 'strTEST_CALL_NAME

                    If strFOUND_RESULT = strEXPECTED_RESULT Then
                        rowMOCK_TEST.vt(ur_testpass_count_bowlname, "1")

                    Else
                        rowMOCK_TEST.vt(ur_testpass_count_bowlname, "0")
                        rowMOCK_TEST.vt(ur_testresult_text_bowlname, Mx.Strapd().dSprtr("`", flnTEST_CODEFILE_PATH.Name).d("`").dS("expected").dS("").dSprtr("`", strEXPECTED_RESULT).d("`").d("; received").dS().dSprtr("`", strFOUND_RESULT).d("`"))
                    End If 'strFOUND_RESULT
                End If
            Next rowMOCK_TEST

            Gather_MockTest_run_Results = retREC_TESTED
        End Function 'Gather_MockTest_run_Results

        Public Shared Function Load_MockTest_action_Files(ur_mocktest_cart As Have.sMockTest, ur_mocktest_file_bowlname As enmMT.zmock_test_file, ur_testcode_file_bowlname As enmMT.ztest_code_file, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_ADDED = 0
            For Each trwMOCK_TEST In ur_mocktest_cart.SelAll
                Dim flnTEST_FILE_PATH = Mx.FileNamed.d(trwMOCK_TEST.v(ur_mocktest_file_bowlname))
                Dim flnTEST_CODE_PATH = flnTEST_FILE_PATH.gParentDir.d(Mx.FileNamed().d(flnTEST_FILE_PATH.FileGroup).FileGroup)
                For Each strTEST_CODE_PATH In ur_windowsfs_env.GetFiles(flnTEST_CODE_PATH)
                    retREC_ADDED += 1
                    trwMOCK_TEST.vt(ur_testcode_file_bowlname, strTEST_CODE_PATH)
                    Exit For
                Next
            Next trwMOCK_TEST

            Load_MockTest_action_Files = retREC_ADDED
        End Function 'Load_MockTest_action_Files

        Public Shared Function Load_MockTest_expected_result_Files(ur_mocktest_cart As Have.sMockTest, ur_mockexpect_file_bowlname As enmMT.zmock_expect_file, ur_testcode_file_bowlname As enmMT.ztest_code_file, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_ADDED = 0
            For Each trwMOCK_TEST In ur_mocktest_cart.SelAll
                Dim flnTEST_CODE_PATH = Mx.FileNamed.d(trwMOCK_TEST.v(ur_testcode_file_bowlname))
                Dim flnRESULT_FILE_PATH = flnTEST_CODE_PATH.gCopy.dAppendEXT(strlit_DOT_MOCK_EXPECT_TXT)
                For Each strEXPECTED_RESULT_PATH In ur_windowsfs_env.GetFiles(flnRESULT_FILE_PATH)
                    retREC_ADDED += 1
                    trwMOCK_TEST.vt(ur_mockexpect_file_bowlname, strEXPECTED_RESULT_PATH)
                    Exit For
                Next
            Next trwMOCK_TEST

            Load_MockTest_expected_result_Files = retREC_ADDED
        End Function 'Load_MockTest_expected_result_Files

        Public Shared Function Load_MockTest_test_Files(ur_mocktest_cart As Have.sMockTest, ur_userbowl_cart As Have.sUserBowl, ur_appfolder_bowlname As enmUN, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_ADDED = 0
            Dim flnAPP_FOLDER = Mx.FileNamed.d(ur_userbowl_cart.SelKey(ur_appfolder_bowlname).Contents)
            Dim flnSEARCH_MOCK_COMMANDLINE = flnAPP_FOLDER.gParentDir.d(strlit_MOCKTEST_FOLDER_STAR_DOT_MOCK_TEST_TXT)
            For Each strPATH In ur_windowsfs_env.GetFiles(flnSEARCH_MOCK_COMMANDLINE)
                Dim trwMOCK_TEST = ur_mocktest_cart.InsKey(strPATH)
                retREC_ADDED += 1
            Next strPATH

            Load_MockTest_test_Files = retREC_ADDED
        End Function 'Load_MockTest_test_Files
    End Class 'Assistant


    Partial Public Class Have
        Private Shared prv_envWindowsCboard As glblWindowsCboard
        Private Shared prv_envWindowsMsgBox As glblWindowsMsgBox
        Private Shared prv_envWindowsFS As glblWindowsFS
        Private Shared prv_envWindowsShell As glblWindowsShell
        Private Shared prv_tblMockTest As sMockTest
        Private Shared prv_tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.prv_tblUserBowl Is Nothing Then
                Have.prv_envWindowsCboard = New glblWindowsCboard
                Have.prv_envWindowsMsgBox = New glblWindowsMsgBox
                Have.prv_envWindowsFS = New glblWindowsFS
                Have.prv_envWindowsShell = New glblWindowsShell
                Have.prv_tblMockTest = New sMockTest
                Have.prv_tblUserBowl = New sUserBowl
            End If 'prv_tblUserBowl
        End Sub 'Connect
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsCboardEnv() As glblWindowsCboard
            Call Have.Connect()
            WindowsCboardEnv = Have.prv_envWindowsCboard
        End Function

        Public Class glblWindowsCboard
            <System.Diagnostics.DebuggerHidden()>
            Public Function SetText(ur_text As String) As Integer
                SetText = Mx.glbl.gCboard.SetText(ur_text)
            End Function
        End Class 'glblWindowsCboard
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsFSEnv() As glblWindowsFS
            Call Have.Connect()
            WindowsFSEnv = Have.prv_envWindowsFS
        End Function

        Public Class glblWindowsFS
            Public Function GetFiles(ur_search_filespec As Mx.MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As String()
                Try
                    GetFiles = Mx.glbl.gWindowsFS.GetFiles(ur_search_filespec.gParentDir, ur_search_filespec.Name, ur_recurse_option)
                Catch ex As System.Exception
                    GetFiles = {}
                End Try
            End Function

            Public Function ReadAllText(ur_file_path As String) As String
                ReadAllText = Mx.glbl.gWindowsFS.ReadAllText(ur_file_path)
            End Function
        End Class 'glblWindowsFS
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsMsgBoxEnv() As glblWindowsMsgBox
            Call Have.Connect()
            WindowsMsgBoxEnv = Have.prv_envWindowsMsgBox
        End Function

        Public Class glblWindowsMsgBox
            <System.Diagnostics.DebuggerHidden()>
            Public Function GetResult(ur_message As String, Optional ur_style As MsgBoxStyle = MsgBoxStyle.OkOnly) As MsgBoxResult
                Dim strAPP_NAME = Have.UserBowl.SelKey(enmUN.app_name).Contents
                GetResult = Mx.glbl.gMsgBox.GetResult(ur_message, ur_style, strAPP_NAME)
            End Function
        End Class 'glblWindowsMsgBox
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsShellEnv() As glblWindowsShell
            Call Have.Connect()
            WindowsShellEnv = Have.prv_envWindowsShell
        End Function

        Public Class glblWindowsShell
            <System.Diagnostics.DebuggerHidden()>
            Public Sub Start_Windows_Program(ur_path As String, ur_parameters As String)
                Call Mx.glbl.gDiagnostics.Start_Windows_Program(ur_path, ur_parameters)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Sleep(ur_milliseconds As Integer)
                Call Mx.glbl.gThread.Sleep(ur_milliseconds)
            End Sub
        End Class 'glblWindowsShell
    End Class 'Have


    Public Class enmMT
        Inherits Mx.bitBASE
        Public Shared mock_test_file As zmock_test_file = Mx.TRow(Of enmMT).glbl.Trbase(Of zmock_test_file).NewBitBase() : Public Class zmock_test_file : Inherits enmMT : End Class
        Public Shared mock_expect_file As zmock_expect_file = Mx.TRow(Of enmMT).glbl.Trbase(Of zmock_expect_file).NewBitBase() : Public Class zmock_expect_file : Inherits enmMT : End Class
        Public Shared test_code_file As ztest_code_file = Mx.TRow(Of enmMT).glbl.Trbase(Of ztest_code_file).NewBitBase() : Public Class ztest_code_file : Inherits enmMT : End Class
        Public Shared test_result_text As ztest_result_text = Mx.TRow(Of enmMT).glbl.Trbase(Of ztest_result_text).NewBitBase() : Public Class ztest_result_text : Inherits enmMT : End Class
        Public Shared test_pass_count As ztest_pass_count = Mx.TRow(Of enmMT).glbl.Trbase(Of ztest_pass_count).NewBitBase() : Public Class ztest_pass_count : Inherits enmMT : End Class
    End Class 'enmMT

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function MockTest() As sMockTest
            Call Have.Connect()
            MockTest = Have.prv_tblMockTest
        End Function

        Public Class rMockTest
            Inherits Mx.TRow(Of enmMT)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmMT, ur_val As String) As rMockTest
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rMockTest

        Public Class sMockTest
            Inherits Mx.TablePKStr(Of enmMT, rMockTest)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                MyBase.New(1)
            End Sub
        End Class
    End Class 'enmMT

    Public Class enmUB
        Inherits Mx.bitBASE
        Public Shared bowl_name As enmUB = Mx.TRow(Of enmUB).glbl.NewBitBase()
        Public Shared contents As enmUB = Mx.TRow(Of enmUB).glbl.NewBitBase()
    End Class

    Public Class enmUN
        Inherits Mx.bitBASE
        Public Shared app_folder As zapp_folder = Mx.TRow(Of enmUN).glbl.Trbase(Of zapp_folder).NewBitBase() : Public Class zapp_folder : Inherits enmUN : End Class
        Public Shared app_name As zapp_name = Mx.TRow(Of enmUN).glbl.Trbase(Of zapp_name).NewBitBase() : Public Class zapp_name : Inherits enmUN : End Class
        Public Shared app_path As zapp_path = Mx.TRow(Of enmUN).glbl.Trbase(Of zapp_path).NewBitBase() : Public Class zapp_path : Inherits enmUN : End Class
        Public Shared audit_parms_to_cboard As zaudit_parms_to_cboard = Mx.TRow(Of enmUN).glbl.Trbase(Of zaudit_parms_to_cboard).NewBitBase() : Public Class zaudit_parms_to_cboard : Inherits enmUN : End Class
        Public Shared cmdline_curexe As zcmdline_curexe = Mx.TRow(Of enmUN).glbl.Trbase(Of zcmdline_curexe).NewBitBase() : Public Class zcmdline_curexe : Inherits enmUN : End Class
        Public Shared cmdline_orig As zcmdline_orig = Mx.TRow(Of enmUN).glbl.Trbase(Of zcmdline_orig).NewBitBase() : Public Class zcmdline_orig : Inherits enmUN : End Class
        Public Shared cmdline_table As zcmdline_table = Mx.TRow(Of enmUN).glbl.Trbase(Of zcmdline_table).NewBitBase() : Public Class zcmdline_table : Inherits enmUN : End Class
        Public Shared compiler_exe As zcompiler_exe = Mx.TRow(Of enmUN).glbl.Trbase(Of zcompiler_exe).NewBitBase() : Public Class zcompiler_exe : Inherits enmUN : End Class
        Public Shared path_unassigned As zpath_unassigned = Mx.TRow(Of enmUN).glbl.Trbase(Of zpath_unassigned).NewBitBase() : Public Class zpath_unassigned : Inherits enmUN : End Class
    End Class

    Public Class enmUR
        Inherits Mx.bitBASE
        Public Shared Ok As enmUR = Mx.TRow(Of enmUR).glbl.NewBitBase()
        Public Shared Cancel As enmUR = Mx.TRow(Of enmUR).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        Public Shared FirstConnect As Object
        Public Shared CmdLineText As String
        Public Shared CurAssembly As System.Reflection.Assembly
        Public Shared CurExe As String

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function UserBowl() As sUserBowl
            Dim bolFIRST_INIT = (Have.FirstConnect Is Nothing)
            Call Have.Connect()
            UserBowl = Have.prv_tblUserBowl
            If bolFIRST_INIT Then
                Have.FirstConnect = "Done"
                Dim apppath_bowlname = enmUN.app_path
                Dim appname_bowlname = enmUN.app_name
                Dim appfolder_bowlname = enmUN.app_folder
                Dim curexe_bowlname = enmUN.cmdline_curexe
                Dim cmdline_orig_bowlname = enmUN.cmdline_orig
                Dim cmdline_table_bowlname = enmUN.cmdline_table
                Dim cmdexport_audit_bowlname = enmUN.audit_parms_to_cboard
                Dim compiler_exe_bowlname = enmUN.compiler_exe
                Dim path_unassigned_bowlname = enmUN.path_unassigned

                Call Have.prv_tblUserBowl.UpdFrom_Application(Have.CurAssembly, Have.CurExe, apppath_bowlname, appname_bowlname, appfolder_bowlname, curexe_bowlname)
                Call Have.prv_tblUserBowl.UpdFrom_CommandLine(Have.CmdLineText, compiler_exe_bowlname, path_unassigned_bowlname, cmdline_orig_bowlname, cmdline_table_bowlname)
                'Have.prv_tblUserBowl.SelKey(cmdexport_audit_bowlname).Contents = "1"
                Call Have.prv_tblUserBowl.UpdCboard_FromAudit(cmdexport_audit_bowlname)
            End If
        End Function

        Public Class rUserBowl
            Inherits Mx.TRow(Of enmUB)

            <System.Diagnostics.DebuggerHidden()>
            Public Function v_is(ur_enm As enmUB, ur_cmp As enmUR) As Boolean
                v_is = Mx.AreEqual(Me.v(ur_enm), ur_cmp.name)
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
            Public Sub UpdCboard_FromAudit(cmdexport_audit_bowlname As enmUN.zaudit_parms_to_cboard)
                If Mx.HasText(Me.SelKey(cmdexport_audit_bowlname).v(enmUB.contents)) Then
                    Dim strAUDIT = Me.ToString(True)
                    Dim ins_msg = Have.WindowsMsgBoxEnv.GetResult(
                        ur_message:=Me.SelKey(enmUN.app_name).v(enmUB.contents),
                        ur_style:=MsgBoxStyle.OkCancel
                        )
                    If ins_msg = MsgBoxResult.Ok Then
                        Have.WindowsCboardEnv.SetText(
                            strAUDIT
                            )
                    End If
                End If
            End Sub 'UpdCboard_FromAudit

            <System.Diagnostics.DebuggerHidden()>
            Public Function UpdFrom_Application(ur_cur_assembly As System.Reflection.Assembly, ur_curdir As String, ur_apppath_bowlname As enmUN.zapp_path, ur_appname_bowlname As enmUN.zapp_name, ur_appfolder_bowlname As enmUN.zapp_folder, ur_curexe_bowlname As enmUN.zcmdline_curexe) As sUserBowl
                UpdFrom_Application = Me
                Dim strCUR_EXE_PATH = ur_cur_assembly.Location
                If Mx.HasText(strCUR_EXE_PATH) = False Then
                    strCUR_EXE_PATH = ur_curdir
                End If

                Dim flnAPP_PATH = Mx.FileNamed().d(strCUR_EXE_PATH.Replace("\bin\Debug", Mx.mt))
                Me.SelKey(ur_apppath_bowlname).Contents = flnAPP_PATH
                Me.SelKey(ur_appname_bowlname).Contents = flnAPP_PATH.FileGroup
                Me.SelKey(ur_appfolder_bowlname).Contents = flnAPP_PATH.ParentDir
                Me.SelKey(ur_curexe_bowlname).Contents = strCUR_EXE_PATH
            End Function 'UpdFrom_Application

            <System.Diagnostics.DebuggerHidden()>
            Public Function UpdFrom_CommandLine(ur_cmdline_text As String, ur_compiler_exe_bowlname As enmUN.zcompiler_exe, ur_path_unassigned_bowlname As enmUN.zpath_unassigned, ur_cmdline_orig_bowlname As enmUN.zcmdline_orig, ur_cmdline_table_bowlname As enmUN.zcmdline_table) As sUserBowl
                UpdFrom_CommandLine = Me
                Dim arlCMD_RET = Mx.MxText.Cmdline_UB(Of enmUN, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, ur_cmdline_text, ur_compiler_exe_bowlname, ur_path_unassigned_bowlname)
                Me.SelKey(ur_cmdline_orig_bowlname).Contents = Mx.qs & ur_cmdline_text.Replace(Mx.qs, Mx.qs & Mx.qs) & Mx.qs
                Me.SelKey(ur_cmdline_table_bowlname).Contents = Mx.qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(Mx.qs, Mx.qs & Mx.qs) & Mx.qs
                For Each trwPARM In arlCMD_RET.ttbUB_PARM
                    For Each trwBOWL In Me.Sel(enmUB.bowl_name, trwPARM.v(enmUB.bowl_name)).SelAll
                        If Mx.HasText(trwBOWL.Contents) = False Then
                            trwBOWL.Contents = trwPARM.v(enmUB.contents)
                        End If
                    Next trwBOWL
                Next trwPARM
            End Function 'UpdFrom_CommandLine

            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCboard(ur_hdr As Boolean) As Integer
                ToCboard = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sUserBowl
    End Class 'UB, UN
End Namespace 'Mx2
