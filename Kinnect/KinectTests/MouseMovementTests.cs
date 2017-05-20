using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KinectControl;

namespace KinectTests
{
    [TestClass]
    public class MouseMovementTests
    {
        [TestMethod]
        public void updateMouseSensibility_withValidSensibility_returnsTrue()
        {
            MouseMovementHandler mh = new MouseMovementHandler();
            Assert.IsTrue(mh.updateMouseSensibility(5), "Mouse sensibility must be betweeen 0-10!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void updateMouseSensibility_withInvalidSensibility_throwsArgumentException()
        {
            MouseMovementHandler mh = new MouseMovementHandler();
            var obj = mh.updateMouseSensibility(11);
        }

        [TestMethod]
        public void updateMouseVisibility_mouseIsVisible_returnTrue()
        {
            MouseMovementHandler mh = new MouseMovementHandler();
            bool expectedValue = true;
            Assert.AreEqual(expectedValue, mh.updateMouseVisibility(true), "Not true");
        }

        [TestMethod]
        public void updateMouseVisibility_mouseIsNotVisible_returnTrue()
        {
            MouseMovementHandler mh = new MouseMovementHandler();
            bool expectedValue = false;
            Assert.AreEqual(expectedValue, mh.updateMouseVisibility(false), "Not false");
        }

        [TestMethod]
        public void updatecursorSmoothing_withValidSmoothing_returnsTrue()
        {
            MouseMovementHandler mh = new MouseMovementHandler();
            Assert.IsTrue(mh.updatecursorSmoothing(0.1f), "Cursor smoothing must be betweeen 0-1!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void updatecursorSmoothing_withInvalidSmoothing_throwsArgumentException()
        {
            MouseMovementHandler mh = new MouseMovementHandler();
            var obj = mh.updatecursorSmoothing(11);
        }
    }
}
