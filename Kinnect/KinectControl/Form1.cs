using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KinectControl
{
    public partial class TaskBar : Form
    {
        KinectControl.Connection conn;
        List<KeyInput> keyInputs;
        int selectedKeyCommand;
        List<Button> keyButtons;
        List<int> selectedKeys;

        public TaskBar()
        {
            InitializeComponent();
            conn = new KinectControl.Connection(pictureBox1, button1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Maximized;
            this.tabControl1.Size = new System.Drawing.Size(this.Width, this.Height);
            this.keyCommandsPanel.Size = new System.Drawing.Size(this.Width/3, this.Height-100);
            this.pictureBox1.Location = new System.Drawing.Point(this.Width / 3 + 200, 100);
            this.pictureBox1.Size = new System.Drawing.Size(this.Width / 3 * 2, this.Height/3 * 2);
            this.button1.Location = new System.Drawing.Point(this.Width / 3 * 2-150, this.Height / 3 *2 + 100);
            this.button1.Location = new System.Drawing.Point(this.Width / 3 * 2 - 150, this.Height / 3 * 2 + 100);

            LoadCommands();
        }

        public void ButtonClicked(Object sender,
                           EventArgs e)
        {
            String buttonText = ((Button)sender).Text.Split('\n')[0];
            ((Button)sender).BackColor = Color.Green;
            int i = 0;
            foreach (KeyInput keyInput in keyInputs)
            {
                if (keyInput.description.Equals(buttonText))
                {
                    selectedKeyCommand = keyInput.id;
                   
                } else
                {
                    keyButtons[i].BackColor = selectedKeys.Exists(element => element == keyInput.id) ? Color.LightBlue : default(Color);

                }
                ++i;
            }

        }
        public void LoadCommands()
        {
            FileManager fileManager = new FileManager();
            keyInputs = fileManager.getAllKeyInput();
            keyButtons = new List<Button>();
            selectedKeys = new List<int>();
            List<Commands.Command> commands = fileManager.readAllCommands();
            foreach(Commands.Command cm in commands)
            {
                selectedKeys.Add(cm.keyID);
            }

            int i = 0;
            foreach (KeyInput keyInput in keyInputs)
            {
                Button newButton = new Button();
                newButton.Text = keyInput.description + "\n\n" + keyInput.descriptionLong;
                int y = keyCommandsPanel.Height / 10;
                newButton.Size = new System.Drawing.Size(keyCommandsPanel.Width, y);
                newButton.Tag = i;
                newButton.Click += new EventHandler(this.ButtonClicked);
                newButton.BackColor = selectedKeys.Exists(element => element == keyInput.id) ? Color.LightBlue : default(Color);

                keyButtons.Add(newButton);

                keyCommandsPanel.Controls.Add(newButton);

                ++i;
            }
            keyCommandsPanel.ResumeLayout();
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

        private void button1_Click(object sender, EventArgs e)
        {
            conn.saveNewGesture();
        }

        private void slider_mouseSensibility_Scroll(object sender, EventArgs e)
        {
            tb_mouseSensibility.Text = ((float)slider_mouseSensibility.Value/10).ToString();
            conn.updateMouseSensibility((float)slider_mouseSensibility.Value/10);
        }

        private void slider_cursorSmoothing_Scroll(object sender, EventArgs e)
        {
            tb_cursorSmoothing.Text = ((float)slider_cursorSmoothing.Value / 10).ToString();
            conn.updatecursorSmoothing((float)slider_cursorSmoothing.Value / 10);
        }

        private void keyCommandsPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
