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
            Assert.IsTrue(mh.updateMouseSensibility(5), "Mouse sensibility must be betweeen 1-10!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void updateMouseSensibility_withInvalidSensibility_throwsArgumentException()
        {
            MouseMovementHandler mh = new MouseMovementHandler();
            var obj = mh.updateMouseSensibility(11);
        }

    }
}
