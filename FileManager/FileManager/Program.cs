using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            KeyInput keyInput = new KeyInput();
            keyInput.sendKey(14);

            //System.Windows.Forms.SendKeys.SendWait("{F1}");
        }
        public string returnPath()
        {
            string folder = Environment.CurrentDirectory;
            return folder;
        }
    }
}
