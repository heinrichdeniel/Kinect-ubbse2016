﻿using Microsoft.Kinect;
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
using System.Diagnostics;
using System.IO;

namespace KinectControl
{
    public partial class TaskBar : Form
    {
       // HandMovementAnalysis movementAnalysis;
        KinectSensor sensor = null;
        MultiSourceFrameReader myReader = null;
        BodyFrameReader bodyFrameReader;
        Body[] bodies;
        Dictionary<string, CameraSpacePoint> handpoints = new Dictionary<string, CameraSpacePoint>();
        bool moving = false;
        CameraSpacePoint prevHandRight;
        int count = 0;
        public TaskBar() 
        {
            InitializeComponent();
           // movementAnalysis = new HandMovementAnalysis();
        }

      
        private void Form1_Load(object sender, EventArgs e)
        {
            sensor = KinectSensor.GetDefault();


            //myReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color);
            //myReader.MultiSourceFrameArrived += myReader_MultiSourceFrameArrived;
            bodyFrameReader = sensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += bodyFrameReader_FrameArrived;
            if (sensor != null)
            {
                sensor.Open();
            }
        }

        private void bodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {

            //bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    //dataReceived = true;
                }
            }
            foreach (Body body in this.bodies)
            {
                // get first tracked body only, notice there's a break below.
                if (body.IsTracked)
                {
                    handpoints.Clear();
                    // get various skeletal positions
                    CameraSpacePoint handLeft = body.Joints[JointType.HandLeft].Position;
                    CameraSpacePoint handRight = body.Joints[JointType.HandRight].Position;
                    CameraSpacePoint wristRight = body.Joints[JointType.WristRight].Position;
                    CameraSpacePoint thumbRight = body.Joints[JointType.ThumbRight].Position;
                    CameraSpacePoint handTipRight = body.Joints[JointType.HandTipRight].Position;
                    CameraSpacePoint hipLeft = body.Joints[JointType.HipLeft].Position;

                    if (prevHandRight != null)
                    {

                        if (Math.Abs(prevHandRight.Y - handRight.Y) > 0.01)
                        {
                            if (!moving)
                            {
                                moving = true;
                                Debug.WriteLine("Elindult");
                            }
                                Debug.WriteLine("X: " + handRight.X);
                                Debug.WriteLine("Y: " + handRight.Y);
                                Debug.WriteLine("Z: " + handRight.Z);
                        }

                    }


                    //Thread.Sleep(100);
                    handpoints["HandLeft"] = handLeft;
                    handpoints["HandRight"] = handRight;
                    handpoints["wristRight"] = wristRight;
                    handpoints["ThumbRight"] = thumbRight;
                    handpoints["HandTipRight"] = handTipRight;
                    // get first tracked body only

                    prevHandRight = body.Joints[JointType.HandRight].Position;
                    break;
                }
            }



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
