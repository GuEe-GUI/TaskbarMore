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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskbarMore));
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
            this.Upload_Text.Location = new System.Drawing.Point(17, 2);
            this.Upload_Text.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Upload_Text.Name = "Upload_Text";
            this.Upload_Text.Size = new System.Drawing.Size(61, 20);
            this.Upload_Text.TabIndex = 2;
            this.Upload_Text.Text = "0.00 K/s";
            this.Upload_Text.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Upload_Text.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Upload_Text.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startupMenuItem,
            this.unStartupMenuItem,
            this.aboutMenuItem,
            this.exitMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.ShowImageMargin = false;
            this.contextMenu.Size = new System.Drawing.Size(149, 100);
            // 
            // startupMenuItem
            // 
            this.startupMenuItem.Name = "startupMenuItem";
            this.startupMenuItem.Size = new System.Drawing.Size(148, 24);
            this.startupMenuItem.Text = "Startup(&S)";
            this.startupMenuItem.Click += new System.EventHandler(this.StartupMenuItem_Click);
            // 
            // unStartupMenuItem
            // 
            this.unStartupMenuItem.Name = "unStartupMenuItem";
            this.unStartupMenuItem.Size = new System.Drawing.Size(148, 24);
            this.unStartupMenuItem.Text = "UnStartup(&U)";
            this.unStartupMenuItem.Click += new System.EventHandler(this.UnStartupMenuItem_Click);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(148, 24);
            this.aboutMenuItem.Text = "About(&a)";
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(148, 24);
            this.exitMenuItem.Text = "Exit(&E)";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // Download_Text
            // 
            this.Download_Text.AutoSize = true;
            this.Download_Text.BackColor = System.Drawing.Color.Transparent;
            this.Download_Text.ContextMenuStrip = this.contextMenu;
            this.Download_Text.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Download_Text.ForeColor = System.Drawing.Color.White;
            this.Download_Text.Location = new System.Drawing.Point(17, 26);
            this.Download_Text.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Download_Text.Name = "Download_Text";
            this.Download_Text.Size = new System.Drawing.Size(61, 20);
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
            this.CPU_Title.Location = new System.Drawing.Point(92, 2);
            this.CPU_Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CPU_Title.Name = "CPU_Title";
            this.CPU_Title.Size = new System.Drawing.Size(37, 20);
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
            this.RAM_Title.Location = new System.Drawing.Point(92, 26);
            this.RAM_Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RAM_Title.Name = "RAM_Title";
            this.RAM_Title.Size = new System.Drawing.Size(42, 20);
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
            this.RAM_Text.Location = new System.Drawing.Point(131, 26);
            this.RAM_Text.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RAM_Text.Name = "RAM_Text";
            this.RAM_Text.Size = new System.Drawing.Size(37, 20);
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
            this.CPU_Text.Location = new System.Drawing.Point(131, 2);
            this.CPU_Text.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CPU_Text.Name = "CPU_Text";
            this.CPU_Text.Size = new System.Drawing.Size(37, 20);
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
            this.Download_Img.Location = new System.Drawing.Point(7, 31);
            this.Download_Img.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Download_Img.Name = "Download_Img";
            this.Download_Img.Size = new System.Drawing.Size(9, 10);
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
            this.Upload_Img.Location = new System.Drawing.Point(7, 8);
            this.Upload_Img.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Upload_Img.Name = "Upload_Img";
            this.Upload_Img.Size = new System.Drawing.Size(9, 10);
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
            this.Key.Location = new System.Drawing.Point(173, 14);
            this.Key.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Key.Name = "Key";
            this.Key.Size = new System.Drawing.Size(21, 22);
            this.Key.TabIndex = 8;
            this.Key.TabStop = false;
            this.Key.MouseLeave += new System.EventHandler(this.TaskbarMore_MouseLeave);
            this.Key.MouseHover += new System.EventHandler(this.TaskbarMore_MouseHover);
            this.Key.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskbarMore_MouseMove);
            // 
            // TaskbarMore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(200, 49);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
    }
}

