@ECHO OFF
CLS

IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current" SET "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\bin\MSBuild.exe"
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current" SET "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\bin\MSBuild.exe"
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current" SET "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\bin\MSBuild.exe"

"%MSBUILD%" /nologo /v:m /m Source\EWSImageMaps.sln /t:Clean;Restore;Build "/p:Configuration=Release;Platform=Any CPU"

COPY Source\WinForms\bin\Release\*.nupkg .\Deployment
COPY Source\WebControls\bin\Release\*.nupkg .\Deployment

IF NOT "%SHFBROOT%"=="" "%MSBUILD%" /nologo /v:m Doc\EWSoftwareImageMaps.sln /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

IF "%SHFBROOT%"=="" ECHO **** Sandcastle help file builder not installed.  Skipping help build. ****
