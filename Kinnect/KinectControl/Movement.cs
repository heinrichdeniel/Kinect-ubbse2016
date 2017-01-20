using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectControl
{
    class Movement
    {
        private int _keyID;
        private Dictionary<float, CameraSpacePoint> _points;
        
        public Movement()
        {
            _points = new Dictionary<float, CameraSpacePoint>();
        }
        public int keyID
        {
            get
            {
                return _keyID;
            }
            set
            {
                _keyID = value;
            }
        }
        public Dictionary<float, CameraSpacePoint> points
        {
            get { return _points; }
            set { _points = value;  }
        }
    }
}
