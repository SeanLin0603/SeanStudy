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

                    doConvexHull(inputImage);
                }
            }
        }

        private static void doConvexHull(Image<Gray, byte> image)
        {
            var imgBGR = image.Convert<Bgra, byte>();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(image, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);

                PointF[] maxContourPtFs = new PointF[1];
                int maxSize = 0;

                for (int i = 0; i < contours.Size; i++)
                {
                    var contour = contours[i];
                    PointF[] temp = Array.ConvertAll(contour.ToArray(), new Converter<Point, PointF>(Point2PointF));
                    PointF[] pts = CvInvoke.ConvexHull(temp, true);
                    if(pts.Length>maxSize)
                    {
                        maxSize = pts.Length;
                        maxContourPtFs = pts;
                    }
                }

                Point[] maxContourPts = new Point[maxContourPtFs.Length];
                for (int i = 0; i < maxContourPtFs.Length; i++)
                {
                    maxContourPts[i] = new Point((int)maxContourPtFs[i].X, (int)maxContourPtFs[i].Y);
                }

                Point[] newMaxContourPts = new Point[23];

                newMaxContourPts[0] = maxContourPts[0];
                newMaxContourPts[1] = maxContourPts[1];
                newMaxContourPts[2] = new Point(70, 190);
                newMaxContourPts[3] = maxContourPts[2];
                newMaxContourPts[4] = new Point(90, 228);
                newMaxContourPts[5] = maxContourPts[3];
                newMaxContourPts[6] = new Point(120, 245);
                newMaxContourPts[7] = maxContourPts[4];
                newMaxContourPts[8] = new Point(161, 252);
                newMaxContourPts[9] = maxContourPts[5];
                newMaxContourPts[10] = new Point(196, 225);
                newMaxContourPts[11] = new Point(225, 226);
                newMaxContourPts[12] = new Point(214, 193);
                newMaxContourPts[13] = maxContourPts[7];
                newMaxContourPts[14] = new Point(188, 143);
                newMaxContourPts[15] = maxContourPts[8];
                newMaxContourPts[16] = new Point(188, 114);
                newMaxContourPts[17] = maxContourPts[9];
                newMaxContourPts[18] = new Point(162, 99);
                newMaxContourPts[19] = maxContourPts[11];
                newMaxContourPts[20] = new Point(130, 108);
                newMaxContourPts[21] = maxContourPts[13];
                newMaxContourPts[22] = new Point(107, 137);

                imgBGR.Draw(maxContourPts, new Bgra(0, 0, 255, 255), 2);
                imgBGR.Draw(newMaxContourPts, new Bgra(0, 255, 0, 255), 2);

                //for (int j = 0; j < maxContourPtFs.Length; j++)
                //{
                //    Point p1 = new Point((int)maxContourPtFs[j].X, (int)maxContourPtFs[j].Y);
                //    Point p2;

                //    if (j == maxContourPtFs.Length - 1)
                //        p2 = new Point((int)maxContourPtFs[0].X, (int)maxContourPtFs[0].Y);
                //    else
                //        p2 = new Point((int)maxContourPtFs[j + 1].X, (int)maxContourPtFs[j + 1].Y);

                //    CvInvoke.Line(imgBGR, p1, p2, new MCvScalar(255, 0, 255, 255), 3);
                //}

            }
            

            CvInvoke.Imshow("thres100", imgBGR);
            //imgBGR.Save("thres100.png");

        }




        private static PointF Point2PointF(Point P)
        {
            PointF PF = new PointF
            {
                X = P.X,
                Y = P.Y
            };
            return PF;
        }
    }
}
