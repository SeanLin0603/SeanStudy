using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Corex.VisionLib;

namespace ConvexHull
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var inputImage = new Image<Bgra, byte>(ofd.FileName);
                    //inputImage = inputImage.ThresholdBinary(new Gray(100), new Gray(255));
                    //CvInvoke.GaussianBlur(inputImage, inputImage, new Size(5, 5), 5);


                    //ROIOption roi = new ROIOption(ROIOption.ROIType.SpecROI);
                    //roi.SpecROI = SpecROI.NewLeftTopModeROI(50, 50, 200, 200);
                    //Image<Bgra, byte> result = new Image<Bgra, byte>(inputImage.Size);
                    //GetCircleImage(inputImage, 100, roi, out result);
                    ////CvInvoke.Imshow("result", result);
                    
                    //ContourMorphology(inputImage, 90, out result);
                }
            }
        }


        public static bool GetCircleImage(Image<Bgra, byte> image, int radius, ROIOption roi, out Image<Bgra, byte> result)
        {
            result = image.Clone();

            Image<Bgra, byte> mask = new Image<Bgra, byte>(2 * radius, 2 * radius);
            CircleF circle = new CircleF(new PointF(radius, radius), radius);
            Bgra white = new Bgra(255, 255, 255, 255);
            mask.Draw(circle, white, -1);

            var cropImg = ROIOption.GetImage(image, roi);
            if (cropImg.Size != mask.Size)
            {
                return false;
            }
            CvInvoke.BitwiseAnd(cropImg, mask, result);
            return true;
        }


        public static bool ContourMorphology(Image<Gray,byte> image, int radius, out Image<Gray,byte> result)
        {
            result = new Image<Gray, byte>(image.Size);

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                var tmpImg = image.Clone();
                CvInvoke.FindContours(tmpImg, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);
                VectorOfPoint contour = contours[0];

                // get the gravity point
                MCvMoments moments = CvInvoke.Moments(contour, false);
                double m00 = moments.M00;
                double m10 = moments.M10;
                double m01 = moments.M01;
                double gx = m10 / m00;
                double gy = m01 / m00;
                Point gravity = new Point((int)gx, (int)gy);

                // elimination by radius
                List<Point> contourList = contour.ToArray().ToList();
                for (int i = 0; i < contour.Size; i++)
                {
                    Point point = contour[i];
                    double dist = distance(point, gravity);
                    
                    if (dist < radius)
                    {
                        contourList.Remove(point);
                    }
                }

                #region [Debug]
                //var imgBGR = image.Convert<Bgra, byte>();
                //CvInvoke.DrawContours(imgBGR, contours, -1, new MCvScalar(255, 255, 0, 255), 2);
                //imgBGR.Draw(new CircleF(new PointF(gravity.X, gravity.Y), 1f), new Bgra(0, 0, 255, 255), 2);
                //imgBGR.Draw(contourList.ToArray(), new Bgra(0, 255, 0, 255), 2);
                //CvInvoke.Imshow("imgBGR", imgBGR);
                #endregion

                try
                {
                    // fill the contour
                    VectorOfPoint refinedPoints = new VectorOfPoint(contourList.ToArray());
                    VectorOfVectorOfPoint vvp = new VectorOfVectorOfPoint(refinedPoints);
                    CvInvoke.FillPoly(result, vvp, new MCvScalar(255));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static void doConvexHull(Image<Gray, byte> image)
        {
            var imgBGR = image.Convert<Bgra, byte>();

            using (Mat defect = new Mat())
            using (VectorOfInt hull = new VectorOfInt())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(image, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);
                //CvInvoke.DrawContours(imgBGR, contours, -1, new MCvScalar(0, 255, 0, 255), 2);

                List<Point> fittingContour = new List<Point>();

                for (int i = 0; i < contours.Size; i++)
                {
                    VectorOfPoint contour = contours[i];
                    CvInvoke.ConvexHull(contour, hull);
                    CvInvoke.ConvexityDefects(contour, hull, defect);

                    //convexity defect is a four channel mat, when k rows and 1 cols, where k = the number of convexity defects. 
                    if (!defect.IsEmpty)
                    {
                        //Data from Mat are not directly readable so we convert it to Matrix<>
                        Matrix<int> defectMatrix = new Matrix<int>(defect.Rows, defect.Cols, defect.NumberOfChannels);
                        defect.CopyTo(defectMatrix);

                        for (int r = 0; r < defectMatrix.Rows; r++)
                        {
                            int startIdx = defectMatrix.Data[r, 0];
                            int endIdx = defectMatrix.Data[r, 1];
                            int farIdx = defectMatrix.Data[r, 2];
                            Point startPoint = contour[startIdx];
                            Point endPoint = contour[endIdx];
                            Point farPoint = contour[farIdx];

                            // draw a line connecting the convexity defect start point and end point in thin red line
                            CvInvoke.Line(imgBGR, startPoint, endPoint, new MCvScalar(0, r * 40, 255, 255), 2);

                            if (r == 0)
                            {
                                fittingContour.Add(startPoint);
                            }
                            else if (r > 0)
                            {
                                Point middlePoint = middle(startPoint, endPoint);
                                Point adjustPoint = middle(middlePoint, farPoint);
                                fittingContour.Add(adjustPoint);
                                fittingContour.Add(endPoint);

                                CvInvoke.Circle(imgBGR, middlePoint, 1, new MCvScalar(0, 255, 0, 255), 2);
                                CvInvoke.Circle(imgBGR, adjustPoint, 1, new MCvScalar(255, 255, 0, 255), 2);
                                CvInvoke.Circle(imgBGR, farPoint, 1, new MCvScalar(255, 0, r * 40, 255), 2);
                            }
                        }

                        imgBGR.Draw(fittingContour.ToArray(), new Bgra(255, 255, 0, 255), 2);
                    }
                }
            }

            CvInvoke.Imshow("thres100", imgBGR);
        }

        private static Point middle(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        private static double distance(Point p1, Point p2)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        

    }
}
