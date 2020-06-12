Option Strict On
Partial Public Class frmInput_1
    Private Sub frmInput_1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Call Mx.Want.Compile_And_Run_Script_errhnd(Me, System.Environment.CommandLine)
    End Sub 'frmInput_1_Load
End Class 'frmInput_1
