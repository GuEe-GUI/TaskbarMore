@echo off
setlocal

set "MSBUILD="
where msbuild >nul 2>&1 && set "MSBUILD=msbuild"
if defined MSBUILD goto :build

for %%A in ("%ProgramFiles(x86)%") do set "PF86=%%~sA"
set "VSWHERE=%PF86%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" goto :notfound

for /f "usebackq delims=" %%i in (`"%VSWHERE%" -latest -products Microsoft.VisualStudio.Product.BuildTools -products Microsoft.VisualStudio.Product.Community -products Microsoft.VisualStudio.Product.Professional -products Microsoft.VisualStudio.Product.Enterprise -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSBUILD=%%i"

if not defined MSBUILD goto :notfound

:build
"%MSBUILD%" "%~dp0TaskbarMore.sln" /t:Rebuild /p:Configuration=Release /p:Platform=x64 /v:minimal
if %ERRORLEVEL% equ 0 (
    echo.
    echo Build OK: %~dp0TaskbarMore\bin\x64\Release\TaskbarMore.exe
    dir /T:W "%~dp0TaskbarMore\bin\x64\Release\TaskbarMore.exe"
)
goto :end

:notfound
echo MSBuild not found.
echo Install Visual Studio Build Tools with ".NET desktop build tools",
echo or run this script from "Developer Command Prompt for VS".
exit /b 1

:end
endlocal
