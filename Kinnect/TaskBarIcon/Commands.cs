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
      
        public struct Command
        {
            public float totalTime;
            public List<MomentInTime> points;
        }

        public struct MomentInTime
        {
            public Dictionary<string, CameraSpacePoint> hand;
            public float time;
        }

        private Command[] commands;


        public Commands()
        {
            commands = new Command[3];
            commands[0].points = new List<MomentInTime>();
            commands[1].points = new List<MomentInTime>();
            commands[2].points = new List<MomentInTime>();
            commands[0].totalTime = new float();
            commands[1].totalTime = new float();
            commands[2].totalTime = new float();
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
           
            for (int i=0; i<=com.points.Count;i++)
            {
                newCommand.points[i] = com.points[i];
                newCommand.points[i].time = (float) com.points[i].time / com.totalTime;
            }
            return newCommand;
        }

    }
}
