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

        XDocument doc;
        String xmlFileName = "eyCommands.xml";

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

            //var authors = doc.Descendants("Name");

            //foreach (var author in authors)
            //{
            //    Console.WriteLine(author.Value);
            //}
            //Console.ReadLine();
        }
    }
}
