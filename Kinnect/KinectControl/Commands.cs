using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectControl
{
    public class Commands
    {

        //data members

        private static int _number = 3;
        private Command[] _commands;

        // set, get for the data members

        public int number
        {
            get { return _number; }
            set { _number = value; }
        }

        public Command[] commands
        {
            get { return _commands; }
            set { _commands = value; }
        }


        //used classes

        public class Average
        {
            // data members

            private int _pointcount = 30;    //length of the arrays
            private float[][] _avg;          //coordinates, for the hand in average command
            private int _keyID;              //which command is associated to this movement
            private float _time;             //average time of the 3 commands
            private float[] _timePointCount; //array of time 
            private Spline _s;              //Spline class associated to this average 

            // set, get for the data members

            public int pointcount
            {
                get { return _pointcount; }
                set { _pointcount = value; }
            }

            public float[][] avg
            {
                get { return _avg; }
                set { _avg = value; }
            }

            public int keyID
            {
                get { return _keyID; }
                set { _keyID = value; }
            }

            public float time
            {
                get { return _time; }
                set { _time = value; }
            }

            public float[] timePointCount
            {
                get { return _timePointCount; }
                set { _timePointCount = value; }
            }

            public Spline _spline
            {
                get { return _s; }
                set { _s = value; }
            }

            //contructors

            public Average()
            {
                timePointCount = new float[pointcount];
                avg = new float[12][];
                for (int i = 0; i < 12; i++)
                {
                    avg[i] = new float[pointcount];
                }
               // _spline = new Spline(pointcount);
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
              //  _spline = new Spline(pointcount);
            }


            //return the 4 CameraSpacePoint of the right hand at a given time

            public CameraSpacePoint[] spline(float t)
            {
                CameraSpacePoint[] returnResult = new CameraSpacePoint[4];
                if (_spline != null)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        _spline.set(avg[i * _number], timePointCount);
                        _spline.calculateAlpha();
                        returnResult[i].X = _spline.calculateRes(t);

                        _spline.set(avg[i * _number + 1], timePointCount);
                        _spline.calculateAlpha();
                        returnResult[i].Y = _spline.calculateRes(t);

                        _spline.set(avg[i * _number + 2], timePointCount);
                        _spline.calculateAlpha();
                        returnResult[i].Z = _spline.calculateRes(t);
                    }
                }
                return returnResult;
            }
        }


        public class Command
        {
            //data members

            private float _totalTime;
            private List<MomentInTime> _points;

            // set, get for the data members

            public float totalTime
            {
                get { return _totalTime; }
                set { _totalTime = value; }
            }

            public List<MomentInTime> points
            {
                get { return _points; }
                set { _points = value; }
            }

            //constructor

            public Command()
            {
                _points = new List<MomentInTime>();
            }
        }

        public class MomentInTime
        {
            //Data members

            private CameraSpacePoint[] _hand = new CameraSpacePoint[4];
            private float _time;

            // set, get for the data members

            public CameraSpacePoint[] hand
            {
                get { return _hand; }
                set { _hand = value; }
            }

            public float time
            {
                get { return _time; }
                set { _time = value; }
            }
        }


        //Constructors for Commands class

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
            newCommand.points = com.points;
            newCommand.totalTime = com.totalTime;

            for (int i = 0; i < com.points.Count; i++)
            {
                newCommand.points[i].time = (float)com.points[i].time / com.totalTime;
            }

            return newCommand;
        }


        //calculate average coordinated
        public Average averageCommand(int k)
        {

            float t = 0;

            //standardization of all 3 commands 
            for (int i = 0; i < number; i++)
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


            //make the coordinates relative, not absolute  
            //get the functions with spline for 3 commands
            for (int comm = 0; comm < number; ++comm)
            {
                for (int j = 0; j < 4; j++)
                {
                    float avg_0_0 = commands[comm].points[0].hand[j].X;
                    float avg_1_0 = commands[comm].points[0].hand[j].Y;
                    float avg_2_0 = commands[comm].points[0].hand[j].Z;

                    int n = commands[comm].points.Count;
                    x1 = new float[n];
                    y1 = new float[n];
                    z1 = new float[n];
                    time1 = new float[n];
                    alpha = new float[n];
                    for (int i = 0; i < n; i++)
                    {
                        x1[i] = commands[comm].points[i].hand[j].X - avg_0_0;
                        y1[i] = commands[comm].points[i].hand[j].Y - avg_1_0;
                        z1[i] = commands[comm].points[i].hand[j].Z - avg_2_0;
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

            //get x point from each command function, and calulate the average coordinates 
            //and make the average relative

            float avg_0 = average.avg[0][0] / (float)number;
            float avg_1 = average.avg[1][0] / (float)number;
            float avg_2 = average.avg[2][0] / (float)number;

            for (int j = 0; j < 4 * number; j++)
            {
                for (int i = 0; i < average.pointcount; i++)
                {
                    average.avg[j][i] /= (float)number;
                    if (j % 3 == 0)
                    {
                        average.avg[j][i] -= avg_0;
                    }
                    else
                    {
                        if (j % 3 == 1)
                        {
                            average.avg[j][i] -= avg_1;
                        }
                        else
                        {
                            average.avg[j][i] -= avg_2;
                        }
                    }

                }
            }

            //set the time array for the average command
            for (int i = 0; i < average.pointcount; ++i)
            {
                average.timePointCount[i] = (float)((float)i / average.pointcount);
            }

            return average;
        }

        public void test()
        {
            /*float a = 1.24f;
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
            }*/

            float[] x = { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f };
            float[] y = { 0.2f, 0.9f, 1.3f, 2.5f, 0.7f, 3.4f, 1.6f, 1.5f, 0.01f, 1.4f };
            //float[] y = { 1f, 4f, 9f, 16f, 25f, 36f, 49f, 64f, 81f, 100f };
            int n = 10;
            Spline s = new Spline(n);
            s.set(y, x, n);
            s.calculateAlpha();
            float a = s.calculateRes(2.5f);
            Debug.Write(a+"\n\n");

            a = s.calculateRes(3.6f);
            Debug.Write(a + "\n\n");

            a = s.calculateRes(7.89f);
            Debug.Write(a + "\n\n");

            a = s.calculateRes(3f);
            Debug.Write(a + "\n\n");

        }
    }
}
