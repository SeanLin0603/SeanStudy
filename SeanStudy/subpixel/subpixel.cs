using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

//using Corex.VisionLib.Algorithm;
//using Corex.VisionLib;

namespace subpixel
{
    public partial class subpixel : Form
    {
        public subpixel()
        {
            InitializeComponent();
        }

        public class Contour
        {
            public PointF[] points;
            public float[] direction;
            public float[] response;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var inputImage = new Image<Gray, byte>(ofd.FileName);
                    //ROIOption roi = new ROIOption(ROIOption.ROIType.SpecROI);
                    //roi.SpecROI = SpecROI.NewLeftTopModeROI(1740, 515, 80, 75);
                    //var cropImage = ROIOption.GetImage(inputImage, roi);


                    //Size kernel = new Size(3, 3);
                    //var gaussian = inputImage.CopyBlank();
                    //CvInvoke.GaussianBlur(inputImage, gaussian, kernel, 3);
                    //var mask = inputImage - gaussian;
                    //mask *= 2;

                    //inputImage += mask;
                    //CvInvoke.Imshow("Sharpen", inputImage);


                    doSubPix(inputImage);
                }
            }
        }

        private static bool doSubPix(Image<Gray, byte> image)
        {
            Image<Gray, byte> canny = image.Canny(300, 100);
            CvInvoke.Imshow("canny", canny);

            VectorOfVectorOfPoint foundContours;
            List<Contour> subContours;
            SubPixel(image, canny, 3.0, out foundContours, out subContours);

            int scale = 1;
            Image<Bgra, byte> drawImg = image.Resize(scale, Inter.Nearest).Convert<Bgra, byte>();

            // find the biggest contour
            int contourIdx = -1;
            int contourSize = -1;
            for (int i = 0; i < subContours.Capacity; i++)
            {
                int size = subContours[i].points.Length;
                if (size > contourSize)
                {
                    contourIdx = i;
                    contourSize = size;
                }
            }

            //// Draw
            //for (int i = 0; i < canny.Rows; i++)
            //{
            //    for (int j = 0; j < canny.Cols; j++)
            //    {
            //        byte value = canny.Data[i, j, 0];
            //        if (value > 0)
            //        {
            //            // white edge
            //            int r = i * scale + scale / 2;
            //            int c = j * scale + scale / 2;

            //            drawImg.Data[r, c, 0] = 0;
            //            drawImg.Data[r, c, 1] = 255;
            //            drawImg.Data[r, c, 2] = 0;
            //        }
            //    }
            //}
            //CvInvoke.Imwrite(".\\subpix_canny.png", drawImg);

            double centerX = 0, centerY = 0;

            VectorOfPoint contour = foundContours[contourIdx];
            Image<Bgra, byte> contourImg = canny.CopyBlank().Convert<Bgra, byte>();
            CvInvoke.DrawContours(contourImg, foundContours, contourIdx, new MCvScalar(255, 0, 255));
            CvInvoke.Imshow("contour", contourImg);
            CvInvoke.Imwrite("contour.png", contourImg);
            for (int i = 0; i < contour.Size; i++)
            {
                Point pt = contour[i];
                centerX += pt.X;
                centerY += pt.Y;

                int r = pt.Y * scale + scale / 2;
                int c = pt.X * scale + scale / 2;

                // green means contour points
                drawImg.Data[r, c, 0] = 0;
                drawImg.Data[r, c, 1] = 255;
                drawImg.Data[r, c, 2] = 0;
            }
            centerX = centerX / contour.Size * scale;
            centerY = centerY / contour.Size * scale;
            // cyan means average point
            drawImg.Data[(int)centerY, (int)centerX, 0] = 255;
            drawImg.Data[(int)centerY, (int)centerX, 1] = 255;
            drawImg.Data[(int)centerY, (int)centerX, 2] = 0;
            CvInvoke.Imwrite("subpix_FindContour.png", drawImg);

            centerX = 0;
            centerY = 0;
            PointF[] biggestContourPoints = subContours[contourIdx].points;
            for (int i = 0; i < biggestContourPoints.Length; i++)
            {
                PointF pt = biggestContourPoints[i];
                centerX += pt.X;
                centerY += pt.Y;

                // draw in center
                int r = (int)(pt.Y * scale + scale / 2);
                int c = (int)(pt.X * scale + scale / 2);

                // red means subpix points
                drawImg.Data[r, c, 0] = 0;
                drawImg.Data[r, c, 1] = 0;
                drawImg.Data[r, c, 2] = 255;
            }
            centerX = centerX / contour.Size * scale;
            centerY = centerY / contour.Size * scale;
            // yellow means average point
            drawImg.Data[(int)centerY, (int)centerX, 0] = 0;
            drawImg.Data[(int)centerY, (int)centerX, 1] = 255;
            drawImg.Data[(int)centerY, (int)centerX, 2] = 255;

            CvInvoke.Imwrite("subpix.png", drawImg);
            CvInvoke.Imshow("drawImg", drawImg);
            return true;
        }


        private static bool getCannyKernel(double alpha, out Image<Gray, int> kx, out Image<Gray, int> ky)
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
            double dataY = dx.Data[i, j, 0];
            return Math.Sqrt(dataX * dataX + dataY + dataY);
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

            /* If the absolute value of an eigenvalue is larger than the other, put that
            eigenvalue into first position.  If both are of equal absolute value, put
            the negative one first. */
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

        public static bool SubPixel(Image<Gray, byte> image, Image<Gray, byte> canny, double alpha, out VectorOfVectorOfPoint foundContours, out List<Contour> subContours)
        {
            Image<Gray, byte> blur = image.Clone();
            Size kernel = new Size(3, 3);
            CvInvoke.GaussianBlur(image, blur, kernel, alpha);

            Image<Gray, int> kx, ky;
            getCannyKernel(alpha, out kx, out ky);
            Image<Gray, int> one = new Image<Gray, int>(1, 1);
            one.Data[0, 0, 0] = 1;

            Image<Gray, short> dx = new Image<Gray, short>(image.Width, image.Height);
            Image<Gray, short> dy = new Image<Gray, short>(image.Width, image.Height);

            CvInvoke.SepFilter2D(image, dx, DepthType.Cv16S, kx, one, new Point(-1, -1));
            CvInvoke.SepFilter2D(image, dy, DepthType.Cv16S, one, kx, new Point(-1, -1));
            //CvInvoke.Imshow("dx", dx);
            //CvInvoke.Imshow("dy", dy);

            //Image<Gray, byte> edge = new Image<Gray, byte>("C:\\Users\\sean8\\source\\repos\\SubPix\\SubPix\\edge.png");
            //CvInvoke.FindContours(edge, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);

            foundContours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(canny, foundContours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);

            //Image<Bgra, byte> contour = canny.Convert<Bgra, byte>();
            //CvInvoke.DrawContours(contour, foundContours, 0, new MCvScalar(255, 0, 255));
            //CvInvoke.Imshow("contour", contour);
            extractSubPixPoints(dx, dy, foundContours, out subContours);

            return true;
        }

    }
}
