namespace TaskbarMore
{
    partial class TaskbarMore
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
<<<<<<< HEAD
        /// 设计器支持所需的方法 - 不要修改
=======
        /// 设计器支持所需的方法 - 不要
>>>>>>> 25b12ccfb54d31de3f1f3b0774ac87a84c562c9c
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskbarMore));
<<<<<<< HEAD
            this.Upload_Text = new System.Windows.Forms.Label();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unStartupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Download_Text = new System.Windows.Forms.Label();
            this.CPU_Title = new System.Windows.Forms.Label();
            this.RAM_Title = new System.Windows.Forms.Label();
            this.RAM_Text = new System.Windows.Forms.Label();
            this.CPU_Text = new System.Windows.Forms.Label();
            this.Download_Img = new System.Windows.Forms.PictureBox();
            this.Upload_Img = new System.Windows.Forms.PictureBox();
            this.Key = new System.Windows.Forms.PictureBox();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Download_Img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Upload_Img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Key)).BeginInit();
            this.SuspendLayout();
            // 
            // Upload_Text
            // 
            this.Upload_Text.AutoSize = true;
            this.Upload_Text.BackColor = System.Drawing.Color.Transparent;
            this.Upload_Text.ContextMenuStrip = this.contextMenu;
            this.Upload_Text.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Upload_Text.ForeColor = System.Drawing.Color.White;
            this.Upload_Text.Location = new System.Drawing.Point(13, 2);
            this.Upload_Text.Name = "Upload_Text";
            this.Upload_Text.Size = new System.Drawing.Size(49, 16);
            this.Upload_Text.TabIndex = 2;
            this.Upload_Text.Text = "0.00 K/s";
            this.Upload_Text.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Upload_Text.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Upload_Text.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startupMenuItem,
            this.unStartupMenuItem,
            this.aboutMenuItem,
            this.exitMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.ShowImageMargin = false;
            this.contextMenu.Size = new System.Drawing.Size(127, 92);
            // 
            // startupMenuItem
            // 
            this.startupMenuItem.Name = "startupMenuItem";
            this.startupMenuItem.Size = new System.Drawing.Size(126, 22);
            this.startupMenuItem.Text = "Startup(&S)";
            this.startupMenuItem.Click += new System.EventHandler(this.startupMenuItem_Click);
            // 
            // unStartupMenuItem
            // 
            this.unStartupMenuItem.Name = "unStartupMenuItem";
            this.unStartupMenuItem.Size = new System.Drawing.Size(126, 22);
            this.unStartupMenuItem.Text = "UnStartup(&U)";
            this.unStartupMenuItem.Click += new System.EventHandler(this.unStartupMenuItem_Click);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aboutMenuItem.Text = "About(&a)";
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(126, 22);
            this.exitMenuItem.Text = "Exit(&E)";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // Download_Text
            // 
            this.Download_Text.AutoSize = true;
            this.Download_Text.BackColor = System.Drawing.Color.Transparent;
            this.Download_Text.ContextMenuStrip = this.contextMenu;
            this.Download_Text.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Download_Text.ForeColor = System.Drawing.Color.White;
            this.Download_Text.Location = new System.Drawing.Point(13, 21);
            this.Download_Text.Name = "Download_Text";
            this.Download_Text.Size = new System.Drawing.Size(49, 16);
            this.Download_Text.TabIndex = 3;
            this.Download_Text.Text = "0.00 K/s";
            this.Download_Text.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Download_Text.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Download_Text.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // CPU_Title
            // 
            this.CPU_Title.AutoSize = true;
            this.CPU_Title.BackColor = System.Drawing.Color.Transparent;
            this.CPU_Title.ContextMenuStrip = this.contextMenu;
            this.CPU_Title.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CPU_Title.ForeColor = System.Drawing.Color.White;
            this.CPU_Title.Location = new System.Drawing.Point(69, 2);
            this.CPU_Title.Name = "CPU_Title";
            this.CPU_Title.Size = new System.Drawing.Size(30, 16);
            this.CPU_Title.TabIndex = 4;
            this.CPU_Title.Text = "CPU";
            this.CPU_Title.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.CPU_Title.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.CPU_Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // RAM_Title
            // 
            this.RAM_Title.AutoSize = true;
            this.RAM_Title.BackColor = System.Drawing.Color.Transparent;
            this.RAM_Title.ContextMenuStrip = this.contextMenu;
            this.RAM_Title.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RAM_Title.ForeColor = System.Drawing.Color.White;
            this.RAM_Title.Location = new System.Drawing.Point(69, 21);
            this.RAM_Title.Name = "RAM_Title";
            this.RAM_Title.Size = new System.Drawing.Size(34, 16);
            this.RAM_Title.TabIndex = 5;
            this.RAM_Title.Text = "RAM";
            this.RAM_Title.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.RAM_Title.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.RAM_Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // RAM_Text
            // 
            this.RAM_Text.AutoSize = true;
            this.RAM_Text.BackColor = System.Drawing.Color.Transparent;
            this.RAM_Text.ContextMenuStrip = this.contextMenu;
            this.RAM_Text.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RAM_Text.ForeColor = System.Drawing.Color.White;
            this.RAM_Text.Location = new System.Drawing.Point(98, 21);
            this.RAM_Text.Name = "RAM_Text";
            this.RAM_Text.Size = new System.Drawing.Size(30, 16);
            this.RAM_Text.TabIndex = 7;
            this.RAM_Text.Text = "00%";
            this.RAM_Text.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.RAM_Text.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.RAM_Text.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // CPU_Text
            // 
            this.CPU_Text.AutoSize = true;
            this.CPU_Text.BackColor = System.Drawing.Color.Transparent;
            this.CPU_Text.ContextMenuStrip = this.contextMenu;
            this.CPU_Text.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CPU_Text.ForeColor = System.Drawing.Color.White;
            this.CPU_Text.Location = new System.Drawing.Point(98, 2);
            this.CPU_Text.Name = "CPU_Text";
            this.CPU_Text.Size = new System.Drawing.Size(30, 16);
            this.CPU_Text.TabIndex = 6;
            this.CPU_Text.Text = "00%";
            this.CPU_Text.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.CPU_Text.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.CPU_Text.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // Download_Img
            // 
            this.Download_Img.ContextMenuStrip = this.contextMenu;
            this.Download_Img.Image = global::TaskbarMore.Properties.Resources.Download;
            this.Download_Img.Location = new System.Drawing.Point(5, 25);
            this.Download_Img.Name = "Download_Img";
            this.Download_Img.Size = new System.Drawing.Size(7, 8);
            this.Download_Img.TabIndex = 1;
            this.Download_Img.TabStop = false;
            this.Download_Img.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Download_Img.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Download_Img.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // Upload_Img
            // 
            this.Upload_Img.ContextMenuStrip = this.contextMenu;
            this.Upload_Img.Image = global::TaskbarMore.Properties.Resources.Upload;
            this.Upload_Img.Location = new System.Drawing.Point(5, 6);
            this.Upload_Img.Name = "Upload_Img";
            this.Upload_Img.Size = new System.Drawing.Size(7, 8);
            this.Upload_Img.TabIndex = 0;
            this.Upload_Img.TabStop = false;
            this.Upload_Img.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Upload_Img.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Upload_Img.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // Key
            // 
            this.Key.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Key.ContextMenuStrip = this.contextMenu;
            this.Key.Image = global::TaskbarMore.Properties.Resources.Lower_case;
            this.Key.Location = new System.Drawing.Point(130, 11);
            this.Key.Name = "Key";
            this.Key.Size = new System.Drawing.Size(16, 18);
            this.Key.TabIndex = 8;
            this.Key.TabStop = false;
            this.Key.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Key.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Key.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // TaskbarMore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(150, 39);
            this.ContextMenuStrip = this.contextMenu;
            this.Controls.Add(this.Key);
            this.Controls.Add(this.RAM_Text);
            this.Controls.Add(this.CPU_Text);
            this.Controls.Add(this.RAM_Title);
            this.Controls.Add(this.CPU_Title);
            this.Controls.Add(this.Download_Text);
            this.Controls.Add(this.Upload_Text);
            this.Controls.Add(this.Download_Img);
            this.Controls.Add(this.Upload_Img);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskbarMore";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaskbarMore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskbarMore_FormClosing);
            this.Load += new System.EventHandler(this.TaskbarMore_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TaskbarMore_KeyUp);
            this.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            this.contextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Download_Img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Upload_Img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Key)).EndInit();
