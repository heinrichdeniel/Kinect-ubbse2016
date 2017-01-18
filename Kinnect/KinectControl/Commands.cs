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

            public Spline s
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
                s = new Spline(pointcount);
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
                s = new Spline(pointcount);
            }


            //return the 4 CameraSpacePoint of the right hand at a given time

            public CameraSpacePoint[] spline(float t)
            {
                CameraSpacePoint[] returnResult = new CameraSpacePoint[4];
                for (int i = 0; i < 4; ++i)
                {
                    s.set(avg[i * _number], timePointCount);
                    s.calculateAlpha();
                    returnResult[i].X = s.calculateRes(t);

                    s.set(avg[i * _number + 1], timePointCount);
                    s.calculateAlpha();
                    returnResult[i].Y = s.calculateRes(t);

                    s.set(avg[i * _number + 2], timePointCount);
                    s.calculateAlpha();
                    returnResult[i].Z = s.calculateRes(t);
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
            newCommand.points = new List<MomentInTime>();
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
            for (int j = 0; j < 4 * number; j++)
            {
                for (int i = 0; i < average.pointcount; i++)
                {
                    average.avg[j][i] /= (float)number;
                }
            }

            //set the time array for the average command
            for (int i = 0; i < average.pointcount; ++i)
            {
                average.timePointCount[i] = (float)((float)i / average.pointcount);
            }

            return average;
        }
    }
}
