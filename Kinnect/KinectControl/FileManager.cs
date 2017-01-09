using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TaskBarIcon;

namespace KinectControl
{
    class FileManager
    {

        private XDocument keyCommandDoc = null;
        private XmlDocument kinnectXMLCommands;
        private String xmlKeyCommandFileName = "KeyCommands.xml"; //this xml document contains all data about saved movements.
        private String xmlKinectMovementFileName = "KinectCommands.xml"; //this xml document contains windows key combination commands.
        private int id;


        //Constructor
        public FileManager()
        {


            try
            {
                keyCommandDoc = XDocument.Load(xmlKeyCommandFileName);
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine(xmlKeyCommandFileName + " not found");
            }
            fileExist("/" + xmlKinectMovementFileName, "movements");
            kinnectXMLCommands = new XmlDocument();
            kinnectXMLCommands.Load("/" + xmlKinectMovementFileName);


        }


        //Write a command into the xml file
        public void writeCommand(Commands.Command command)
        {
            XmlElement movement = kinnectXMLCommands.CreateElement("command");
            XmlElement id = kinnectXMLCommands.CreateElement("id");
            id.InnerText = command.keyID.ToString();
            XmlElement totalTime = kinnectXMLCommands.CreateElement("total_time");
            totalTime.InnerText = command.totalTime.ToString();
            XmlElement moments = kinnectXMLCommands.CreateElement("moments");
            foreach (Commands.MomentInTime moment in command.points)
            {
                XmlElement cmoment = kinnectXMLCommands.CreateElement("moment");
                XmlElement time = kinnectXMLCommands.CreateElement("time");
                time.InnerText = moment.time.ToString();

                XmlElement points = kinnectXMLCommands.CreateElement("points");
                foreach (KeyValuePair<int, CameraSpacePoint> pair in moment.hand)
                {
                    XmlElement point = kinnectXMLCommands.CreateElement("point");
                    XmlElement key = kinnectXMLCommands.CreateElement("key");
                    XmlElement x = kinnectXMLCommands.CreateElement("x");
                    XmlElement y = kinnectXMLCommands.CreateElement("y");
                    XmlElement z = kinnectXMLCommands.CreateElement("z");
                    key.InnerText = pair.Key.ToString();
                    x.InnerText = pair.Value.X.ToString();
                    y.InnerText = pair.Value.Y.ToString();
                    z.InnerText = pair.Value.Z.ToString();
                    point.AppendChild(key);
                    point.AppendChild(x);
                    point.AppendChild(y);
                    point.AppendChild(z);
                    points.AppendChild(point);
                }
                cmoment.AppendChild(time);
                cmoment.AppendChild(points);
                moments.AppendChild(cmoment);
            }
            movement.AppendChild(id);
            movement.AppendChild(totalTime);
            movement.AppendChild(moments);
            kinnectXMLCommands.DocumentElement.AppendChild(movement);
            kinnectXMLCommands.Save("/" + xmlKinectMovementFileName);
        }

        //Read a command from the xml file
        public Commands.Command readCommand(int keyInputID)
        {
            Commands.Command command = new Commands.Command();
            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {
                    if (Int32.Parse(movement.SelectSingleNode("id").InnerText) == keyInputID)
                    {

                        command.keyID = keyInputID;
                        command.totalTime = float.Parse(movement.SelectSingleNode("total_time").InnerText);
                        List<Commands.MomentInTime> moments = new List<Commands.MomentInTime>();
                        foreach (XmlNode moment in movement.SelectSingleNode("moments").SelectNodes("moment"))
                        {
                            Commands.MomentInTime cmoment = new Commands.MomentInTime();
                            cmoment.time = float.Parse(moment.SelectSingleNode("time").InnerText);
                            Dictionary<int, CameraSpacePoint> cpoints = new Dictionary<int, CameraSpacePoint>();

                            foreach (XmlNode point in moment.SelectSingleNode("points").SelectNodes("point"))
                            {
                                CameraSpacePoint cpoint = new CameraSpacePoint();
                                cpoint.X = Int32.Parse(point.SelectSingleNode("x").InnerText);
                                cpoint.Y = Int32.Parse(point.SelectSingleNode("y").InnerText);
                                cpoint.Z = Int32.Parse(point.SelectSingleNode("z").InnerText);
                                cpoints.Add(Int32.Parse(point.SelectSingleNode("key").InnerText), cpoint);
                            }
                            cmoment.hand = cpoints;

                        }
                        command.points = moments;
                        return command;
                    }
                }
            }
            command.keyID = -1;
            return command;
        }


