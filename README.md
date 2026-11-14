# TaskbarMore

Windows 10 taskbar DeskBand displaying upload/download speed, CPU usage, memory usage and CapsLock status. The only file to distribute is **TaskbarMore.exe** (.NET Framework 4.7.2 required).

## Run

Place `TaskbarMore.exe` in a permanent folder and double-click it. The first run requests administrator permission to register the COM control, then asks Explorer to display the toolbar. Subsequent launches show the registered toolbar. The EXE exits after registration/display; Explorer hosts the control from the same EXE assembly. No CSDeskBand DLLs or extracted runtime files are needed.

Enable **TaskbarMore** from taskbar right-click → Toolbars if needed. Explorer remembers visibility between sessions. Keep the EXE in its registered location. Close a previously running standalone version when migrating. Registration removes legacy TaskbarMore EXE startup entries.

To uninstall:

```powershell
Start-Process .\TaskbarMore.exe -ArgumentList '--uninstall' -Wait
```

This hides the toolbar and requests administrator permission to unregister it. Explorer may retain the loaded assembly until sign-out, so close the toolbar and sign out/in before replacing or deleting a loaded EXE. Updating an already loaded version may also require sign-out/in.

## Build and verify

Requires Visual Studio Build Tools with .NET desktop build tools:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\scripts\build.ps1
powershell -NoProfile -STA -ExecutionPolicy Bypass -File .\scripts\verify.ps1
```

Output: `TaskbarMore/bin/AnyCPU/Release/TaskbarMore.exe`. Debug symbols are optional and need not be distributed. The build script downloads .NET Framework 4.7.2 reference assemblies from NuGet if needed.

For development, `scripts/register.ps1` copies only the EXE into a versioned directory under `artifacts/deskband` and launches it, leaving build output unlocked. Use `-Action Uninstall` to remove that installation.

## Implementation

- CSDeskBand core and WinForms source files compile directly into TaskbarMore.exe; there are no separate library projects or assembly merging steps.
- Explorer manages taskbar layout through DeskBand interfaces; no taskbar button windows are manually resized.
- CPU, memory and network samples update every second; CapsLock is polled every 200 ms without a global keyboard hook.
- Network speeds sum active non-loopback/non-tunnel interfaces using elapsed time. Virtual adapters may count the same traffic twice.
- Buffered painting supports light/dark themes, DPI scaling and vertical taskbars.
- Timers, counters, menus and fonts follow control visibility/close/disposal lifecycle.
- Windows 11's native taskbar does not support classic DeskBand toolbars.

`vendor/CSDeskBand` contains the MIT-licensed source from the supplied CSDeskBand 2.1.0 checkout. Its license is retained in the repository. Original author: GUI / GuEe Studio, geeesgui@gmail.com.
