using System;
using KinectControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KinectTests
{
	[TestClass]
	public class SplineMethodTesting
	{
		[TestMethod]
		public void SplineAlpha_WithGoodLearningData_ReturnsSquareNumber()
		{
			float[] x = { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f };
			float[] y = { 1f, 4f, 9f, 16f, 25f, 36f, 49f, 64f, 81f, 100f };
			int n = 10;
			Spline s = new Spline(n);
			s.set(y, x, n);
			s.calculateAlpha();
			float a = s.calculateRes(2.5f);
			Assert.AreEqual(true, Math.Abs(a - 6.25) < 0.2);
		}

        [TestMethod]
        public void SplineAlpha_WithWrongLearningData_ReturnsSquareNumber()
        {
            float[] x = { 1f, 2f, 3f, 4f, 5f, 6f, 7f };
            float[] y = { 1f, 2f, 3f, 4f, 5f, 6f, 7f };
            int n = 7;
            Spline s = new Spline(n);
            s.set(y, x, n);
            s.calculateAlpha();
            float a = s.calculateRes(2.5f);
            Assert.AreEqual(false, Math.Abs(a - 6.25) < 0.2);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplineAlpha_WhitoutLearningData_ThrowsError()
        {
            float[] x = { };
            float[] y = { };
            int n = 0;
            Spline s = new Spline(n);
            s.set(y, x, n);
            s.calculateAlpha();
            float a = s.calculateRes(2.5f);
            Assert.AreEqual(false, Math.Abs(a - 6.25) < 0.2);
        }
    }
}
