@ECHO OFF
CLS

DEL /Q .\Deployment\*.*

"%WINDIR%\Microsoft.Net\Framework\v4.0.30319\msbuild.exe" /nologo /v:m /m "Source\EWSImageMaps.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

"%WINDIR%\Microsoft.Net\Framework\v4.0.30319\msbuild.exe" /nologo /v:m /m "Demos\WinFormsDemos.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

"%WINDIR%\Microsoft.Net\Framework\v4.0.30319\msbuild.exe" /nologo /v:m /m "Demos\WebDemos.sln" /t:Clean;Build

IF NOT "%SHFBROOT%"=="" "%WINDIR%\Microsoft.Net\Framework\v4.0.30319\msbuild.exe" /nologo /v:m "Doc\EWSoftwareImageMaps.sln" /t:Clean;Build "/p:Configuration=Release;Platform=Any CPU"

IF "%SHFBROOT%"=="" ECHO **** Sandcastle help file builder not installed.  Skipping help build. ****

CD .\NuGet

NuGet Pack EWSoftware.ImageMap.Web.nuspec -NoPackageAnalysis -OutputDirectory ..\Deployment
NuGet Pack EWSoftware.ImageMap.WinForms.nuspec -NoPackageAnalysis -OutputDirectory ..\Deployment

CD ..
