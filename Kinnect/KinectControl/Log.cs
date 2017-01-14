using System;

namespace KinectControl
{
    class Log
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Log()
        {
            log.Info("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Application is working");
            Console.WriteLine("Loo");

        }
    }
}
