using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Corex.VisionLib.Algorithm;
using Corex.VisionLib;
using System.Diagnostics;
using Emgu.CV.CvEnum;

namespace SeanStudy
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
                    var threSrc = inputImage.ThresholdBinary(new Gray(115), new Gray(255));
                    CvInvoke.Imshow("threSrc", threSrc);

                    Image<Gray, float> harris = new Image<Gray, float>(inputImage.Size);
                    CvInvoke.CornerHarris(threSrc.Convert<Gray, byte>(), harris, 2); 
                    CvInvoke.Normalize(harris, harris, 0, 255, NormType.MinMax, DepthType.Cv32F);
                    double min = 0, max = 0;
                    Point minp = new Point(0, 0);
                    Point maxp = new Point(0, 0);
                    CvInvoke.MinMaxLoc(harris, ref min, ref max, ref minp, ref maxp);

                    var cornerThres = harris.ThresholdBinary(new Gray(51), new Gray(255));
                    var cornerMap = cornerThres.Convert<Gray, byte>();
                    List<Point> points = new List<Point>();

                    var rgbSrc = inputImage.Convert<Bgra, byte>();
                    for (int h = 0; h < cornerMap.Height; h++)
                    {
                        for (int w = 0; w < cornerMap.Width; w++)
                        {
                            if (cornerMap[h, w].Intensity > 0)
                            {
                                CircleF circle = new CircleF(new PointF(w, h), 1);
                                rgbSrc.Draw(circle, new Bgra(0, 0, 255, 255), 1);
                                points.Add(new Point(w, h));
                            }
                        }
                    }

                    CvInvoke.Imshow("cornerMap", rgbSrc);

                    //PointF[] src = new PointF[4];
                    //PointF[] dst = new PointF[4];

                    //src[0] = new PointF(55, 72);
                    //src[1] = new PointF(418, 75);
                    //src[2] = new PointF(410, 266);
                    //src[3] = new PointF(176, 382);

                    //dst[0] = new PointF(55, 72);
                    //dst[1] = new PointF(418, 72);
                    //dst[2] = new PointF(418, 405);
                    //dst[3] = new PointF(55, 405);

                    //Mat homo = new Mat();
                    //Mat perspective = new Mat();
                    //Mat warpAffine = new Mat();

                    //CvInvoke.FindHomography(src, dst, homo, HomographyMethod.Ransac);
                    //perspective = CvInvoke.GetPerspectiveTransform(src, dst);
                    //warpAffine = CvInvoke.GetAffineTransform(src, dst);

                    //Image<Gray, double> homoImg = homo.ToImage<Gray, double>();
                    //Image<Gray, double> perspectiveImg = perspective.ToImage<Gray, double>();
                    //Image<Gray, double> warpAffineImg = warpAffine.ToImage<Gray, double>();

                    //CvInvoke.WarpPerspective(inputImage, outputImage, homo, outputImage.Size);
                    //CvInvoke.Imshow("homo", outputImage);
                    //CvInvoke.Imwrite("homo.png", outputImage);


                }
            }
        }

    }
}
