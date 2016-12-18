using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace KinectControl
{
    public partial class TaskBar : Form
    {
        public TaskBar() 
        {
            InitializeComponent();
            KinectControl.Connection conn = new KinectControl.Connection(pictureBox1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void Service_Opening(object sender, CancelEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TaskBar_Move(object sender, EventArgs e)
        {
           
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            show();
        }
        public void show()
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Service_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void TaskBar_Shown(object sender, EventArgs e)
        {
           
        }

        private void TaskBar_Leave(object sender, EventArgs e)
        {
            
        }

        private void TaskBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                return;
            }
            Application.Exit();
        }

        private void TaskBar_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void TakePicture_Click(object sender, EventArgs e)
        {

        }
    }
}
