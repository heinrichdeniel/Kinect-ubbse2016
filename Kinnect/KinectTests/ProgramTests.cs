using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KinectControl;

namespace KinectTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void KinectConnection_isConnected_returnTrue()
        {
            Program program = new Program();
            bool expectedValue = true;

            Assert.AreEqual(expectedValue, program.isConnected() , "Kinect is not connected!");


        }
    }
}
