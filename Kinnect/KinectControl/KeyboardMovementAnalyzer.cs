using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectControl
{

    class KeyboardMovementAnalyzer
    {
        private List<Commands.Average> existingCommands;
        private Dictionary<int, Commands.Average> goodCommands;
        private Dictionary<int, CameraSpacePoint> firstPoints;
        private Dictionary<int, float> startTime;

        public KeyboardMovementAnalyzer(List<Commands.Average> commands)
        {
            existingCommands = commands;
        }


        public void AnalyzeFrames(DateTime time, List<CameraSpacePoint> handpoints)
        {
            compareWithExistingCommands(handpoints,(float)time.Millisecond/1000f);

            compareWithGoodCommands(handpoints, (float)time.Millisecond / 1000f);
        }

        private void compareWithGoodCommands(List<CameraSpacePoint> handpoints, float time)
        {
            for (int i = 0; i < goodCommands.Count; i++)
            {
                float[][] handPoints = new float[handpoints.Count][];
                for (int j = 0; j < handpoints.Count; j++)
                {
                    handPoints[j] = new float[3];
                    handPoints[j][0] = handpoints[j].X - firstPoints[i].X;
                    handPoints[j][1] = handpoints[j].Y - firstPoints[i].Y;
                    handPoints[j][2] = handpoints[j].Z - firstPoints[i].Z;
                }
                CameraSpacePoint[] csp = goodCommands[i].spline(time-startTime[i]);
                
                if (goodCommands[i].time < time - startTime[i])
                {
                    FileManager.getInstance().getKeyInput(goodCommands[i].keyID).execute();
                    goodCommands.Clear();
                    startTime.Remove(i);
                    firstPoints.Remove(i);
                }
                if (!isGoodCommand(handPoints, csp))         //vizsgalja, hogy megeggyezik az adott command kezdopontjaival
                {
                    goodCommands.Remove(i);
                    startTime.Remove(i);
                    firstPoints.Remove(i);
                }

            }
           
        }

        private void compareWithExistingCommands(List<CameraSpacePoint> handpoints, float time)   //megvizsgalja, hogy a letezo commandok kozul melyik kezdodik ujra
        {
            CameraSpacePoint point = handpoints[0];
            float[][] handPoints = new float[handpoints.Count][];
            for (int i = 0; i < handpoints.Count; i++)
            {
                handPoints[i] = new float[3];
                handPoints[i][0] = handpoints[i].X - point.X;
                handPoints[i][1] = handpoints[i].Y - point.Y;
                handPoints[i][2] = handpoints[i].Z - point.Z;
            }

            for (int i = 0; i < existingCommands.Count; i++)
            {
                CameraSpacePoint[] csp = existingCommands[i].spline(0.0f);

                if (isGoodCommand(handPoints, csp))         //vizsgalja, hogy megeggyezik az adott command kezdopontjaival
                {
                    goodCommands.Add(goodCommands.Count, existingCommands[i]);
                    firstPoints.Add(goodCommands.Count, handpoints[0]);
                    startTime.Add(goodCommands.Count, time);
                }
            }
        }

        private bool isGoodCommand(float[][] handpoints, CameraSpacePoint[] commandpoints)
        {
            if (!compareSpacePoints(handpoints[0], commandpoints[0]))
            {
                return false;
            }
            if (!compareSpacePoints(handpoints[1], commandpoints[1]))
            {
                return false;
            }
            if (!compareSpacePoints(handpoints[2], commandpoints[2]))
            {
                return false;
            }
            if (!compareSpacePoints(handpoints[3], commandpoints[3]))
            {
                return false;
            }

            return true;
        }

        private bool compareSpacePoints(float[] handpoint, CameraSpacePoint commandpoint)
        {
            if (Math.Abs(handpoint[0] - commandpoint.X) < 0.05)
            {
                return false;
            }
            if (Math.Abs(handpoint[1] - commandpoint.Y) < 0.05)
            {
                return false;
            }
            if (Math.Abs(handpoint[2] - commandpoint.Y) < 0.05)
            {
                return false;
            }
            return true;
        }
    }
}
