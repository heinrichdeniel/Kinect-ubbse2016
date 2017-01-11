using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace KinectControl
{
    class MousePointer
    {
        private Brush b;
        private int x = 0, y = 0;
        private bool action = false;
        private double screen = 0;
        private int refresh;
        private bool showPointer = false;

        public MousePointer()
        {
            screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            if (screen > 1920)
            {
                screen = screen / 1920.0;
            }
            else
            {
                screen = 1920 / screen;
            }
            new Thread(moveCursor).Start();
        }

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);


        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        public void setMouseLocation(int x, int y)
        {
            this.x = (int)(x * screen);
            this.y = (int)(y * screen);
        }

        public void pointerVisibility(bool visible)
        {
            if (showPointer != visible)
            {
                showPointer = visible;
                ToggleDesktopIcons();
            }
        }

        public void setPrivateMouseLocation(int x, int y)
        {
            this.x = x;
            this.y = y;


        }
        public void setAction(bool action)
        {
            if (this.action != action)
            {
                this.action = action;
            }
        }

        public void moveCursor()
        {

            while (true)
            {
                if (showPointer)
                {
                    System.Windows.Point pMouse = MouseControl.GetCursorPosition();
                    if (pMouse != null)
                    {
                        if (this.x != (int)pMouse.X || this.y != (int)pMouse.Y)
                        {
                            this.x = (int)pMouse.X;
                            this.y = (int)pMouse.Y;
                            using (Graphics g = Graphics.FromHwndInternal(IntPtr.Zero))
                            {
                                g.FillEllipse(action ? Brushes.Blue : Brushes.Red, (int)(x * screen - 27 / 2), (int)(y * screen - 27 / 2), (int)(27 * screen), (int)(27 * screen));

                            }
                            ++refresh;


                            if (refresh % 3 == 0)
                            {
                                refresh = 0;
                                ToggleDesktopIcons();
                            }
                        }
                    }
                    Thread.Sleep(50);
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }




        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        private const int WM_COMMAND = 0x111;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size++ > 0)
            {
                var builder = new StringBuilder(size);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        public static IEnumerable<IntPtr> FindWindowsWithClass(string className)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                StringBuilder cl = new StringBuilder(256);
                GetClassName(wnd, cl, cl.Capacity);
                if (cl.ToString() == className && (GetWindowText(wnd) == "" || GetWindowText(wnd) == null))
                {
                    windows.Add(wnd);
                }
                return true;
            },
                        IntPtr.Zero);

            return windows;
        }

        static void ToggleDesktopIcons()
        {
            var toggleDesktopCommand = new IntPtr(0x7402);
            IntPtr hWnd = IntPtr.Zero;
            if (Environment.OSVersion.Version.Major < 6 || Environment.OSVersion.Version.Minor < 2) //7 and -
                hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
            else
            {
                var ptrs = FindWindowsWithClass("WorkerW");
                int i = 0;
                while (hWnd == IntPtr.Zero && i < ptrs.Count())
                {
                    hWnd = FindWindowEx(ptrs.ElementAt(i), IntPtr.Zero, "SHELLDLL_DefView", null);
                    i++;
                }
            }
            SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
        }
    }
}
