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
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.ContextMenuStrip = this.contextMenuStrip1;
            this.label1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(5, 4);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = " ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label1, "TaskbarMore");
            this.label1.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.label1.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.Color.White;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Timer,
            this.Boot,
            this.About,
            this.Exit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(128, 92);
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
            this.Reset.Click += new System.EventHandler(this.Reset_Click_1);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ContextMenuStrip = this.contextMenuStrip1;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(23, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "59:59";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label2, "TaskbarMore");
            this.label2.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.label2.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.label2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 999;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TaskbarMore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(120, 22);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TaskbarMore";
            this.Text = "Form1";
            this.toolTip1.SetToolTip(this, "TaskbarMore");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskbarMore_FormClosing);
            this.Load += new System.EventHandler(this.TaskbarMore_Load);
            this.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Exit;
        private System.Windows.Forms.ToolStripMenuItem Boot;
        private System.Windows.Forms.ToolStripMenuItem Open;
        private System.Windows.Forms.ToolStripMenuItem Close;
        private System.Windows.Forms.ToolStripMenuItem About;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem Timer;
        private System.Windows.Forms.ToolStripMenuItem Reset;
        private System.Windows.Forms.ToolStripMenuItem On;
        private System.Windows.Forms.ToolStripMenuItem Pause;
        private System.Windows.Forms.ToolStripMenuItem End;
    }
}

