Option Strict On

Namespace Mx
    Public Class UserAction
        Public Shared Function Add_New_Row_errhnd(ur_form As dbUserInput, Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Add_New_Row_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim user_grid = Have.SettingsGrid(ur_form.grdSetting)
                Dim rowkey_tbox = ur_form.txtRowKey
                Dim newval_tbox = ur_form.txtNewVal
                Dim savechanges_btn = ur_form.btnSaveAllChanges
                Dim windows_inputbox_env = Have.WindowsInputBox
                Dim windows_msgbox_env = Have.WindowsMsgBox
                Dim devsettings_cart = Have.DevSettings
                Dim settingval_bowlname = enmDNV.setting_value
                Dim colvalue_gridcol = enmSG.colValue
                Dim new_parm_name = Assistant.Ask_User_for_NewParmName(windows_inputbox_env)
                new_parm_name = Assistant.Error_User_for_ParmName_Validation(devsettings_cart, new_parm_name, windows_msgbox_env)
                Dim new_parm_row = Assistant.Add_Parm_to_Cart(devsettings_cart, new_parm_name)
                Dim update_button_count = Assistant.Update_SaveAllBtn_enabled(savechanges_btn, new_parm_row)
                Dim update_tbox_count = Assistant.Update_Tbox_disabled(newval_tbox)
                Dim new_grid_row = Assistant.Add_Row_to_Grid(user_grid, colvalue_gridcol, new_parm_row, settingval_bowlname)
                Dim navigated_row_count = Assistant.Navigate_Grid_to_Row(new_grid_row)
                Dim overwrite_row_count = Assistant.Display_RowName(rowkey_tbox, new_parm_row)
                overwrite_row_count = Assistant.Display_RowValue(newval_tbox, new_parm_row, settingval_bowlname)
                update_tbox_count = Assistant.Update_Tbox_enabled(newval_tbox)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Add_New_Row_errhnd

        Public Shared Function Audit_Paths_errhnd(ur_form As dbUserInput, Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Audit_Paths_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim user_grid = Have.SettingsGrid(ur_form.grdSetting)
                Dim windows_msgbox_env = Have.WindowsFS
                Dim colvalue_gridcol = enmSG.colValue
                Dim colfound_gridcol = enmSG.colFound
                Dim tested_path_count = Assistant.Display_AuditColumn(user_grid, colvalue_gridcol, colfound_gridcol, windows_msgbox_env)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Audit_Paths_errhnd

        Public Shared Function Delete_Existing_Row_errhnd(ur_form As dbUserInput, Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Delete_Existing_Row_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim user_grid = Have.SettingsGrid(ur_form.grdSetting)
                Dim savechanges_btn = ur_form.btnSaveAllChanges
                Dim rowkey_tbox = ur_form.txtRowKey
                Dim newval_tbox = ur_form.txtNewVal
                Dim windows_msgbox_env = Have.WindowsMsgBox
                Dim devsettings_cart = Have.DevSettings
                Dim settingval_bowlname = enmDNV.setting_value
                Dim colsetting_gridcol = enmSG.colSetting
                Dim colvalue_gridcol = enmSG.colValue
                Dim found_parm_row = Assistant.Get_Parm_from_Grid_Row(user_grid, colsetting_gridcol, devsettings_cart)
                found_parm_row = Assistant.Ask_User_for_DeleteVerify(found_parm_row, windows_msgbox_env)
                Dim update_button_count = Assistant.Update_SaveAllBtn_enabled(savechanges_btn, found_parm_row)
                Dim remove_row_count = Assistant.Remove_Parm_from_Cart(devsettings_cart, found_parm_row)
                If remove_row_count = 1 Then
                    Dim reset_row_count = Assistant.Reset_Rows_in_Grid(user_grid, colvalue_gridcol, rowkey_tbox, newval_tbox, devsettings_cart, settingval_bowlname)
                End If

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Delete_Existing_Row_errhnd

        Public Shared Function Change_current_Row_Value_errhnd(ur_form As dbUserInput, Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Change_current_Row_Value_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim user_grid = Have.SettingsGrid(ur_form.grdSetting)
                Dim savechanges_btn = ur_form.btnSaveAllChanges
                Dim newval_tbox = ur_form.txtNewVal
                Dim devsettings_cart = Have.DevSettings
                Dim settingval_bowlname = enmDNV.setting_value
                Dim colsetting_gridcol = enmSG.colSetting
                Dim colvalue_gridcol = enmSG.colValue
                Dim found_parm_row = Assistant.Get_Parm_from_Grid_Row(user_grid, enmSG.colSetting, devsettings_cart)
                found_parm_row = Assistant.Ask_NewValTbox_enabled(newval_tbox, found_parm_row)
                Dim update_button_count = Assistant.Update_SaveAllBtn_enabled(savechanges_btn, found_parm_row)
                Dim update_row_count = Assistant.Update_Parm_Value(found_parm_row, settingval_bowlname, newval_tbox)
                update_row_count = Assistant.Update_GridRow_Value(user_grid, colvalue_gridcol, found_parm_row, settingval_bowlname)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Change_current_Row_Value_errhnd

        Public Shared Function Open_HelpFile_errhnd(Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Open_HelpFile_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim windowsmsgbox_env = Have.WindowsMsgBox
                Dim windowsfs_env = Have.WindowsFS
                Dim appexternal_cart = Have.AppExternal
                Dim userbowl_cart = Have.UserBowl
                Dim appfolder_bowlname = enmUN.app_folder
                Dim helpfile_path_bowlname = enmUN.helpfile_path
                Dim app_opened_count = Assistant.Open_External_App(appexternal_cart, userbowl_cart, helpfile_path_bowlname, windowsfs_env, windowsmsgbox_env)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Open_HelpFile

        Public Shared Function Open_Main_Form_errhnd(ur_form As dbUserInput, ur_appconfig As dbConfigInput, ur_cur_assembly As System.Reflection.Assembly, ur_commandline_text As String, Optional ur_flag_msgbox As Boolean = True) As Strap
            Have.CurAssembly = ur_cur_assembly
            Have.CmdLineText = ur_commandline_text
            Have.objAPP_CONFIG = ur_appconfig

            Dim stpRET = Strapd() : Open_Main_Form_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim user_grid = Have.SettingsGrid(ur_form.grdSetting)
                Dim saveall_btn = ur_form.btnSaveAllChanges
                Dim rowkey_tbox = ur_form.txtRowKey
                Dim newval_tbox = ur_form.txtNewVal
                Dim file_path_ref_tbox = ur_form.txtUserFile
                Dim windows_envvar_env = Have.WindowsEnvVar
                Dim windows_msgbox_env = Have.WindowsMsgBox
                Dim windowsfs_env = Have.WindowsFS
                Dim appconfig_cart = Have.AppConfigSettings
                Dim akv_settingval_bowlname = enmAKV.setting_value
                Dim devsettings_cart = Have.DevSettings
                Dim cap_settingval_bowlname = enmDNV.setting_value
                Dim userbowl_cart = Have.UserBowl
                Dim appfolder_bowlname = enmUN.app_folder
                Dim appname_bowlname = enmUN.app_name
                Dim helpfile_path_bowlname = enmUN.helpfile_path
                Dim devsettings_path_bowlname = enmUN.devsettings_path
                Dim colvalue_gridcol = enmSG.colValue
                Dim updated_row_count = Assistant.Store_DevSettings_Path(windows_envvar_env, userbowl_cart, devsettings_path_bowlname)
                updated_row_count = Assistant.Store_HelpFile_Path(userbowl_cart, helpfile_path_bowlname, appfolder_bowlname, appname_bowlname)
                Dim updated_tbox_count = Assistant.Show_DevSettings_Path(file_path_ref_tbox, userbowl_cart, devsettings_path_bowlname)
                Dim reset_row_count = prv.Load_DevSettings_and_Reset_Rows(user_grid, colvalue_gridcol, saveall_btn, rowkey_tbox, newval_tbox, windowsfs_env, appconfig_cart, akv_settingval_bowlname, devsettings_cart, cap_settingval_bowlname, userbowl_cart, devsettings_path_bowlname, appfolder_bowlname, windows_msgbox_env)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Open_Main_Form_errhnd

        Public Shared Function Overwrite_displayed_Row_Value_errhnd(ur_form As dbUserInput, Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Overwrite_displayed_Row_Value_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim user_grid = Have.SettingsGrid(ur_form.grdSetting)
                Dim rowkey_tbox = ur_form.txtRowKey
                Dim newval_tbox = ur_form.txtNewVal
                Dim devsettings_cart = Have.DevSettings
                Dim settingval_bowlname = enmDNV.setting_value
                Dim colsetting_gridcol = enmSG.colSetting
                Dim found_parm_row = Assistant.Get_Parm_from_Grid_Row(user_grid, colsetting_gridcol, devsettings_cart)
                Dim overwrite_row_count = Assistant.Display_RowName(rowkey_tbox, found_parm_row)
                found_parm_row = Assistant.Ask_NewValTbox_enabled(newval_tbox, found_parm_row)
                If found_parm_row IsNot Nothing Then
                    Dim update_tbox_count = Assistant.Update_Tbox_disabled(newval_tbox)
                    overwrite_row_count = Assistant.Display_RowValue(newval_tbox, found_parm_row, settingval_bowlname)
                    update_tbox_count = Assistant.Update_Tbox_enabled(newval_tbox)
                End If

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Overwrite_displayed_Row_Value_errhnd

        Public Shared Function Refresh_Grid_errhnd(ur_form As dbUserInput, Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Refresh_Grid_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim user_grid = Have.SettingsGrid(ur_form.grdSetting)
                Dim saveall_btn = ur_form.btnSaveAllChanges
                Dim rowkey_tbox = ur_form.txtRowKey
                Dim newval_tbox = ur_form.txtNewVal
                Dim windowsfs_env = Have.WindowsFS
                Dim windows_msgbox_env = Have.WindowsMsgBox
                Dim appconfig_cart = Have.AppConfigSettings
                Dim akv_settingval_bowlname = enmAKV.setting_value
                Dim devsettings_cart = Have.DevSettings
                Dim cap_settingval_bowlname = enmDNV.setting_value
                Dim userbowl_cart = Have.UserBowl
                Dim appfolder_bowlname = enmUN.app_folder
                Dim devsettings_path_bowlname = enmUN.devsettings_path
                Dim colvalue_gridcol = enmSG.colValue
                Dim reset_row_count = prv.Load_DevSettings_and_Reset_Rows(user_grid, colvalue_gridcol, saveall_btn, rowkey_tbox, newval_tbox, windowsfs_env, appconfig_cart, akv_settingval_bowlname, devsettings_cart, cap_settingval_bowlname, userbowl_cart, devsettings_path_bowlname, appfolder_bowlname, windows_msgbox_env)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'Refresh_Grid_errhnd

        Public Shared Function Save_To_File_errhnd(ur_form As dbUserInput, Optional ur_flag_msgbox As Boolean = True) As Strap
            Dim stpRET = Strapd() : Save_To_File_errhnd = stpRET : Dim objERR_LIST = New ErrListBase : Try

                Dim saveall_btn = ur_form.btnSaveAllChanges
                Dim devsettings_cart = Have.DevSettings
                Dim userbowl_cart = Have.UserBowl
                Dim devsettings_path_bowlname = enmUN.devsettings_path
                Dim written_file_count = Assistant.Update_DevSettings_File(devsettings_cart, userbowl_cart, devsettings_path_bowlname)
                Dim update_btn_count = Assistant.Update_Button_disabled(saveall_btn)

            Catch ex As System.Exception : Call objERR_LIST.dError_Stack(ex) : End Try : prv.Handle_MsgBox(objERR_LIST, stpRET, ur_flag_msgbox)
        End Function 'SaveToFile_errhnd

        Private Class prv
            Public Shared Sub Handle_MsgBox(ur_errlst As ErrListBase, ur_strap As Strap, ur_flag_msgbox As Boolean)
                If ur_errlst.Found Then
                    ur_strap.Clear().d(ur_errlst.ToString)
                End If

                If ur_flag_msgbox AndAlso
                  ur_strap.HasText Then
                    Call glbl.gMsgBox.GetResult(ur_strap, MsgBoxStyle.OkOnly, Have.UserBowl.SelKey(enmUN.app_name).Contents)
                End If
            End Sub 'Handle_MsgBox

            Public Shared Function Load_DevSettings_and_Reset_Rows(ur_grid As Have.uSettingsGrid, ur_colvalue_gridcol As enmSG.zcolValue, ur_saveall_btn As dbUserInput.componentButton, ur_rowkey_tbox As dbUserInput.ztxtRowKey, ur_newval_tbox As dbUserInput.ztxtNewVal, ur_windowsfs_env As Have.glblWindowsFS, ur_appconfig_cart As Have.sAppConfigSettings, ur_akv_settingval_bowlname As enmAKV.zsetting_value, ur_devsettings_cart As Have.sDevSettings, ur_cap_settingval_bowlname As enmDNV.zsetting_value, ur_userbowl_cart As Have.sUserBowl, ur_devsettings_path_bowlname As enmUN.zdevsettings_path, ur_appfolder_bowlname As enmUN.zapp_folder, ur_windows_msgbox_env As Have.glblWindowsMsgBox) As Integer
                Dim update_btn_count = Assistant.Update_Button_disabled(ur_saveall_btn)
                Dim found_row_count = Assistant.Load_DevSettings_Table(ur_devsettings_cart, ur_userbowl_cart, ur_devsettings_path_bowlname)
                Dim sdaMISSING_PARM = Assistant.Find_extra_AppConfig_Parms(ur_appconfig_cart, ur_devsettings_cart, ur_akv_settingval_bowlname)
                Dim added_parm_defaults = Assistant.Add_Defaults_for_missing_DevSettings(sdaMISSING_PARM, ur_devsettings_cart, ur_cap_settingval_bowlname, ur_appconfig_cart, ur_akv_settingval_bowlname, ur_userbowl_cart, ur_appfolder_bowlname, ur_windowsfs_env)
                If added_parm_defaults.Count > 0 Then
                    update_btn_count = Assistant.Update_Button_enabled(ur_saveall_btn)
                    Dim stpNOTICE_MSG = Strapd()
                    If added_parm_defaults.Count = 1 Then
                        stpNOTICE_MSG.d("There was").dS(added_parm_defaults.ToString).dS("default search setting")
                    Else
                        stpNOTICE_MSG.d("There were").dS(added_parm_defaults.ToString).dS("default search settings")
                    End If

                    ur_windows_msgbox_env.GetResult(stpNOTICE_MSG.dS("found. You must click the Save Changes button so they can be used by other applications.").dLine().dList("; ", added_parm_defaults.ToArray))
                End If

                Dim reset_row_count = Assistant.Reset_Rows_in_Grid(ur_grid, ur_colvalue_gridcol, ur_rowkey_tbox, ur_newval_tbox, ur_devsettings_cart, ur_cap_settingval_bowlname)
                Load_DevSettings_and_Reset_Rows = reset_row_count
            End Function 'Load_DevSettings_and_Reset_Rows
        End Class 'prv
    End Class 'User

    Public Class Assistant
        Public Shared Function Add_Defaults_for_missing_DevSettings(ur_missing_parm_list As Sdata, ur_devsettings_cart As Have.sDevSettings, ur_cap_settingval_bowlname As enmDNV.zsetting_value, ur_appconfig_cart As Have.sAppConfigSettings, ur_akv_settingval_bowlname As enmAKV.zsetting_value, ur_userbowl_cart As Have.sUserBowl, ur_appfolder_bowlname As enmUN.zapp_folder, ur_windowsfs_env As Have.glblWindowsFS) As Sdata
            Dim retPARM_NAME = New Sdata
            Add_Defaults_for_missing_DevSettings = retPARM_NAME
            For Each strPARM_NAME In ur_missing_parm_list
                Dim strPARM_PATH = Assistant.Search_for_AppConfig_Path(ur_appconfig_cart, ur_akv_settingval_bowlname, strPARM_NAME, ur_userbowl_cart, ur_appfolder_bowlname, ur_windowsfs_env)
                If HasText(strPARM_PATH) Then
                    retPARM_NAME.Add(strPARM_NAME)
                    Dim new_parm_row = Assistant.Add_Parm_to_Cart(ur_devsettings_cart, strPARM_NAME)
                    Dim update_row_count = Assistant.Store_Parm_Value(new_parm_row, ur_cap_settingval_bowlname, strPARM_PATH)
                End If
            Next strPARM_NAME
        End Function 'Add_Defaults_for_missing_DevSettings

        Public Shared Function Add_Parm_to_Cart(ur_devsettings_cart As Have.sDevSettings, ur_new_parm As String) As Have.rDevSettings
            Add_Parm_to_Cart = Nothing
            If HasText(ur_new_parm) Then
                Dim trwNEW_PARM = ur_devsettings_cart.InsKey(ur_new_parm)
                Add_Parm_to_Cart = trwNEW_PARM
            End If 'ur_new_parm
        End Function 'Add_Parm_to_Cart

        Public Shared Function Add_Row_to_Grid(ur_grid As Have.uSettingsGrid, ur_colvalue_gridcol As enmSG.zcolValue, ur_parm_row As Have.rDevSettings, ur_settingval_bowlname As enmDNV.zsetting_value) As Have.rSettingsGrid
            Add_Row_to_Grid = Nothing
            If ur_parm_row IsNot Nothing Then
                Dim objNEW_ROW = ur_grid.Add_Row(ur_parm_row.v(ur_parm_row.PK))
                Add_Row_to_Grid = objNEW_ROW
                ur_grid.Update_Row(objNEW_ROW, ur_colvalue_gridcol, ur_parm_row.v(ur_settingval_bowlname))
            End If 'ur_parm_row
        End Function 'Add_Row_to_Grid

        Public Shared Function Ask_NewValTbox_enabled(ur_newval_tbox As dbUserInput.ztxtNewVal, ur_parm_row As Have.rDevSettings) As Have.rDevSettings
            Ask_NewValTbox_enabled = Nothing
            If ur_newval_tbox IsNot Nothing Then
                If ur_newval_tbox.Enabled Then
                    Ask_NewValTbox_enabled = ur_parm_row
                End If
            End If 'ur_newval_tbox
        End Function 'Ask_NewValTbox_enabled

        Public Shared Function Ask_User_for_DeleteVerify(ur_parm_row As Have.rDevSettings, ur_windows_msgbox_env As Have.glblWindowsMsgBox) As Have.rDevSettings
            Ask_User_for_DeleteVerify = Nothing
            If ur_parm_row IsNot Nothing Then
                Dim enrUSER_INPUT = ur_windows_msgbox_env.GetResult("Do you want to delete the ", MsgBoxStyle.YesNoCancel)
                If enrUSER_INPUT = MsgBoxResult.Yes Then
                    Ask_User_for_DeleteVerify = ur_parm_row
                End If
            End If 'ur_parm_row
        End Function 'Ask_User_for_DeleteVerify

        Public Shared Function Ask_User_for_NewParmName(ur_windows_inputbox_env As Have.glblWindowsInputBox) As String
            Dim strNEW_ENTRY = ur_windows_inputbox_env.GetResult("New Entry Name", mt)
            Ask_User_for_NewParmName = strNEW_ENTRY
        End Function 'Ask_User_for_NewParmName

        Public Shared Function Display_AuditColumn(ur_user_grid As Have.uSettingsGrid, ur_colvalue_gridcol As enmSG.zcolValue, ur_colfound_gridcol As enmSG.zcolFound, ur_windows_msgbox_env As Have.glblWindowsFS) As Integer
            Dim intTEST_COUNT = 0
            ur_user_grid.Column_Visible(ur_colfound_gridcol) = True

            For Each curROW In ur_user_grid.SelAll
                curROW.Value(ur_colfound_gridcol) = mt
                curROW.BackColor(ur_colfound_gridcol) = System.Drawing.Color.Empty
            Next curROW

            For Each curROW In ur_user_grid.SelAll
                If ur_windows_msgbox_env.GetFiles(curROW.Value(ur_colvalue_gridcol)).Count > 0 Then
                    curROW.Value(ur_colfound_gridcol) = "found"
                    curROW.BackColor(ur_colfound_gridcol) = System.Drawing.Color.LawnGreen
                Else
                    curROW.Value(ur_colfound_gridcol) = "missing"
                    curROW.BackColor(ur_colfound_gridcol) = System.Drawing.Color.Khaki
                End If
            Next curROW

            Display_AuditColumn = intTEST_COUNT
        End Function 'Display_AuditColumn

        Public Shared Function Display_RowName(ur_dest_tbox As dbUserInput.componentTextBox, ur_parm_row As Have.rDevSettings) As Integer
            Display_RowName = 0
            If ur_parm_row IsNot Nothing Then
                Display_RowName = 1
                ur_dest_tbox.Text = ur_parm_row.v(ur_parm_row.PK)
            End If 'ur_new_parm
        End Function 'Display_RowName

        Public Shared Function Display_RowValue(ur_dest_tbox As dbUserInput.componentTextBox, ur_parm_row As Have.rDevSettings, ur_source_col_bowlname As enmDNV.zsetting_value) As Integer
            Display_RowValue = 0
            If ur_parm_row IsNot Nothing Then
                Display_RowValue = 1
                ur_dest_tbox.Text = ur_parm_row.v(ur_source_col_bowlname)
            End If 'ur_new_parm
        End Function 'Display_RowValue

        Public Shared Function Error_User_for_FilePath_Missing(ur_file_path As String, ur_err_msg As String, ur_windows_msgbox_env As Have.glblWindowsMsgBox) As Integer
            Error_User_for_FilePath_Missing = 0
            'A blank new parameter name just means they canceled out of creating a new parameter name
            If HasText(ur_file_path) AndAlso
              HasText(ur_err_msg) Then
                Dim enrUSER_INPUT = ur_windows_msgbox_env.GetResult(Strapd().d(ur_err_msg).d(":").dS("").dSprtr("[", ur_file_path).d("]"), MsgBoxStyle.Exclamation)
                Error_User_for_FilePath_Missing = 1
            End If
        End Function 'Error_User_for_FilePath_Missing

        Public Shared Function Error_User_for_ParmName_Validation(ur_devsettings_cart As Have.sDevSettings, ur_new_parmname As String, ur_windows_msgbox_env As Have.glblWindowsMsgBox) As String
            Error_User_for_ParmName_Validation = ur_new_parmname
            'A blank new parameter name just means they canceled out of creating a new parameter name
            If HasText(ur_new_parmname) AndAlso
              ur_devsettings_cart.SelOnKey(ur_new_parmname).SelAll.Count > 0 Then
                Dim enrUSER_INPUT = ur_windows_msgbox_env.GetResult("That entry already exists", MsgBoxStyle.Exclamation)
                Error_User_for_ParmName_Validation = mt
            End If
        End Function 'Error_User_for_ParmName_Validation

        Public Shared Function Find_extra_AppConfig_Parms(ur_appconfig_cart As Have.sAppConfigSettings, ur_devsettings_cart As Have.sDevSettings, ur_akv_settingval_bowlname As enmAKV.zsetting_value) As Sdata
            Dim sdaMISSING_PARM = New Sdata
            Find_extra_AppConfig_Parms = sdaMISSING_PARM
            For Each trwKEY_LIST In ur_appconfig_cart.SelOnKey("key_search").SelAll
                Dim sdaKEY_SEARCH = New Sdata().dList(trwKEY_LIST.v(ur_akv_settingval_bowlname).Split(","c))
                For Each strPARM_NAME In sdaKEY_SEARCH
                    If ur_devsettings_cart.SelOnKey(strPARM_NAME).SelAll.Count = 0 Then
                        sdaMISSING_PARM.Add(strPARM_NAME)
                    End If
                Next strPARM_NAME
            Next trwKEY_LIST
        End Function 'Find_extra_AppConfig_Parms

        Public Shared Function Get_Parm_from_Grid_Row(ur_grid As Have.uSettingsGrid, ur_colsetting_gridcol As enmSG.zcolSetting, ur_devsettings_cart As Have.sDevSettings) As Have.rDevSettings
            Get_Parm_from_Grid_Row = Nothing
            Dim objGRID_ROW = ur_grid.SelCurrent
            If objGRID_ROW IsNot Nothing Then
                Get_Parm_from_Grid_Row = ur_devsettings_cart.SelKey(objGRID_ROW.Value(ur_colsetting_gridcol))
            End If
        End Function 'Get_Parm_from_Grid_Row

        Public Shared Function Load_DevSettings_Table(ur_devsettings_cart As Have.sDevSettings, ur_userbowl_cart As Have.sUserBowl, ur_devsettings_path_bowlname As enmUN.zdevsettings_path) As Integer
            Dim strDEVSETTINGS_PATH = ur_userbowl_cart.SelKey(ur_devsettings_path_bowlname).Contents
            Call ur_devsettings_cart.DelAll()
            Dim found_row_count = ur_devsettings_cart.PersistRead(strDEVSETTINGS_PATH).Count
            Load_DevSettings_Table = found_row_count
        End Function 'Load_DevSettings_Table

        Public Shared Function Navigate_Grid_to_Row(ur_grid_row As Have.rSettingsGrid) As Integer
            Navigate_Grid_to_Row = 0
            If ur_grid_row IsNot Nothing Then
                Navigate_Grid_to_Row = 1
                Call ur_grid_row.NavigateToHere()
            End If
        End Function 'Navigate_Grid_to_Row

        Public Shared Function Open_External_App(ur_appexternal_cart As Have.glblAppExternal, ur_userbowl_cart As Have.sUserBowl, ur_helpfile_path_bowlname As enmUN.zhelpfile_path, ur_windowsfs_env As Have.glblWindowsFS, ur_windowsmsgbox_env As Have.glblWindowsMsgBox) As Integer
            Dim retFOUND_FILE_COUNT = 0
            Dim flnHELP_FILE_PATH = FileNamed().d(ur_userbowl_cart.SelKey(ur_helpfile_path_bowlname).Contents)
            For Each strPATH In ur_windowsfs_env.GetFiles(flnHELP_FILE_PATH)
                retFOUND_FILE_COUNT = 1
                ur_appexternal_cart.Start_Windows_Program(strPATH, mt)
                Exit For
            Next strPATH

            If retFOUND_FILE_COUNT = 0 Then
                Dim shown_msgbox_count = Assistant.Error_User_for_FilePath_Missing(flnHELP_FILE_PATH, "Help file not found", ur_windowsmsgbox_env)
            End If

            Open_External_App = retFOUND_FILE_COUNT
        End Function 'Open_External_App

        Public Shared Function Reset_Rows_in_Grid(ur_grid As Have.uSettingsGrid, ur_colvalue_gridcol As enmSG.zcolValue, ur_rowkey_tbox As dbUserInput.ztxtRowKey, ur_newval_tbox As dbUserInput.ztxtNewVal, ur_devsettings_cart As Have.sDevSettings, ur_settingval_bowlname As enmDNV.zsetting_value) As Integer
            Dim retROW_COUNT = 0
            Dim update_tbox_count = Assistant.Update_Tbox_disabled(ur_newval_tbox)
            Call ur_grid.Clear_Rows()
            ur_newval_tbox.Text = mt
            For Each kvpENTRY In ur_devsettings_cart.SelAll.kvp
                retROW_COUNT += 1
                Dim objNEW_ROW = Assistant.Add_Row_to_Grid(ur_grid, ur_colvalue_gridcol, kvpENTRY.row, ur_settingval_bowlname)
                If (kvpENTRY.Indexb1 = 1) Then
                    Call objNEW_ROW.NavigateToHere()
                    Dim overwrite_row_count = Assistant.Display_RowName(ur_rowkey_tbox, kvpENTRY.row)
                    overwrite_row_count = Assistant.Display_RowValue(ur_newval_tbox, kvpENTRY.row, ur_settingval_bowlname)
                End If
            Next kvpENTRY

            Call ur_grid.Focus_On_Grid()
            update_tbox_count = Assistant.Update_Tbox_enabled(ur_newval_tbox)
            Reset_Rows_in_Grid = retROW_COUNT
        End Function 'Reset_Rows_in_Grid

        Public Shared Function Search_for_AppConfig_Path(ur_appconfig_cart As Have.sAppConfigSettings, ur_settingval_bowlname As enmAKV.zsetting_value, ur_parm_name As String, ur_userbowl_cart As Have.sUserBowl, ur_appfolder_bolwname As enmUN.zapp_folder, ur_windowsfs_env As Have.glblWindowsFS) As String
            Dim retFOUND_PATH = mt
            Dim flnAPP_FOLDER_PATH = FileNamed().d(ur_userbowl_cart.SelKey(ur_appfolder_bolwname).Contents)
            For Each trwKEY_LIST In ur_appconfig_cart.SelAll
                If StartingWithText(trwKEY_LIST.v(trwKEY_LIST.PK), ur_parm_name) Then
                    Dim flnPARM_PATH = flnAPP_FOLDER_PATH.gParentDir.d(trwKEY_LIST.v(ur_settingval_bowlname))
                    For Each strFILE_PATH In ur_windowsfs_env.GetFiles(flnPARM_PATH)
                        retFOUND_PATH = strFILE_PATH
                        Exit For
                    Next

                    If HasText(retFOUND_PATH) Then
                        Exit For
                    End If
                End If
            Next trwKEY_LIST

            Search_for_AppConfig_Path = retFOUND_PATH
        End Function 'Search_for_AppConfig_Path

        Public Shared Function Show_DevSettings_Path(ur_file_path_tbox As dbUserInput.ztxtUserFile, ur_userbowl_cart As Have.sUserBowl, ur_devsettings_path_bowlname As enmUN.zdevsettings_path) As Integer
            Show_DevSettings_Path = 0
            If ur_file_path_tbox IsNot Nothing Then
                Show_DevSettings_Path = 1
                ur_file_path_tbox.Text = ur_userbowl_cart.SelKey(ur_devsettings_path_bowlname).Contents
            End If
        End Function 'Show_DevSettings_Path

        Public Shared Function Store_DevSettings_Path(ur_windows_envvar_env As Have.glblEnvironment, ur_userbowl_cart As Have.sUserBowl, ur_devsettings_path_bowlname As enmUN.zdevsettings_path) As Integer
            Store_DevSettings_Path = 1
            Dim flnDEVSETTINGS = FileNamed().d(ur_windows_envvar_env.ExpandEnvironmentVariables("%APPDATA%")).d("DevCustomApp_settings").d("user.config.tsv")
            ur_userbowl_cart.SelKey(ur_devsettings_path_bowlname).Contents = flnDEVSETTINGS
        End Function ' Store_DevSettings_Path

        Public Shared Function Store_HelpFile_Path(ur_userbowl_cart As Have.sUserBowl, ur_helpfile_path_bowlname As enmUN.zhelpfile_path, ur_appfolder_bowlname As enmUN.zapp_folder, ur_appname_bowlname As enmUN.zapp_name) As Integer
            Store_HelpFile_Path = 1
            Dim flnAPP_FOLDER_PATH = FileNamed().d(ur_userbowl_cart.SelKey(ur_appfolder_bowlname).Contents)
            Dim flnHELP_FILE_PATH = flnAPP_FOLDER_PATH.gCopy.d(Strapd().d(ur_userbowl_cart.SelKey(ur_appname_bowlname).Contents).dS("App.html"))
            ur_userbowl_cart.SelKey(ur_helpfile_path_bowlname).Contents = flnHELP_FILE_PATH
        End Function 'Store_HelpFile_Path

        Public Shared Function Store_Parm_Value(ur_parm_row As Have.rDevSettings, ur_settingval_bowlname As enmDNV.zsetting_value, ur_new_value As String) As Integer
            Store_Parm_Value = 0
            If ur_parm_row IsNot Nothing Then
                Store_Parm_Value = 0
                ur_parm_row.v(ur_settingval_bowlname) = ur_new_value
            End If 'ur_new_parm
        End Function 'Store_Parm_Value

        Public Shared Function Remove_Parm_from_Cart(ur_devsettings_cart As Have.sDevSettings, ur_parm_row As Have.rDevSettings) As Integer
            Remove_Parm_from_Cart = 0
            If ur_parm_row IsNot Nothing Then
                Remove_Parm_from_Cart = 1
                ur_devsettings_cart.Del(ur_parm_row)
            End If 'ur_parm_row
        End Function 'Remove_Parm_from_Cart

        Public Shared Function Update_DevSettings_File(ur_devsettings_cart As Have.sDevSettings, ur_userbowl_cart As Have.sUserBowl, ur_devsettings_path As enmUN.zdevsettings_path) As Integer
            Update_DevSettings_File = 1
            Dim strDEVSETTINGS_PATH = Have.UserBowl.SelKey(ur_devsettings_path).Contents
            Dim tblSORTED_DEVSETTINGS = New Have.sDevSettings
            Dim sdaKEY_LIST = ur_devsettings_cart.SelDistinct(ur_devsettings_cart.PK(b0(1)))
            sdaKEY_LIST.Sort()
            For Each strKEY In sdaKEY_LIST
                tblSORTED_DEVSETTINGS.Ins(ur_devsettings_cart.SelKey(strKEY))
            Next

            Call tblSORTED_DEVSETTINGS.PersistWrite(strDEVSETTINGS_PATH)
        End Function 'DevSettings

        Public Shared Function Update_GridRow_Value(ur_grid As Have.uSettingsGrid, ur_colvalue_gridcol As enmSG.zcolValue, ur_parm_row As Have.rDevSettings, ur_settingval_bowlname As enmDNV.zsetting_value) As Integer
            Update_GridRow_Value = 0
            If ur_parm_row IsNot Nothing Then
                Dim objGRID_ROW = ur_grid.SelKey(ur_parm_row.v(ur_parm_row.PK))
                If objGRID_ROW IsNot Nothing Then
                    objGRID_ROW.Value(ur_colvalue_gridcol) = ur_parm_row.v(ur_settingval_bowlname)
                End If
            End If 'ur_new_parm
        End Function 'Update_GridRow_Value

        Public Shared Function Update_SaveAllBtn_enabled(ur_saveall_btn As dbUserInput.componentButton, ur_parm_row As Have.rDevSettings) As Integer
            Update_SaveAllBtn_enabled = 0
            If ur_parm_row IsNot Nothing Then
                Update_SaveAllBtn_enabled = 1
                ur_saveall_btn.Enabled = True
            End If
        End Function 'Update_SaveAllBtn_enabled

        Public Shared Function Update_Button_enabled(ur_btn As dbUserInput.componentButton) As Integer
            Update_Button_enabled = 0
            If ur_btn.Enabled = False Then
                Update_Button_enabled = 1
                ur_btn.Enabled = True
            End If
        End Function 'Update_Button_enabled

        Public Shared Function Update_Tbox_enabled(ur_tbox As dbUserInput.componentTextBox) As Integer
            Update_Tbox_enabled = 0
            If ur_tbox.Enabled = False Then
                Update_Tbox_enabled = 1
                ur_tbox.Enabled = True
            End If
        End Function 'Update_Tbox_enabled

        Public Shared Function Update_Button_disabled(ur_btn As dbUserInput.componentButton) As Integer
            Update_Button_disabled = 0
            If ur_btn.Enabled = True Then
                Update_Button_disabled = 1
                ur_btn.Enabled = False
            End If
        End Function 'Update_Button_disabled

        Public Shared Function Update_Tbox_disabled(ur_tbox As dbUserInput.componentTextBox) As Integer
            Update_Tbox_disabled = 0
            If ur_tbox.Enabled = True Then
                Update_Tbox_disabled = 1
                ur_tbox.Enabled = False
            End If
        End Function 'Update_Tbox_disabled

        Public Shared Function Update_Parm_Value(ur_parm_row As Have.rDevSettings, ur_settingval_bowlname As enmDNV.zsetting_value, ur_newval_tbox As dbUserInput.ztxtNewVal) As Integer
            Update_Parm_Value = 0
            If ur_parm_row IsNot Nothing Then
                Update_Parm_Value = 1
                ur_parm_row.v(ur_settingval_bowlname) = ur_newval_tbox.Text
            End If 'ur_new_parm
        End Function 'Update_Parm_Value
    End Class 'Assistant

    Partial Public Class Have
        Private Shared prv_envAppExternal As glblAppExternal
        Private Shared prv_envWindowsCboard As glblWindowsCboard
        Private Shared prv_envWindowsEnvVar As glblEnvironment
        Private Shared prv_envWindowsFS As glblWindowsFS
        Private Shared prv_envWindowsInputBox As glblWindowsInputBox
        Private Shared prv_envWindowsMsgBox As glblWindowsMsgBox
        Private Shared prv_tblAppConfigSettings As sAppConfigSettings
        Private Shared prv_tblDevSettings As sDevSettings
        Private Shared prv_tblUserBowl As sUserBowl
        Private Shared prv_usrSettingsGrid As uSettingsGrid

        <System.Diagnostics.DebuggerHidden()>
        Private Shared Sub Connect()
            If Have.prv_tblUserBowl Is Nothing Then
                Have.prv_tblAppConfigSettings = New sAppConfigSettings
                Have.prv_envAppExternal = New glblAppExternal
                Have.prv_envWindowsCboard = New glblWindowsCboard
                Have.prv_envWindowsEnvVar = New glblEnvironment
                Have.prv_envWindowsFS = New glblWindowsFS
                Have.prv_envWindowsInputBox = New glblWindowsInputBox
                Have.prv_envWindowsMsgBox = New glblWindowsMsgBox
                Have.prv_tblDevSettings = New sDevSettings
                Have.prv_tblUserBowl = New sUserBowl
            End If
        End Sub 'Connect
    End Class 'Have

    Public Class enmAKV
        Inherits bitBASE
        Public Shared setting_name As zsetting_name = Mx.TRow(Of enmAKV).glbl.Trbase(Of zsetting_name).NewBitBase() : Public Class zsetting_name : Inherits enmAKV : End Class
        Public Shared setting_value As zsetting_value = Mx.TRow(Of enmAKV).glbl.Trbase(Of zsetting_value).NewBitBase() : Public Class zsetting_value : Inherits enmAKV : End Class
    End Class

    Public Class gConfigSettings
        Public Shared Function AppSettings() As dbConfigInput
            AppSettings = Have.objAPP_CONFIG
        End Function
    End Class 'gConfigSettings

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function AppConfigSettings() As sAppConfigSettings
            Call Have.Connect()
            AppConfigSettings = Have.prv_tblAppConfigSettings
        End Function

        Public Class rAppConfigSettings
            Inherits TRow(Of enmAKV)
            Public PK As enmAKV

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.PK = enmAKV.setting_name
            End Sub
        End Class

        Public Class sAppConfigSettings
            Inherits TablePKStr(Of enmAKV, rAppConfigSettings)

            Public Sub New()
                Call MyBase.New(1)
                Dim objAPP_SETTINGS = Have.objAPP_CONFIG
                For KEYCTR = 0 To objAPP_SETTINGS.Keys.Count - 1
                    Dim strKEY = objAPP_SETTINGS.Keys.Item(KEYCTR)
                    Dim trwNEW_SETTING = Me.InsKey(strKEY)
                    trwNEW_SETTING.v(enmAKV.setting_value) = objAPP_SETTINGS.Item(strKEY)
                Next KEYCTR
            End Sub 'New
        End Class 'glblAppConfigSettings
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function AppExternal() As glblAppExternal
            Call Have.Connect()
            AppExternal = Have.prv_envAppExternal
        End Function

        Public Class glblAppExternal
            Public Sub Start_Windows_Program(ur_exec_path As String, ur_exec_param As String)
                Call Mx.glbl.gDiagnostics.Start_Windows_Program(ur_exec_path, ur_exec_param)
            End Sub
        End Class 'glblAppExternal
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsEnvVar() As glblEnvironment
            Call Have.Connect()
            WindowsEnvVar = Have.prv_envWindowsEnvVar
        End Function

        Public Class glblEnvironment
            Public Function ExpandEnvironmentVariables(ur_path As String) As String
                ExpandEnvironmentVariables = Mx.glbl.gEnvironment.ExpandEnvironmentVariables(ur_path)
            End Function
        End Class 'glblEnvironment
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
        Public Shared Function WindowsFS() As glblWindowsFS
            Call Have.Connect()
            WindowsFS = Have.prv_envWindowsFS
        End Function

        Public Class glblWindowsFS
            <System.Diagnostics.DebuggerHidden()>
            Public Function GetFiles(ur_search_filespec As MxText.FileName, Optional ur_recurse_option As System.IO.SearchOption = System.IO.SearchOption.TopDirectoryOnly) As String()
                Try
                    GetFiles = Mx.glbl.gWindowsFS.GetFiles(ur_search_filespec.gParentDir, ur_search_filespec.Name, ur_recurse_option)
                Catch ex As System.Exception
                    GetFiles = {}
                End Try
            End Function
        End Class 'glblWindowsFS
    End Class 'Have

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsMsgBox() As glblWindowsMsgBox
            Call Have.Connect()
            WindowsMsgBox = Have.prv_envWindowsMsgBox
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
        Public Shared Function WindowsInputBox() As glblWindowsInputBox
            Call Have.Connect()
            WindowsInputBox = Have.prv_envWindowsInputBox
        End Function

        Public Class glblWindowsInputBox
            <System.Diagnostics.DebuggerHidden()>
            Public Function GetResult(ur_message As String, Optional ur_default_value As String = "") As String
                Dim strAPP_NAME = Have.UserBowl.SelKey(enmUN.app_name).Contents
                GetResult = Microsoft.VisualBasic.InputBox(ur_message, strAPP_NAME, ur_default_value)
            End Function
        End Class 'glblWindowsInputBox
    End Class 'Have

    Public Class enmSG
        Inherits bitBASE
        Public Shared colSetting As zcolSetting = Mx.TRow(Of enmSG).glbl.Trbase(Of zcolSetting).NewBitBase() : Public Class zcolSetting : Inherits enmSG : End Class
        Public Shared colValue As zcolValue = Mx.TRow(Of enmSG).glbl.Trbase(Of zcolValue).NewBitBase() : Public Class zcolValue : Inherits enmSG : End Class
        Public Shared colFound As zcolFound = Mx.TRow(Of enmSG).glbl.Trbase(Of zcolFound).NewBitBase() : Public Class zcolFound : Inherits enmSG : End Class
    End Class 'enmSG

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function SettingsGrid(ur_grid As dbUserInput.componentDataGridView) As uSettingsGrid
            If Have.prv_usrSettingsGrid Is Nothing Then
                Have.prv_usrSettingsGrid = New uSettingsGrid(ur_grid)
            End If

            Have.prv_usrSettingsGrid.Assign_Grid(ur_grid)
            SettingsGrid = Have.prv_usrSettingsGrid
        End Function 'SettingsGrid

        Public Class rSettingsGrid
            Private objvGRID As dbUserInput.componentDataGridView
            Private objvROW_SEQ_b1 As Integer
            Public PK As enmSG

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_settings_grid As dbUserInput.componentDataGridView, ur_row_seq_b1 As Integer)
                Me.objvGRID = ur_settings_grid
                Me.objvROW_SEQ_b1 = ur_row_seq_b1
                Me.PK = enmSG.colSetting
            End Sub 'New

            Public Property BackColor(ur_column As enmSG) As System.Drawing.Color
                <System.Diagnostics.DebuggerHidden()>
                Get
                    BackColor = System.Drawing.Color.Empty
                    If Me.objvGRID IsNot Nothing Then
                        Dim objROW = Me.objvGRID.Rows.Item(b0(Me.objvROW_SEQ_b1))
                        BackColor = objROW.Cells.Item(ur_column.seq).Style.BackColor
                    End If 'Me
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As System.Drawing.Color)
                    If Me.objvGRID IsNot Nothing Then
                        Dim objROW = Me.objvGRID.Rows.Item(b0(Me.objvROW_SEQ_b1))
                        objROW.Cells.Item(ur_column.seq).Style.BackColor = value
                    End If 'Me
                End Set
            End Property 'BackgroundColor

            <System.Diagnostics.DebuggerHidden()>
            Public Sub NavigateToHere()
                If Me.objvGRID IsNot Nothing Then
                    Me.objvGRID.CurrentCell = Me.objvGRID.Rows(b0(Me.objvROW_SEQ_b1)).Cells.Item(b0(1))
                End If
            End Sub 'NavigateToHere

            Public Property Value(ur_column As enmSG) As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Value = mt
                    If Me.objvGRID IsNot Nothing Then
                        Dim objROW = Me.objvGRID.Rows.Item(b0(Me.objvROW_SEQ_b1))
                        Dim strCELL = objROW.Cells.Item(ur_column.seq).Value
                        If strCELL IsNot Nothing Then
                            Value = strCELL
                        End If
                    End If 'Me
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    If Me.objvGRID IsNot Nothing Then
                        Dim objROW = Me.objvGRID.Rows.Item(b0(Me.objvROW_SEQ_b1))
                        objROW.Cells.Item(ur_column.seq).Value = value
                    End If 'Me
                End Set
            End Property 'Value
        End Class 'rSettingsGrid

        Public Class uSettingsGrid
            Private objvGRID As dbUserInput.componentDataGridView
            Public PK As enmSG

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_grid As dbUserInput.componentDataGridView)
                If Me.objvGRID Is Nothing Then
                    Call Me.Assign_Grid(ur_grid)
                    For Each kvpENTRY In TRow(Of enmSG).glbl.RefKeys.kvp
                        If kvpENTRY.Indexb1 = 1 Then
                            Me.PK = kvpENTRY.row
                        End If

                        If kvpENTRY.Indexb1 <= Me.objvGRID.Columns.Count Then
                            Dim objCOL = Me.objvGRID.Columns.Item(kvpENTRY.Indexenm)
                            If AreEqual(objCOL.Name, kvpENTRY.row.name) = False Then
                                Throw New System.Exception("Column sequence " & kvpENTRY.Indexb1 & " name " & objCOL.Name & " does not match expected column name: " & kvpENTRY.row.name)
                            End If

                        Else
                            Throw New System.Exception("Column sequence " & kvpENTRY.Indexb1 & " name " & kvpENTRY.row.name & " does not exist in grid")
                        End If
                    Next kvpENTRY
                End If 'Me

                Call Me.Assign_Grid(ur_grid)
            End Sub 'New

            <System.Diagnostics.DebuggerHidden()>
            Public Function Add_Row(ur_row_pk As String) As rSettingsGrid
                Add_Row = Nothing
                If Me.objvGRID IsNot Nothing Then
                    Dim intROW_SEQ = Me.objvGRID.Rows.Add()
                    Dim objNEW_ROW = New rSettingsGrid(Me.objvGRID, b1(intROW_SEQ))
                    Add_Row = objNEW_ROW
                    objNEW_ROW.Value(Me.PK) = ur_row_pk
                End If
            End Function 'Add_Row

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Assign_Grid(ur_grid As dbUserInput.componentDataGridView)
                Me.objvGRID = ur_grid
            End Sub 'Assign_Grid

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Clear_Rows()
                If Me.objvGRID IsNot Nothing Then
                    Me.objvGRID.Rows.Clear()
                End If
            End Sub 'Clear_Rows

            Public Property Column_Visible(ur_column As enmSG) As Boolean
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Column_Visible = False
                    If Me.objvGRID IsNot Nothing Then
                        Column_Visible = Me.objvGRID.Columns.Item(ur_column.seq).Visible
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As Boolean)
                    If Me.objvGRID IsNot Nothing Then
                        Me.objvGRID.Columns.Item(ur_column.seq).Visible = value
                    End If
                End Set
            End Property

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Focus_On_Grid()
                If Me.objvGRID IsNot Nothing Then
                    Call Me.objvGRID.Select()
                End If
            End Sub 'Focus_On_Grid

            <System.Diagnostics.DebuggerHidden()>
            Public Function Navigate_To_Row(ur_row As rSettingsGrid, ur_col As enmSG, ur_value As String) As rSettingsGrid
                Navigate_To_Row = ur_row
                If ur_row IsNot Nothing Then
                    Call ur_row.NavigateToHere()
                End If
            End Function 'Navigate_To_Row

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelAll() As Objlist(Of rSettingsGrid)
                Dim retLIST = New Objlist(Of rSettingsGrid)
                SelAll = retLIST
                If Me.objvGRID IsNot Nothing Then
                    For ROWCTR = 1 To Me.objvGRID.Rows.Count
                        Dim objROW = New rSettingsGrid(Me.objvGRID, ROWCTR)
                        retLIST.Add(objROW)
                    Next ROWCTR
                End If 'Me
            End Function 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelCurrent() As rSettingsGrid
                SelCurrent = Nothing
                If Me.objvGRID IsNot Nothing AndAlso
                  Me.objvGRID.CurrentRow IsNot Nothing AndAlso
                  Me.objvGRID.CurrentRow.Cells IsNot Nothing Then
                    Dim strCUR_PK = Me.objvGRID.CurrentRow.Cells.Item(Me.PK.seq).Value
                    For ROWCTR = 1 To Me.objvGRID.Rows.Count
                        Dim objROW = Me.objvGRID.Rows.Item(b0(ROWCTR))
                        If AreEqual(objROW.Cells.Item(PK.seq).Value, strCUR_PK) Then
                            SelCurrent = New rSettingsGrid(Me.objvGRID, ROWCTR)
                            Exit For
                        End If
                    Next ROWCTR
                End If 'Me
            End Function 'SelCurrent

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelKey(ur_row_pk As String) As rSettingsGrid
                SelKey = Nothing
                If Me.objvGRID IsNot Nothing Then
                    For ROWCTR = 1 To Me.objvGRID.Rows.Count
                        Dim objROW = Me.objvGRID.Rows.Item(b0(ROWCTR))
                        If AreEqual(objROW.Cells.Item(PK.seq).Value, ur_row_pk) Then
                            SelKey = New rSettingsGrid(Me.objvGRID, ROWCTR)
                            Exit For
                        End If
                    Next ROWCTR
                End If 'Me
            End Function 'SelKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function Update_Row(ur_row As rSettingsGrid, ur_col As enmSG, ur_value As String) As rSettingsGrid
                Update_Row = ur_row
                If ur_row IsNot Nothing Then
                    ur_row.Value(ur_col) = ur_value
                End If
            End Function 'Update_Row
        End Class 'sSettingsGrid
    End Class 'SG

    Public Class enmDNV
        Inherits bitBASE
        Public Shared setting_name As zsetting_name = Mx.TRow(Of enmDNV).glbl.Trbase(Of zsetting_name).NewBitBase() : Public Class zsetting_name : Inherits enmDNV : End Class
        Public Shared setting_value As zsetting_value = Mx.TRow(Of enmDNV).glbl.Trbase(Of zsetting_value).NewBitBase() : Public Class zsetting_value : Inherits enmDNV : End Class
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function DevSettings() As sDevSettings
            Call Have.Connect()
            DevSettings = Have.prv_tblDevSettings
        End Function 'DevSettings

        Public Class rDevSettings
            Inherits TRow(Of enmDNV)
            Public PK As enmDNV

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.PK = enmDNV.setting_name
            End Sub
        End Class 'rDevSettings

        Public Class sDevSettings
            Inherits TablePKStr(Of enmDNV, rDevSettings)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Call MyBase.New(1)
            End Sub
        End Class 'sDevSettings
    End Class 'enmDS

    Public Class enmUB
        Inherits bitBASE
        Public Shared bowl_name As enmUB = TRow(Of enmUB).glbl.NewBitBase()
        Public Shared contents As enmUB = TRow(Of enmUB).glbl.NewBitBase()
    End Class

    Public Class enmUN
        Inherits bitBASE
        Public Shared app_folder As zapp_folder = Mx.TRow(Of enmUN).glbl.Trbase(Of zapp_folder).NewBitBase() : Public Class zapp_folder : Inherits enmUN : End Class
        Public Shared app_name As zapp_name = Mx.TRow(Of enmUN).glbl.Trbase(Of zapp_name).NewBitBase() : Public Class zapp_name : Inherits enmUN : End Class
        Public Shared app_path As zapp_path = Mx.TRow(Of enmUN).glbl.Trbase(Of zapp_path).NewBitBase() : Public Class zapp_path : Inherits enmUN : End Class
        Public Shared audit_parms_to_cboard As zaudit_parms_to_cboard = Mx.TRow(Of enmUN).glbl.Trbase(Of zaudit_parms_to_cboard).NewBitBase() : Public Class zaudit_parms_to_cboard : Inherits enmUN : End Class
        Public Shared cmdline_curexe As zcmdline_curexe = Mx.TRow(Of enmUN).glbl.Trbase(Of zcmdline_curexe).NewBitBase() : Public Class zcmdline_curexe : Inherits enmUN : End Class
        Public Shared cmdline_orig As zcmdline_orig = Mx.TRow(Of enmUN).glbl.Trbase(Of zcmdline_orig).NewBitBase() : Public Class zcmdline_orig : Inherits enmUN : End Class
        Public Shared cmdline_table As zcmdline_table = Mx.TRow(Of enmUN).glbl.Trbase(Of zcmdline_table).NewBitBase() : Public Class zcmdline_table : Inherits enmUN : End Class
        Public Shared compiler_exe As zcompiler_exe = Mx.TRow(Of enmUN).glbl.Trbase(Of zcompiler_exe).NewBitBase() : Public Class zcompiler_exe : Inherits enmUN : End Class
        Public Shared devsettings_path As zdevsettings_path = TRow(Of enmUN).glbl.Trbase(Of zdevsettings_path).NewBitBase() : Public Class zdevsettings_path : Inherits enmUN : End Class
        Public Shared path_unassigned As zpath_unassigned = Mx.TRow(Of enmUN).glbl.Trbase(Of zpath_unassigned).NewBitBase() : Public Class zpath_unassigned : Inherits enmUN : End Class
        Public Shared helpfile_path As zhelpfile_path = TRow(Of enmUN).glbl.Trbase(Of zhelpfile_path).NewBitBase() : Public Class zhelpfile_path : Inherits enmUN : End Class
    End Class 'enmUN

    Partial Public Class Have
        Public Shared objAPP_CONFIG As dbConfigInput
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
        End Function 'UserBowl

        Public Class rUserBowl
            Inherits TRow(Of enmUB)
            Public PK As enmUB

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.PK = enmUB.bowl_name
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmUB, ur_val As String) As rUserBowl
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Property Contents() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Contents = Me.v(enmUB.contents)
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(ur_val As String)
                    Me.v(enmUB.contents) = ur_val
                End Set
            End Property
        End Class 'rUserBowl

        Public Class sUserBowl
            Inherits TablePKEnum(Of enmUB, enmUN, rUserBowl)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub UpdCboard_FromAudit(cmdexport_audit_bowlname As enmUN.zaudit_parms_to_cboard)
                If Mx.HasText(Me.SelKey(cmdexport_audit_bowlname).v(enmUB.contents)) Then
                    Dim strAUDIT = Me.ToString(True)
                    Dim ins_msg = Have.WindowsMsgBox.GetResult(
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
End Namespace 'Mx
