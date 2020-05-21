@echo off
SET ur_company=tpr
SET ur_branch=ffe891d2bd975d50e23310dac5a91986cfd2d21f
rem "C:\BitBucket\PortableGit\bin\git.exe" -C %ur_company%_gotoassisttickets checkout -b test-branch %ur_branch%
rem change to another branch and bring the changes to the new branch for 'git checkout.cmd'
rem git branch -d test-branch
rem git push origin -d test-branch   ( Git v1.7.0 )
rem or  git push origin :test-branch   ( Git older )