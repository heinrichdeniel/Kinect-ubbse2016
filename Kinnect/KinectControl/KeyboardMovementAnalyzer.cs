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
        private Dictionary<int, Commands.Average> goodCommands = new Dictionary<int, Commands.Average>();
        private Dictionary<int, CameraSpacePoint> firstPoints = new Dictionary<int, CameraSpacePoint>();
        private Dictionary<int, long> startTime = new Dictionary<int, long>();

        public KeyboardMovementAnalyzer(List<Commands.Average> commands)
        {
            existingCommands = commands;
        }


        public void AnalyzeFrames(long time, List<CameraSpacePoint> handpoints)
        {
            compareWithExistingCommands(handpoints,time);

            compareWithGoodCommands(handpoints, time);
        }

        private void compareWithGoodCommands(List<CameraSpacePoint> handpoints, long time)
        {

            for (int i = 0; i < goodCommands.Count; i++)
            {
                float[][] handPoints = new float[handpoints.Count][];
                   for (int j = 0; j < handpoints.Count; j++)
                {
                    handPoints[j] = new float[3];
                    handPoints[j][0] = handpoints[j].X - firstPoints.ElementAt(i).Value.X;
                    handPoints[j][1] = handpoints[j].Y - firstPoints.ElementAt(i).Value.Y;
                    handPoints[j][2] = handpoints[j].Z - firstPoints.ElementAt(i).Value.Z;
                }

                CameraSpacePoint[] csp = goodCommands.ElementAt(i).Value.spline(time-startTime.ElementAt(i).Value);
                
                if (goodCommands[i].time < time - startTime.ElementAt(i).Value)
                {
                    FileManager.getInstance().getKeyInput(goodCommands[i].keyID).execute();
                    goodCommands.Clear();
                    startTime.Clear();
                    firstPoints.Clear();
                }
                else if (!isGoodCommand(handPoints, csp))         //vizsgalja, hogy megeggyezik az adott command kezdopontjaival
                {
                    if (i != goodCommands.Count - 1)
                    {
                        goodCommands[i] = goodCommands.ElementAt(goodCommands.Count - 1).Value;
                        startTime[i] = startTime.ElementAt(goodCommands.Count - 1).Value;
                        firstPoints[i] = firstPoints.ElementAt(goodCommands.Count - 1).Value;
                    }

                    startTime.Remove(goodCommands.Count-1);
                    firstPoints.Remove(goodCommands.Count-1);
                    goodCommands.Remove(goodCommands.Count-1);

                }

            }
           
        }

        private void compareWithExistingCommands(List<CameraSpacePoint> handpoints, long time)   //megvizsgalja, hogy a letezo commandok kozul melyik kezdodik ujra
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

            {
                return false;
            }
            {
                return false;
            }
            {
                return false;
            }
            return true;
        }
    }
}
