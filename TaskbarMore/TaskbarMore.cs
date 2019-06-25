using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Media;

namespace TaskbarMore {
    public partial class TaskbarMore : Form {
        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, String lpsz1, String lpsz2);
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        static extern int GetKeyboardState(byte[] pbKeyState);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        static extern IntPtr GetF();
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        static extern bool SetF(IntPtr hWnd);
        KeyboardHook KeyHook;
        bool StatusBool = false, HadKeyUp = true;
        /* InitializeComponent */
        public TaskbarMore() {
            InitializeComponent();
            KeyHook = new KeyboardHook();
            KeyHook.KeyDownEvent += new KeyEventHandler(TaskbarMore_Key_Down);
            KeyHook.KeyUpEvent += new KeyEventHandler(TaskbarMore_Key_Up);
            KeyHook.Start();
        }
        /* 使用 Caplock 键 */
        private static bool CapsLockStatus {
            get {
                byte[] KeyName = new byte[256];
                GetKeyboardState(KeyName);
                return (KeyName[0x14]==1);
            }
        }
        /* 窗体加载准备，检测键盘大小写状态 */
        private void TaskbarMore_Load(object sender, EventArgs e) {
            /* Size: 120, 22*/
            if (CapsLockStatus==true) {
                Status.Image = Properties.Resources.Capital;
                Tip.ToolTipTitle = "Capital";
                StatusBool = false;
            }
            else {
                Status.Image = Properties.Resources.Lower_case;
                Tip.ToolTipTitle = "Lower-case";
                StatusBool = true;
            }
            /* 获取任务栏句柄 */
            IntPtr hTaskbar = FindWindowEx((IntPtr)0, (IntPtr)0, "Shell_TrayWnd", null);
            IntPtr hBar = FindWindowEx(hTaskbar, (IntPtr)0, "ReBarWindow32", null);
            Size TaskBarSize = new Size(SystemInformation.WorkingArea.Width, SystemInformation.WorkingArea.Height);
            Point TaskBarPoint = new Point(this.Location.X, this.Location.Y);
            /* 设置窗体、控件的参数 */
            Status.Location = new Point(5, (this.Height >> 1) - (Status.Height >> 1));
            Time.Location = new Point(22, (this.Height >> 1) - (Time.Height >> 1));
            this.Location = new Point(TaskBarSize.Width - 520, 0);
            /* 将主窗体嵌入任务栏 */
            SetParent(this.Handle, hBar);
        }
        /* 按下键盘Caplock键 */
        private void TaskbarMore_Key_Down(object sender, KeyEventArgs e) {
            if (e.KeyCode==Keys.CapsLock && HadKeyUp) {
                SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.Tip);
                simpleSound.Play();
                /* 启用线程修改控件参数 */
                new Action(delegate {
                    Status.Invoke(new Action(delegate {
                        if (!StatusBool) {
                            Status.Image = Properties.Resources.Lower_case;
                            Tip.ToolTipTitle = "Lower-case";
                            StatusBool = true;
                        }
                        else {
                            Status.Image = Properties.Resources.Capital;
                            Tip.ToolTipTitle = "Capital";
                            StatusBool = false;
                        }
                        HadKeyUp = false;
                    }));
                }).BeginInvoke(null, null);
            }
        }
        /* 抬起键盘Caplock键 */
        private void TaskbarMore_Key_Up(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.CapsLock) HadKeyUp = true;
        }
        /* 关闭键盘钩子 */
        private void TaskbarMore_FormClosing(object sender, FormClosingEventArgs e) {
            KeyHook.Stop();
        }
        static int Minute = 59, Second = 59;
        /* 计时器 */
        private void Timer_Tick(object sender, EventArgs e) {
            if (Second>0) {
                Second--;
            }
            else {
                if (Minute>0) {
                    Minute--;
                    Second = 59;
                }
            }
            if (Minute<10) {
                if (Second<10) Time.Text = "0" + Minute + ":" + "0" + Second;
                else Time.Text = "0" + Minute + ":" + Second;
            }
            else {
                if (Second<10) Time.Text = Minute + ":" + "0" + Second;
                else Time.Text = Minute + ":" + Second;
            }
            if (Minute==0 && Second<10) {
                Time.ForeColor = Color.Red;
                if (Minute==0 && Second==0) {
                    TimerMain.Enabled = false;
                    TimerMain.Stop();
                    if (MessageBox.Show("The develop time is over.Do you want to reset?", "TaskbarMore Timer Tip", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                        ==DialogResult.Yes) System.Windows.Forms.Application.Restart();
                }
            }
        }
        /* 鼠标悬空事件 */
        private void TaskbarMore_MouseHover(object sender, EventArgs e) {
            this.BackColor = Color.FromArgb(30, 30, 30);
            if (CapsLockStatus==true)
                Tip.ToolTipTitle = "Capital";
            else
                Tip.ToolTipTitle = "Lower-case";
        }
        /* 鼠标悬空移动事件 */
        private void TaskbarMore_MouseMove(object sender, MouseEventArgs e) {
            this.BackColor = Color.FromArgb(30, 30, 30);
        }
        /* 鼠标离开事件 */
        private void TaskbarMore_MouseLeave(object sender, EventArgs e) {
            this.BackColor = Color.Black;
        }
        /* 菜单：重置计时器（重启程序） */
        private void Reset_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Sure to reset?", "TaskbarMore Reset Tip", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                System.Windows.Forms.Application.Restart();
        }
        /* 菜单：启动计时器，启动TimerMain */
        private void On_Click(object sender, EventArgs e) {
            TimerMain.Enabled = true;
            TimerMain.Start();
            Time.ForeColor = Color.White;
        }
        /* 菜单：暂停计时器，关闭TimerMain */
        private void Pause_Click(object sender, EventArgs e) {
            TimerMain.Enabled = false;
            TimerMain.Stop();
            Time.ForeColor = Color.Orange;
        }
        /* 菜单：隐藏计时器，将Time居中 */
        private void End_Click(object sender, EventArgs e) {
            Time.Visible = false;
            Status.Location = new Point((this.Width >> 1) - (Status.Width >> 1), (this.Height >> 1) - (Status.Height >> 1));
        }
        /* 菜单：注册表自启动，开启 */
        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            string KeyName = "TaskbarMore";
            try {
                string strName = Application.ExecutablePath;
                if (!File.Exists(strName)) return;
                Microsoft.Win32.RegistryKey Rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (Rkey==null) {
                    Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.SetValue(KeyName, strName + " -s");
                MessageBox.Show("The program will boot starting!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch {
                MessageBox.Show("Please use the administrator's permission to open the application!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /* 菜单：注册表自启动，关闭 */
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            string KeyName = "TaskbarMore";
            try {
                string strName = Application.ExecutablePath;
                RegistryKey Rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (Rkey==null) {
                    Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.DeleteValue(KeyName, false);
                MessageBox.Show("The program will not boot starting!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch {
                MessageBox.Show("Please use the administrator's permission to open the application!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /* 菜单：关于框 */
        private void About_Click(object sender, EventArgs e) {
            MessageBox.Show("Ii is powered by GUI.\n\rYou can use it to indicate time(only an hour) and CapsLockStatus.", "TaskbarMore About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /* 菜单：程序结束 */
        private void Exit_Click(object sender, EventArgs e) {
            this.Close();
            this.Dispose();
            Application.Exit();
        }
    }
}