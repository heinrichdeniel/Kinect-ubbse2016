using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Threading;

namespace Kinnect
{
        public partial class Service1 : ServiceBase
    {
        private bool started = false;

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };


        private int eventId = 0;

        public Service1(string[] args)
        {
            InitializeComponent();

            string eventSourceName = "MySource";
            string logName = "MyNewLog";
            if (args.Count() > 0) { eventSourceName = args[0]; }
            if (args.Count() > 1) { logName = args[1]; }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;

        }

        protected override void OnStart(string[] args)
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In OnStart");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }

        public void OnTimer(Object sender, System.Timers.ElapsedEventArgs args)
        {
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_USBHub");
            mbsList = mbs.Get();

            foreach (ManagementObject mo in mbsList)
            {
               // Console.WriteLine("USBHub device Friendly name:{0}", mo["Name"].ToString());
                if (Convert.ToString(mo["Name"]).IndexOf("SuperSpeed") > -1)
                {
                    // Process.Start("notepad.exe", "D:/myd.txt");
                    if (started == false)
                    {
                        string lines = "Kinect elindult";
                        eventLog1.WriteEntry("Kinect kapcsolodott !!!", EventLogEntryType.Information, eventId++);

                        // Write the string to a file.
                        // System.IO.StreamWriter file = new System.IO.StreamWriter("d:\\test.txt");
                        //file.WriteLine(lines);

                        //file.Close();

                        FileManager.Program pb = new FileManager.Program();
                        Process.Start(pb.returnPath() + "Project_B.exe");
                        started = true;
            
                    }
                }
                else
                {
                    started = false;
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);


    }
}
