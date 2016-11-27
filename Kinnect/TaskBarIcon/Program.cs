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
        Boolean started = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            new Program().runProgram();
        }
        public void runProgram()
        {
            
            int i = 0;
            while ( true )
            {
                i++;
                ManagementObjectCollection mbsList = null;
                ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_USBHub");
                mbsList = mbs.Get();

                bool isConnected = false;
                foreach (ManagementObject mo in mbsList)
                {
                    // Console.WriteLine("USBHub device Friendly name:{0}", mo["Name"].ToString());
                    if (Convert.ToString(mo["Name"]).IndexOf("SuperSpeed") > -1)
                    {
                      
                        isConnected = true;
                        if (!started)
                        {
                            started = true;
                            if (taskbar == null)
                            {
                                taskbar = new TaskBar();
                                Application.Run(taskbar);

                            }
                            taskbar.show();
                        }
                        break;
                    }
                }
                if (!isConnected && started )
                {
                     started = false;
                }
            }
        }
      

    }
   
}
