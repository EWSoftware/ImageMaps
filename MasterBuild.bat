@ECHO OFF
CLS

REM Use the earliest version of MSBuild available
IF EXIST "%ProgramFiles(x86)%\MSBuild\14.0" SET "MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\MSBuild\12.0" SET "MSBUILD=%ProgramFiles(x86)%\MSBuild\12.0\bin\MSBuild.exe"

"%MSBUILD%" /nologo /v:m /m "Source\EWSImageMaps.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

"%MSBUILD%" /nologo /v:m /m "Demos\WinFormsDemos.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

"%MSBUILD%" /nologo /v:m /m "Demos\WebDemos.sln" /t:Clean;Build

IF NOT "%SHFBROOT%"=="" "%MSBUILD%" /nologo /v:m "Doc\EWSoftwareImageMaps.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

IF "%SHFBROOT%"=="" ECHO **** Sandcastle help file builder not installed.  Skipping help build. ****

CD .\NuGet

NuGet Pack EWSoftware.ImageMap.Web.nuspec -NoPackageAnalysis -OutputDirectory ..\Deployment
NuGet Pack EWSoftware.ImageMap.WinForms.nuspec -NoPackageAnalysis -OutputDirectory ..\Deployment

CD ..
