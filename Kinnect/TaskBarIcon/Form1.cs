using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace KinectControl
{
    public partial class TaskBar : Form
    {
       // HandMovementAnalysis movementAnalysis;
        KinectSensor sensor = null;
        MultiSourceFrameReader myReader = null;
        public TaskBar() 
        {
            InitializeComponent();
           // movementAnalysis = new HandMovementAnalysis();
        }

      
        private void Form1_Load(object sender, EventArgs e)
        {
            sensor = KinectSensor.GetDefault();  

            if (sensor != null)
            {
                sensor.Open();
            }
            myReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color);
            myReader.MultiSourceFrameArrived += myReader_MultiSourceFrameArrived;
        }

        private void myReader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var width = frame.FrameDescription.Width;
                    var height = frame.FrameDescription.Height;
                    var data = new byte[width * height * 32 / 8];
                    frame.CopyConvertedFrameDataToArray(data, ColorImageFormat.Bgra);

                    var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

                    Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
                    bitmap.UnlockBits(bitmapData);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);

                    Bitmap b = new Bitmap(800, 800);
                    using (Graphics g = Graphics.FromImage((Image)b))
                    {
                        g.DrawImage(bitmap, 0, 0, 800, 600);
                    }
                    pictureBox1.Image = b;

                }
            }
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
            //movementAnalysis.Close();
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
