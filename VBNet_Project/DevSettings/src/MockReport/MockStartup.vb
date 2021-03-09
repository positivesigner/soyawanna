Namespace Mx
    Module subs
        Sub Main()
            Dim RetVal = Mx2.UserAction.CompileCode_Report_errhnd(System.Reflection.Assembly.GetExecutingAssembly, System.Environment.CommandLine, False)
            If RetVal <> "QUIT" Then MsgBox(RetVal)
        End Sub
    End Module 'subs

    Public Class Class1
        Public Shared SourceFolder As String = "UrFolder"
        Public Shared SourcePath As String = "UrFolder\MyApp.exe"
    End Class
End Namespace 'Mx2

