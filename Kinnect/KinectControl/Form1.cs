using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
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
        Boolean isWorking;
        private Commands.Average selectedCommand;
        private float pos = 0.0f;
        private FileManager fileManager;
        private Thread drawCountThread;

        public TaskBar()
        {
            fileManager = FileManager.getInstance();
            InitializeComponent();

            conn = new KinectControl.Connection(pictureBox1, button1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to start working with the Kinect device?", "Are you ready?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.TopMost = true;
                this.WindowState = FormWindowState.Maximized;
                this.tabControl1.Size = new System.Drawing.Size(this.Width, this.Height);
                this.keyCommandsPanel.Size = new System.Drawing.Size(this.Width / 3, this.Height - 100);
                this.pictureBox1.Location = new System.Drawing.Point(this.Width / 3 + 200, 100);
                this.pictureBox1.Size = new System.Drawing.Size(this.Width / 3 * 2, this.Height / 3 * 2);
                this.button1.Location = new System.Drawing.Point(this.Width / 3 * 2 - 250, this.Height / 3 * 2 + 100);
                this.button2.BackColor = Color.Green;
                this.Icon = this.Settings.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(TaskBar)).GetObject("Settings_Green.Icon")));
                this.button2.Text = "Mouse On";
                this.isWorking = false;
                this.showToolStripMenuItem.Text = "Stop";
                this.conn.startStop(isWorking);
                this.conn.sensor.Open();

            }
            else if (dialogResult == DialogResult.No)
            {
                this.button2.BackColor = Color.Red;
                this.button2.Text = "Mouse Off";
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.isWorking = false;
                this.showToolStripMenuItem.Text = "Start";
                this.conn.startStop(isWorking);
                this.conn.sensor.Close();

            }

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
                    conn.setSelectedKeyId(selectedKeyCommand);
                    if (selectedKeys.Exists(element => element == keyInput.id))
                    {

                        selectedCommand = fileManager.readCommand(keyInput.id);
                        if (selectedCommand.keyID != -1)
                        {
                            conn.kinnectImage = false;
                            drawCountThread = new Thread(Paint_Thread);
                            drawCountThread.Start();
                            pictureBox1.Refresh();
                        }
                    }
                    else
                    {
                        conn.kinnectImage = true;
                    }
                }
                else
                {
                    keyButtons[i].BackColor = selectedKeys.Exists(element => element == keyInput.id) ? Color.LightBlue : default(Color);

                }
                ++i;
            }

        }

        public void LoadCommands()
        {
            FileManager fileManager = FileManager.getInstance();
            keyInputs = fileManager.getAllKeyInput();
            keyButtons = new List<Button>();
            selectedKeys = new List<int>();
            List<Commands.Average> commands = fileManager.readAllCommands();
            foreach (Commands.Average cm in commands)
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

        private void Form_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                Service.Hide();
        }

        private void Service_Opening(object sender, CancelEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TaskBar_Move(object sender, EventArgs e)
        {

        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.show();
        }
        public void show()
        {
            if (showToolStripMenuItem.Text.Equals("Start"))
            {
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Maximized;
                this.tabControl1.Size = new System.Drawing.Size(this.Width, this.Height);
                this.keyCommandsPanel.Size = new System.Drawing.Size(this.Width / 3, this.Height - 100);
                this.pictureBox1.Location = new System.Drawing.Point(this.Width / 3 + 200, 100);
                this.pictureBox1.Size = new System.Drawing.Size(this.Width / 3 * 2, this.Height / 3 * 2);
                this.button1.Location = new System.Drawing.Point(this.Width / 3 * 2 - 150, this.Height / 3 * 2 + 100);
                this.button1.Location = new System.Drawing.Point(this.Width / 3 * 2 - 150, this.Height / 3 * 2 + 100);
                this.showToolStripMenuItem.Text = "Stop";
                try
                {
                    this.Settings.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(TaskBar)).GetObject("Settings_Green.Icon")));
                }
                catch (NullReferenceException e)
                {

                }
                this.conn.sensor.Open();
                this.isWorking = true;
                this.conn.startStop(isWorking);
                this.button2.BackColor = Color.Green;
                this.button2.Text = "Mouse On";
                this.BringToFront();
            }
            else if (showToolStripMenuItem.Text.Equals("Stop"))
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                try
                {
                    this.Settings.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(TaskBar)).GetObject("Settings.Icon")));
                }
                catch (NullReferenceException e)
                {

                }
                this.showToolStripMenuItem.Text = "Start";
                this.isWorking = false;
                this.conn.startStop(isWorking);
                this.conn.sensor.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.sensor.Close();
            this.Close();
            Application.Exit();
            System.Environment.Exit(1);
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
            tb_mouseSensibility.Text = ((float)slider_mouseSensibility.Value / 10).ToString();
            conn.updateMouseSensibility((float)slider_mouseSensibility.Value / 10);
        }

        private void slider_cursorSmoothing_Scroll(object sender, EventArgs e)
        {
            tb_cursorSmoothing.Text = ((float)slider_cursorSmoothing.Value / 10).ToString();
            conn.updatecursorSmoothing((float)slider_cursorSmoothing.Value / 10);
        }

        private void keyCommandsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Paint_Thread()
        {
            pos = 0;
            int keyInputID = selectedCommand.keyID;
            while (pos < selectedCommand.time)
            {
                pos += 30;
                Thread.Sleep(30);
                if (selectedCommand.keyID != keyInputID || selectedCommand.keyID == -1)
                {
                    break;
                }
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Paint_Selected_Command(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (selectedCommand != null)
            {
                if (!conn.kinnectImage)
                {
                    // Create a local version of the graphics object for the PictureBox.
                    CameraSpacePoint point = selectedCommand.spline(pos/selectedCommand.time)[0];
                    Graphics g = e.Graphics;
                    g.DrawEllipse(Pens.Azure, point.X + 50, point.Y + 50, 50, 50);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isWorking == true)
            {
                isWorking = false;
                conn.startStop(isWorking);
                button2.BackColor = Color.Red;
                button2.Text = "Mouse Off";
            }
            else
            {
                isWorking = true;
                conn.startStop(isWorking);
                button2.BackColor = Color.Green;
                button2.Text = "Mouse On";
            }
        }
    }
}
