for %%f in (*.sln) do (
    for %%m in ("C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe") do (
        %%m /v:n /p:Configuration=Release "%%f"
        goto :compile_complete
       )
	
    for /D %%m in ("C:\Program Files (x86)\MSBuild\*\Bin\MSBuild.exe") do (
        "%%~m" /v:n /p:Configuration=Release "%%f"
        goto :compile_complete
       )
    
    for %%m in ("C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe") do (
        %%m /v:n /p:Configuration=Release "%%f"
        goto :compile_complete
       )
    
    for %%m in ("C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe") do (
        %%m /v:n /p:Configuration=Release "%%f"
        goto :compile_complete
       )
   )
:compile_complete
pause
del ..\Dep\*.exe
move bin\Release\*.exe ..\Dep
rd /s /q bin
rd /s /q obj
pause