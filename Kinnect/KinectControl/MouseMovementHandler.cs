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
            screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            screenHeight = (int)SystemParameters.PrimaryScreenHeight;

            // set up timer, execute every 0.1s
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
        }

        public void updateMouseSensibility(float sensibility)
        {
            mouseSensitivity = sensibility;
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
                        // get current cursor position
                        Point curPos = MouseControl.GetCursorPosition();
                        // smoothing for using should be 0 - 0.95f. The way we smooth the cusor is: oldPos + (newPos - oldPos) * smoothValue
                        float smoothing = 1 - cursorSmoothing;
                        // set cursor position

                        MouseControl.SetCursorPos((int)(curPos.X + (x * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));
                    }
                    alreadyTrackedPos = true;

                    if (body.HandRightState == HandState.Closed)
                    {
                        if (!wasRightGrip)
                        {
                            MouseControl.MouseLeftDown();
                            wasRightGrip = true;
                        }
                    }
                    else if (body.HandRightState == HandState.Open)
                    {
                        if (wasRightGrip)
                        {
                            MouseControl.MouseLeftUp();
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
