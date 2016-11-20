using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManager
{
    class KeyInput
    {

        private XDocument doc = null;
        private String xmlFileName = "KeyCommands.xml";

        public KeyInput()
        {
            try
            {
                doc = XDocument.Load(xmlFileName);
            }
            catch(System.IO.FileNotFoundException e)
            {
                Console.WriteLine(xmlFileName + " not found");
            }
        }

        public void sendKey(int ID)
        {
            foreach (XElement xe in doc.Descendants("Item"))
            {
                if (xe.Element("ID").Value.Equals(ID + ""))
                {
                    System.Windows.Forms.SendKeys.SendWait(xe.Element("Name").Value);
                    break;
                }
            }
        }
    }
}
