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
using System.Runtime.InteropServices;
using System.IO;

namespace EMD
{
    public partial class EMDForm : Form
    {
        public static float[] HistValF1;
        public static float[] HistValF2;
        public static Image<Gray, float> HistImg1;
        public static Image<Gray, float> HistImg2;
        public static int BinSize;

        public EMDForm()
        {
            InitializeComponent();
        }

        private void btnLoadPic1_Click(object sender, EventArgs e)
        {
            BinSize = int.Parse(txtBinSize.Text);
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image<Gray, byte> image1 = new Image<Gray, byte>(ofd.FileName);
                    picImg1.Image = image1.Bitmap;

                    HistValF1 = getHistogram(image1, BinSize);
                    Image<Gray, byte> histImg = drawHistogram(HistValF1, BinSize);
                    HistImg1 = histImg.Convert<Gray, float>();
                    picHist1.Image = histImg.Bitmap;
                }
            }
        }

        private void btnLoadPic2_Click(object sender, EventArgs e)
        {
            BinSize = int.Parse(txtBinSize.Text);
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image<Gray, byte> image2 = new Image<Gray, byte>(ofd.FileName);
                    picImg2.Image = image2.Bitmap;

                    HistValF2 = getHistogram(image2, BinSize);
                    Image<Gray, byte> histImg = drawHistogram(HistValF2, BinSize);
                    HistImg2 = histImg.Convert<Gray, float>();
                    picHist2.Image = histImg.Bitmap;
                }
            }
        }

        private static float[] getHistogram(Image<Gray, byte> image, int binSize)
        {
            RangeF range = new RangeF(0, 256);
            DenseHistogram hist = new DenseHistogram(binSize, range);
            Image<Gray, byte>[] images = new Image<Gray, byte>[] { image };
            hist.Calculate(images, false, null);

            float[] histValF = new float[hist.Rows];
            Marshal.Copy(hist.DataPointer, histValF, 0, hist.Rows);

            return histValF;
        }

        private static Image<Gray, byte> drawHistogram(float[] histArray, int binSize)
        {
            float maxVal = histArray.Max();

            Image<Gray, byte> histImg = new Image<Gray, byte>(binSize, binSize, new Gray(255));
            for (int i = 0; i < binSize; i++)
            {
                // normalization
                int y = binSize - (int)(histArray[i] / maxVal * binSize) - 1;
                Point p1 = new Point(i, y);
                Point p2 = new Point(i, binSize - 1);
                LineSegment2D line = new LineSegment2D(p1, p2);
                histImg.Draw(line, new Gray(0), 1);
            }
            return histImg;
        }

        //private static Mat calcHistHSV(Image<Gray, byte> image)
        //{
        //    int[] channels = new int[] { 0, 1};
        //    int[] histbins = new int[] { 256, 256 };
        //    float[] ranges = new float[] { 0.0f, 360.0f, 0.0f, 256.0f };

        //    Mat hist = new Mat();
        //    VectorOfMat vecOfMat = new VectorOfMat();
        //    Image<Hsv, float> imghsv = image.Convert<Hsv, float>();
        //    vecOfMat.Push(imghsv);
        //    CvInvoke.CalcHist(vecOfMat, channels, null, hist, histbins, ranges, false);

        //    return hist;
        //}

        //private static double earthMoverDistance(Mat mat1, Mat mat2, int histWidth, int histHeight)
        //{
        //    //CvInvoke.Normalize(hist1, hist1, 0, 1, NormType.MinMax);
        //    //CvInvoke.Normalize(hist2, hist2, 0, 1, NormType.MinMax);

        //    Matrix<float> signature1 = new Matrix<float>(histWidth * histHeight, 3, 1);
        //    Matrix<float> signature2 = new Matrix<float>(histWidth * histHeight, 3, 1);
        //    Matrix<float> matrixHist1 = new Matrix<float>(histHeight, histWidth);
        //    Matrix<float> matrixHist2 = new Matrix<float>(histHeight, histWidth);
        //    mat1.CopyTo(matrixHist1);
        //    mat2.CopyTo(matrixHist2);

        //    for (int r = 0; r < histHeight; r++)
        //    {
        //        for (int c = 0; c < histWidth; c++)
        //        {
        //            //matHist1;
        //            signature1.Data[r * histWidth + c, 0] = matrixHist1.Data[r, c];
        //            signature1.Data[r * histWidth + c, 1] = r;
        //            signature1.Data[r * histWidth + c, 2] = c;

        //            //matHist2;
        //            signature2.Data[r * histWidth + c, 0] = matrixHist2.Data[r, c];
        //            signature2.Data[r * histWidth + c, 1] = r;
        //            signature2.Data[r * histWidth + c, 2] = c;
        //        }
        //    }

        //    double emd = CvInvoke.EMD(signature1, signature2, DistType.L2);
        //    return emd;
        //}

        private static double earthMoverDistance(float[] hist1, float[] hist2, int binSize)
        {
            //Matrix<float> signature1 = new Matrix<float>(4, 4, 1);
            //Matrix<float> signature2 = new Matrix<float>(3, 4, 1);

            //signature1.Data[0, 0] = 0.4f;
            //signature1.Data[0, 1] = 100;
            //signature1.Data[0, 2] = 40;
            //signature1.Data[0, 3] = 22;
            //signature1.Data[1, 0] = 0.3f;
            //signature1.Data[1, 1] = 211;
            //signature1.Data[1, 2] = 20;
            //signature1.Data[1, 3] = 2;
            //signature1.Data[2, 0] = 0.2f;
            //signature1.Data[2, 1] = 32;
            //signature1.Data[2, 2] = 190;
            //signature1.Data[2, 3] = 150;
            //signature1.Data[3, 0] = 0.1f;
            //signature1.Data[3, 1] = 2;
            //signature1.Data[3, 2] = 100;
            //signature1.Data[3, 3] = 100;

            //signature2.Data[0, 0] = 0.5f;
            //signature2.Data[0, 1] = 0;
            //signature2.Data[0, 2] = 0;
            //signature2.Data[0, 3] = 0;
            //signature2.Data[1, 0] = 0.3f;
            //signature2.Data[1, 1] = 50;
            //signature2.Data[1, 2] = 100;
            //signature2.Data[1, 3] = 80;
            //signature2.Data[2, 0] = 0.2f;
            //signature2.Data[2, 1] = 255;
            //signature2.Data[2, 2] = 255;
            //signature2.Data[2, 3] = 255;

            Matrix<float> signature1 = new Matrix<float>(binSize, 2, 1);
            Matrix<float> signature2 = new Matrix<float>(binSize, 2, 1);

            for (int i = 0; i < binSize; i++)
            {
                // weight
                signature1.Data[i, 0] = hist1[i] / binSize / BinSize;
                // feature
                signature1.Data[i, 1] = i;

                // weight
                signature2.Data[i, 0] = hist2[i] / BinSize / binSize;
                // feature
                signature2.Data[i, 1] = i;
            }

            //write2file(hist1, hist2);

            double emd = CvInvoke.EMD(signature1, signature2, DistType.L2);
            return emd;
        }

        //private static double earthMoverDistance(Image<Gray, float> hist1, Image<Gray, float> hist2, int histWidth, int histHeight)
        //{
        //    Matrix<float> signature1 = new Matrix<float>(histWidth * histHeight, 3, 1);
        //    Matrix<float> signature2 = new Matrix<float>(histWidth * histHeight, 3, 1);

        //    for (int r = 0; r < histHeight; r++)
        //    {
        //        for (int c = 0; c < histWidth; c++)
        //        {
        //            //matHist1;
        //            signature1.Data[r * histWidth + c, 0] = hist1.Data[r, c, 0] / 255;
        //            signature1.Data[r * histWidth + c, 1] = r;
        //            signature1.Data[r * histWidth + c, 2] = c;

        //            //matHist2;
        //            signature2.Data[r * histWidth + c, 0] = hist2.Data[r, c, 0] / 255;
        //            signature2.Data[r * histWidth + c, 1] = r;
        //            signature2.Data[r * histWidth + c, 2] = c;
        //        }
        //    }

        //    double emd = CvInvoke.EMD(signature1, signature2, DistType.L2);
        //    return emd;
        //}

        private void btnCalc_Click(object sender, EventArgs e)
        {
            lblCorrel.Text = "Correlation: Computing...";
            lblChisqr.Text = "Chi-Square: Computing...";
            lblIntersect.Text = "Intersection: Computing...";
            lblBhattacharyya.Text = "Bhattacharyya: Computing...";
            lblChisqrAlt.Text = "Alternative Chi-Square: Computing...";
            lblEMD.Text = "EMD: Computing...";

            double correl = CvInvoke.CompareHist(HistImg1, HistImg2, HistogramCompMethod.Correl);
            double chisqr = CvInvoke.CompareHist(HistImg1, HistImg2, HistogramCompMethod.Chisqr);
            double intersection = CvInvoke.CompareHist(HistImg1, HistImg2, HistogramCompMethod.Intersect);
            double bhattach = CvInvoke.CompareHist(HistImg1, HistImg2, HistogramCompMethod.Bhattacharyya);
            double chisqralt = CvInvoke.CompareHist(HistImg1, HistImg2, HistogramCompMethod.ChisqrAlt);
            double emd = earthMoverDistance(HistValF1, HistValF2, BinSize);
            //double emd = 0;

            lblCorrel.Text = "Correlation: " + correl.ToString();
            lblChisqr.Text = "Chi-Square: " + chisqr.ToString();
            lblIntersect.Text = "Intersection: " + intersection.ToString();
            lblBhattacharyya.Text = "Bhattacharyya: " + bhattach.ToString();
            lblChisqrAlt.Text = "Alternative Chi-Square: " + chisqralt.ToString();
            lblEMD.Text = "EMD: " + emd.ToString();
        }


        private static void write2file(float[] data1, float[] data2)
        {
            FileStream file = new FileStream("data.txt", FileMode.Create, FileAccess.Write);
            StreamWriter stream = new StreamWriter(file, Encoding.UTF8);

            // gray value of hist1
            stream.Write("{ ");
            for (int i = 0; i < data1.Length; i++)
            {
                stream.Write(data1[i]);
                if (i != data1.Length - 1)
                {
                    stream.Write(", ");
                }
                else
                {
                    stream.Write(" ");
                }
            }
            stream.WriteLine("}\n");

            // index of hist1
            stream.Write("{ ");
            for (int i = 0; i < data1.Length; i++)
            {
                stream.Write(i);
                if (i != data1.Length - 1)
                {
                    stream.Write(", ");
                }
                else
                {
                    stream.Write(" ");
                }
            }
            stream.WriteLine("}\n");

            // gray value of hist2
            stream.Write("{ ");
            for (int i = 0; i < data2.Length; i++)
            {
                stream.Write(data2[i]);
                if (i != data2.Length - 1)
                {
                    stream.Write(", ");
                }
                else
                {
                    stream.Write(" ");
                }
            }
            stream.WriteLine("}\n");

            // index of hist2
            stream.Write("{ ");
            for (int i = 0; i < data2.Length; i++)
            {
                stream.Write(i);
                if (i != data2.Length - 1)
                {
                    stream.Write(", ");
                }
                else
                {
                    stream.Write(" ");
                }
            }
            stream.WriteLine("}\n");

            stream.Close();
            file.Close();

        }
    }
}
