param([string]$Configuration = 'Release', [string]$BinaryRoot)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms, System.Drawing
$projectRoot = Split-Path $PSScriptRoot -Parent
if (!$BinaryRoot) { $BinaryRoot = Join-Path $projectRoot "TaskbarMore\bin\AnyCPU\$Configuration" }
$assembly = [Reflection.Assembly]::LoadFrom((Join-Path $binaryRoot 'TaskbarMore.exe'))
if ($assembly.GetReferencedAssemblies().Name -match '^CSDeskBand') { throw 'External CSDeskBand assembly dependency remains.' }
if ($assembly.GetManifestResourceNames() -notcontains 'TaskbarMore.CSDeskBand.LICENSE') { throw 'Embedded license is missing.' }
$flags = [Reflection.BindingFlags]'Instance,NonPublic'
$control = [Activator]::CreateInstance($assembly.GetType('TaskbarMore.TaskbarMore'))
try {
    $monitor = $control.GetType().GetField('monitor', $flags).GetValue($control)
    $sample = $monitor.GetType().GetMethod('Sample', $flags)
    $sample.Invoke($monitor, @()) | Out-Null
    Start-Sleep -Milliseconds 1100
    $sample.Invoke($monitor, @()) | Out-Null
    foreach ($property in @('CpuText', 'MemoryText', 'UploadBytesPerSecond', 'DownloadBytesPerSecond')) {
        $value = $monitor.GetType().GetProperty($property, $flags).GetValue($monitor)
        if ($property -match 'Text' -and $value -notmatch '^\d+%$') { throw "Invalid sample $property = $value" }
        if ($property -notmatch 'Text' -and $value -lt 0) { throw "Negative rate $property" }
        Write-Host "$property = $value"
    }
    $imageRoot = Join-Path $projectRoot 'artifacts\verification'
    New-Item -ItemType Directory -Force $imageRoot | Out-Null
    foreach ($size in @((New-Object Drawing.Size 164,40), (New-Object Drawing.Size 200,40), (New-Object Drawing.Size 300,60))) {
        $control.Size = $size
        $image = New-Object Drawing.Bitmap $size.Width,$size.Height
        try { $control.DrawToBitmap($image, (New-Object Drawing.Rectangle 0,0,$size.Width,$size.Height)); $image.Save((Join-Path $imageRoot "horizontal-$($size.Width).png")) } finally { $image.Dispose() }
    }
    foreach ($property in @('CpuText', 'MemoryText')) {
        $monitor.GetType().GetProperty($property, $flags).SetValue($monitor, '100%')
    }
    $monitor.GetType().GetProperty('UploadBytesPerSecond', $flags).SetValue($monitor, [double](1023.9 * 1024))
    $monitor.GetType().GetProperty('DownloadBytesPerSecond', $flags).SetValue($monitor, [double](999.9 * 1024 * 1024))
    $control.Size = New-Object Drawing.Size 164,40
    $image = New-Object Drawing.Bitmap 164,40
    try { $control.DrawToBitmap($image, (New-Object Drawing.Rectangle 0,0,164,40)); $image.Save((Join-Path $imageRoot 'horizontal-max-values.png')) } finally { $image.Dispose() }
    $drawText = $control.GetType().GetMethod('DrawText', $flags)
    foreach ($foreground in @([Drawing.Color]::Black, [Drawing.Color]::White)) {
        $glyphs = New-Object Drawing.Bitmap 164,40
        $canvas = [Drawing.Graphics]::FromImage($glyphs)
        try {
            $canvas.Clear([Drawing.Color]::Transparent)
            $drawText.Invoke($control, @($canvas, 'CPU 100%', [Drawing.Rectangle]::new(18,0,120,40), $foreground)) | Out-Null
            $opaqueGlyphs = 0
            for ($y = 0; $y -lt 40; $y++) {
                for ($x = 18; $x -lt 138; $x++) {
                    $pixel = $glyphs.GetPixel($x,$y)
                    if ($pixel.A -gt 128 -and [Math]::Abs($pixel.R - $foreground.R) -le 5 -and [Math]::Abs($pixel.G - $foreground.G) -le 5 -and [Math]::Abs($pixel.B - $foreground.B) -le 5) { $opaqueGlyphs++ }
                }
            }
            if ($opaqueGlyphs -lt 20) { throw "Text lost its alpha channel: $foreground" }
            if ($glyphs.GetPixel(0,0).A -ne 0) { throw 'Text rendering added an opaque background.' }
            Write-Host "$foreground text: $opaqueGlyphs opaque glyph pixels on a transparent background."
        } finally { $canvas.Dispose(); $glyphs.Dispose() }
    }
    $backgroundHost = New-Object Windows.Forms.Panel
    try {
        $backgroundHost.Size = New-Object Drawing.Size 164,40
        $control.Size = $backgroundHost.Size
        $backgroundHost.Controls.Add($control)
        foreach ($background in @([Drawing.Color]::FromArgb(37,74,111), [Drawing.Color]::FromArgb(222,190,140))) {
            $backgroundHost.BackColor = $background
            $image = New-Object Drawing.Bitmap 164,40
            try {
                $backgroundHost.DrawToBitmap($image, (New-Object Drawing.Rectangle 0,0,164,40))
                if ($image.GetPixel(0,0).ToArgb() -ne $background.ToArgb()) { throw "The control obscures its parent background: $($image.GetPixel(0,0)) expected $background; parent=$($control.Parent) background=$($control.BackColor)." }
                $control.GetType().GetMethod('OnMouseEnter', $flags).Invoke($control, @([EventArgs]::Empty)) | Out-Null
                $backgroundHost.DrawToBitmap($image, (New-Object Drawing.Rectangle 0,0,164,40))
                if ($image.GetPixel(0,0).ToArgb() -ne $background.ToArgb()) { throw 'Hover obscures the parent background.' }
            } finally { $image.Dispose() }
        }
    } finally { $backgroundHost.Controls.Remove($control); $backgroundHost.Dispose() }
    $timer = $control.GetType().GetField('timer', $flags).GetValue($control)
    $visibility = $control.GetType().GetMethod('VisibilityChanged', $flags, $null, [Type[]]@([bool]), $null)
    $visibility.Invoke($control, @($false)) | Out-Null
    if ($timer.Enabled) { throw 'Timer did not stop when hidden.' }
    $visibility.Invoke($control, @($true)) | Out-Null
    if (!$timer.Enabled) { throw 'Timer did not resume when shown.' }
    $control.GetType().GetMethod('OnClose', $flags, $null, [Type[]]@(), $null).Invoke($control, @()) | Out-Null
    if ($timer.Enabled) { throw 'Timer did not stop on close.' }
    Write-Host 'PASS: sampling, bitmap rendering, black/white text alpha, transparent/hover backgrounds, hide/show and close lifecycle.'
} finally { $control.Dispose() }
