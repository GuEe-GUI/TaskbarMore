param([ValidateSet('Debug', 'Release')][string]$Configuration = 'Release', [string]$OutputDirectory)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (!$msbuild) { throw 'Install Visual Studio Build Tools with .NET desktop build tools first.' }
$referenceRoot = Join-Path $projectRoot '.build-deps\net472\build'
$frameworkList = Join-Path $referenceRoot '.NETFramework\v4.7.2\RedistList\FrameworkList.xml'
if (!(Test-Path $frameworkList)) {
    $dependencyRoot = Join-Path $projectRoot '.build-deps'
    New-Item -ItemType Directory -Force $dependencyRoot | Out-Null
    $archive = Join-Path $dependencyRoot 'net472.zip'
    Invoke-WebRequest 'https://api.nuget.org/v3-flatcontainer/microsoft.netframework.referenceassemblies.net472/1.0.3/microsoft.netframework.referenceassemblies.net472.1.0.3.nupkg' -OutFile $archive
    Expand-Archive -LiteralPath $archive -DestinationPath (Join-Path $dependencyRoot 'net472') -Force
}
$outputArguments = @()
if ($OutputDirectory) { $outputArguments = @("/p:OutputPath=$([IO.Path]::GetFullPath($OutputDirectory))" ) }
& $msbuild (Join-Path $projectRoot 'TaskbarMore.sln') /t:Build "/p:Configuration=$Configuration" '/p:Platform=Any CPU' "/p:TargetFrameworkRootPath=$referenceRoot" /p:RegisterForComInterop=false /nologo /v:minimal @outputArguments
if ($LASTEXITCODE -ne 0) { throw "Build failed: $LASTEXITCODE" }
if (!$OutputDirectory) { $OutputDirectory = Join-Path $projectRoot "TaskbarMore\bin\AnyCPU\$Configuration" }
Write-Host "Built: $(Join-Path $OutputDirectory 'TaskbarMore.exe')"
