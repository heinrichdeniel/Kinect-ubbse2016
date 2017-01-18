using MatrixInverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectControl
{
    class Spline
    {
        private float[] x;
        private float[] time;
        private float[] alpha;
        private float[][] kernel;
        private int n;

        public Spline(int n)
        {
            alpha = new float[n];
        }
        public Spline(float[] x1, float[] time1, int n1)
        {
            x = x1;
            time = time1;
            n = n1;
            alpha = new float[n];
        }

        public void set(float[] x1, float[] time1)
        {
            x = x1;
            time = time1;
        }

        public void set(float[] x1, float[] time1, int n1)
        {
            x = x1;
            time = time1;
            n = n1;
        }

        public float kernelK2(float i, float j)
        {
            return (float)(1f + i * j + (float)Math.Pow((float)Math.Min(i, j), 3f) / 3f + ((float)Math.Pow((float)Math.Min(i, j), 2f) * (float)Math.Abs(i - j)) / 2f);
        }

        public void calculateKernel()
        {
            kernel = new float[n][];
            for (int i = 0; i < n; i++)
            {
                kernel[i] = new float[n];
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    kernel[i][j] = kernelK2(time[i], time[j]);
                    //Log.log.Info(kernel[i][j]);
                }
            }
        }

        public float[] multiply(float[][] k1, float[] x1)
        {
            float[] res = new float[n];
            for (int i=0; i < n; i++)
            {
                res[i] = 0;
                for (int j=0; j < n; j++)
                {
                    res[i] += k1[i][j] * x1[j];
                }
            }
            return res;
        }

        public float[] calculateAlpha()
        {
            calculateKernel();
            float[][] kernelInv;
            kernelInv = new float[n][];
            for (int i = 0; i < n; i++)
            {
                kernelInv[i] = new float[n];
            }

            kernelInv = MatrixInverseProgram.MatrixInverse(kernel);

            alpha = multiply(kernelInv, x);
            return alpha;
        }

        public float calculateRes(float t)
        {
            float res = 0;
            for (int i=0; i < n; i++)
            {
                       // Log.log.Info(kernelK2(t, time[i]) + "alpha:" + alpha[i]);
                res += alpha[i] * kernelK2(t, time[i]);
            }
            return res;
        }

    }
}
