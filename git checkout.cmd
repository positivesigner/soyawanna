@echo off
rem You have to copy this up to the parent directory, because changing this file will stop creating the new branch
cd UrDir
"..\PortableGit\cmd\git.exe" checkout -b test-branch UrHash
rem "..\PortableGit\cmd\git.exe" branch -d test-branch
rem delete a branch: git push origin -d test-branch   ( Git v1.7.0 )
rem or:  git push origin :test-branch   ( Git older )