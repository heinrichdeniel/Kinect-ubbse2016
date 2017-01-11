using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectControl
{
    class Commands
    {
        public int number = 3;

        public class Average
        {
            public int pointcount;
            public float[][] avg;
            public float[] timePointCount;
            public Average(int n)
            {
                pointcount = n;
                timePointCount = new float[n];
                avg = new float[12][];
                for (int i = 0; i < 12; i++)
                {
                    avg[i] = new float[n];
                }
            }
        }


        public class Command
        {
            public float totalTime;
            public List<MomentInTime> points;
            public int keyID;
        }

        public class MomentInTime
        {
            public CameraSpacePoint[] hand = new CameraSpacePoint[4];
            public float time;
        }

        private Command[] commands;


        public Commands()
        {
            commands = new Command[number];
            commands[0].points = new List<MomentInTime>();
            commands[1].points = new List<MomentInTime>();
            commands[2].points = new List<MomentInTime>();
            commands[0].totalTime = new float();
            commands[1].totalTime = new float();
            commands[2].totalTime = new float();
        }

        public Commands(Command[] command)
        {
            this.commands = command;
        }

        public void setCommandByIndex(int index, Command command)
        {
            if (index < number && index >= 0)
            {
                commands[index] = command;
            }
        }

        public Command getCommandByIndex(int index)
        {
            if (index < number && index >= 0)
            {
                return commands[index];
            }
            return new Command();
        }

        public void addDictionary(MomentInTime currentPosition, int i)
        {
            commands[i].points.Add(currentPosition);
        }

        public void setTime(float t, int i)
        {
            commands[i].totalTime = t;
        }

        public Command standardization(Command com)
        {
            Command newCommand = new Command();
            newCommand.points = new List<MomentInTime>();
            newCommand.totalTime = com.totalTime;
            newCommand.keyID = com.keyID;

            for (int i = 0; i <= com.points.Count; i++)
            {
                newCommand.points[i].hand = com.points[i].hand;
                newCommand.points[i].time = (float)com.points[i].time / com.totalTime;
            }

            return newCommand;
        }


        //standardization of all 3 commands
        //spline all 3 standardize commands
        //get x point from each command function
        //calculate average function
        public Average averageCommand()
        {
  
            int n = Math.Max(Math.Max(commands[0].points.Count, commands[1].points.Count), commands[2].points.Count);

            Average average = new Average(n);
            float[] x1;
            float[] y1;
            float[] z1;
            float[] time1;
            float[] alpha;
            

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    average.avg[j][i] = 0;
                }
            }

            for (int comm = 0; comm < number; ++comm)
            {
                for (int j = 0; j < 4; j++)
                {
                    n = commands[comm].points.Count;
                    x1 = new float[n];
                    y1 = new float[n];
                    z1 = new float[n];
                    time1 = new float[n];
                    alpha = new float[n];
                    for (int i = 0; i < n; i++)
                    {
                        x1[i] = commands[comm].points[i].hand[j].X;
                        y1[i] = commands[comm].points[i].hand[j].Y;
                        z1[i] = commands[comm].points[i].hand[j].Z;
                        time1[i] = commands[comm].points[i].time;
                    }

                    Spline s = new Spline(x1, time1, n);
                    alpha = s.calculateAlpha();
                    for (int i = 0; i < average.pointcount; i++)
                    {
                        average.avg[j * 3][i] += s.calculateRes((float)i / average.pointcount);
                    }
                    s.set(y1, time1, n);
                    alpha = s.calculateAlpha();
                    for (int i = 0; i < average.pointcount; i++)
                    {
                        average.avg[j * 3 + 1][i] += s.calculateRes((float)i / average.pointcount);
                    }
                    s.set(z1, time1, n);
                    alpha = s.calculateAlpha();
                    for (int i = 0; i < average.pointcount; i++)
                    {
                        average.avg[j * 3 + 2][i] += s.calculateRes((float)i / average.pointcount);
                    }
                }
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    average.avg[j][i] /= 3f;
                }
            }
            return average;
        }


        //spline this average function :)
        //return the 4 CameraSpacePoint at a given 

        public CameraSpacePoint[] spline(float t, Average average)
        {
            for (int i = 0; i < average.pointcount; ++i)
            {
                average.timePointCount[i] = (float)(i / average.pointcount);
            }
            CameraSpacePoint[] returnResult = new CameraSpacePoint[4];
            for (int i = 0; i < 4; ++i)
            {
                Spline s = new Spline(average.avg[i * 3], average.timePointCount, average.pointcount);
                s.calculateAlpha();
                returnResult[i].X = s.calculateRes(t);

                s = new Spline(average.avg[i * 3 + 1], average.timePointCount, average.pointcount);
                s.calculateAlpha();
                returnResult[i].Y = s.calculateRes(t);

                s = new Spline(average.avg[i * 3 + 2], average.timePointCount, average.pointcount);
                s.calculateAlpha();
                returnResult[i].Z = s.calculateRes(t);
            }
            return returnResult;
        }


    }
}
