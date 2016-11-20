using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public class Program
    {
        private static string path = System.IO.Directory.GetCurrentDirectory();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        public static void Main()
        {

            path = System.IO.Directory.GetCurrentDirectory();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            KeyInput keyInput = new KeyInput();
            keyInput.sendKey(14);
            //System.Windows.Forms.SendKeys.SendWait("{F1}");
        }
        public string returnPath()
        {
            return new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).AbsolutePath ;
        }
    }
}
