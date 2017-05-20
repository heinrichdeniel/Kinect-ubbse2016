﻿using System;
using KinectControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KinectTests
{
	[TestClass]
	public class FileManagerTesting
	{
        [TestMethod]
        public void RealCommandDeleting_ExistingCommand_CommandWillBeDeleted()
        {
            FileManager fm = FileManager.getInstance();
            Assert.AreEqual(true, fm.removeCommand(1));
        }

        [TestMethod]
        public void FileExist_ExistingFile_FileIsExist()
        {
            FileManager fm = FileManager.getInstance();
            Assert.AreEqual(true, fm.fileExist("KinectCommands.xml", "movements"));
        }

        [TestMethod]
        public void FileNotExist_FictivFile_FileIsExist()
        {
            FileManager fm = FileManager.getInstance();
            fm.fileExist("InvalidFile.xml", "invalid");
            Assert.AreEqual(true, fm.fileExist("InvalidFile.xml", "invalid"));
        }

        [TestMethod]
        public void InvalidCommandDeleting_ExistingCommand_CommandWillBeDeleted()
        {
            FileManager fm = FileManager.getInstance();
            Assert.AreEqual(false, fm.removeCommand(50));
        }

        [TestMethod]
		public void CommandSaving_WithNewCommand_CommandWillBeSaved()
		{
			Commands.Average avg = new Commands.Average();
			avg.keyID = 20;
			avg.time = 3.2f;
			Random rand = new Random();
			avg.pointcount = rand.Next(30, 60);
			avg.avg[0] = new float[avg.pointcount];
			avg.avg[1] = new float[avg.pointcount];
			avg.avg[2] = new float[avg.pointcount];
			avg.timePointCount = new float[avg.pointcount];
			for (int i = 0; i < avg.time; ++i)
			{ 
				avg.avg[0][i] = (float)rand.NextDouble();
				avg.avg[1][i] = (float)rand.NextDouble();
				avg.avg[2][i] = (float)rand.NextDouble();
				avg.timePointCount[i] = (i > 0 ? avg.timePointCount[i - 1] : 0) + (float)rand.NextDouble() / 3;
			}
			FileManager fm = FileManager.getInstance();
			fm.writeCommand(avg);
			Commands.Average avg2 = fm.readCommand(avg.keyID);
			Assert.AreEqual(true, avg2.keyID == avg.keyID && avg.pointcount == avg2.pointcount);
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandSaving_WithExistingKeyId_throwsException()
        {
            Commands.Average avg = new Commands.Average();
            avg.keyID = 20;
            avg.time = 3.2f;
            Random rand = new Random();
            avg.pointcount = rand.Next(30, 60);
            avg.avg[0] = new float[avg.pointcount];
            avg.avg[1] = new float[avg.pointcount];
            avg.avg[2] = new float[avg.pointcount];
            avg.timePointCount = new float[avg.pointcount];
            for (int i = 0; i < avg.time; ++i)
            {
                avg.avg[0][i] = (float)rand.NextDouble();
                avg.avg[1][i] = (float)rand.NextDouble();
                avg.avg[2][i] = (float)rand.NextDouble();
                avg.timePointCount[i] = (i > 0 ? avg.timePointCount[i - 1] : 0) + (float)rand.NextDouble() / 3;
            }
            FileManager fm = FileManager.getInstance();
            fm.writeCommand(avg);
            Commands.Average avg2 = fm.readCommand(avg.keyID);
            Assert.AreEqual(true, avg2.keyID == avg.keyID && avg.pointcount == avg2.pointcount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandSaving_WithoutAverage_throwsException()
        {
            Commands.Average avg = new Commands.Average();
            avg.time = 3.2f;
            Random rand = new Random();
            avg.pointcount = rand.Next(30, 60);
            avg.avg[0] = new float[avg.pointcount];
            avg.avg[1] = new float[avg.pointcount];
            avg.avg[2] = new float[avg.pointcount];
            avg.timePointCount = new float[avg.pointcount];
            for (int i = 0; i < avg.time; ++i)
            {
                avg.avg[0][i] = (float)rand.NextDouble();
                avg.avg[1][i] = (float)rand.NextDouble();
                avg.avg[2][i] = (float)rand.NextDouble();
                avg.timePointCount[i] = (i > 0 ? avg.timePointCount[i - 1] : 0) + (float)rand.NextDouble() / 3;
            }
            FileManager fm = FileManager.getInstance();
            avg = null;
            fm.writeCommand(avg);
            Commands.Average avg2 = fm.readCommand(avg.keyID);
            Assert.AreEqual(true, avg2.keyID == avg.keyID && avg.pointcount == avg2.pointcount);
        }
    }
}
