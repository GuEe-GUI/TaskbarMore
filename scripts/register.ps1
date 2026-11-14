param([ValidateSet('Install', 'Uninstall')][string]$Action = 'Install', [string]$ExecutablePath)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
if ($Action -eq 'Install') {
    $source = if ($ExecutablePath) { [IO.Path]::GetFullPath($ExecutablePath) } else { Join-Path $projectRoot 'TaskbarMore\bin\AnyCPU\Release\TaskbarMore.exe' }
    if (!(Test-Path $source)) { throw 'Run scripts\build.ps1 first.' }
    $deployment = Join-Path $projectRoot ('artifacts\deskband\' + (Get-Date -Format 'yyyyMMdd-HHmmss-fff'))
    New-Item -ItemType Directory -Force $deployment | Out-Null
    $executable = Join-Path $deployment 'TaskbarMore.exe'
    Copy-Item -LiteralPath $source -Destination $executable
    $process = Start-Process -FilePath $executable -ArgumentList '--install' -Wait -PassThru
} else {
    $key = 'Registry::HKEY_CLASSES_ROOT\CLSID\{A96DCE38-8F92-4B9D-B780-024E43CFB865}\InprocServer32'
    $codebase = (Get-ItemProperty $key -ErrorAction SilentlyContinue).CodeBase
    if (!$codebase) { Write-Host 'TaskbarMore is not registered.'; return }
    $executable = [Uri]::UnescapeDataString(($codebase -replace '^file:///', '')).Replace('/', '\')
    if ([IO.Path]::GetExtension($executable) -ne '.exe') { throw 'Legacy DLL installation: unregister it with Framework64\v4.0.30319\RegAsm.exe /unregister before installing the EXE.' }
    $process = Start-Process -FilePath $executable -ArgumentList '--uninstall' -Wait -PassThru
}
if ($process.ExitCode -ne 0) { throw "TaskbarMore $Action failed: $($process.ExitCode)" }
Write-Host "$Action complete: $executable"
