using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using NetWorkSpeedMonitor;
using KeyboardHookNS;
using Microsoft.Win32;
using System.IO;

namespace TaskbarMore
{
    public partial class TaskbarMore : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(String lpsz1, String lpsz2);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, String lpsz1, String lpsz2);
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
        [DllImport("user32.dll")]
        static extern int GetWindowRect(IntPtr hwnd, out Rectangle lpRect);
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        private readonly KeyboardHook KeyHook;
        private readonly NetworkAdapter[] Adapters;
        private readonly NetworkMonitor Monitor;
        private readonly PerformanceCounter CPU_Counter;
        private readonly PerformanceCounter RAM_Counter;
        private readonly Timer TaskbarMoreTimer;
        private readonly long RAM_ALL;

        private readonly IntPtr MSTaskSwWClass;
        private readonly IntPtr MSTaskListWClass;
        private readonly IntPtr ReBarWindow32;
        private Rectangle MSTaskSwWClassRect;
        private Rectangle MSTaskListWClassRect;
        private int MSTaskSwWClassRectBeforeRight;

        public TaskbarMore()
        {
            InitializeComponent();

            IntPtr Shell_TrayWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
            ReBarWindow32 = FindWindowEx(Shell_TrayWnd, IntPtr.Zero, "ReBarWindow32", null);
            MSTaskSwWClass = FindWindowEx(ReBarWindow32, IntPtr.Zero, "MSTaskSwWClass", null);
            MSTaskListWClass = FindWindowEx(MSTaskSwWClass, IntPtr.Zero, "MSTaskListWClass", null);

            KeyHook = new KeyboardHook();
            KeyHook.KeyUpEvent += new KeyEventHandler(TaskbarMore_KeyUp);
            KeyHook.Start();
            if (Console.CapsLock)
            {
                this.Key.Image = Properties.Resources.Capital;
            }
            else
            {
                this.Key.Image = Properties.Resources.Lower_case;
            }

            Monitor = new NetworkMonitor();
            Adapters = Monitor.Adapters;
            if (Adapters.Length == 0)
            {
                MessageBox.Show("No network adapters found on this computer.");
            }
            Monitor.StopMonitoring();
            Monitor.StartMonitoring(Adapters[0]);

            ManagementClass MC = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection MOC = MC.GetInstances();
            foreach (ManagementObject MO in MOC)
            {
                if (MO["TotalPhysicalMemory"] != null)
                {
                    RAM_ALL = long.Parse(MO["TotalPhysicalMemory"].ToString()) >> 20;
                }
            }

            CPU_Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            RAM_Counter = new PerformanceCounter("Memory", "Available MBytes");

            TaskbarMoreTimer = new Timer();
            TaskbarMoreTimer.Tick += TaskbarMore_ShowInfo;
            TaskbarMoreTimer.Interval = 1000;
            TaskbarMoreTimer.Enabled = true;
            //40 || 30
        }

        private void TaskbarMore_Load(object sender, EventArgs e)
        {
            MSTaskSwWClassRect = new Rectangle();
            GetWindowRect(MSTaskSwWClass, out MSTaskSwWClassRect);
            MSTaskListWClassRect = new Rectangle();
            GetWindowRect(MSTaskListWClass, out MSTaskListWClassRect);
            MSTaskSwWClassRectBeforeRight = MSTaskSwWClassRect.Right;
            MoveWindow(MSTaskListWClass, 0, 0, MSTaskSwWClassRect.Right - MSTaskListWClassRect.Left - MSTaskListWClassRect.X - this.Width, MSTaskListWClassRect.Bottom - MSTaskListWClassRect.Top - MSTaskListWClassRect.Y, true);
            this.Left = MSTaskSwWClassRect.Width - this.Width - MSTaskSwWClassRect.Left;
            this.Top = 1;
            SetParent(this.Handle, ReBarWindow32);
        }

        private void TaskbarMore_FormClosing(object sender, FormClosingEventArgs e)
        {
            MoveWindow(MSTaskListWClass, 0, 0, MSTaskSwWClassRect.Right - MSTaskListWClassRect.Left - MSTaskListWClassRect.X, MSTaskListWClassRect.Bottom - MSTaskListWClassRect.Top - MSTaskListWClassRect.Y, true);
            KeyHook.Stop();
            TaskbarMoreTimer.Stop();
            TaskbarMoreTimer.Dispose();
            Monitor.StopMonitoring();
            CPU_Counter.Close();
            CPU_Counter.Dispose();
            RAM_Counter.Close();
            RAM_Counter.Dispose();
        }

        private void TaskbarMore_ShowInfo(object sender, EventArgs e)
        {
            GetWindowRect(MSTaskSwWClass, out MSTaskSwWClassRect);
            if (MSTaskSwWClassRectBeforeRight != MSTaskSwWClassRect.Right)
            {
                GetWindowRect(MSTaskListWClass, out MSTaskListWClassRect);
                MSTaskSwWClassRectBeforeRight = MSTaskSwWClassRect.Right;
                MoveWindow(MSTaskListWClass, 0, 0, MSTaskSwWClassRect.Right - MSTaskListWClassRect.Left - MSTaskListWClassRect.X - this.Width, MSTaskListWClassRect.Bottom - MSTaskListWClassRect.Top - MSTaskListWClassRect.Y, true);
                MoveWindow(this.Handle, MSTaskSwWClassRect.Width - this.Width - MSTaskSwWClassRect.Left, 1, this.Width, this.Height, true);
                SetParent(this.Handle, MSTaskSwWClass);
            }

            double UploadSpeedKbps = Adapters[0].UploadSpeedKbps;
            double DownloadSpeedKbps = Adapters[0].DownloadSpeedKbps;
            if (UploadSpeedKbps < 100)
            {
                this.Upload_Text.Text = string.Format("{0:0.00} K/s", UploadSpeedKbps);
            }
            else
            {
                this.Upload_Text.Text = string.Format("{0:0.0} M/s", UploadSpeedKbps / 1024);
            }
            if (DownloadSpeedKbps < 100)
            {
                this.Download_Text.Text = string.Format("{0:0.00} K/s", DownloadSpeedKbps);
            }
            else
            {
                this.Download_Text.Text = string.Format("{0:0.0} M/s", DownloadSpeedKbps / 1024);
            }
            this.CPU_Text.Text = string.Format("{0}%", (int)CPU_Counter.NextValue());
            this.RAM_Text.Text = string.Format("{0}%", (int)((1 - RAM_Counter.NextValue() / RAM_ALL) * 100));
        }

        private void TaskbarMore_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
        }

        private void TaskbarMore_MouseMove(object sender, MouseEventArgs e)
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
        }

        private void TaskbarMore_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
        }

        private void TaskbarMore_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Capital)
            {
                if (Console.CapsLock)
                {
                    this.Key.Image = Properties.Resources.Capital;
                }
                else
                {
                    this.Key.Image = Properties.Resources.Lower_case;
                }
            }
        }

        private void StartupMenuItem_Click(object sender, EventArgs e)
        {
            string KeyName = "TaskbarMore";
            try
            {
                string strName = Application.ExecutablePath;
                if (!File.Exists(strName)) return;
                Microsoft.Win32.RegistryKey Rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (Rkey == null)
                {
                    Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.SetValue(KeyName, strName + " -s");
                MessageBox.Show("The program will boot starting!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {
                MessageBox.Show("Please use the administrator's permission to open the application!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnStartupMenuItem_Click(object sender, EventArgs e)
        {
            string KeyName = "TaskbarMore";
            try
            {
                string strName = Application.ExecutablePath;
                RegistryKey Rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (Rkey == null)
                {
                    Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.DeleteValue(KeyName, false);
                MessageBox.Show("The program will not boot starting!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {
                MessageBox.Show("Please use the administrator's permission to open the application!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It is powered by GUI.\n\rIt can display network, CPU, RAM, caps lock status.", "TaskbarMore About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Application.Exit();
        }
    }
}
