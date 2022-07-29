@ECHO OFF
CLS

IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2022\Community\MSBuild\Current" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2022\Community\MSBuild\Current\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2022\Professional\MSBuild\Current" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\bin\MSBuild.exe"
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current" SET "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\bin\MSBuild.exe"

"%MSBUILD%" /nologo /v:m /m "Source\EWSImageMaps.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

"%MSBUILD%" /nologo /v:m /m "Demos\WinFormsDemos.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

"%MSBUILD%" /nologo /v:m /m "Demos\WebDemos.sln" /t:Clean;Build

IF NOT "%SHFBROOT%"=="" "%MSBUILD%" /nologo /v:m "Doc\EWSoftwareImageMaps.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

IF "%SHFBROOT%"=="" ECHO **** Sandcastle help file builder not installed.  Skipping help build. ****

CD .\NuGet

NuGet Pack EWSoftware.ImageMap.Web.nuspec -NoPackageAnalysis -OutputDirectory ..\Deployment
NuGet Pack EWSoftware.ImageMap.WinForms.nuspec -NoPackageAnalysis -OutputDirectory ..\Deployment

CD ..
