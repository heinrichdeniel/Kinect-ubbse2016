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
        TaskBar taskbar = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            new Program().runProgram();
            /*float[] x = { 1, 4, 9, 16, 25 };
            float[] time = { 1, 2, 3, 4, 5 };
            float t = 2.2f;
            Spline s = new Spline(x, time, 5);
            Console.WriteLine(s.calculateRes(t));*/
        }

        public void runProgram()
        {

            while (true)
            {
                ManagementObjectCollection mbsList = null;
                ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_USBHub");
                mbsList = mbs.Get();

                //itarate through USB hubs, until we found Kinect device connected
                foreach (ManagementObject mo in mbsList)
                {
                    if (Convert.ToString(mo["Name"]).IndexOf("SuperSpeed") > -1)
                    {
                        Console.WriteLine("Connected");
                        if (taskbar == null)
                        {
                            taskbar = new TaskBar();
                            Application.Run(taskbar);

                        }
                        taskbar.show();
                    }
                }
            }
        }
    }


}

