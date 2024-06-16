@echo off
set WINRAR=D:\Tools\winrar\winrar.exe

if exist C64StudioRelease.zip del C64StudioRelease.zip
if exist C64StudioRelease3.5.zip del C64StudioRelease3.5.zip
if exist C64StudioRelease4.8.zip del C64StudioRelease4.8.zip
if exist C64StudioRelease6.0.zip del C64StudioRelease6.0.zip

rem complete archive
%winrar% -ibck a -afzip C64StudioRelease.zip C64StudioRelease\net3.5
%winrar% -ibck a -afzip C64StudioRelease.zip C64StudioRelease\net4.8
%winrar% -ibck a -afzip C64StudioRelease.zip C64StudioRelease\net6.0-windows

%winrar% -ibck a -afzip C64StudioRelease3.5.zip C64StudioRelease\net3.5
%winrar% -ibck a -afzip C64StudioRelease4.8.zip C64StudioRelease\net4.8
%winrar% -ibck a -afzip C64StudioRelease6.0.zip C64StudioRelease\net6.0-windows

cd "C64StudioRelease\shared content"
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net3.5 ..\..\C64StudioRelease.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net4.8 ..\..\C64StudioRelease.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net6.0-windows ..\..\C64StudioRelease.zip "*.*" 

%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net3.5 ..\..\C64StudioRelease3.5.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net4.8 ..\..\C64StudioRelease4.8.zip "*.*" 
%winrar% -ibck a -afzip -r -ep1 -apC64StudioRelease\net6.0-windows ..\..\C64StudioRelease6.0.zip "*.*" 
cd ..\..

rem increase build number for next release
powershell .\IncreaseBuildNumber.ps1