using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectControl
{
    class Commands
    {

        //used classes

        public class Average
        {
            public int pointcount = 60;
            public float[][] avg;
            public int keyID;
            public float time;
            public float[] timePointCount;

            public Average()
            {
                timePointCount = new float[pointcount];
                avg = new float[12][];
                for (int i = 0; i < 12; i++)
                {
                    avg[i] = new float[pointcount];
                }
            }
            public Average(int k)
            {
                keyID = k;
                timePointCount = new float[pointcount];
                avg = new float[12][];
                for (int i = 0; i < 12; i++)
                {
                    avg[i] = new float[pointcount];
                }
            }
        }


        public class Command
        {
            public float totalTime;
            public List<MomentInTime> points;

            public Command()
            {
                points = new List<MomentInTime>();
            }
        }

        public class MomentInTime
        {
            public CameraSpacePoint[] hand = new CameraSpacePoint[4];
            public float time;
        }

        //data members

        public int number = 3;
        private Command[] commands;

        //Constructors

        public Commands()
        {
            commands = new Command[number];

            for (int i = 0; i < number; i++)
            {
                commands[i] = new Command();
                commands[i].points = new List<MomentInTime>();
                commands[i].totalTime = new float();
            }
        }

        public Commands(Command[] command)
        {
            commands = new Command[number];

            for (int i = 0; i < number; i++)
            {
                commands[i] = new Command();
                commands[i].points = new List<MomentInTime>();
                commands[i].totalTime = new float();
            }
            this.commands = command;
        }



        //function members
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

        public Command standardization(Command com)
        {
            Command newCommand = new Command();
            newCommand.points = new List<MomentInTime>();
            newCommand.points = com.points;
            newCommand.totalTime = com.totalTime;

            for (int i = 0; i < com.points.Count; i++)
            {
                newCommand.points[i].time = (float)com.points[i].time / com.totalTime;
            }

            return newCommand;
        }


        //standardization of all 3 commands
        //spline all 3 standardize commands
        //get x point from each command function
        //calculate average function
        public Average averageCommand(int k)
        {

            float t = 0;
            for (int i = 0;  i < number; i++)
            {
                commands[i] = standardization(commands[i]);
                t += commands[i].totalTime;
            }
            t /= (float)number;

            Average average = new Average(k);
            average.time = t;
            float[] x1;
            float[] y1;
            float[] z1; 
            float[] time1;
            float[] alpha;
            

            for (int j = 0; j < 4 * number; j++)
            {
                for (int i = 0; i < average.pointcount; i++)
                {
                    average.avg[j][i] = 0;
                }
            }

            for (int comm = 0; comm < number; ++comm)
            {
                for (int j = 0; j < 4; j++)
                {
                    int n = commands[comm].points.Count;
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
                        average.avg[j * number][i] += s.calculateRes((float)i / average.pointcount);
                    }
                    s.set(y1, time1, n);
                    alpha = s.calculateAlpha();
                    for (int i = 0; i < average.pointcount; i++)
                    {
                        average.avg[j * number + 1][i] += s.calculateRes((float)i / average.pointcount);
                    }
                    s.set(z1, time1, n);
                    alpha = s.calculateAlpha();
                    for (int i = 0; i < average.pointcount; i++)
                    {
                        average.avg[j * number + 2][i] += s.calculateRes((float)i / average.pointcount);
                    }
                }
            }

            for (int j = 0; j < 4 * number; j++)
            {
                float avg_j_0 = average.avg[j][0]/ (float) number; 
                for (int i = 0; i < average.pointcount; i++)
                {
                    average.avg[j][i] /= (float)number;
                    average.avg[j][i] -= avg_j_0;
                }
            }

            for (int i = 0; i < average.pointcount; ++i)
            {
                average.timePointCount[i] = (float)((float)i / average.pointcount);
            }

            return average;
        }


        //spline this average function :)
        //return the 4 CameraSpacePoint at a given 

        public CameraSpacePoint[] spline(float t, Average average)
        {
            CameraSpacePoint[] returnResult = new CameraSpacePoint[4];
            for (int i = 0; i < 4; ++i)
            {
                Spline s = new Spline(average.avg[i * number], average.timePointCount, average.pointcount);
                s.calculateAlpha();
                returnResult[i].X = s.calculateRes(t);

                s = new Spline(average.avg[i * number + 1], average.timePointCount, average.pointcount);
                s.calculateAlpha();
                returnResult[i].Y = s.calculateRes(t);

                s = new Spline(average.avg[i * number + 2], average.timePointCount, average.pointcount);
                s.calculateAlpha();
                returnResult[i].Z = s.calculateRes(t);
            }
            return returnResult;
        }

        public void test()
        {
            float a = 1.24f;
            int n = 40;
            Random rnd = new Random();
            Commands comms = new Commands();
            List<MomentInTime> p;
            for (int i = 0; i < 3; i++)
            {
                Command com = new Command();
                com.totalTime = a + 0.01f;
                p = new List<MomentInTime>();
                float[] prev = new float[4];
                for (int j = 0; j < 4; j++)
                {
                    prev[j] = 0;
                }

                for (int j = 0; j < n + i * 4; j++)
                {
                    MomentInTime mit = new MomentInTime();

                    for (int t = 0; t < 4; t++)
                    {
                        float something = (((float)rnd.NextDouble()) / 100f);
                        mit.hand[t].X = prev[t] + something;
                        prev[t] = mit.hand[t].X;
                        mit.hand[t].Y = 0f + ((float)(rnd.NextDouble() * 2.0 - 1.0) / 1000f);
                        mit.hand[t].Z = 0f + ((float)(rnd.NextDouble() * 2.0 - 1.0) / 1000f);
                    }
                    mit.time = (com.totalTime / (n + i * 4)) * j;
                    p.Add(mit);
                }
                com.points = p;
                comms.setCommandByIndex(i, com);
            }
            Average average = new Average();
            average = comms.averageCommand(2);

            Debug.Write("Time: ");
            for (int j = 0; j < average.pointcount; j++)
            {
                Debug.Write(average.timePointCount[j]);
                Debug.Write(", ");
            }
            Debug.Write("\n\n");
            for (int i = 0; i < 12; i++)
            {
                Debug.Write(i);
                Debug.Write(": ");
                for (int j = 0; j < average.pointcount; j++)
                {
                    Debug.Write(average.avg[i][j]);
                    Debug.Write(", ");
                }
                Debug.Write("\n\n");
            }
        }


    }
}
