using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectControl
{
    class KeyInput
    {
        public int id;
        public String descriptionLong;
        public String description;
        public String windowsCommand;


        public void execute()
        {
            System.Windows.Forms.SendKeys.SendWait(windowsCommand);
        }
    }
}
