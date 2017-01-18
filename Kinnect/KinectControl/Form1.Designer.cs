
namespace KinectControl
{
    partial class TaskBar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary> 
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
      

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskBar));
            this.Service = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.keyCommandsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.tb_cursorSmoothing = new System.Windows.Forms.TextBox();
            this.tb_mouseSensibility = new System.Windows.Forms.TextBox();
            this.slider_cursorSmoothing = new System.Windows.Forms.TrackBar();
            this.slider_mouseSensibility = new System.Windows.Forms.TrackBar();
            this.lb_cursorSmoothing = new System.Windows.Forms.Label();
            this.lb_mouseSensitivity = new System.Windows.Forms.Label();
            this.Service.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slider_cursorSmoothing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider_mouseSensibility)).BeginInit();
            this.SuspendLayout();
            // 
            // Service
            // 
            this.Service.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Service.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.Service.Name = "Service";
            this.Service.Size = new System.Drawing.Size(104, 48);
            this.Service.Text = "Service";
            this.Service.Opening += new System.ComponentModel.CancelEventHandler(this.Service_Opening);
            this.Service.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Service_MouseClick);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Settings
            // 
            this.Settings.ContextMenuStrip = this.Service;
            this.Settings.Icon = ((System.Drawing.Icon)(resources.GetObject("Settings.Icon")));
            this.Settings.Text = "KinectControl";
            this.Settings.Visible = true;
            this.Settings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(418, 16);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(783, 597);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint_Selected_Command);
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(495, 636);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(580, 151);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add gesture";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1139, 1026);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.keyCommandsPanel);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(1131, 1000);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Keyboard";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // keyCommandsPanel
            // 
            this.keyCommandsPanel.AutoScroll = true;
            this.keyCommandsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.keyCommandsPanel.Location = new System.Drawing.Point(15, 16);
            this.keyCommandsPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.keyCommandsPanel.Name = "keyCommandsPanel";
            this.keyCommandsPanel.Size = new System.Drawing.Size(385, 597);
            this.keyCommandsPanel.TabIndex = 4;
            this.keyCommandsPanel.WrapContents = false;
            this.keyCommandsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.keyCommandsPanel_Paint);
            // 
            // tabPage2
            // 
            this.tabPage2.AllowDrop = true;
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.tb_cursorSmoothing);
            this.tabPage2.Controls.Add(this.tb_mouseSensibility);
            this.tabPage2.Controls.Add(this.slider_cursorSmoothing);
            this.tabPage2.Controls.Add(this.slider_mouseSensibility);
            this.tabPage2.Controls.Add(this.lb_cursorSmoothing);
            this.tabPage2.Controls.Add(this.lb_mouseSensitivity);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(1131, 1000);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mouse";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.Location = new System.Drawing.Point(176, 307);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(231, 78);
            this.button2.TabIndex = 6;
            this.button2.Text = "Mouse On";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tb_cursorSmoothing
            // 
            this.tb_cursorSmoothing.Enabled = false;
            this.tb_cursorSmoothing.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_cursorSmoothing.Location = new System.Drawing.Point(439, 175);
            this.tb_cursorSmoothing.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_cursorSmoothing.Name = "tb_cursorSmoothing";
            this.tb_cursorSmoothing.Size = new System.Drawing.Size(76, 19);
            this.tb_cursorSmoothing.TabIndex = 5;
            this.tb_cursorSmoothing.Text = "0,3";
            this.tb_cursorSmoothing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_mouseSensibility
            // 
            this.tb_mouseSensibility.Enabled = false;
            this.tb_mouseSensibility.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_mouseSensibility.Location = new System.Drawing.Point(439, 37);
            this.tb_mouseSensibility.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_mouseSensibility.Name = "tb_mouseSensibility";
            this.tb_mouseSensibility.Size = new System.Drawing.Size(76, 19);
            this.tb_mouseSensibility.TabIndex = 4;
            this.tb_mouseSensibility.Text = "3.5";
            this.tb_mouseSensibility.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // slider_cursorSmoothing
            // 
            this.slider_cursorSmoothing.Location = new System.Drawing.Point(59, 215);
            this.slider_cursorSmoothing.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.slider_cursorSmoothing.Name = "slider_cursorSmoothing";
            this.slider_cursorSmoothing.Size = new System.Drawing.Size(454, 45);
            this.slider_cursorSmoothing.TabIndex = 3;
            this.slider_cursorSmoothing.Value = 3;
            this.slider_cursorSmoothing.Scroll += new System.EventHandler(this.slider_cursorSmoothing_Scroll);
            // 
            // slider_mouseSensibility
            // 
            this.slider_mouseSensibility.Location = new System.Drawing.Point(59, 79);
            this.slider_mouseSensibility.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.slider_mouseSensibility.Maximum = 100;
            this.slider_mouseSensibility.Name = "slider_mouseSensibility";
            this.slider_mouseSensibility.Size = new System.Drawing.Size(454, 45);
            this.slider_mouseSensibility.TabIndex = 2;
            this.slider_mouseSensibility.Value = 35;
            this.slider_mouseSensibility.Scroll += new System.EventHandler(this.slider_mouseSensibility_Scroll);
            // 
            // lb_cursorSmoothing
            // 
            this.lb_cursorSmoothing.Location = new System.Drawing.Point(57, 177);
            this.lb_cursorSmoothing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_cursorSmoothing.Name = "lb_cursorSmoothing";
            this.lb_cursorSmoothing.Size = new System.Drawing.Size(151, 28);
            this.lb_cursorSmoothing.TabIndex = 1;
            this.lb_cursorSmoothing.Text = "Cursor Smoothing";
            // 
            // lb_mouseSensitivity
            // 
            this.lb_mouseSensitivity.Location = new System.Drawing.Point(57, 40);
            this.lb_mouseSensitivity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_mouseSensitivity.Name = "lb_mouseSensitivity";
            this.lb_mouseSensitivity.Size = new System.Drawing.Size(163, 30);
            this.lb_mouseSensitivity.TabIndex = 0;
            this.lb_mouseSensitivity.Text = "Mouse Sensitivity";
            // 
            // TaskBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 843);
            this.ContextMenuStrip = this.Service;
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TaskBar";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskBar_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TaskBar_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.TaskBar_Shown);
            this.Leave += new System.EventHandler(this.TaskBar_Leave);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form_MouseClick);
            this.Move += new System.EventHandler(this.TaskBar_Move);
            this.Service.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slider_cursorSmoothing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider_mouseSensibility)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip Service;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon Settings;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel keyCommandsPanel;
        private System.Windows.Forms.TrackBar slider_cursorSmoothing;
        private System.Windows.Forms.TrackBar slider_mouseSensibility;
        private System.Windows.Forms.Label lb_cursorSmoothing;
        private System.Windows.Forms.Label lb_mouseSensitivity;
        private System.Windows.Forms.TextBox tb_cursorSmoothing;
        private System.Windows.Forms.TextBox tb_mouseSensibility;
        private System.Windows.Forms.Button button2;
    }
}

