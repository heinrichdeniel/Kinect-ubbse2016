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
        private int[][][] momentum;
        private List< Dictionary<String, CameraSpacePoint> > [] points;
        
        public Commands()
        {
            points = new List<Dictionary<string, CameraSpacePoint>>[3];
            points[0] = new List<Dictionary<string, CameraSpacePoint>>();
            points[1] = new List<Dictionary<string, CameraSpacePoint>>();
            points[2] = new List<Dictionary<string, CameraSpacePoint>>();
            momentum = new int[3][][];
        }

        public void addDictionary(Dictionary<String, CameraSpacePoint> currentPosition, int i)
        {
            points[i].Add(currentPosition);   
        }

    }
}
