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
        public KeyInput()
        {
            XDocument doc = XDocument.Load("KeyCommands.xml");

            var authors = doc.Descendants("Name");

            foreach (var author in authors)
            {
                Console.WriteLine(author.Value);
            }
            Console.ReadLine();
        }
    }
}
