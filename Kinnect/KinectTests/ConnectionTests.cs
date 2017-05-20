using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KinectControl;
using System.Windows.Forms;
using Microsoft.Kinect;

namespace KinectTests
{
    [TestClass]
    public class ConnectionTests
    {

        [TestMethod]
        public void ConnectionHandMoved_handMoved_returnTrue()
        {
            Connection connection = new Connection(new PictureBox(), new Button());
            CameraSpacePoint a = new CameraSpacePoint();
            a.X = 1.1f;
            a.Y = 1.2f;
            a.Z = 1.3f;

            CameraSpacePoint b = new CameraSpacePoint();
            b.X = 1.2f;
            b.Y = 1.2f;
            b.Z = 1.3f;

            bool expectedValue = true;

            Assert.AreEqual(expectedValue,connection.handMoved(a,b), "Hand did not move!");
        }

        [TestMethod]
        public void ConnectionHandMoved_handDidNotMove_returnTrue()
        {
            Connection connection = new Connection(new PictureBox(), new Button());
            CameraSpacePoint a = new CameraSpacePoint();
            a.X = 1.222f;
            a.Y = 1.2f;
            a.Z = 1.3f;

            CameraSpacePoint b = new CameraSpacePoint();
            b.X = 1.2221f;
            b.Y = 1.2f;
            b.Z = 1.3f;

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, connection.handMoved(a, b), "Hand did not move!");
        }

        [TestMethod]
        public void ConnectionStartStop_Starts_returnTrue()
        {
            Connection connection = new Connection(new PictureBox(), new Button());
            CameraSpacePoint a = new CameraSpacePoint();

            bool expectedValue = true;

            Assert.AreEqual(expectedValue, connection.startStop(true), "Did not start!");
        }

        [TestMethod]
        public void ConnectionStartStop_Stops_returnTrue()
        {
            Connection connection = new Connection(new PictureBox(), new Button());
            CameraSpacePoint a = new CameraSpacePoint();

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, connection.startStop(false), "Did not stop!");
        }
    }
}