        //Update command
        public bool updateCommand(Commands.Command command)
        {

            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {
                    if (Int32.Parse(movement.SelectSingleNode("id").InnerText) == command.keyID)
                    {

                        movement.SelectSingleNode("total_time").InnerText = command.totalTime.ToString();

                        movement.SelectSingleNode("moments").RemoveAll();
                        foreach (Commands.MomentInTime moment in command.points)
                        {
                            XmlElement cmoment = kinnectXMLCommands.CreateElement("moment");
                            XmlElement time = kinnectXMLCommands.CreateElement("time");
                            time.InnerText = moment.time.ToString();

                            XmlElement points = kinnectXMLCommands.CreateElement("points");
                            foreach (KeyValuePair<int, CameraSpacePoint> pair in moment.hand)
                            {
                                XmlElement point = kinnectXMLCommands.CreateElement("point");
                                XmlElement key = kinnectXMLCommands.CreateElement("key");
                                XmlElement x = kinnectXMLCommands.CreateElement("x");
                                XmlElement y = kinnectXMLCommands.CreateElement("y");
                                XmlElement z = kinnectXMLCommands.CreateElement("z");
                                key.InnerText = pair.Key.ToString();
                                x.InnerText = pair.Value.X.ToString();
                                y.InnerText = pair.Value.Y.ToString();
                                z.InnerText = pair.Value.Z.ToString();
                                point.AppendChild(key);
                                point.AppendChild(x);
                                point.AppendChild(y);
                                point.AppendChild(z);
                                points.AppendChild(point);
                            }
                            cmoment.AppendChild(time);
                            cmoment.AppendChild(points);
                            movement.SelectSingleNode("moments").AppendChild(cmoment);
                        }
                        kinnectXMLCommands.Save("/" + xmlKinectMovementFileName);
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
                        kinnectXMLCommands.Save("/" + xmlKinectMovementFileName);
                        return true;
                    }
                }
            }
            return false;
        }

        //Read all commands from the xml file
        public List<Commands.Command> readAllCommands()
        {
            List<Commands.Command> commands = new List<Commands.Command>();
            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {

                    Commands.Command command = new Commands.Command();
                    command.keyID = Int32.Parse(movement.SelectSingleNode("id").InnerText);
                    command.totalTime = float.Parse(movement.SelectSingleNode("total_time").InnerText);
                    List<Commands.MomentInTime> moments = new List<Commands.MomentInTime>();
                    foreach (XmlNode moment in movement.SelectSingleNode("moments").SelectNodes("moment"))
                    {
                        Commands.MomentInTime cmoment = new Commands.MomentInTime();
                        cmoment.time = float.Parse(moment.SelectSingleNode("time").InnerText);
                        Dictionary<int, CameraSpacePoint> cpoints = new Dictionary<int, CameraSpacePoint>();

                        foreach (XmlNode point in moment.SelectSingleNode("points").SelectNodes("point"))
                        {
                            CameraSpacePoint cpoint = new CameraSpacePoint();
                            cpoint.X = Int32.Parse(point.SelectSingleNode("x").InnerText);
                            cpoint.Y = Int32.Parse(point.SelectSingleNode("y").InnerText);
                            cpoint.Z = Int32.Parse(point.SelectSingleNode("z").InnerText);
                            cpoints.Add(Int32.Parse(point.SelectSingleNode("key").InnerText), cpoint);
                        }
                        cmoment.hand = cpoints;

                    }
                    command.points = moments;
                    commands.Add(command);
                }
            }

            return commands;
        }

        //check the file (filename) existence
        //and if the file does not exist, create the file
        private void fileExist(String filename, String startingTag)
        {
            if (!File.Exists(filename))
            {
                File.Create(filename);
                XDocument doc = new XDocument(filename, new XElement(startingTag));
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
