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
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace EMD
{
    public partial class Form1 : Form
    {
        Image<Bgra, byte> image1;
        Image<Bgra, byte> image2;
        Mat img1Hist;
        Mat img2Hist;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadPic1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    image1 = new Image<Bgra, byte>(ofd.FileName);
                    img1Hist = calcHistHSV(image1);
                    CvInvoke.Normalize(img1Hist, img1Hist, 0, 255, NormType.MinMax);
                }
            }
        }

        private void btnLoadPic2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    image2 = new Image<Bgra, byte>(ofd.FileName);
                    img2Hist = calcHistHSV(image2);
                    CvInvoke.Normalize(img2Hist, img2Hist, 0, 255, NormType.MinMax);
                }
            }

            double correl = CvInvoke.CompareHist(img1Hist, img2Hist, HistogramCompMethod.Correl);
            double chisqr = CvInvoke.CompareHist(img1Hist, img2Hist, HistogramCompMethod.Chisqr);
            double intersection = CvInvoke.CompareHist(img1Hist, img2Hist, HistogramCompMethod.Intersect);
            double bhattach = CvInvoke.CompareHist(img1Hist, img2Hist, HistogramCompMethod.Bhattacharyya);
            double chisqralt = CvInvoke.CompareHist(img1Hist, img2Hist, HistogramCompMethod.ChisqrAlt);

            double emd = earthMoverDistance(img1Hist, img2Hist, 256, 256);
        }

        private static Mat calcHistHSV(Image<Bgra, byte> image)
        {
            int[] channels = new int[] { 0, 1 };
            int[] histbins = new int[] { 256, 256 };
            float[] ranges = new float[] { 0.0f, 180.0f, 0.0f, 256.0f };

            Mat hist = new Mat();
            VectorOfMat vecOfMat = new VectorOfMat();
            Image<Hsv, float> imghsv = image.Convert<Hsv, float>();
            vecOfMat.Push(imghsv);
            CvInvoke.CalcHist(vecOfMat, channels, null, hist, histbins, ranges, false);
            return hist;
        }

        private static double earthMoverDistance(Mat hist1, Mat hist2, int histWidth, int histHeight)
        {
            CvInvoke.Normalize(hist1, hist1, 0, 1, NormType.MinMax);
            CvInvoke.Normalize(hist2, hist2, 0, 1, NormType.MinMax);

            Matrix<float> signature1 = new Matrix<float>(histWidth * histHeight, 3, 1);
            Matrix<float> signature2 = new Matrix<float>(histWidth * histHeight, 3, 1);
            Matrix<float> matHist1 = new Matrix<float>(histHeight, histWidth);
            Matrix<float> matHist2 = new Matrix<float>(histHeight, histWidth);
            hist1.CopyTo(matHist1);
            hist2.CopyTo(matHist2);

            for (int r = 0; r < histHeight; r++)
            {
                for (int c = 0; c < histWidth; c++)
                {
                    //matHist1;
                    signature1.Data[r * histWidth + c, 0] = matHist1.Data[r, c];
                    signature1.Data[r * histWidth + c, 1] = r;
                    signature1.Data[r * histWidth + c, 2] = c;

                    //matHist2;
                    signature2.Data[r * histWidth + c, 0] = matHist2.Data[r, c];
                    signature2.Data[r * histWidth + c, 1] = r;
                    signature2.Data[r * histWidth + c, 2] = c;
                }
            }

            double emd = CvInvoke.EMD(signature1, signature2, DistType.L2);
            return emd;
        }


    }
}
