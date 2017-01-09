using KinectControl;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace KinectControl
{
    public class Connection
    {
        Button btn = null;
        PictureBox pb = null;
        Body[] bodies = null;
        int handpointsnumber = 0;
        long gestureStartedAt = 0;
        int frameWhileNotMoved = 0;
        long lastSendedTime = 0;
        Dictionary<int, CameraSpacePoint> handpoints = new Dictionary<int, CameraSpacePoint>();
        KinectSensor sensor;
        Stopwatch stopwatch = new Stopwatch();
        BodyFrameReader bodyFrameReader;
        bool waitingForGesture = false;
        Commands.Command[] commands = new Commands.Command[3];
        int commandNumber = 0;
        Commands newCommand; 

       // MouseMovementHandler movementHandler;
        MultiSourceFrameReader myReader = null;

        public Connection(PictureBox pictureBox, Button btn)
        {
            stopwatch.Start();
            pb = pictureBox;
            this.btn = btn;
            sensor = KinectSensor.GetDefault();
            bodyFrameReader = sensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += bodyFrameReader_FrameArrived;

            myReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color);
            myReader.MultiSourceFrameArrived += myReader_MultiSourceFrameArrived;

            if (sensor != null)
            {
                sensor.Open();
            }
          //  movementHandler = new MouseMovementHandler();
        }

        private void bodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            long stopwatchTime = stopwatch.ElapsedMilliseconds;
            if ((stopwatchTime - lastSendedTime) / 33 > 0)
            {
                lastSendedTime = stopwatchTime;
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
                        //movementHandler.bodyUpdated(body);
                        // get various skeletal positions
                        CameraSpacePoint handLeft = body.Joints[JointType.HandLeft].Position;
                        CameraSpacePoint handRight = body.Joints[JointType.HandRight].Position;
                        CameraSpacePoint wristRight = body.Joints[JointType.WristRight].Position;
                        CameraSpacePoint thumbRight = body.Joints[JointType.ThumbRight].Position;
                        CameraSpacePoint handTipRight = body.Joints[JointType.HandTipRight].Position;
                        CameraSpacePoint hipLeft = body.Joints[JointType.HipLeft].Position;

                        if (waitingForGesture)
                        {
                            compareHands(hipLeft, handRight);
                            if (handpointsnumber == 100)
                            {
                                btn.Text = "Capturing";
                                btn.BackColor = Color.Green;
                                gestureStartedAt = stopwatchTime;
                            }
                            else if (handpointsnumber > 100)
                            {
                                Commands.MomentInTime moment = new Commands.MomentInTime();
                                moment.time = stopwatchTime - gestureStartedAt;
                                moment.hand[0] = wristRight;
                                moment.hand[1] = handRight;
                                moment.hand[2] = thumbRight;
                                moment.hand[3] = handTipRight;

                                commands[commandNumber].points.Add(moment);
                            }
                                
                            if (frameWhileNotMoved == 50)       //ha vege a mozdulatsornak
                            {
           
                                waitingForGesture = false;
                                if (commands[commandNumber].points.Count > 5)               //ha minimum 5 kepet kapott a kinect-tol
                                {
                                    commands[commandNumber].totalTime = stopwatchTime - gestureStartedAt;
                                    if (commandNumber == 2)     //ha a mozdulat harmadszor volt megismetelve
                                    {
                                        newCommand = new Commands(commands);
                                        btn.Text = "The gesture was saved! Please push the button to create a new gesture!";
                                        btn.BackColor = Color.Red;
                                        btn.Enabled = true;
                                    }
                                    else
                                    {
                                        if (commandNumber == 0)
                                        {
                                            btn.Text = "Please push the button to repeat the gesture for a second time!";
                                        }
                                        else
                                        {
                                            btn.Text = "Please push the button to repeat the gesture for the third time!";
                                        }
                                        btn.BackColor = Color.Red;
                                        btn.Enabled = true;
                                        commandNumber++;
                                    }
                                }
                                else     //ha a mozdulat tul hamar veget ert
                                {
                                    btn.Text = "The gesture was not accepted! Please push the button to repeat!";
                                    btn.BackColor = Color.Red;
                                    btn.Enabled = true;
                                    commands[commandNumber].points.Clear(); 
                                }
                            }

                        }

                        break;
                    }
                }
            }
        }

        private void compareHands(CameraSpacePoint hipLeft, CameraSpacePoint hand)
        {
            if (handpointsnumber == 0)
            {
                handpoints.Add(0,hand);
                handpointsnumber = 1;
            }
            else
            {
                if (hand.Y - hipLeft.Y > 0 && !handMoved(handpoints[handpointsnumber-1], hand) && handpointsnumber < 100)
                {
                    btn.Text = "Capturing in "+ (5 - handpointsnumber/20) + " sec";
                    handpoints.Add(handpointsnumber, hand);
                    handpointsnumber++;
                }
                else if (hand.Y - hipLeft.Y > 0 && !handMoved(handpoints[handpointsnumber - 1], hand) && handpointsnumber > 100)
                {
                    frameWhileNotMoved++;
                }
                else if (handMoved(handpoints[handpointsnumber - 1], hand))
                {
                    frameWhileNotMoved = 0 ;
                }
                else if (hand.Y - hipLeft.Y < 0)
                {
                    handpoints.Clear();
                    handpoints.Add(0, hand);
                    handpointsnumber = 1;
                    btn.Text = "Please raise up your right hand before starting capture!";
                    btn.BackColor = Color.Red;
                }
            }
        }

        private Boolean handMoved(CameraSpacePoint prevHand, CameraSpacePoint hand)
        {
            if(Math.Abs(prevHand.X - hand.X) > 0.01 || Math.Abs(prevHand.Y - hand.Y) > 0.01 || Math.Abs(prevHand.Z - hand.Z) > 0.01)
            {
                return true;
            }
            return false;
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
        public void saveNewGesture()
        {
            this.waitingForGesture = true;
            btn.Text = "Please raise up your right hand before starting capture!";
            btn.BackColor = Color.Red;
            btn.Enabled = false;
        }
    }

   
}
