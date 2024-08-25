@echo off
set WINRAR=D:\Tools\winrar\winrar.exe

if exist C64StudioReleaseARM.zip del C64StudioReleaseARM.zip
if exist C64StudioRelease3.5ARM.zip del C64StudioRelease3.5ARM.zip
if exist C64StudioRelease4.8ARM.zip del C64StudioRelease4.8ARM.zip
if exist C64StudioRelease6.0ARM.zip del C64StudioRelease6.0ARM.zip

rem complete archive
cd "C64StudioRelease\net3.5\ARM64"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioReleaseARM.zip -apC64StudioRelease\net3.5 .
cd ..\..\..
cd "C64StudioRelease\net4.8\ARM64"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioReleaseARM.zip -apC64StudioRelease\net4.8 .
cd ..\..\..
cd "C64StudioRelease\net6.0-windows\ARM64"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioReleaseARM.zip -apC64StudioRelease\net6.0-windows .
cd ..\..\..

cd "C64StudioRelease\net3.5\ARM64"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease3.5ARM.zip -apC64StudioRelease\net3.5 .
cd ..\..\..
cd "C64StudioRelease\net4.8\ARM64"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease4.8ARM.zip -apC64StudioRelease\net4.8 .
cd ..\..\..
cd "C64StudioRelease\net6.0-windows\ARM64"
%winrar% -ibck a -r -afzip ..\..\..\C64StudioRelease6.0ARM.zip -apC64StudioRelease\net6.0-windows .
cd ..\..\..

cd "C64StudioRelease\shared content"
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net3.5 ..\..\C64StudioReleaseARM.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net4.8 ..\..\C64StudioReleaseARM.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net6.0-windows ..\..\C64StudioReleaseARM.zip "*.*" 

%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net3.5 ..\..\C64StudioRelease3.5ARM.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net4.8 ..\..\C64StudioRelease4.8ARM.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net6.0-windows ..\..\C64StudioRelease6.0ARM.zip "*.*" 
cd ..\..

IF "%~1"=="noincbuildno" GOTO buildcomplete
rem increase build number for next release
powershell .\IncreaseBuildNumber.ps1

:buildcomplete