=======
            this.Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Timer = new System.Windows.Forms.ToolStripMenuItem();
            this.Reset = new System.Windows.Forms.ToolStripMenuItem();
            this.On = new System.Windows.Forms.ToolStripMenuItem();
            this.Pause = new System.Windows.Forms.ToolStripMenuItem();
            this.End = new System.Windows.Forms.ToolStripMenuItem();
            this.Boot = new System.Windows.Forms.ToolStripMenuItem();
            this.Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Close = new System.Windows.Forms.ToolStripMenuItem();
            this.About = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Time = new System.Windows.Forms.Label();
            this.TimerMain = new System.Windows.Forms.Timer(this.components);
            this.Tip = new System.Windows.Forms.ToolTip(this.components);
            this.Status = new System.Windows.Forms.PictureBox();
            this.Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Status)).BeginInit();
            this.SuspendLayout();
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.Color.White;
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Timer,
            this.Boot,
            this.About,
            this.Exit});
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(128, 92);
            // 
            // Timer
            // 
            this.Timer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Reset,
            this.On,
            this.Pause,
            this.End});
            this.Timer.Image = global::TaskbarMore.Properties.Resources.Timer;
            this.Timer.Name = "Timer";
            this.Timer.Size = new System.Drawing.Size(127, 22);
            this.Timer.Text = "Timer(&T)";
            // 
            // Reset
            // 
            this.Reset.Image = global::TaskbarMore.Properties.Resources.Reset;
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(125, 22);
            this.Reset.Text = "Reset(&R)";
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // On
            // 
            this.On.Image = global::TaskbarMore.Properties.Resources.On;
            this.On.Name = "On";
            this.On.Size = new System.Drawing.Size(125, 22);
            this.On.Text = "On(&O)";
            this.On.Click += new System.EventHandler(this.On_Click);
            // 
            // Pause
            // 
            this.Pause.Image = global::TaskbarMore.Properties.Resources.Pause;
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(125, 22);
            this.Pause.Text = "Pause(&P)";
            this.Pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // End
            // 
            this.End.Image = global::TaskbarMore.Properties.Resources.End;
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(125, 22);
            this.End.Text = "End(&E)";
            this.End.Click += new System.EventHandler(this.End_Click);
            // 
            // Boot
            // 
            this.Boot.BackColor = System.Drawing.Color.White;
            this.Boot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open,
            this.Close});
            this.Boot.ForeColor = System.Drawing.Color.Black;
            this.Boot.Image = ((System.Drawing.Image)(resources.GetObject("Boot.Image")));
            this.Boot.Name = "Boot";
            this.Boot.Size = new System.Drawing.Size(127, 22);
            this.Boot.Text = "Boot(&B)";
            // 
            // Open
            // 
            this.Open.BackColor = System.Drawing.Color.White;
            this.Open.ForeColor = System.Drawing.Color.Black;
            this.Open.Image = global::TaskbarMore.Properties.Resources.Open;
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(126, 22);
            this.Open.Text = "Open(&O)";
            this.Open.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // Close
            // 
            this.Close.BackColor = System.Drawing.Color.White;
            this.Close.ForeColor = System.Drawing.Color.Black;
            this.Close.Image = global::TaskbarMore.Properties.Resources.Close;
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(126, 22);
            this.Close.Text = "Close(&C)";
            this.Close.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // About
            // 
            this.About.BackColor = System.Drawing.Color.White;
            this.About.ForeColor = System.Drawing.Color.Black;
            this.About.Image = global::TaskbarMore.Properties.Resources.About;
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(127, 22);
            this.About.Text = "About(&A)";
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // Exit
            // 
            this.Exit.BackColor = System.Drawing.Color.White;
            this.Exit.ForeColor = System.Drawing.Color.Black;
            this.Exit.Image = global::TaskbarMore.Properties.Resources.Exit;
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(127, 22);
            this.Exit.Text = "Exit(&E)";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Time
            // 
            this.Time.AutoSize = true;
            this.Time.ContextMenuStrip = this.Menu;
            this.Time.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Time.ForeColor = System.Drawing.Color.White;
            this.Time.Location = new System.Drawing.Point(23, 2);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(39, 17);
            this.Time.TabIndex = 1;
            this.Time.Text = "59:59";
            this.Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Tip.SetToolTip(this.Time, "TaskbarMore");
            this.Time.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Time.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Time.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // TimerMain
            // 
            this.TimerMain.Enabled = true;
            this.TimerMain.Interval = 1000;
            this.TimerMain.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // Status
            // 
            this.Status.BackColor = System.Drawing.Color.White;
            this.Status.ContextMenuStrip = this.Menu;
            this.Status.Location = new System.Drawing.Point(5, 4);
            this.Status.Margin = new System.Windows.Forms.Padding(0);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(13, 13);
            this.Status.TabIndex = 2;
            this.Status.TabStop = false;
            this.Tip.SetToolTip(this.Status, "TaskbarMore");
            // 
            // TaskbarMore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(120, 23);
            this.ContextMenuStrip = this.Menu;
            this.ControlBox = false;
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Time);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskbarMore";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TaskbarMore";
            this.Tip.SetToolTip(this, "TaskbarMore");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskbarMore_FormClosing);
            this.Load += new System.EventHandler(this.TaskbarMore_Load);
            this.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            this.Menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Status)).EndInit();
