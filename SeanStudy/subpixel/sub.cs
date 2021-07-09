using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace subpixel
{
    public class sub
    {
        public class Contour
        {
            public PointF[] points;
            public float[] direction;
            public float[] response;
        }

        public class Parameter
        {
            public double Sigma;

            public double Threshold_H;

            public double Threshold_L;

            public Parameter()
            {
                Sigma = 0.0;
                Threshold_H = 300;
                Threshold_L = 100;
            }

            public Parameter(double sigma, double high, double low)
            {
                Sigma = sigma;
                Threshold_H = high;
                Threshold_L = low;
            }
        }

        public class Result
        {
            public VectorOfVectorOfPoint FoundContours;

            public List<Contour> SubContours;

            public Result()
            {
                FoundContours = new VectorOfVectorOfPoint();
                SubContours = new List<Contour>();
            }
        }

        private static bool getFilterKernel(double alpha, out Image<Gray, int> kx, out Image<Gray, int> ky)
        {
            int r = (int)Math.Round(alpha * 3);
            int ksize = 2 * r + 1;
            kx = new Image<Gray, int>(1, ksize);
            ky = new Image<Gray, int>(ksize, 1);
            float[] kerF = new float[ksize];

            kerF[r] = 0.0f;
            float sum = 0.0f;
            for (int x = 1; x <= r; x++)
            {
                float value = (float)(-x * Math.Exp(-x * x / (2 * alpha * alpha)));
                sum += value;
                kerF[r + x] = value;
                kerF[r - x] = value * -1;
            }

            float scale = 128 / sum;
            for (int i = 0; i < ksize; i++)
            {
                kerF[i] *= scale;
                kx.Data[i, 0, 0] = (int)kerF[i];
                ky.Data[0, i, 0] = (int)kerF[i];
            }
            return true;
        }

        private static double getAmplitude(Image<Gray, short> dx, Image<Gray, short> dy, int i, int j)
        {
            double dataX = dx.Data[i, j, 0];
            double dataY = dy.Data[i, j, 0];
            return Math.Sqrt(dataX * dataX + dataY * dataY);
        }

        private static double[] getMagNeighbourhood(Image<Gray, short> dx, Image<Gray, short> dy, Point point, int w, int h)
        {
            double[] magNeighbour = new double[9];

            int top = point.Y - 1 >= 0 ? point.Y - 1 : point.Y;
            int down = point.Y + 1 < h ? point.Y + 1 : point.Y;
            int left = point.X - 1 >= 0 ? point.X - 1 : point.X;
            int right = point.X + 1 < w ? point.X + 1 : point.X;

            magNeighbour[0] = getAmplitude(dx, dy, top, left);
            magNeighbour[1] = getAmplitude(dx, dy, top, point.X);
            magNeighbour[2] = getAmplitude(dx, dy, top, right);
            magNeighbour[3] = getAmplitude(dx, dy, point.Y, left);
            magNeighbour[4] = getAmplitude(dx, dy, point.Y, point.X);
            magNeighbour[5] = getAmplitude(dx, dy, point.Y, right);
            magNeighbour[6] = getAmplitude(dx, dy, down, left);
            magNeighbour[7] = getAmplitude(dx, dy, down, point.X);
            magNeighbour[8] = getAmplitude(dx, dy, down, right);

            return magNeighbour;
        }

        private static double[] get2ndFacetModelIn3x3(double[] mag)
        {
            double[] a = new double[9];
            a[0] = (-mag[0] + 2.0 * mag[1] - mag[2] + 2.0 * mag[3] + 5.0 * mag[4] + 2.0 * mag[5] - mag[6] + 2.0 * mag[7] - mag[8]) / 9.0;
            a[1] = (-mag[0] + mag[2] - mag[3] + mag[5] - mag[6] + mag[8]) / 6.0;
            a[2] = (mag[6] + mag[7] + mag[8] - mag[0] - mag[1] - mag[2]) / 6.0;
            a[3] = (mag[0] - 2.0 * mag[1] + mag[2] + mag[3] - 2.0 * mag[4] + mag[5] + mag[6] - 2.0 * mag[7] + mag[8]) / 6.0;
            a[4] = (-mag[0] + mag[2] + mag[6] - mag[8]) / 4.0;
            a[5] = (mag[0] + mag[1] + mag[2] - 2.0 * (mag[3] + mag[4] + mag[5]) + mag[6] + mag[7] + mag[8]) / 6.0;
            return a;
        }

        // Compute the eigenvalues and eigenvectors of the Hessian matrix given by
        // dfdrr, dfdrc, and dfdcc, and sort them in descending order according to
        // their absolute values.
        private static bool eigenvals(double[] a, out double[] eigval, out double[][] eigvec)
        {
            eigval = new double[2];
            eigvec = new double[2][];
            eigvec[0] = new double[2];
            eigvec[1] = new double[2];


            // derivatives
            // fx = a[1], fy = a[2]
            // fxy = a[4]
            // fxx = 2 * a[3]
            // fyy = 2 * a[5]
            double dfdrc = a[4];
            double dfdcc = a[3] * 2.0;
            double dfdrr = a[5] * 2.0;
            double theta, t, c, s, e1, e2, n1, n2; /* , phi; */

            /* Compute the eigenvalues and eigenvectors of the Hessian matrix. */
            if (dfdrc != 0.0)
            {
                theta = 0.5 * (dfdcc - dfdrr) / dfdrc;
                t = 1.0 / (Math.Abs(theta) + Math.Sqrt(theta * theta + 1.0));
                if (theta < 0.0) t = -t;
                c = 1.0 / Math.Sqrt(t * t + 1.0);
                s = t * c;
                e1 = dfdrr - t * dfdrc;
                e2 = dfdcc + t * dfdrc;
            }
            else
            {
                c = 1.0;
                s = 0.0;
                e1 = dfdrr;
                e2 = dfdcc;
            }
            n1 = c;
            n2 = -s;

            // If the absolute value of an eigenvalue is larger than the other, put that
            // eigenvalue into first position.If both are of equal absolute value, put
            // the negative one first.

            if (Math.Abs(e1) > Math.Abs(e2))
            {
                eigval[0] = e1;
                eigval[1] = e2;
                eigvec[0][0] = n1;
                eigvec[0][1] = n2;
                eigvec[1][0] = -n2;
                eigvec[1][1] = n1;
            }
            else if (Math.Abs(e1) < Math.Abs(e2))
            {
                eigval[0] = e2;
                eigval[1] = e1;
                eigvec[0][0] = -n2;
                eigvec[0][1] = n1;
                eigvec[1][0] = n1;
                eigvec[1][1] = n2;
            }
            else
            {
                if (e1 < e2)
                {
                    eigval[0] = e1;
                    eigval[1] = e2;
                    eigvec[0][0] = n1;
                    eigvec[0][1] = n2;
                    eigvec[1][0] = -n2;
                    eigvec[1][1] = n1;
                }
                else
                {
                    eigval[0] = e2;
                    eigval[1] = e1;
                    eigvec[0][0] = -n2;
                    eigvec[0][1] = n1;
                    eigvec[1][0] = n1;
                    eigvec[1][1] = n2;
                }
            }

            return true;
        }

        private static double vector2angle(double x, double y)
        {
            double a = Math.Atan2(y, x);
            return a >= 0.0 ? a : a + Math.PI;
        }

        private static bool extractSubPixPoints(Image<Gray, short> dx, Image<Gray, short> dy, VectorOfVectorOfPoint contours, out List<Contour> result)
        {
            int w = dx.Cols;
            int h = dx.Rows;
            result = new List<Contour>(contours.Size);

            for (int i = 0; i < contours.Size; i++)
            {
                Contour tmp = new Contour();
                result.Add(tmp);

                // each contour
                VectorOfPoint contour = contours[i];
                result[i].points = new PointF[contour.Size];
                result[i].direction = new float[contour.Size];
                result[i].response = new float[contour.Size];

                for (int j = 0; j < contour.Size; j++)
                {
                    // each contour point
                    double[] magNeighbour = getMagNeighbourhood(dx, dy, contour[j], w, h);
                    double[] a = get2ndFacetModelIn3x3(magNeighbour);

                    // Hessian eigen vector 
                    double[] eigval = new double[2];
                    double[][] eigvec = new double[2][];
                    eigvec[0] = new double[2];
                    eigvec[1] = new double[2];

                    eigenvals(a, out eigval, out eigvec);
                    double t = 0.0;
                    double ny = eigvec[0][0];
                    double nx = eigvec[0][1];
                    if (eigval[0] < 0.0)
                    {
                        double rx = a[1], ry = a[2], rxy = a[4], rxx = a[3] * 2.0, ryy = a[5] * 2.0;
                        t = -(rx * nx + ry * ny) / (rxx * nx * nx + 2.0 * rxy * nx * ny + ryy * ny * ny);
                    }
                    double px = nx * t;
                    double py = ny * t;
                    float x = contour[j].X;
                    float y = contour[j].Y;
                    if (Math.Abs(px) <= 0.5 && Math.Abs(py) <= 0.5)
                    {
                        x += (float)px;
                        y += (float)py;
                    }

                    result[i].points[j] = new PointF(x, y);
                    result[i].response[j] = (float)(a[0] / 128);
                    result[i].direction[j] = (float)vector2angle(ny, nx);
                }
            }
            return true;
        }

        public static bool SubPixel(Image<Gray, byte> image, Parameter parameter, out Result result)
        {
            result = new Result();

            // preprocessing
            Size kernel = new Size(3, 3);
            Image<Gray, byte> blur = image.CopyBlank();
            CvInvoke.GaussianBlur(image, blur, kernel, parameter.Sigma);
            Image<Gray, byte> cannyImg = blur.Canny(300, 100);
            CvInvoke.Imwrite("CannyImg.png", cannyImg);

            // kernel
            Image<Gray, int> kx, ky;
            Image<Gray, int> one = new Image<Gray, int>(1, 1);
            getFilterKernel(parameter.Sigma, out kx, out ky);
            one.Data[0, 0, 0] = 1;

            // gradient
            Image<Gray, short> dx = new Image<Gray, short>(image.Width, image.Height);
            Image<Gray, short> dy = new Image<Gray, short>(image.Width, image.Height);
            CvInvoke.SepFilter2D(image, dx, DepthType.Cv16S, kx, one, new Point(-1, -1));
            CvInvoke.SepFilter2D(image, dy, DepthType.Cv16S, one, kx, new Point(-1, -1));

            // do subpix
            CvInvoke.FindContours(cannyImg, result.FoundContours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
            bool isOk = extractSubPixPoints(dx, dy, result.FoundContours, out result.SubContours);

            #region [DEBUG] Draw result
            //CvInvoke.Imshow("cannyImg", cannyImg);
            //CvInvoke.Imshow("dx", dx);
            //CvInvoke.Imshow("dy", dy);
            //Image<Bgra, byte> contour = canny.Convert<Bgra, byte>();
            //CvInvoke.DrawContours(contour, foundContours, -1, new MCvScalar(255, 0, 255));
            //CvInvoke.Imshow("contour", contour);
            #endregion

            return isOk;
        }

    }
}
