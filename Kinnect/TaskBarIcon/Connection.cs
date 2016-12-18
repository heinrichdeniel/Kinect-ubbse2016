using KinectControl;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KinectControl
{
    public class Connection
    {
        PictureBox pb = null;
        Body[] bodies = null;
        Dictionary<string, CameraSpacePoint> handpoints = new Dictionary<string, CameraSpacePoint>();
        KinectSensor sensor;
        BodyFrameReader bodyFrameReader;
        bool moving = false;
        CameraSpacePoint prevHandRight;

        MouseMovementHandler movementHandler;
        MultiSourceFrameReader myReader = null;

        public Connection(PictureBox pictureBox)
        {
            pb = pictureBox;
            sensor = KinectSensor.GetDefault();
            bodyFrameReader = sensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += bodyFrameReader_FrameArrived;

            myReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color);
            myReader.MultiSourceFrameArrived += myReader_MultiSourceFrameArrived;
            if (sensor != null)
            {
                sensor.Open();
            }
            movementHandler = new MouseMovementHandler();
        }

        private void bodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {


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
                }
            }
            foreach (Body body in this.bodies)
            {
                // get first tracked body only, notice there's a break below.
                if (body.IsTracked)
                {
                    movementHandler.bodyUpdated(body);
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
                    /*  if (first == true)
                      {
                          //first = false;
                          if (handLeft.Y - hipLeft.Y > 0)
                          {
                              MessageBox.Show("Fent");
                              first = false;
                          }
                      }
                      if (first == false)
                      {
                          if (handLeft.Y - hipLeft.Y <= 0)
                          {
                              MessageBox.Show("Lent");
                              first = true;
                          }
                      }*/


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

                     Bitmap b = new Bitmap(800, 600);
                     using (Graphics g = Graphics.FromImage((Image)b))
                     {
                         g.DrawImage(bitmap, 0, 0, 800, 600);
                     }
                     pb.Image = b;

                 }
             }
         }

        public void DrawImage(MultiSourceFrameArrivedEventArgs e, PictureBox pictureBox1)
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
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipNone);

                    Bitmap b = new Bitmap(800, 800);
                    using (Graphics g = Graphics.FromImage((Image)b))
                    {
                        g.DrawImage(bitmap, 0, 0, 800, 600);
                    }
                    pictureBox1.Image = b;

                }
            }
        }

    }
}
