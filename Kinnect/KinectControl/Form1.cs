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
        static List<int> selectedKeys;
        Boolean isWorking;
        private Movement selectedCommand;
        private CameraSpacePoint point = new CameraSpacePoint();
        private FileManager fileManager;
        private Thread drawCountThread;
        private float drawSize = 50.0f;
        private bool drawed = true;
        private int buttonClicked = 0;
        private BackgroundWorker backgroundWorker1;
        //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskBar));

        public class CommandSaved : ClickInterface
        {
            void ClickInterface.CommandSaved(int keyID)
            {
                selectedKeys.Add(keyID);
            }
        }

        public TaskBar()
        {
            fileManager = FileManager.getInstance();
            InitializeComponent();
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            //conn = new KinectControl.Connection(pictureBox1, button1);
            //conn.setCommandSaveInterface(new CommandSaved());
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
                conn = new KinectControl.Connection(pictureBox1, button1);
                conn.setCommandSaveInterface(new CommandSaved());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to start working with the Kinect device?", "Are you ready?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.WindowState = FormWindowState.Maximized;
                this.tabControl1.Size = new System.Drawing.Size(this.Width, this.Height);
                this.keyCommandsPanel.Size = new System.Drawing.Size(this.Width / 3, this.Height - this.Height / 10);
                this.pictureBox1.Location = new System.Drawing.Point(this.Width / 3 + this.Width / 10, this.Height / 10);
                this.pictureBox1.Size = new System.Drawing.Size(this.Width / 100 * 46, this.Height / 100 * 55);
                this.button1.Location = new System.Drawing.Point(this.Width / 3 + this.Width / 20 * 3, this.Height / 100 * 55 + this.Height / 20 * 3);
                this.button1.Size = new System.Drawing.Size(this.Width / 100 * 35, this.Height / 100 * 15);
                this.button2.BackColor = Color.Green;
                //this.Icon = this.Settings.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(TaskBar)).GetObject("Settings_Green.Icon")));
                //this.Icon = ((System.Drawing.Icon)(resources.GetObject("Settings_Green.Icon")));
                this.button2.Text = "Mouse On";
                this.isWorking = true;
                this.showToolStripMenuItem.Text = "Stop";
                while (this.conn == null)
                {
                    
                }
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
                        ++buttonClicked;
                        if(buttonClicked > 100)
                        {
                            buttonClicked = 0;
                        }
                        selectedCommand = fileManager.readMovement(keyInput.id);
                        if (selectedCommand.keyID != -1)
                        {
                            conn.kinnectImage = false;
                            if(drawCountThread != null)
                            {
                                drawCountThread.Abort();
                            }
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
            selectedKeys = fileManager.readAllMovementsID();
            conn.setExistingCommands(fileManager.readAllCommands());

            int i = 0;
            foreach (KeyInput keyInput in keyInputs)
            {
                Button newButton = new Button();
                newButton.Text = keyInput.description + "\n\n" + keyInput.descriptionLong;
                int y = keyCommandsPanel.Height / 10;
                newButton.Size = new System.Drawing.Size(this.Width / 3 - this.Width / 100 * 2, y);
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
                this.keyCommandsPanel.Size = new System.Drawing.Size(this.Width / 3, this.Height - this.Height / 10);
                this.pictureBox1.Location = new System.Drawing.Point(this.Width / 3 + this.Width / 10, this.Height / 10);
                this.pictureBox1.Size = new System.Drawing.Size(this.Width / 100 * 46, this.Height / 100 * 55);
                this.button1.Location = new System.Drawing.Point(this.Width / 3 + this.Width / 20 * 3, this.Height / 100 * 55 + this.Height / 20 * 3);
                this.button1.Size = new System.Drawing.Size(this.Width / 100 * 35, this.Height / 100 * 15);
                this.showToolStripMenuItem.Text = "Stop";
/*                try
                {
                    this.Settings.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(TaskBar)).GetObject("Settings_Green.Icon")));
                }
                catch (NullReferenceException e)
                {

                }
*/
                this.conn.sensor.Open();
                this.isWorking = true;
                this.conn.startStop(isWorking);
                this.button2.BackColor = Color.Green;
                this.button2.Text = "Mouse On";
                this.BringToFront();
                LoadCommands();
                //this.Refresh();
            }
            else if (showToolStripMenuItem.Text.Equals("Stop"))
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
/*                try
               {
                    this.Settings.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(TaskBar)).GetObject("Settings.Icon")));
                }
                catch (NullReferenceException e)
                {

                }
*/
                this.showToolStripMenuItem.Text = "Start";
                this.isWorking = false;
                this.conn.startStop(isWorking);
                this.conn.sensor.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.enableRecognition = true;

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
                this.ShowInTaskbar = false;
/*
                try
               {
                    this.Settings.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(TaskBar)).GetObject("Settings.Icon")));
               }
                catch (NullReferenceException ef)
                {

                }
*/
                this.showToolStripMenuItem.Text = "Start";
                this.isWorking = false;
                this.conn.startStop(isWorking);
                this.conn.sensor.Close();
            }
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
            conn.kinnectImage = true;
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

            drawSize = 30.0f;
            int button = buttonClicked;
            float last = 0.0f;
            foreach(KeyValuePair<float, CameraSpacePoint> point in selectedCommand.points)
            {

                if(last == 0.0f)
                {
                    last = point.Key;
                }
                if (drawed)
                {
                    this.point.X = point.Value.X;
                    this.point.Y = point.Value.Y;
                    this.point.Z = point.Value.Z;
                    drawed = false;
                    Thread.Sleep((int)(point.Key - last));
                    if (button != buttonClicked)
                    {
                        break;
                    }
                    last = point.Key;
                    Log.log.Info("INFO: " + last + " " + point.Key + " " + point.Value.X);
                    pictureBox1.Invoke(new MethodInvoker(
                        delegate ()
                        {
                            pictureBox1.Refresh();
                        })
                    );
                }
                else
                {
                    Thread.Sleep(5);
                }

            }
        }

        private void pictureBox1_Paint_Selected_Command(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (selectedCommand != null)
            {
                if (!conn.kinnectImage)
                {
                    // Create a local version of the graphics object for the PictureBox.
                    Graphics g = e.Graphics;
                    g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                    g.FillEllipse(new SolidBrush(Color.Red), point.X * pictureBox1.Width / 5 + pictureBox1.Width / 2, point.Y * pictureBox1.Height / 5 + pictureBox1.Height / 2, drawSize, drawSize);
                    drawed = true;
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
