using System;

namespace KinectControl
{
    class Log
    {
        public static log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    }
}
