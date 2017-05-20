using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Threading;

using Microsoft.Kinect;

namespace KinectControl
{
    public class MouseMovementHandler
    {

        int screenWidth, screenHeight;
        DispatcherTimer timer = new DispatcherTimer();

        public float mouseSensitivity = MOUSE_SENSITIVITY;
        public float cursorSmoothing = CURSOR_SMOOTHING;

        // Default values
        public const float MOUSE_SENSITIVITY = 2.5f;
        public const float CURSOR_SMOOTHING = 0.2f;

        private float kinectLastPositionY = 0;
        private float kinectLastPositionX = 0;

        private float mousePositionY = 0;
        private float mousePositionX = 0;

        public Boolean canMove = true;

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
            screenWidth = (int)SystemParameters.PrimaryScreenWidth / 4 * 5;
            screenHeight = (int)SystemParameters.PrimaryScreenHeight / 4 * 5;
            mousePositionX = 0.0f;
            mousePositionY = 0.0f;

            // set up timer, execute every 0.1s
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            canMove = true;
            timer.Start();

        }


        public bool updateMouseSensibility(float sensibility)
        {
            if (sensibility < 0 || sensibility > 10)
            {
                throw new ArgumentException("Incorrect sensibility passed", "sensibility");
            }
            mouseSensitivity = sensibility;
            return true;
        }

        public bool updateMouseVisibility(bool visible)
        {
            if (canMove)
                pointer.pointerVisibility(visible);
            return visible;
        }

        public bool updatecursorSmoothing(float smoothing)
        {
            if (smoothing < 0 || smoothing > 1)
            {
                throw new ArgumentException("Incorrect smoothing passed", "smoothing");
            }
            cursorSmoothing = smoothing;
            return true;
        }

        /// <summary>
        /// Read body frames
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bodyUpdated(Body body)
        {
            if (canMove)
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
                            if (diffx < 0)
                            {
                                diffx = -diffx;
                            }
                            if (diffx > 0.6)
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
                                if (mousePositionX < -0.60f)
                                {
                                    mousePositionX = -0.60f;
                                }
                            }
                            else if (x > kinectLastPositionX)
                            {
                                mousePositionX += diffx;
                                if (mousePositionX > 0.60f)
                                {
                                    mousePositionX = 0.60f;
                                }
                            }
                            if (y < kinectLastPositionY)
                            {
                                mousePositionY -= diffy;
                                if (mousePositionY < -0.60f)
                                {
                                    mousePositionY = -0.60f;
                                }
                            }
                            else if (y > kinectLastPositionY)
                            {
                                mousePositionY += diffy;
                                if (mousePositionY > 0.60f)
                                {
                                    mousePositionY = 0.60f;
                                }
                            }
                            Point curPos = MouseControl.GetCursorPosition();

                            kinectLastPositionX = x;
                            kinectLastPositionY = y;
                            // get current cursor position
                            // smoothing for using should be 0 - 0.95f. The way we smooth the cusor is: oldPos + (newPos - oldPos) * smoothValue
                            float smoothing = 1 - cursorSmoothing;
                            // set cursor position

                            MouseControl.SetCursorPos((int)(curPos.X + (mousePositionX * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((mousePositionY + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));

                        }
                        alreadyTrackedPos = true;

                        if (body.HandRightState == HandState.Closed && !wasRightGrip)
                        {
                            if (!wasLeftGrip)
                            {

                                pointer.setLeftClick(true);
                                MouseControl.MouseLeftDown();
                                wasLeftGrip = true;

                            }
                        }
                        if (handLeft.Y > spineBase.Y && body.HandRightState == HandState.Lasso && !wasLeftGrip)
                        {
                            if (!wasRightGrip)
                            {

                                pointer.setRightClick(true);
                                MouseControl.MouseRightDown();
                                wasRightGrip = true;

                            }
                        } else if( handLeft.Y < spineBase.Y)
                        {
                            if (wasRightGrip)
                            {
                                pointer.setRightClick(false);
                                MouseControl.MouseRightUp();
                                wasRightGrip = false;
                            }
                        }
                        if (body.HandRightState == HandState.Open)
                        {
                            if (wasLeftGrip)
                            {

                                pointer.setLeftClick(false);
                                MouseControl.MouseLeftUp();
                                wasLeftGrip = false;

                            }

                            if (wasRightGrip)
                            {

                                pointer.setRightClick(false);
                                MouseControl.MouseRightUp();
                                wasRightGrip = false;

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
