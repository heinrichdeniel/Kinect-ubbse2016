using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// 
        /// 
        /// 
        /// EZEKEt TOROLNI!!!
        /// 
        /// 
        private String xmlKinectDalmaCommandFileName = "KinectCommand.xml";
        private XmlDocument kinnectXMLCommand;
        ///
        /// 
        /// 
        /// 
        /// EDDIG!!!!
        /// 
        /// 


        private static FileManager INSTANCE;

        public static FileManager getInstance()
        {
            if (INSTANCE == null)
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

            /// 
            /// 
            /// TORLNI
            /// 

            kinnectXMLCommand = new XmlDocument();
            try
            {
                kinnectXMLCommand.Load(xmlKinectDalmaCommandFileName);
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine(xmlKinectDalmaCommandFileName + " not found");
            }
            /// 
            /// EDDIG
            /// 
            /// 
            fileExist(xmlKinectMovementFileName, "movements");
            kinnectXMLCommands = new XmlDocument();
            try
            {
                kinnectXMLCommands.Load(xmlKinectMovementFileName);
            }
            catch (FileNotFoundException e)
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
            XmlElement time = kinnectXMLCommands.CreateElement("time");
            time.InnerText = average.time.ToString();
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
            foreach (float[] ctime in average.avg)
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
            movement.AppendChild(time);
            movement.AppendChild(timePoints);
            movement.AppendChild(avg);

            kinnectXMLCommands.DocumentElement.AppendChild(movement);
            kinnectXMLCommands.Save(xmlKinectMovementFileName);
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
                        command.time = float.Parse(movement.SelectSingleNode("time").InnerText.Replace(",", "."));
                        float[] timePoints = new float[60];
                        int i = 0;
                        foreach (XmlNode moment in movement.SelectSingleNode("time_points_count").SelectNodes("time_point"))
                        {
                            timePoints[i] = float.Parse(moment.InnerText.Replace(",", "."));
                            ++i;
                        }
                        command.timePointCount = timePoints;

                        float[][] avg = new float[12][];
                        i = 0;
                        int j = 0;
                        foreach (XmlNode moment in movement.SelectSingleNode("avg").SelectNodes("avg_point"))
                        {
                            if (j >= command.pointcount)
                            {
                                j = 0;
                                ++i;

                            }
                            if (j == 0)
                            {
                                avg[i] = new float[command.pointcount];
                            }
                            avg[i][j] = float.Parse(moment.InnerText.Replace(",", "."));
                            ++j;
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
                        foreach (float[] moment in command.avg)
                        {
                            foreach (float cmoment in moment)
                            {
                                XmlElement timePoint = kinnectXMLCommands.CreateElement("avg_point");
                                timePoint.InnerText = cmoment.ToString();
                                movement.SelectSingleNode("time_point_count").AppendChild(timePoint);
                            }
                        }
                        kinnectXMLCommands.DocumentElement.AppendChild(movement);
                        kinnectXMLCommands.Save(xmlKinectMovementFileName);
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
                        kinnectXMLCommands.Save(xmlKinectMovementFileName);
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
                    command.time = float.Parse(movement.SelectSingleNode("time").InnerText.Replace(",", "."));
                    command.pointcount = Int32.Parse(movement.SelectSingleNode("point_count").InnerText);
                    float[] timePoints = new float[command.pointcount];
                    int i = 0;
                    foreach (XmlNode moment in movement.SelectSingleNode("time_points_count").SelectNodes("time_point"))
                    {
                        timePoints[i] = float.Parse(moment.InnerText.Replace(",", "."));
                        ++i;
                    }
                    command.timePointCount = timePoints;

                    float[][] avg = new float[12][];
                    i = 0;
                    int j = 0;
                    foreach (XmlNode moment in movement.SelectSingleNode("avg").SelectNodes("avg_point"))
                    {

                        if (j >= command.pointcount)
                        {
                            j = 0;
                            ++i;

                        }
                        if (j == 0)
                        {
                            avg[i] = new float[command.pointcount];
                        }
                        avg[i][j] = float.Parse(moment.InnerText.Replace(",", "."));
                        ++j;
                    }
                    command.avg = avg;
                    commands.Add(command);
                }
            }

            return commands;
        }
        public bool saveCommand(Commands.Average avarage)
        {

            XmlNodeList movements = kinnectXMLCommands.GetElementsByTagName("command");
            if (movements.Count > 0)
            {
                foreach (XmlNode movement in movements)
                {
                    if (Int32.Parse(movement.SelectSingleNode("id").InnerText) == avarage.keyID)
                    {
                        updateCommand(avarage);
                        return true;
                    }
                }
                writeCommand(avarage);
                return false;
            }
            return false;

        }
        //check the file (filename) existence
        //and if the file does not exist, create the file
        private void fileExist(String filename, String startingTag)
        {
            if (!File.Exists(Environment.CurrentDirectory + "\\" + filename))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);
                    doc.AppendChild(doc.CreateElement(string.Empty, startingTag, string.Empty));
                    doc.Save(Environment.CurrentDirectory + "\\" + filename);
                }
                catch (ArgumentException e)
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

        ///
        ///
        ///
        ///
        ///////////////////////////////////////////////////
        ///                                            ///
        ///          DALMANAK TESZTELNI               ///
        ///   !!!!!!!A VEGSO KODBOL TOROLNI!!!!!!!   ///
        ///                                         ///
        //////////////////////////////////////////////
        /// 
        /// 
        /// 

        //Write a command into the xml file
        public void writeCommand(Commands command)
        {
            XmlElement commands = kinnectXMLCommand.CreateElement("commands");
            foreach (Commands.Command cmd in command.commands)
            {
                XmlElement xmlcommand = kinnectXMLCommand.CreateElement("command");
                XmlElement totalTime = kinnectXMLCommand.CreateElement("total_time");
                totalTime.InnerText = cmd.totalTime.ToString();
                XmlElement moments = kinnectXMLCommand.CreateElement("moments");
                foreach (Commands.MomentInTime moment in cmd.points)
                {
                    XmlElement cmoment = kinnectXMLCommand.CreateElement("moment");
                    XmlElement time = kinnectXMLCommand.CreateElement("time");
                    time.InnerText = moment.time.ToString();

                    XmlElement points = kinnectXMLCommand.CreateElement("points");
                    foreach (CameraSpacePoint pair in moment.hand)
                    {
                        XmlElement point = kinnectXMLCommand.CreateElement("point");
                        XmlElement x = kinnectXMLCommand.CreateElement("x");
                        XmlElement y = kinnectXMLCommand.CreateElement("y");
                        XmlElement z = kinnectXMLCommand.CreateElement("z");
                        x.InnerText = pair.X.ToString();
                        y.InnerText = pair.Y.ToString();
                        z.InnerText = pair.Z.ToString();
                        point.AppendChild(x);
                        point.AppendChild(y);
                        point.AppendChild(z);
                        points.AppendChild(point);
                    }
                    cmoment.AppendChild(time);
                    cmoment.AppendChild(points);
                    moments.AppendChild(cmoment);
                }
                xmlcommand.AppendChild(totalTime);
                xmlcommand.AppendChild(moments);
                commands.AppendChild(xmlcommand);
            }

            kinnectXMLCommand.DocumentElement.AppendChild(commands);
            kinnectXMLCommand.Save(xmlKinectDalmaCommandFileName);
        }

        //Read a command from the xml file
        public Commands readCommand()
        {

            Commands command = new Commands();
            XmlNodeList xmlcommands = kinnectXMLCommand.GetElementsByTagName("commands");
            if (xmlcommands.Count > 0)
            {

                foreach (XmlNode xmlcommand in xmlcommands)
                {

                    Commands.Command[] cmd = new Commands.Command[3];
                    int i = 0;
                    foreach (XmlNode xmlcmd in xmlcommand.SelectNodes("command"))
                    {
                        Commands.Command ccmd = new Commands.Command();
                        ccmd.totalTime = float.Parse(xmlcmd.SelectSingleNode("total_time").InnerText.Replace(",", "."));
                        List<Commands.MomentInTime> cmoments = new List<Commands.MomentInTime>();

                        foreach (XmlNode xmlmoments in xmlcmd.SelectSingleNode("moments").SelectNodes("moment"))
                        {
                            Commands.MomentInTime cmoment = new Commands.MomentInTime();
                            cmoment.time = float.Parse(xmlmoments.SelectSingleNode("time").InnerText.Replace(",", "."));
                            CameraSpacePoint[] chand = new CameraSpacePoint[4];
                            int k = 0;
                            foreach (XmlNode xmlpoint in xmlmoments.SelectSingleNode("points").SelectNodes("point"))
                            {
                                CameraSpacePoint chandpoint = new CameraSpacePoint();
                                Log.log.Info(xmlpoint.SelectSingleNode("x").InnerText);
                                chandpoint.X = float.Parse(xmlpoint.SelectSingleNode("x").InnerText.Replace(",", "."));
                                chandpoint.Z = float.Parse(xmlpoint.SelectSingleNode("z").InnerText.Replace(",", "."));
                                chandpoint.Y = float.Parse(xmlpoint.SelectSingleNode("y").InnerText.Replace(",", "."));
                                chand[k] = chandpoint;
                                ++k;
                            }
                            cmoment.hand = chand;
                            cmoments.Add(cmoment);
                        }


                        ccmd.points = cmoments;
                        cmd[i] = ccmd;
                        ++i;
                    }
                    command.commands = cmd;

                }
                return command;
            }

            return command;
        }
    }
}
