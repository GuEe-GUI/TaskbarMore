using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using CSDeskBand.Interop.COM;
using Microsoft.Win32;

namespace TaskbarMore
{
    internal static class Program
    {
        private static readonly Guid BandId = new Guid("A96DCE38-8F92-4B9D-B780-024E43CFB865");
        private static string RegistrationKey => @"CLSID\" + BandId.ToString("B") + @"\InprocServer32";

        [STAThread]
        private static int Main(string[] args)
        {
            try
            {
                string action = args.Length == 0 ? "--install" : args[0].ToLowerInvariant();
                if (action == "--register" || action == "--unregister")
                {
                    if (!IsAdministrator()) return Elevate(action);
                    var services = new RegistrationServices();
                    var assembly = Assembly.GetExecutingAssembly();
                    bool success = action == "--register"
                        ? services.RegisterAssembly(assembly, AssemblyRegistrationFlags.SetCodeBase)
                        : services.UnregisterAssembly(assembly);
                    if (!success) throw new InvalidOperationException("No registerable DeskBand control was found.");
                    if (action == "--register")
                    {
                        PrunePreviousRegistrations();
                        RemoveLegacyStartup();
                    }
                    return 0;
                }
                if (action == "--uninstall")
                {
                    SetVisible(false);
                    int result = Elevate("--unregister");
                    return result;
                }
                if (action != "--install")
                {
                    MessageBox.Show("Double-click to install and show the toolbar.\nUse --uninstall to unregister it.", "TaskbarMore");
                    return 1;
                }
                bool registeredHere;
                using (var key = Registry.ClassesRoot.OpenSubKey(RegistrationKey))
                {
                    registeredHere = string.Equals(Uri.UnescapeDataString(key?.GetValue("CodeBase") as string ?? ""),
                        Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().Location).AbsoluteUri), StringComparison.OrdinalIgnoreCase)
                        && string.Equals(key?.GetValue("Assembly") as string,
                            Assembly.GetExecutingAssembly().FullName, StringComparison.OrdinalIgnoreCase);
                }
                if (!registeredHere)
                {
                    SetVisible(false);
                    if (Elevate("--register") != 0) return 1;
                }
                if (!SetVisible(true))
                    throw new InvalidOperationException("The toolbar is registered, but Explorer has not shown it yet. Right-click the taskbar, open Toolbars, and select TaskbarMore. After updating a loaded version, you may need to sign out and sign back in.");
                return 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "TaskbarMore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
        }

        private static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static int Elevate(string action)
        {
            using (var process = Process.Start(new ProcessStartInfo(Assembly.GetExecutingAssembly().Location, action)
            {
                UseShellExecute = true,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden
            }))
            {
                if (process == null) throw new InvalidOperationException("Unable to start the registration process.");
                process.WaitForExit();
                return process.ExitCode;
            }
        }

        private static bool SetVisible(bool visible)
        {
            var tray = (ITrayDeskband)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("E6442437-6C68-4f52-94DD-2CFED267EFB9")));
            try
            {
                Guid id = BandId;
                tray.DeskBandRegistrationChanged();
                int result = visible ? tray.ShowDeskBand(ref id) : tray.HideDeskBand(ref id);
                if (result < 0) Marshal.ThrowExceptionForHR(result);
                if (!visible) return true;
                // Explorer may complete activation asynchronously.
                for (int attempt = 0; attempt < 100; attempt++)
                {
                    if (tray.IsDeskBandShown(ref id) == 0) return true;
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
                return false;
            }
            finally { Marshal.ReleaseComObject(tray); }
        }

        private static void PrunePreviousRegistrations()
        {
            // Drop version-specific codebases left by the earlier multi-DLL installation.
            using (var key = Registry.ClassesRoot.OpenSubKey(RegistrationKey, true))
            {
                if (key == null) return;
                string current = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                foreach (string name in key.GetSubKeyNames())
                    if (name != current && Version.TryParse(name, out _)) key.DeleteSubKeyTree(name);
            }
        }
        private static void RemoveLegacyStartup()
        {
            foreach (var hive in new[] { Registry.CurrentUser, Registry.LocalMachine })
            {
                using (var key = hive.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    string value = key?.GetValue("TaskbarMore") as string;
                    if (value != null && value.IndexOf("TaskbarMore.exe", StringComparison.OrdinalIgnoreCase) >= 0)
                        key.DeleteValue("TaskbarMore", false);
                }
            }
        }
    }
}
