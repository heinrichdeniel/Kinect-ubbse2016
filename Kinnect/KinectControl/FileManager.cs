using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace KinectControl
{
    class FileManager
    {

        private XDocument keyCommandDoc = null;
        private XmlDocument kinnectXMLCommands;
        private String xmlKeyCommandFileName = "KeyCommands.xml"; //this xml document contains all data about saved movements.
        private String xmlKinectMovementFileName = "KinectCommands.xml"; //this xml document contains windows key combination commands.

        private static FileManager INSTANCE;

        public static FileManager getInstance()
        {
            if(INSTANCE == null)
            {
                INSTANCE = new FileManager();
            }
            return INSTANCE;
        }
        //Constructor
        private FileManager()
        {


            try
            {
                keyCommandDoc = XDocument.Load(xmlKeyCommandFileName);
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine(xmlKeyCommandFileName + " not found");
            }
            fileExist(xmlKinectMovementFileName, "movements");
            kinnectXMLCommands = new XmlDocument();
            try {
                kinnectXMLCommands.Load(xmlKinectMovementFileName);
            } catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }


        }


        //Write a command into the xml file
        public void writeCommand(Commands.Average average)
        {
            XmlElement movement = kinnectXMLCommands.CreateElement("command");
            XmlElement id = kinnectXMLCommands.CreateElement("id");
            id.InnerText = average.keyID.ToString();
            XmlElement pointCount = kinnectXMLCommands.CreateElement("point_count");
            pointCount.InnerText = average.pointcount.ToString();
            XmlElement timePoints = kinnectXMLCommands.CreateElement("time_points_count");
            foreach (float ctime in average.timePointCount)
            {
                XmlElement timePoint = kinnectXMLCommands.CreateElement("time_point");
                timePoint.InnerText = ctime.ToString();
                timePoints.AppendChild(timePoint);
            }

            XmlElement avg = kinnectXMLCommands.CreateElement("avg");
            int i = 0;
            foreach (float []ctime in average.avg)
            {
                foreach (float cctime in ctime)
                {
                    XmlElement avg_point = kinnectXMLCommands.CreateElement("avg_point");
                    avg_point.InnerText = cctime.ToString();
                    avg.AppendChild(avg_point);
                }
            }
            movement.AppendChild(id);
            movement.AppendChild(pointCount);
            movement.AppendChild(timePoints);
            movement.AppendChild(avg);
            kinnectXMLCommands.DocumentElement.AppendChild(movement);
            kinnectXMLCommands.Save("/" + xmlKinectMovementFileName);
        }

        //Read a command from the xml file
        public Commands.Average readCommand(int keyInputID)
        {
            Commands.Average command = new Commands.Average();
            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {
                    if (Int32.Parse(movement.SelectSingleNode("id").InnerText) == keyInputID)
                    {

                        command.keyID = keyInputID;
                        command.pointcount = Int32.Parse(movement.SelectSingleNode("point_count").InnerText);
                        float[] timePoints = new float[60];
                        int i = 0;
                        foreach (XmlNode moment in movement.SelectSingleNode("time_points_count").SelectNodes("time_point"))
                        {
                            timePoints[i] = float.Parse(moment.InnerText);
                            ++i;
                        }
                        command.timePointCount = timePoints;

                        float[][] avg = new float[12][];
                        i = 0;
                        int j = 0;
                        foreach (XmlNode moment in movement.SelectSingleNode("avg").SelectNodes("avg_point"))
                        {
                            if(j == 0)
                            {
                                avg[i] = new float[60];
                            }
                            if(j >= 60)
                            {
                                j = 0;
                                ++i;

                            }
                            avg[i][j] = float.Parse(moment.InnerText);
                            ++i;
                        }
                        command.avg = avg;

                        return command;
                    }
                }
            }
            command.keyID = -1;
            return command;
        }


        //Update command
        public bool updateCommand(Commands.Average command)
        {

            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {
                    if (Int32.Parse(movement.SelectSingleNode("id").InnerText) == command.keyID)
                    {

                        movement.SelectSingleNode("point_count").InnerText = command.pointcount.ToString();

                        movement.SelectSingleNode("time_points_count").RemoveAll();
                        foreach (float moment in command.timePointCount)
                        {
                            XmlElement timePoint = kinnectXMLCommands.CreateElement("time_point");
                            timePoint.InnerText = moment.ToString();
                            movement.SelectSingleNode("time_point_count").AppendChild(timePoint);
                        }

                        movement.SelectSingleNode("avg").RemoveAll();
                        foreach (float []moment in command.avg)
                        {
                            foreach (float cmoment in moment)
                            {
                                XmlElement timePoint = kinnectXMLCommands.CreateElement("avg_point");
                                timePoint.InnerText = cmoment.ToString();
                                movement.SelectSingleNode("time_point_count").AppendChild(timePoint);
                            }
                        }
                        kinnectXMLCommands.DocumentElement.AppendChild(movement);
                        kinnectXMLCommands.Save("\\" + xmlKinectMovementFileName);
                        return true;
                    }
                }
            }
            return false;
        }


        //Remove a command
        public bool removeCommand(int keyInputID)
        {

            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {
                    if (Int32.Parse(movement.SelectSingleNode("id").InnerText) == keyInputID)
                    {
                        movement.RemoveAll();
                        kinnectXMLCommands.Save("\\" + xmlKinectMovementFileName);
                        return true;
                    }
                }
            }
            return false;
        }

        //Read all commands from the xml file
        public List<Commands.Average> readAllCommands()
        {
            List<Commands.Average> commands = new List<Commands.Average>();
            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {

                    Commands.Average command = new Commands.Average();
                    command.keyID = Int32.Parse(movement.SelectSingleNode("id").InnerText);
                    command.pointcount = Int32.Parse(movement.SelectSingleNode("point_count").InnerText);
                    float[] timePoints = new float[60];
                    int i = 0;
                    foreach (XmlNode moment in movement.SelectSingleNode("time_points_count").SelectNodes("time_point"))
                    {
                        timePoints[i] = float.Parse(moment.InnerText);
                        ++i;
                    }
                    command.timePointCount = timePoints;

                    float[][] avg = new float[12][];
                    i = 0;
                    int j = 0;
                    foreach (XmlNode moment in movement.SelectSingleNode("avg").SelectNodes("avg_point"))
                    {
                        if (j == 0)
                        {
                            avg[i] = new float[60];
                        }
                        if (j >= 60)
                        {
                            j = 0;
                            ++i;

                        }
                        avg[i][j] = float.Parse(moment.InnerText);
                        ++i;
                    }
                    command.avg = avg;
                    commands.Add(command);
                }
            }

            return commands;
        }

        //check the file (filename) existence
        //and if the file does not exist, create the file
        private void fileExist(String filename, String startingTag)
        {
            if (!File.Exists(Environment.CurrentDirectory + "\\" + filename))
            {
                try {
                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);
                    doc.AppendChild(doc.CreateElement(string.Empty, startingTag, string.Empty));
                    doc.Save(Environment.CurrentDirectory + "\\" + filename);
                } catch (ArgumentException e)
                {
                    Console.WriteLine(filename + " not found: " + e.ToString());
                }
            }

        }

        public KeyInput getKeyInput(int keyInputId)
        {
            KeyInput keyInput = new KeyInput();
            if (keyInputId < 32 && keyInputId > 0)
            {
                foreach (XElement xe in keyCommandDoc.Descendants("Item"))
                {
                    if (Int32.Parse(xe.Element("ID").Value) == keyInputId)
                    {
                        keyInput.id = keyInputId;
                        keyInput.windowsCommand = xe.Element("Windows_command").Value;
                        keyInput.description = xe.Element("Description").Value;
                        keyInput.descriptionLong = xe.Element("Description_long").Value;
                        return keyInput;
                    }
                }
            }
            keyInput.id = -1;
            return keyInput;
        }

        //Based on the ID this method send a key command for the windows
        public List<KeyInput> getAllKeyInput()
        {
            List<KeyInput> keyList = new List<KeyInput>();
            //searching command 
            foreach (XElement xe in keyCommandDoc.Descendants("Item"))
            {
                KeyInput cKeyInput = new KeyInput();
                cKeyInput.id = Int32.Parse(xe.Element("ID").Value);
                cKeyInput.windowsCommand = xe.Element("Windows_command").Value;
                cKeyInput.description = xe.Element("Description").Value;
                cKeyInput.descriptionLong = xe.Element("Description_long").Value;
                keyList.Add(cKeyInput);
            }
            return keyList;
        }
    }
}
