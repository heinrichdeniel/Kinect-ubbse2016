using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskBarIcon
{


    public class Connection
    {
        Body[] bodies = null;
        Dictionary<string, CameraSpacePoint> handpoints = new Dictionary<string, CameraSpacePoint>();
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

      /* public void bodyFrameReader_FrameArrived(BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;
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
                    dataReceived = true;
                }
            }
            if (!dataReceived)
            {
                return;
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
                    CameraSpacePoint wristLeft = body.Joints[JointType.WristLeft].Position;
                    CameraSpacePoint wristRight = body.Joints[JointType.WristRight].Position;
                    CameraSpacePoint thumbLeft = body.Joints[JointType.ThumbLeft].Position;
                    CameraSpacePoint thumbRight = body.Joints[JointType.ThumbRight].Position;
                    CameraSpacePoint handTipLeft = body.Joints[JointType.HandTipLeft].Position;
                    CameraSpacePoint handTipRight = body.Joints[JointType.HandTipRight].Position;
                    handpoints["HandLeft"] = handLeft;
                    handpoints["HandRight"] = handRight;
                    handpoints["WristLeft"] = wristLeft;
                    handpoints["wristRight"] = wristRight;
                    handpoints["ThumbLeft"] = thumbLeft;
                    handpoints["ThumbRight"] = thumbRight;
                    handpoints["HandTipLeft"] = handTipLeft;
                    handpoints["HandTipRight"] = handTipRight;
                    CameraSpacePoint spineBase = body.Joints[JointType.SpineBase].Position;
                    // get first tracked body only
                    break;
                }
            }
        }*/

    }
}