>>>>>>> 25b12ccfb54d31de3f1f3b0774ac87a84c562c9c
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
<<<<<<< HEAD

        private System.Windows.Forms.PictureBox Upload_Img;
        private System.Windows.Forms.PictureBox Download_Img;
        private System.Windows.Forms.Label Upload_Text;
        private System.Windows.Forms.Label Download_Text;
        private System.Windows.Forms.Label CPU_Title;
        private System.Windows.Forms.Label RAM_Title;
        private System.Windows.Forms.Label RAM_Text;
        private System.Windows.Forms.Label CPU_Text;
        private System.Windows.Forms.PictureBox Key;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unStartupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
=======
        private System.Windows.Forms.Timer TimerMain;
        private System.Windows.Forms.ContextMenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem Exit;
        private System.Windows.Forms.ToolStripMenuItem Boot;
        private System.Windows.Forms.ToolStripMenuItem Open;
        private System.Windows.Forms.ToolStripMenuItem Close;
        private System.Windows.Forms.ToolStripMenuItem About;
        private System.Windows.Forms.ToolTip Tip;
        private System.Windows.Forms.ToolStripMenuItem Timer;
        private System.Windows.Forms.ToolStripMenuItem Reset;
        private System.Windows.Forms.ToolStripMenuItem On;
        private System.Windows.Forms.ToolStripMenuItem Pause;
        private System.Windows.Forms.ToolStripMenuItem End;
        private System.Windows.Forms.PictureBox Status;
        private System.Windows.Forms.Label Time;
>>>>>>> 25b12ccfb54d31de3f1f3b0774ac87a84c562c9c
    }
}

