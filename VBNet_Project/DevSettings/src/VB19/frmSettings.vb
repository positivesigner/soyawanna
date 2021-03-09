Option Strict On

Public Class frmSettings
    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Mx.UserAction.Open_Main_Form_errhnd(Me, System.Configuration.ConfigurationManager.AppSettings, System.Reflection.Assembly.GetExecutingAssembly, System.Environment.CommandLine)
    End Sub

    Private Sub btnAddNewRow_Click(sender As Object, e As EventArgs) Handles btnAddNewRow.Click
        Mx.UserAction.Add_New_Row_errhnd(Me)
    End Sub

    Private Sub btnAuditPaths_Click(sender As Object, e As EventArgs) Handles btnAuditPaths.Click
        Mx.UserAction.Audit_Paths_errhnd(Me)
    End Sub

    Private Sub btnDeleteRow_Click(sender As Object, e As EventArgs) Handles btnDeleteRow.Click
        Mx.UserAction.Delete_Existing_Row_errhnd(Me)
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Mx.UserAction.Refresh_Grid_errhnd(Me)
    End Sub

    Private Sub btnSaveAllChanges_Click(sender As Object, e As EventArgs) Handles btnSaveAllChanges.Click
        Mx.UserAction.Save_To_File_errhnd(Me)
    End Sub

    Private Sub txtNewVal_TextChanged(sender As Object, e As EventArgs) Handles txtNewVal.TextChanged
        Mx.UserAction.Change_current_Row_Value_errhnd(Me)
    End Sub

    Private Sub grdSetting_SelectionChanged(sender As Object, e As EventArgs) Handles grdSetting.SelectionChanged
        Mx.UserAction.Overwrite_displayed_Row_Value_errhnd(Me)
    End Sub

    Private Sub frmSettings_HelpRequested(Optional sender As Object = Nothing, Optional hlpevent As HelpEventArgs = Nothing) Handles Me.HelpRequested
        Mx.UserAction.Open_HelpFile_errhnd()
    End Sub

    Private Sub lblDesignDoc_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblDesignDoc.LinkClicked
        Mx.UserAction.Open_HelpFile_errhnd()
    End Sub

    <System.Diagnostics.DebuggerHidden()>
    Public Shared Widening Operator CType(b As frmSettings) As Mx.dbUserInput
        Return New Mx.dbUserInput(b, b.grdSetting, b.btnSaveAllChanges, b.txtRowKey, b.txtNewVal, b.txtUserFile)
    End Operator
End Class 'frmSettings
