using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    var inputImage = new Image<Gray, byte>(ofd.FileName);
                    inputImage = inputImage.ThresholdBinary(new Gray(100), new Gray(255));
                    CvInvoke.GaussianBlur(inputImage, inputImage, new Size(5, 5), 5);

                    //CvInvoke.Imshow("inputImage", inputImage);
                    doConvexHull(inputImage);
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

                                CvInvoke.Circle(imgBGR, adjustPoint, 1, new MCvScalar(0, 255, 0, 255), 2);
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

    }
}
