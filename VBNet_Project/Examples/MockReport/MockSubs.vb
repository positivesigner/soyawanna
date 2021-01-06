Namespace Mx2

    Public Class UserAction
        Public Shared Function CompileCode_Report_errhnd(ur_commandline_text As String, Optional ur_flag_msgbox As Boolean = True) As Mx.Strap
            Have.CmdLineText = ur_commandline_text
            Dim stpRET = Mx.Strapd() : CompileCode_Report_errhnd = stpRET : Dim objERR_LIST = New Mx.ErrListBase : Try

                stpRET.d("RED").dLine()
                Dim windowsfs_env = Have.WindowsFS
                Dim userbowl_cart = Have.UserBowl
                Dim appfolder_bowlname = enmUN.app_folder
                Dim mocktest_cart = Have.MockTest

                Dim found_test_count = Assistant.Load_MockTest_commandline_Files(mocktest_cart, userbowl_cart, appfolder_bowlname, windowsfs_env)
                Dim updated_rec_count = Assistant.Load_MockTest_action_Files(mocktest_cart, windowsfs_env)
                updated_rec_count = Assistant.Load_MockTest_expected_result_Files(mocktest_cart, windowsfs_env)
                Dim ran_test_count = Assistant.Gather_MockTest_run_Results(mocktest_cart, windowsfs_env)
                Dim compiled_report_count = Assistant.Compile_MockTest_results_Report(mocktest_cart, ran_test_count, stpRET)

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
        Public Shared strlit_MOCKTEST_FOLDER_STAR_DOT_MOCK_COMMANDLINE_TXT = "MockTest\*.mock_commandline.txt"
        Public Shared strlit_DOT_MOCK_EXPECT_TXT = ".mock_expect.txt"

        Public Shared Function Load_MockTest_commandline_Files(ur_mocktest_cart As Have.sMockTest, ur_userbowl_cart As Have.sUserBowl, ur_appfolder_bowlname As enmUN, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_ADDED = 0
            Dim flnAPP_FOLDER = Mx.FileNamed.d(ur_userbowl_cart.SelKey(ur_appfolder_bowlname).Contents)
            Dim flnSEARCH_MOCK_COMMANDLINE = flnAPP_FOLDER.gCopy.d(strlit_MOCKTEST_FOLDER_STAR_DOT_MOCK_COMMANDLINE_TXT)
            For Each strPATH In ur_windowsfs_env.GetFiles(flnSEARCH_MOCK_COMMANDLINE)
                Dim trwMOCK_TEST = ur_mocktest_cart.InsKey(strPATH)
                retREC_ADDED += 1
            Next strPATH

            Load_MockTest_commandline_Files = retREC_ADDED
        End Function 'Load_MockTest_commandline_Files

        Public Shared Function Load_MockTest_action_Files(ur_mocktest_cart As Have.sMockTest, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_UPDATED = 0
            For Each trwMOCK_TEST In ur_mocktest_cart.SelAll
                Dim flnTEST_FILE_PATH = Mx.FileNamed.d(trwMOCK_TEST.v(enmMT.mock_commandline_file))
                Dim flnTEST_CODE_PATH = flnTEST_FILE_PATH.gParentDir.d(Mx.FileNamed().d(flnTEST_FILE_PATH.FileGroup).FileGroup)
                trwMOCK_TEST.vt(enmMT.test_code_file, flnTEST_CODE_PATH)
                retREC_UPDATED += 1
            Next trwMOCK_TEST

            Load_MockTest_action_Files = retREC_UPDATED
        End Function 'Load_MockTest_action_Files

        Public Shared Function Load_MockTest_expected_result_Files(ur_mocktest_cart As Have.sMockTest, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_UPDATED = 0
            For Each trwMOCK_TEST In ur_mocktest_cart.SelAll
                Dim flnTEST_CODE_PATH = Mx.FileNamed.d(trwMOCK_TEST.v(enmMT.test_code_file))
                Dim flnRESULT_FILE_PATH = flnTEST_CODE_PATH.gCopy.dAppendEXT(strlit_DOT_MOCK_EXPECT_TXT)
                trwMOCK_TEST.vt(enmMT.mock_expect_file, flnRESULT_FILE_PATH)
                retREC_UPDATED += 1
            Next trwMOCK_TEST

            Load_MockTest_expected_result_Files = retREC_UPDATED
        End Function 'Load_MockTest_expected_result_Files

        Public Shared Function Gather_MockTest_run_Results(ur_mocktest_cart As Have.sMockTest, ur_windowsfs_env As Have.glblWindowsFS) As Integer
            Dim retREC_TESTED = 0
            For Each rowMOCK_TEST In ur_mocktest_cart.SelAll
                retREC_TESTED += 1
                Dim flnTEST_CODEFILE_PATH = Mx.FileNamed().d(rowMOCK_TEST.v(enmMT.test_code_file))
                'Dim strTEST_CALL_NAME = ur_windowsfs_env.ReadAllText(flnTEST_CODEFILE_PATH)
                Dim sdaCOMMANDLINE_TEST = ur_windowsfs_env.ReadAllLines(rowMOCK_TEST.v(enmMT.mock_commandline_file))
                'Dim sdaEXPECTED_RESULT = ur_windowsfs_env.ReadAllLines(rowMOCK_TEST.v(enmMT.mock_expect_file))
                Dim intTEST_PASS = 0
                Dim stpTEST_RESULT = Mx.Strapd
                For Each kvpTEST In sdaCOMMANDLINE_TEST.kvp
                    If Mx.HasText(kvpTEST.v) AndAlso
                      Mx.StartingWithText(kvpTEST.v, "-") = False Then
                        If kvpTEST.Indexb1 > 1 Then retREC_TESTED += 1
                        Dim strUSER_INPUT = kvpTEST.v
                        Dim strEXPECTED_RESULT = Mx.mt
                        For EXPCTR = kvpTEST.Indexb1 + 1 To kvpTEST.LastIndexb1
                            strEXPECTED_RESULT = sdaCOMMANDLINE_TEST.v_b1(EXPCTR)
                            If Left(strEXPECTED_RESULT, 2) = "- " Then
                                strEXPECTED_RESULT = Mid(strEXPECTED_RESULT, 3)
                                Exit For

                            ElseIf Left(strEXPECTED_RESULT, 1) <> "-" Then
                                strEXPECTED_RESULT = Mx.mt
                                Exit For

                            Else
                                strEXPECTED_RESULT = Mx.mt
                            End If
                        Next EXPCTR

                        Dim strFOUND_RESULT = Mx.Assistant.Roman_Numeral_Conversion_Result(strUSER_INPUT)

                        If strFOUND_RESULT = strEXPECTED_RESULT Then
                            intTEST_PASS += 1

                        Else
                            stpTEST_RESULT.dLine().dLine()
                            Call stpTEST_RESULT.dSprtr("`", flnTEST_CODEFILE_PATH.Name).d("`").dS("ln.").d(kvpTEST.Indexb1).dS("").dSprtr("`", kvpTEST.v).d("`").dS("expected").dS("").dSprtr("`", strEXPECTED_RESULT).d("`").d("; received").dSprtr("`", strFOUND_RESULT).d("`")
                            'System.IO.File.WriteAllText(rowMOCK_TEST.v(enmMT.mock_expect_file), strFOUND_RESULT, Mx.gUTF8_FileEncoding)
                        End If 'strFOUND_RESULT
                    End If
                Next kvpTEST

                rowMOCK_TEST.vt(enmMT.test_pass_count, intTEST_PASS.ToString)
                rowMOCK_TEST.vt(enmMT.test_result_text, stpTEST_RESULT.ToString)
            Next rowMOCK_TEST

            Gather_MockTest_run_Results = retREC_TESTED
        End Function 'Gather_MockTest_run_Results

        Public Shared Function Compile_MockTest_results_Report(ur_mocktest_cart As Have.sMockTest, ur_test_count As Integer, ur_report As Mx.Strap) As Integer
            Compile_MockTest_results_Report = 1
            Dim intTEST_PASS = 0
            For Each trwMOCK_TEST In ur_mocktest_cart.SelAll
                intTEST_PASS = CInt(trwMOCK_TEST.v(enmMT.test_pass_count))
            Next trwMOCK_TEST

            Dim intTEST_FAIL = ur_test_count - intTEST_PASS
            If intTEST_PASS = ur_test_count Then
                ur_report.Clear().dLine("GREEN").dLine()
            End If

            ur_report.dLine(intTEST_PASS.ToString).dS("Tests passed")
            ur_report.dLine(intTEST_FAIL.ToString).dS("Tests failed")
            ur_report.dLine()
            For Each rowMOCK_TEST In ur_mocktest_cart.SelAll
                If rowMOCK_TEST.v(enmMT.test_result_text) <> Mx.mt Then
                    ur_report.d(rowMOCK_TEST.v(enmMT.test_result_text))
                End If
            Next rowMOCK_TEST
        End Function 'Compile_MockTest_results_Report

    End Class 'Assistant


    Partial Public Class Have
        Private Shared envWindowsCboard As glblWindowsCboard
        Private Shared envWindowsMsgBox As glblWindowsMsgBox
        Private Shared envWindowsFS As glblWindowsFS
        Private Shared tblMockTest As sMockTest
        Private Shared tblUserBowl As sUserBowl

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.tblUserBowl Is Nothing Then
                Have.envWindowsCboard = New glblWindowsCboard
                Have.envWindowsMsgBox = New glblWindowsMsgBox
                Have.envWindowsFS = New glblWindowsFS
                Have.tblMockTest = New sMockTest
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
            Public Function GetFiles(ur_search_filespec As Mx.MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As String()
                Try
                    GetFiles = Mx.glbl.gWindowsFS.GetFiles(ur_search_filespec.gParentDir, ur_search_filespec.Name, ur_recurse_option)
                Catch ex As System.Exception
                    GetFiles = {}
                End Try
            End Function

            Public Function ReadAllText(ur_file_path As String) As String
                ReadAllText = Mx.mt
                If Mx.HasText(ur_file_path) Then
                    ReadAllText = Mx.glbl.gWindowsFS.ReadAllText(ur_file_path)
                End If
            End Function

            Public Function ReadAllLines(ur_file_path As String) As Mx.Sdata
                ReadAllLines = New Mx.Sdata().dList(System.IO.File.ReadAllLines(ur_file_path, Mx.gUTF8_FileEncoding))
            End Function
        End Class 'glblWindowsFS
    End Class 'Have

    Public Class enmMT
        Inherits Mx.bitBASE
        Public Shared mock_commandline_file As enmMT = Mx.TRow(Of enmMT).glbl.NewBitBase()
        Public Shared mock_expect_file As enmMT = Mx.TRow(Of enmMT).glbl.NewBitBase()
        Public Shared test_code_file As enmMT = Mx.TRow(Of enmMT).glbl.NewBitBase()
        Public Shared test_result_text As enmMT = Mx.TRow(Of enmMT).glbl.NewBitBase()
        Public Shared test_pass_count As enmMT = Mx.TRow(Of enmMT).glbl.NewBitBase()
    End Class 'enmMT

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function MockTest() As sMockTest
            Call Have.Connect()
            MockTest = Have.tblMockTest
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
        Public Shared app_folder As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
        Public Shared app_name As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
        Public Shared app_path As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmd_export_cmdline_audit As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_orig As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
        Public Shared cmdline_table As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
        Public Shared from_messagebox As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
        Public Shared report_output As enmUN = Mx.TRow(Of enmUN).glbl.NewBitBase()
    End Class

    Public Class enmUR
        Inherits Mx.bitBASE
        Public Shared Ok As enmUR = Mx.TRow(Of enmUR).glbl.NewBitBase()
        Public Shared Cancel As enmUR = Mx.TRow(Of enmUR).glbl.NewBitBase()
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
            Public Sub Cboard_CmdlineAudit()
                If Mx.HasText(Me.SelKey(enmUN.cmd_export_cmdline_audit).v(enmUB.contents)) Then
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
            Public Function InsFrom_Application() As rUserBowl
                Dim ret = New rUserBowl
                InsFrom_Application = ret
                Me.SelKey(enmUN.app_name).Contents = Mx.FileNamed().d(Mx.Class1.SourcePath).FileGroup
                Dim flnAPP_PATH = Mx.FileNamed().d(Mx.Class1.SourcePath)
                If Mx.AreEqual(flnAPP_PATH.gParentDir.Name, "UrFolder") Then
                    flnAPP_PATH = Mx.FileNamed().d(System.Reflection.Assembly.GetExecutingAssembly.Location)
                    If Mx.AreEqual(flnAPP_PATH.gParentDir.Name, "Debug") Then
                        flnAPP_PATH = flnAPP_PATH.gParentDir.gParentDir.gParentDir
                    End If
                End If

                Me.SelKey(enmUN.app_path).Contents = flnAPP_PATH
                Me.SelKey(enmUN.app_folder).Contents = flnAPP_PATH.gParentDir

                Dim arlCMD_RET = Mx.MxText.Cmdline_UB(Of enmUN, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, Have.CmdLineText)
                Me.SelKey(enmUN.cmdline_orig).Contents = Mx.qs & Have.CmdLineText.Replace(Mx.qs, Mx.qs & Mx.qs) & Mx.qs
                Me.SelKey(enmUN.cmdline_table).Contents = Mx.qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(Mx.qs, Mx.qs & Mx.qs) & Mx.qs
                For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                    Me.Ins(
                        New Have.rUserBowl().
                        vt(enmUB.bowl_name, rowFOUND.v(enmUB.bowl_name)).
                        vt(enmUB.contents, rowFOUND.v(enmUB.contents))
                        )
                Next
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sUserBowl
    End Class 'UB, UN
End Namespace 'Mx2
