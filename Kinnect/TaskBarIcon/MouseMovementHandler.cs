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
        public float timeRequired = TIME_REQUIRED;
        public float pauseThresold = PAUSE_THRESOLD;
        public bool doClick = DO_CLICK;
        public bool useGripGesture = USE_GRIP_GESTURE;
        public float cursorSmoothing = CURSOR_SMOOTHING;

        // Default values
        public const float MOUSE_SENSITIVITY = 3.5f;
        public const float TIME_REQUIRED = 2f;
        public const float PAUSE_THRESOLD = 60f;
        public const bool DO_CLICK = true;
        public const bool USE_GRIP_GESTURE = true;
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
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();

        }



        /// <summary>
        /// Pause to click timer
        /// </summary>
        void Timer_Tick(object sender, EventArgs e)
        {
            if (!doClick || useGripGesture) return;

            if (!alreadyTrackedPos)
            {
                timeCount = 0;
                return;
            }

            Point curPos = MouseControl.GetCursorPosition();

            if ((lastCurPos - curPos).Length < pauseThresold)
            {
                if ((timeCount += 0.1f) > timeRequired)
                {
                    MouseControl.DoMouseClick();
                    timeCount = 0;
                }
            }
            else
            {
                timeCount = 0;
            }

            lastCurPos = curPos;
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
                    /* hand x calculated by this. we don't use shoulder right as a reference cause the shoulder right
                        * is usually behind the lift right hand, and the position would be inferred and unstable.
                        * because the spine base is on the left of right hand, we plus 0.05f to make it closer to the right. */
                    float x = handRight.X - spineBase.X + 0.05f;
                    /* hand y calculated by this. ss spine base is way lower than right hand, we plus 0.51f to make it
                        * higer, the value 0.51f is worked out by testing for a several times, you can set it as another one you like. */
                    float y = spineBase.Y - handRight.Y + 0.51f;
                    // get current cursor position
                    Point curPos = MouseControl.GetCursorPosition();
                    // smoothing for using should be 0 - 0.95f. The way we smooth the cusor is: oldPos + (newPos - oldPos) * smoothValue
                    float smoothing = 1 - cursorSmoothing;
                    // set cursor position
                    MouseControl.SetCursorPos((int)(curPos.X + (x * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));

                    alreadyTrackedPos = true;

                    // Grip gesture
                    if (doClick && useGripGesture)
                    {
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
                }
                else if (handLeft.Z - spineBase.Z < -0.15f) // if left hand lift forward
                {
                    float x = handLeft.X - spineBase.X + 0.3f;
                    float y = spineBase.Y - handLeft.Y + 0.51f;
                    Point curPos = MouseControl.GetCursorPosition();
                    MouseControl.SetCursorPos((int)(curPos.X + (x * mouseSensitivity * screenWidth - curPos.X)), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) ));
                    alreadyTrackedPos = true;

                    if (doClick && useGripGesture)
                    {
                        if (body.HandLeftState == HandState.Closed)
                        {
                            if (!wasLeftGrip)
                            {
                                MouseControl.MouseLeftDown();
                                wasLeftGrip = true;
                            }
                        }
                        else if (body.HandLeftState == HandState.Open)
                        {
                            if (wasLeftGrip)
                            {
                                MouseControl.MouseLeftUp();
                                wasLeftGrip = false;
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
