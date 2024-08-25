@echo off
call buildrelease.bat noincbuildno
call buildreleaseARM64.bat noincbuildno

rem increase build number for next release
powershell .\IncreaseBuildNumber.ps1

