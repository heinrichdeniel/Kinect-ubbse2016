using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KinectControl;

namespace KinectTests
{
    /// <summary>
    /// Summary description for FileManagerTests
    /// </summary>
    [TestClass]
    public class FileManagerTests
    {

        [TestMethod]
        public void ReadCommand_WithAnId_ReturnsTheMovement()
        {
            FileManager fm = FileManager.getInstance();
            Commands.Average comm = fm.readCommand(2);

            Assert.AreEqual(comm.keyID, 2, "The keyId returned should match what was passed in");
        }

        [TestMethod]
        public void ReadCommand_WithInvalidId_ReturnsMinusOne()
        {
            FileManager fm = FileManager.getInstance();
            Commands.Average comm = fm.readCommand(-5);

            Assert.AreEqual(comm.keyID, -1, "The keyId passed in was invalid");

        }


        [TestMethod]
        public void ReadKeyInput_WithAnId_ReturnsTheMovement()
        {
            FileManager fm = FileManager.getInstance();
            KeyInput key = fm.getKeyInput(1);

            Assert.AreEqual(key.id, 1, "The keyId returned should match what was passed in");
        }

        [TestMethod]
        public void ReadKeyInput_WithInvalidId_ReturnsMinusOne()
        {
            FileManager fm = FileManager.getInstance();
            KeyInput key = fm.getKeyInput(-5);

            Assert.AreEqual(key.id, -1, "The keyId passed in was invalid");

        }
    }
}
