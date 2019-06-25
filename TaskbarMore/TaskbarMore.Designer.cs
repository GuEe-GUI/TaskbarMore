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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskbarMore));
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Status)).BeginInit();
            this.SuspendLayout();
            // 
            // ContextMenuStrip
            // 
            this.ContextMenuStrip.BackColor = System.Drawing.Color.White;
            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Timer,
            this.Boot,
            this.About,
            this.Exit});
            this.ContextMenuStrip.Name = "ContextMenuStrip";
            this.ContextMenuStrip.Size = new System.Drawing.Size(128, 92);
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
            this.Time.ContextMenuStrip = this.ContextMenuStrip;
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
            this.Status.ContextMenuStrip = this.ContextMenuStrip;
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
            this.ClientSize = new System.Drawing.Size(120, 22);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Time);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TaskbarMore";
            this.Text = "TaskbarMore";
            this.Tip.SetToolTip(this, "TaskbarMore");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskbarMore_FormClosing);
            this.Load += new System.EventHandler(this.TaskbarMore_Load);
            this.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            this.ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Status)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer TimerMain;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip;
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
    }
}

