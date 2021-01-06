Namespace Mx
    Module subs
        Sub Main()
            Dim RetVal = Mx2.UserAction.CompileCode_Report_errhnd(System.Environment.CommandLine, False)
            If RetVal <> "QUIT" Then
                If MsgBox(RetVal, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    My.Computer.Clipboard.SetText(RetVal)
                End If
            End If
        End Sub
    End Module 'subs

    Public Class Class1
        Public Shared SourceFolder As String = "UrFolder"
        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
    End Class
End Namespace 'Mx2

