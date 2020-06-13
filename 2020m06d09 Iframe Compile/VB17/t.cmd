set sln_file=None.sln
for /f "delims=" %%a in ('dir /b *.sln') do set "sln_file=%%a"
set compile_exe=None.exe
rem for /f "delims=" %%a in ('dir /b "C:\Program Files (x86)\MSBuild\*\Bin\MSBuild.exe"') do set "compile_exe=%%a"
for /f "delims=" %%a in ('dir /b "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"') do set "compile_exe=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\%%a"
for /f "delims=" %%a in ('dir /b "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"') do set "compile_exe=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\%%a"
for /f "delims=" %%a in ('dir /b "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"') do set "compile_exe=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\%%a"
"%compile_exe%" /v:n /p:Configuration=Release "%sln_file%"
pause