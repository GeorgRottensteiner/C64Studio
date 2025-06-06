@echo off
set WINRAR=D:\Tools\winrar\winrar.exe

if exist C64StudioRelease.zip del C64StudioRelease.zip
if exist C64StudioRelease3.5.zip del C64StudioRelease3.5.zip
if exist C64StudioRelease4.8.zip del C64StudioRelease4.8.zip
if exist C64StudioRelease6.0.zip del C64StudioRelease6.0.zip
if exist C64StudioRelease8.0.zip del C64StudioRelease8.0.zip

rem complete archive
cd "C64StudioRelease\net3.5\AnyCPU"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease.zip -apC64StudioRelease\net3.5 .
cd ..\..\..
cd "C64StudioRelease\net4.8\AnyCPU"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease.zip -apC64StudioRelease\net4.8 .
cd ..\..\..
cd "C64StudioRelease\net8.0-windows\AnyCPU"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease.zip -apC64StudioRelease\net8.0-windows .
cd ..\..\..

cd "C64StudioRelease\net3.5\AnyCPU"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease3.5.zip -apC64StudioRelease\net3.5 .
cd ..\..\..
cd "C64StudioRelease\net4.8\AnyCPU"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease4.8.zip -apC64StudioRelease\net4.8 .
cd ..\..\..
cd "C64StudioRelease\net8.0-windows\AnyCPU"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease8.0.zip -apC64StudioRelease\net8.0-windows .
cd ..\..\..

cd "C64StudioRelease\shared content"
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net3.5 ..\..\C64StudioRelease.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net4.8 ..\..\C64StudioRelease.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net8.0-windows ..\..\C64StudioRelease.zip "*.*" 

%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net3.5 ..\..\C64StudioRelease3.5.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net4.8 ..\..\C64StudioRelease4.8.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net8.0-windows ..\..\C64StudioRelease8.0.zip "*.*" 
cd ..\..

IF "%~1"=="noincbuildno" GOTO buildcomplete
rem increase build number for next release
powershell .\IncreaseBuildNumber.ps1

:buildcomplete