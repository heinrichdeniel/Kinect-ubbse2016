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
        private byte action = 0;
        private double screenx = 0;
        private double screeny = 0;
        private int refresh;
        private bool showPointer = false;

        public MousePointer()
        {
            screenx = Screen.PrimaryScreen.Bounds.Width;
            screeny = Screen.PrimaryScreen.Bounds.Height;
            if (screenx > 1920)
            {
                screenx = screenx / 1920.0;
                screeny = screeny / 1080;
            }
            else
            {
                screeny = 1080 / screeny;
                screenx = 1920 / screenx;
            }
            new Thread(moveCursor).Start();
        }

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr dc);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hwnd, IntPtr lpRect, bool bErase);

        public void setMouseLocation(int x, int y)
        {
            this.x = (int)(x * screenx);
            this.y = (int)(y * screenx);
        }

        public void pointerVisibility(bool visible)
        {
            if (showPointer != visible)
            {
                showPointer = visible;
                if (showPointer)
                {
                    new Thread(moveCursor).Start();
                }
            }
        }

        public void setPrivateMouseLocation(int x, int y)
        {
            this.x = x;
            this.y = y;


        }
        public void setLeftClick(bool action)
        {
            if (action && this.action != 2)
            {
                this.action = 2;
            }
            else if (!action && this.action == 2)
            {
                this.action = 0;
            }
        }

        public void setRightClick(bool action)
        {
            if (action && this.action != 1)
            {
                this.action = 1;
            }
            else if (!action && this.action == 1)
            {
                this.action = 0;
            }
        }

        public void moveCursor()
        {

            while (showPointer)
            {

                System.Windows.Point pMouse = MouseControl.GetCursorPosition();
                if (pMouse != null)
                {
                    if ((this.x != (int)pMouse.X || this.y != (int)pMouse.Y) && refresh % 2 == 0)
                    {
                        this.x = (int)pMouse.X;
                        this.y = (int)pMouse.Y;


                        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
                        {
                            g.FillEllipse(action == 0 ? Brushes.Red : action == 1 ? Brushes.LawnGreen : Brushes.Blue, (int)(x * screenx - 27 / 2), (int)(y * screenx - 27 / 2), (int)(27 * screeny), (int)(27 * screeny));
                        }

                        ++refresh;


                        if (refresh % 3 == 0)
                        {

                            InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
                            refresh = 0;
                        }
                    }
                    else
                    {
                        ++refresh;
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
