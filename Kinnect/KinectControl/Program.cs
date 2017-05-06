using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KinectControl
{
    public class Program
    {
        Form1 form = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Log.log.Info("Application Started");
            new Program().runProgram();
        }

        public void runProgram()
        {

            while (true)
            {
                if (isConnected())
                {
                    Log.log.Info("Kinect connected");
                    if (form == null)
                    {
                        form = new Form1();
                        Application.Run(form);

                    }
                    form.show();
                }
            }
        }

        public bool isConnected()
        {
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_USBHub");
            mbsList = mbs.Get();

            //itarate through USB hubs, until we found Kinect device connected
            foreach (ManagementObject mo in mbsList)
            {
                if (Convert.ToString(mo["Name"]).IndexOf("SuperSpeed") > -1)
                {
                    return true;
                }
            }
            return false;
        }
    }


}

