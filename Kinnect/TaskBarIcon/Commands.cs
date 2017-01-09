using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBarIcon
{
    class Commands
    {
        public int number = 3;

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

        /*public Command average(Commands coms)
        {
            Command newCommand = new Command();
            newCommand.points = new List<MomentInTime>();
            newCommand.totalTime = (coms.commands[0].totalTime + coms.commands[1].totalTime + coms.commands[2].totalTime) / 3;

            int min = coms.commands[0].points.Count;
            int ind = 0;
            for (int i = 1; i<= 2; i++)
            {
                if (coms.commands[i].points.Count < min)
                {
                    min = coms.commands[i].points.Count;
                    ind = i;
                }
            }

            for (int i = 0; i <= min; i++)
            {                
                newCommand.points[i].hand = com.points[i].hand;
                newCommand.points[i].time = (float)com.points[i].time / com.totalTime;
            }
            
            return newCommand;
        }*/

        public Dictionary<int, CameraSpacePoint> spline(Commands coms, int t)
        {
            Dictionary<int, CameraSpacePoint> h = new Dictionary<int, CameraSpacePoint>();

            //calculate points coordinates of a given Command at a given time... ??? 

            return h;
        }

    }
}
