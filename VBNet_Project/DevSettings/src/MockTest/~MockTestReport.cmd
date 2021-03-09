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
@START "" "%mx_dir%\MxClasses\VBNetScript.exe" /path=%0

:mx_end
@EXIT
DLL_WinForm2019m09d13\System.Drawing.dll
..\VB19\MxClasses\MxBaseEc13.vb
..\VB19\dbUserInputMock.vb
..\MockReport\MockSubs.vb
..\VB19\subs.vb
RetVal = Mx2.UserAction.CompileCode_Report_errhnd(System.Reflection.Assembly.GetExecutingAssembly, System.Environment.CommandLine, False, Mx.Class1.SourcePath)
End Function
End Class
End Namespace
