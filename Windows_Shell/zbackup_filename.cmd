echo off
For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%cm%%ad%%b)
For /f "tokens=1 delims=/ " %%a in ('time /t') do (set mytime=%%a)
For /f "tokens=2 delims=/ " %%a in ('time /t') do (set myampm=%%a)
SET hours=%mytime:~0,2%
SET hours=%myampm%%hours%
SET minutes=%mytime:~3,2%
CALL SET hours=%%hours:PM12=N012%%
CALL SET hours=%%hours:AM12=AM00%%
copy %1 "%~dp1%~n1 %mydate%_%hours%%minutes%%~x1"
