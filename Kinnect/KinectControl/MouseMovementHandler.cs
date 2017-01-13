using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Threading;

using Microsoft.Kinect;

namespace KinectControl
{
    class MouseMovementHandler
    {

        int screenWidth, screenHeight;
        DispatcherTimer timer = new DispatcherTimer();

        public float mouseSensitivity = MOUSE_SENSITIVITY;
        public float cursorSmoothing = CURSOR_SMOOTHING;

        // Default values
        public const float MOUSE_SENSITIVITY = 3.5f;
        public const float CURSOR_SMOOTHING = 0.2f;

        private float kinectLastPositionY = 0;
        private float kinectLastPositionX = 0;

        private float mousePositionY = 0;
        private float mousePositionX = 0;

        public Boolean canMove;

        public MousePointer pointer;
        /// <summary>
        /// Determine if we have tracked the hand and used it to move the cursor,
        /// If false, meaning the user may not lift their hands, we don't get the last hand position and some actions like pause-to-click won't be executed.
        /// </summary>
        bool alreadyTrackedPos = false;

        float timeCount = 0;
        Point lastCurPos = new Point(0, 0);

        bool wasLeftGrip = false;
        bool wasRightGrip = false;


        public MouseMovementHandler()
        {
            pointer = new MousePointer();
            pointer.pointerVisibility(false);
            screenWidth = (int)SystemParameters.PrimaryScreenWidth / 3 * 4;
            screenHeight = (int)SystemParameters.PrimaryScreenHeight / 3 * 4;
            Point p = MouseControl.GetCursorPosition();
            p.X /= 3 * 4;
            p.Y /= 3 * 4;
            mousePositionX = (float)((float)p.X * 2.0 / (float)screenWidth * 4.0 / 3.0);
            mousePositionY = (float)((float)p.Y * 2.0 / (float)screenHeight * 4.0 / 3.0);
            // set up timer, execute every 0.1s
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            canMove = true;
            timer.Start();

        }
        

        public void updateMouseSensibility(float sensibility)
        {
            mouseSensitivity = sensibility;
        }

        public void updateMouseVisibility(bool visible)
        {
            if (canMove)
            pointer.pointerVisibility(visible);
        }

        public void updatecursorSmoothing(float smoothing)
        {
            cursorSmoothing = smoothing;
        }

        /// <summary>
        /// Read body frames
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bodyUpdated(Body body)
        {
            if (body.IsTracked)
            {
                // get various skeletal positions
                CameraSpacePoint handLeft = body.Joints[JointType.HandLeft].Position;
                CameraSpacePoint handRight = body.Joints[JointType.HandRight].Position;
                CameraSpacePoint spineBase = body.Joints[JointType.SpineBase].Position;

                if (handRight.Z - spineBase.Z < -0.15f) // if right hand lift forward
                {
                    float x;
                    float y;

                    // move cursor only if left hand is behind of spinebase
                    if (handLeft.Y < spineBase.Y)
                    {

                        x = handRight.X - spineBase.X + 0.05f;
                        y = spineBase.Y - handRight.Y + 0.51f;

                        float diffx = kinectLastPositionX - x;
                        if(diffx < 0)
                        {
                            diffx = -diffx;
                        }
                        if( diffx > 0.1)
                        {
                            diffx = 0.0f;
                        }
                        
                        float diffy = kinectLastPositionY - y;
                        if (diffy < 0)
                        {
                            diffy = -diffy;
                        }
                        if (diffy > 0.1f)
                        {
                            diffy = 0.0f;
                        }

                        if (x < kinectLastPositionX)
                        {
                            mousePositionX -= diffx;
                        }
                        else if (x > kinectLastPositionX)
                        {
                            mousePositionX += diffx;

                        }
                        if (y < kinectLastPositionY)
                        {
                            mousePositionY -= diffy;

                        }
                        else if (y > kinectLastPositionY)
                        {
                            mousePositionY += diffy;
                        }
                        Point curPos = MouseControl.GetCursorPosition();
                        if (mousePositionX > 1.0f || mousePositionX < -1.0f)
                        {
                            mousePositionX = (float)((float)curPos.X * 2.0 / (float)screenWidth * 4.0 / 3.0);
                        }
                        if (mousePositionY > 1.0f || mousePositionY < -1.0f)
                        {
                            mousePositionY = (float)((float)curPos.Y * 2.0 / (float)screenHeight * 4.0 / 3.0);
                        }

                        kinectLastPositionX = x;
                        kinectLastPositionY = y;
                        Console.WriteLine("Kinexct: " + x + " " + y + " Mouse: " + mousePositionX + " " + mousePositionY);
                        // get current cursor position
                        // smoothing for using should be 0 - 0.95f. The way we smooth the cusor is: oldPos + (newPos - oldPos) * smoothValue
                        float smoothing = 1 - cursorSmoothing;
                        // set cursor position
                        if (canMove)
                        {
                            MouseControl.SetCursorPos((int)(curPos.X + (mousePositionX * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((mousePositionY + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));
                        }
                    }
                    alreadyTrackedPos = true;

                    if (body.HandRightState == HandState.Closed)
                    {
                        if (!wasRightGrip)
                        {
                            if (canMove)
                            {
                                pointer.setAction(true);
                                MouseControl.MouseLeftDown();
                                wasRightGrip = true;
                            }
                        }
                    }
                    else if (body.HandRightState == HandState.Open)
                    {
                        if (wasRightGrip)
                        {
                            if (canMove)
                            {
                                pointer.setAction(false);
                                MouseControl.MouseLeftUp();
                                wasRightGrip = false;
                            }
                        }
                    }
                }
                else
                {
                    wasLeftGrip = true;
                    wasRightGrip = true;
                    alreadyTrackedPos = false;
                }

            }
        }

        public void Close()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

    }
}
