namespace TaskBarIcon
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
            this.label1 = new System.Windows.Forms.Label();
            this.Service.SuspendLayout();
            this.SuspendLayout();
            // 
            // Service
            // 
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
            this.Settings.Text = "Settings";
            this.Settings.Visible = true;
            this.Settings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(61, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hali :)";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TaskBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 96);
            this.ContextMenuStrip = this.Service;
            this.Controls.Add(this.label1);
            this.Name = "TaskBar";
            this.Text = "TaskBar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskBar_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TaskBar_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.TaskBar_Shown);
            this.Leave += new System.EventHandler(this.TaskBar_Leave);
            this.Move += new System.EventHandler(this.TaskBar_Move);
            this.Service.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip Service;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon Settings;
        private System.Windows.Forms.Label label1;
    }
}

