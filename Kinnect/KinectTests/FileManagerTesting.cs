using System;
using KinectControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KinectTests
{
	[TestClass]
	public class FileManagerTesting
	{
		[TestMethod]
		public void CommandSaving_WithNewCommand_CommandWillBeSaved()
		{
			Commands.Average avg = new Commands.Average();
			avg.keyID = 11;
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
	}
}
