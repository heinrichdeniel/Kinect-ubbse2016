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
        int selectedKeyId = 0;
        long gestureStartedAt = 0;
        int frameWhileNotMoved = 0;
        long lastSendedTime = 0;
        CameraSpacePoint prevHand = new CameraSpacePoint();
        public KinectSensor sensor;
        Stopwatch stopwatch = new Stopwatch();
        BodyFrameReader bodyFrameReader;
        bool waitingForGesture = false;
        bool gestureStarted = false;
        Commands.Command[] commands = new Commands.Command[3];
        int commandNumber = 0;
        Commands newCommand;
        int WAITINGTIME = 50;
        MouseMovementHandler movementHandler;
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
                //sensor.Open();
            }
            movementHandler = new MouseMovementHandler();

        }

        public void startStop(Boolean can)
        {
            if (can == false)
            {
                movementHandler.canMove = false;
                movementHandler.pointer.pointerVisibility(false);
            }
            else
            {
                movementHandler.canMove = true;
                movementHandler.pointer.pointerVisibility(true);
            }
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
                if (bodies != null && bodies.Length > 0)
                {
                    foreach (Body body in this.bodies)
                    {

                        // get first tracked body only, notice there's a break below.
                        if (body.IsTracked)
                        {
                            movementHandler.bodyUpdated(body);
                            // get various skeletal positions
                            CameraSpacePoint handLeft = body.Joints[JointType.HandLeft].Position;
                            CameraSpacePoint handRight = body.Joints[JointType.HandRight].Position;
                            CameraSpacePoint wristRight = body.Joints[JointType.WristRight].Position;
                            CameraSpacePoint thumbRight = body.Joints[JointType.ThumbRight].Position;
                            CameraSpacePoint handTipRight = body.Joints[JointType.HandTipRight].Position;
                            CameraSpacePoint hipRight = body.Joints[JointType.HipRight].Position;

                            if (waitingForGesture)
                            {
                                compareHands(hipRight, handRight);
                                if (handpointsnumber == WAITINGTIME)
                                {
                                    btn.Text = "Capturing";
                                    btn.BackColor = Color.Green;
                                    gestureStartedAt = stopwatchTime;
                                    handpointsnumber = WAITINGTIME + 1;
                                    commands[commandNumber] = new Commands.Command();
                                }
                                else if (gestureStarted)
                                {
                                    Commands.MomentInTime moment = new Commands.MomentInTime();
                                    moment.time = stopwatchTime - gestureStartedAt;

                                    moment.hand[0] = wristRight;
                                    moment.hand[1] = handRight;
                                    moment.hand[2] = thumbRight;
                                    moment.hand[3] = handTipRight;

                                    if (frameWhileNotMoved == 0)
                                    {
                                        commands[commandNumber].points.Add(moment);
                                        commands[commandNumber].totalTime = stopwatchTime - gestureStartedAt;
                                    }

                                    if (frameWhileNotMoved == WAITINGTIME / 2)       //ha vege a mozdulatsornak
                                    {
                                        gestureStarted = false;
                                        waitingForGesture = false;
                                        if (commands[commandNumber].points.Count > 5)               //ha minimum 5 kepet kapott a kinect-tol
                                        {
                                            if (commandNumber == 2)     //ha a mozdulat harmadszor volt megismetelve
                                            {
                                                newCommand = new Commands(commands);
                                                FileManager fileManager = FileManager.getInstance();
                                                fileManager.writeCommand(newCommand.averageCommand(selectedKeyId));
                                             
                                                btn.Text = "The gesture was saved! Please push the button to create a new gesture!";
                                                btn.BackColor = Color.Green;
                                                btn.Enabled = true;
                                            }
                                            else
                                            {
                                                if (commandNumber == 0)
                                                {
                                                    btn.BackColor = Color.Transparent;
                                                    btn.Text = "Please push the button to repeat the gesture for a second time!";
                                                    
                                                }
                                                else
                                                {
                                                    btn.BackColor = Color.Transparent;
                                                    btn.Text = "Please push the button to repeat the gesture for the third time!";
                                                    
                                                }
                                                //btn.BackColor = Color.Red;
                                                btn.Enabled = true;
                                                commandNumber++;
                                                handpointsnumber = 1;
                                            }
                                        }
                                        else     //ha a mozdulat tul hamar veget ert
                                        {
                                            btn.Text = commands[commandNumber].points.Count.ToString();
                                            btn.BackColor = Color.Red;
                                            btn.Enabled = true;
                                            commands[commandNumber].points.Clear();
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void compareHands(CameraSpacePoint hipRight, CameraSpacePoint hand)
        {

            if (handpointsnumber == 0)
            {
                handpointsnumber = 1;
            }
            else
            {
                Boolean isHandMoved = handMoved(prevHand, hand);

                if (hand.Y - hipRight.Y > 0 && !isHandMoved && handpointsnumber < WAITINGTIME)
                {
                    btn.Text = "Capturing in " + (5 - handpointsnumber / 10) + " sec";
                    handpointsnumber++;
                }
                else if (hand.Y - hipRight.Y > 0 && !isHandMoved && gestureStarted)
                {
                    handpointsnumber++;
                    frameWhileNotMoved++;
                }
                else if (isHandMoved && handpointsnumber == WAITINGTIME + 1)
                {
                    gestureStarted = true;
                }
                else if (handpointsnumber < WAITINGTIME)
                {
                    handpointsnumber = 1;
                    btn.Text = "Please raise up your right hand before starting capture!";
                    btn.BackColor = Color.Red;
                }

                if (isHandMoved)
                {
                    frameWhileNotMoved = 0;
                }
            }
            prevHand = hand;

        }

        private Boolean handMoved(CameraSpacePoint prevHand, CameraSpacePoint hand)
        {
            if (Math.Abs(prevHand.X - hand.X) > 0.01 || Math.Abs(prevHand.Y - hand.Y) > 0.01 || Math.Abs(prevHand.Z - hand.Z) > 0.01)
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

                    Bitmap b = new Bitmap(900, 600);
                    using (Graphics g = Graphics.FromImage((Image)b))
                    {
                        g.DrawImage(bitmap, 0, 0, 900, 600);
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

                    Bitmap b = new Bitmap(800, 600);
                    using (Graphics g = Graphics.FromImage((Image)b))
                    {
                        g.DrawImage(bitmap, 0, 0, 800, 600);
                    }
                    pictureBox1.Image = b;

                }
            }
        }

        public void setSelectedKeyId(int keyId)
        {
            selectedKeyId = keyId;
        }

        public void updateMouseSensibility(float sensibility)
        {
            movementHandler.updateMouseSensibility(sensibility);
        }

        public void updatecursorSmoothing(float smoothing)
        {
            movementHandler.updateMouseSensibility(smoothing);
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
