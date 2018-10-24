using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Media;
namespace TaskbarMore{
    public partial class TaskbarMore:Form{
        KeyboardHook k_hook;//引入一个全局钩子
        public TaskbarMore(){
            InitializeComponent();
            //定义钩子
            k_hook = new KeyboardHook();
            //将按键事件绑进钩子并开始安装钩子
            k_hook.KeyDownEvent += new KeyEventHandler(TaskbarMore_Key);
            k_hook.Start();
        }
        //NotifyIcon托盘区图标
        //private NotifyIcon notifyIcon = null;
        //窗体嵌入API调用
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SetParen(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, String lpsz1, String lpsz2);
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndNewParent);
        //窗体显示自动移动调节API
        //[DllImport("user32.dll")]
        //static extern IntPtr MoveWindow(IntPtr hWnd, IntPtr x, IntPtr y, int nWidth, int nHeight, int bRepaint);
        //任务栏信息获取API
        Size TaskBarSize = new Size(SystemInformation.WorkingArea.Width, SystemInformation.WorkingArea.Height);
        //全局按键API
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        //活动窗体API
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetF();
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetF(IntPtr hWnd);
        public static bool CapsLockStatus{//使用Caplock键
            get{
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x14] == 1);
            }
        }
        private void TaskbarMore_Load(object sender, EventArgs e){
            //notifyIcon提示气泡
            //notifyIcon = new NotifyIcon();
            //notifyIcon.BalloonTipText = "TakbarMore is starting";
            //notifyIcon.Text = "TakbarMore";
            //notifyIcon.Icon = Properties.Resources.TaskbarMore;
            //notifyIcon.Visible = true;
            //notifyIcon.ShowBalloonTip(2000);
            //检测键盘大小写状态,改变相应参数
            if(CapsLockStatus==true){
                label1.BackColor=Color.OrangeRed;
                toolTip1.ToolTipTitle="Capital";
            }              
            else{
                label1.BackColor=Color.FromArgb(66,200,0);
                toolTip1.ToolTipTitle="Lower-case";
            }              
            //获取任务栏句柄
            IntPtr hTaskbar=FindWindow("Shell_TrayWnd",null);
            IntPtr hBar=FindWindowEx(hTaskbar,(IntPtr)0,"ReBarWindow32",null);
            //设置窗体、控件的参数
            this.Width=label2.Width*8/5;
            label1.Location=new Point(label1.Width/3,this.Height/2-label1.Height/2+1);
            label2.Location=new Point(22,this.Height/2-label2.Height/2+1);
            this.Location=new Point(TaskBarSize.Width-485,0);
            //将主窗体嵌入任务栏
            SetParent(this.Handle,hBar);
        }
        private void TaskbarMore_Key(object sender, KeyEventArgs e){//抬起键盘Caplock键
            if(e.KeyCode==Keys.CapsLock){//提示音
                SoundPlayer simpleSound=new SoundPlayer(Properties.Resources.Tip);
                simpleSound.Play();
                new Action(delegate{//启用线程修改控件参数
                    label1.Invoke(new Action(delegate{
                        if(label1.BackColor==Color.OrangeRed){
                            label1.BackColor=Color.FromArgb(66,200,0);
                            toolTip1.ToolTipTitle="Lower-case";
                            return;
                        }
                        if(label1.BackColor==Color.FromArgb(66,200,0)){
                            label1.BackColor = Color.OrangeRed;
                            toolTip1.ToolTipTitle="Capital";
                            return;
                        }
                    }));
                }).BeginInvoke(null,null); 
            }
        }
        private void TaskbarMore_FormClosing(object sender, FormClosingEventArgs e){//关闭键盘钩子
            k_hook.Stop();
        }
        int minute=59,second=59;//定时器
        private void timer1_Tick(object sender, EventArgs e){
            if(second>0){
                second--;
            }
            else{
                if(minute>0){
                    minute--;
                    second=59;
                }
            }
            if(minute<10){
                if(second<10)
                    label2.Text="0"+minute+":"+"0"+second;
                else
                    label2.Text="0"+minute+":"+second;
            }    
            else{
                if(second<10)
                    label2.Text=minute+":"+"0"+second;
                else
                    label2.Text=minute+":"+second;
            }
            if(minute==0&&second<10){
                label2.ForeColor=Color.Red;
                if(minute==0&&second==0){
                    timer1.Enabled=false;
                    timer1.Stop();
                    if (MessageBox.Show("The develop time is over.Do you want to reset?", "TaskbarMore Timer Tip", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                        System.Windows.Forms.Application.Restart();
                }
            }   
        }
        //鼠标悬空事件
        private void TaskbarMore_MouseHover(object sender, EventArgs e){
            this.BackColor = Color.FromArgb(30,30,30);
            if(CapsLockStatus==true)
                toolTip1.ToolTipTitle="Capital";
            else
                toolTip1.ToolTipTitle="Lower-case";
        }
        private void TaskbarMore_MouseMove(object sender, MouseEventArgs e){
            this.BackColor=Color.FromArgb(30,30,30);
        }
        private void TaskbarMore_MouseLeave(object sender, EventArgs e){
            this.BackColor=Color.Black;
        }
        //菜单
        private void Reset_Click_1(object sender, EventArgs e){//重置计时器（重启程序）
            if(MessageBox.Show("Sure to reset?", "TaskbarMore Reset Tip", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                System.Windows.Forms.Application.Restart();
        }
        private void On_Click(object sender, EventArgs e){//播放计时器，启动timer1
            timer1.Enabled=true;
            timer1.Start();
            label2.ForeColor=Color.White;
        }
        private void Pause_Click(object sender, EventArgs e){//暂停计时器，关闭timer1
            timer1.Enabled=false;
            timer1.Stop();
            label2.ForeColor=Color.Orange;
        }
        private void End_Click(object sender, EventArgs e){//隐藏计时器，将label1居中
            label2.Visible=false;
            label1.Location=new Point(this.Width/2-label1.Width/2,this.Height/2-label1.Height/2+1);
        }
        public string KeyName = "TaskbarMore";//注册表自启动名称
        private void openToolStripMenuItem_Click(object sender, EventArgs e){//注册表自启动，开启
            try{
                string strName=Application.ExecutablePath;
                if(!File.Exists(strName))  
                    return;
                Microsoft.Win32.RegistryKey Rkey=Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",true);
                if(Rkey==null){
                    Rkey=Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.SetValue(KeyName,strName+" -s");
                MessageBox.Show("The program will boot starting!","TaskbarMore Boot Tip",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch{
                MessageBox.Show("Please use the administrator's permission to open the application!","TaskbarMore Boot Tip",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e){//注册表自启动，关闭
            try{
                string strName=Application.ExecutablePath;
                RegistryKey Rkey=Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",true);
                if(Rkey==null){
                    Rkey=Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.DeleteValue(KeyName,false);
                MessageBox.Show("The program will not boot starting!","TaskbarMore Boot Tip",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch{
                MessageBox.Show("Please use the administrator's permission to open the application!","TaskbarMore Boot Tip",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void About_Click(object sender, EventArgs e){//关于程序的对话框
            MessageBox.Show("Ii is powered by GUI.\n\rYou can use it to indicate time(only an hour) and CapsLockStatus.","TaskbarMore About",MessageBoxButtons.OK,MessageBoxIcon.Information);
        } 
        private void Exit_Click(object sender, EventArgs e){//关闭窗体，结束程序
            this.Close();
            this.Dispose();
            Application.Exit();
        }              
    }
}