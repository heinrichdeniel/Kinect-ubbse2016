using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KinectControl;

namespace KinectTests
{
    [TestClass]
    public class SplineTests
    {

        [TestMethod]
        public void SplineKernel_correctValue_returnTrue()
        {
            Spline spline = new Spline(1);
            float a = 3.5f;
            float b = 2.5f;

            float expectedValue = 18.0833333333f;

            Assert.AreEqual(expectedValue, spline.kernelK2(a,b), "Value is not correct!");
        }

        [TestMethod]
        public void SplineKernel_incorrectValue_returnTrue()
        {
            Spline spline = new Spline(1);
            float a = 3.5f;
            float b = 2.5f;

            float expectedValue = 18.09f;

            Assert.AreNotEqual(expectedValue, spline.kernelK2(a, b), "Value is correct!");
        }

        [TestMethod]
        public void SplineMultiply_correctValue_returnTrue()
        {
            Spline spline = new Spline(2);
            float[][] a = new float[2][];
            a[0] = new float[2];
            a[1] = new float[2];
            a[0][0] = 1f;
            a[0][1] = 3f;
            a[1][0] = 2f;
            a[1][1] = 2f;

            float[] b = new float[2];
            b[0] = 2f;
            b[1] = 3f; 

            float[] expectedValue = new float[2];
            expectedValue[0] = 11f;
            expectedValue[1] = 10f;
          
            CollectionAssert.AreEqual(expectedValue, spline.multiply(a, b), "Value is not correct!");
        }


        [TestMethod]
        public void SplineMultiply_incorrectValue_returnTrue()
        {
            Spline spline = new Spline(2);
            float[][] a = new float[2][];
            a[0] = new float[2];
            a[1] = new float[2];
            a[0][0] = 1f;
            a[0][1] = 3f;
            a[1][0] = 2f;
            a[1][1] = 2f;

            float[] b = new float[2];
            b[0] = 2f;
            b[1] = 3f;

            float[] expectedValue = new float[2];
            expectedValue[0] = 1f;
            expectedValue[1] = 1f;

            CollectionAssert.AreNotEqual(expectedValue, spline.multiply(a, b), "Value is correct!");
        }
    }
}
