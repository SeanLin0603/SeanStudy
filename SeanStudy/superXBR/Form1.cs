using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace superXBR
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
                    int scale = 4;

                    var inputImage = new Image<Bgra, byte>(ofd.FileName);
                    Image<Bgra, byte> result = inputImage.Clone();

                    Stopwatch sw = new Stopwatch();
                    sw.Reset();
                    sw.Start();
                    SuperxBR(inputImage, scale, out result);
                    sw.Stop();
                    string timeXBR = sw.Elapsed.TotalMilliseconds.ToString();

                    sw.Reset();
                    sw.Start();
                    var resizeImg = inputImage.Resize(scale, Emgu.CV.CvEnum.Inter.Nearest);
                    sw.Stop();
                    string timeResize = sw.Elapsed.TotalMilliseconds.ToString();

                    Console.WriteLine("Super-xBR takes :" + timeXBR + "ms");
                    Console.WriteLine("OpenCV resize takes :" + timeResize + "ms");

                    CvInvoke.Imshow("inputImage", inputImage);
                    CvInvoke.Imshow("result", result);
                    CvInvoke.Imwrite("result.png", result);

                    CvInvoke.Imshow("resizeImg", resizeImg);
                    CvInvoke.Imwrite("resizeImg.png", resizeImg);

                }
            }
        }

        private double df(double a, double b)
        {
            return Math.Abs(a - b);
        }

        private double clamp(double x, double floor, double ceil)
        {
            return Math.Max(Math.Min(x, ceil), floor);
        }

        private int getMin(int a, int b, int c, int d)
        {
            int minVal = int.MaxValue;
            if (a < minVal) minVal = a;
            if (b < minVal) minVal = b;
            if (c < minVal) minVal = c;
            if (d < minVal) minVal = d;
            return minVal;
        }

        private int getMax(int a, int b, int c, int d)
        {
            int maxVal = int.MinValue;
            if (a > maxVal) maxVal = a;
            if (b > maxVal) maxVal = b;
            if (c > maxVal) maxVal = c;
            if (d > maxVal) maxVal = d;
            return maxVal;
        }

        private int[][] fourByfourIntMat()
        {
            int[][] array = new int[4][];
            array[0] = new int[4];
            array[1] = new int[4];
            array[2] = new int[4];
            array[3] = new int[4];
            return array;
        }

        private double[][] fourByfourDoubleMat()
        {
            double[][] array = new double[4][];
            array[0] = new double[4];
            array[1] = new double[4];
            array[2] = new double[4];
            array[3] = new double[4];
            return array;
        }

        private double diagonal_edge(double[][] mat1, double[] wp)
        {
            double dw1 = wp[0] * (df(mat1[0][2], mat1[1][1]) + df(mat1[1][1], mat1[2][0]) + df(mat1[1][3], mat1[2][2]) + df(mat1[2][2], mat1[3][1])) +
                        wp[1] * (df(mat1[0][3], mat1[1][2]) + df(mat1[2][1], mat1[3][0])) +
                        wp[2] * (df(mat1[0][3], mat1[2][1]) + df(mat1[1][2], mat1[3][0])) +
                        wp[3] * df(mat1[1][2], mat1[2][1]) +
                        wp[4] * (df(mat1[0][2], mat1[2][0]) + df(mat1[1][3], mat1[3][1])) +
                        wp[5] * (df(mat1[0][1], mat1[1][0]) + df(mat1[2][3], mat1[3][2]));

            double dw2 = wp[0] * (df(mat1[0][1], mat1[1][2]) + df(mat1[1][2], mat1[2][3]) + df(mat1[1][0], mat1[2][1]) + df(mat1[2][1], mat1[3][2])) +
                        wp[1] * (df(mat1[0][0], mat1[1][1]) + df(mat1[2][2], mat1[3][3])) +
                        wp[2] * (df(mat1[0][0], mat1[2][2]) + df(mat1[1][1], mat1[3][3])) +
                        wp[3] * df(mat1[1][1], mat1[2][2]) +
                        wp[4] * (df(mat1[1][0], mat1[3][2]) + df(mat1[0][1], mat1[2][3])) +
                        wp[5] * (df(mat1[0][2], mat1[1][3]) + df(mat1[2][0], mat1[3][1]));

            return (dw1 - dw2);
        }


        static double wgt1 = 0.129633;
        static double wgt2 = 0.129633;
        static double w1 = -wgt1;
        static double w2 = wgt1 + 0.5;
        static double w3 = -wgt2;
        static double w4 = wgt2 + 0.5;

        public bool SuperxBR(Image<Bgra, byte> image, int factor, out Image<Bgra, byte> result)
        {
            result = new Image<Bgra, byte>(factor * image.Width, factor * image.Height);

            int[][] r = fourByfourIntMat();
            int[][] b = fourByfourIntMat();
            int[][] g = fourByfourIntMat();
            int[][] a = fourByfourIntMat();
            double[][] Y = fourByfourDoubleMat();

            double rf, gf, bf, af;
            byte ri, gi, bi, ai;
            double d_edge;
            double min_r_sample, max_r_sample;
            double min_g_sample, max_g_sample;
            double min_b_sample, max_b_sample;
            double min_a_sample, max_a_sample;

            #region First Pass
            double[] wp = new double[] { 2.0, 1.0, -1.0, 4.0, -1.0, 1.0 };
            for (int y = 0; y < result.Height; ++y)
            {
                for (int x = 0; x < result.Width; ++x)
                {
                    // central pixels on original images
                    int cx = x / factor;
                    int cy = y / factor;

                    // sample supporting pixels in original image
                    for (int sx = -1; sx <= 2; ++sx)
                    {
                        for (int sy = -1; sy <= 2; ++sy)
                        {
                            // clamp pixel locations
                            int csy = (int)clamp(sy + cy, 0, image.Height - 1);
                            int csx = (int)clamp(sx + cx, 0, image.Width - 1);
                            // sample & add weighted components
                            byte bSample = image.Data[csy, csx, 0];
                            byte gSample = image.Data[csy, csx, 1];
                            byte rSample = image.Data[csy, csx, 2];
                            byte aSample = image.Data[csy, csx, 3];

                            r[sx + 1][sy + 1] = rSample;
                            g[sx + 1][sy + 1] = gSample;
                            b[sx + 1][sy + 1] = bSample;
                            a[sx + 1][sy + 1] = aSample;
                            Y[sx + 1][sy + 1] = (0.2126 * r[sx + 1][sy + 1] + 0.7152 * g[sx + 1][sy + 1] + 0.0722 * b[sx + 1][sy + 1]);
                        }
                    }
                    min_r_sample = getMin(r[1][1], r[2][1], r[1][2], r[2][2]);
                    min_g_sample = getMin(g[1][1], g[2][1], g[1][2], g[2][2]);
                    min_b_sample = getMin(b[1][1], b[2][1], b[1][2], b[2][2]);
                    min_a_sample = getMin(a[1][1], a[2][1], a[1][2], a[2][2]);
                    max_r_sample = getMax(r[1][1], r[2][1], r[1][2], r[2][2]);
                    max_g_sample = getMax(g[1][1], g[2][1], g[1][2], g[2][2]);
                    max_b_sample = getMax(b[1][1], b[2][1], b[1][2], b[2][2]);
                    max_a_sample = getMax(a[1][1], a[2][1], a[1][2], a[2][2]);
                    d_edge = diagonal_edge(Y, wp);

                    if (d_edge <= 0)
                    {
                        rf = w1 * (r[0][3] + r[3][0]) + w2 * (r[1][2] + r[2][1]);
                        gf = w1 * (g[0][3] + g[3][0]) + w2 * (g[1][2] + g[2][1]);
                        bf = w1 * (b[0][3] + b[3][0]) + w2 * (b[1][2] + b[2][1]);
                        af = w1 * (a[0][3] + a[3][0]) + w2 * (a[1][2] + a[2][1]);
                    }
                    else
                    {
                        rf = w1 * (r[0][0] + r[3][3]) + w2 * (r[1][1] + r[2][2]);
                        gf = w1 * (g[0][0] + g[3][3]) + w2 * (g[1][1] + g[2][2]);
                        bf = w1 * (b[0][0] + b[3][3]) + w2 * (b[1][1] + b[2][2]);
                        af = w1 * (a[0][0] + a[3][3]) + w2 * (a[1][1] + a[2][2]);
                    }
                    // anti-ringing, clamp.
                    rf = clamp(rf, min_r_sample, max_r_sample);
                    gf = clamp(gf, min_g_sample, max_g_sample);
                    bf = clamp(bf, min_b_sample, max_b_sample);
                    af = clamp(af, min_a_sample, max_a_sample);
                    ri = (byte)clamp(Math.Ceiling(rf), 0, 255);
                    gi = (byte)clamp(Math.Ceiling(gf), 0, 255);
                    bi = (byte)clamp(Math.Ceiling(bf), 0, 255);
                    ai = (byte)clamp(Math.Ceiling(af), 0, 255);
                    result.Data[y, x, 0] = result.Data[y, x + 1, 0] = result.Data[y + 1, x, 0] = image.Data[cy, cx, 0];
                    result.Data[y, x, 1] = result.Data[y, x + 1, 1] = result.Data[y + 1, x, 1] = image.Data[cy, cx, 1];
                    result.Data[y, x, 2] = result.Data[y, x + 1, 2] = result.Data[y + 1, x, 2] = image.Data[cy, cx, 2];
                    result.Data[y, x, 3] = result.Data[y, x + 1, 3] = result.Data[y + 1, x, 3] = image.Data[cy, cx, 3];
                    result.Data[y + 1, x + 1, 0] = bi;
                    result.Data[y + 1, x + 1, 1] = gi;
                    result.Data[y + 1, x + 1, 2] = ri;
                    result.Data[y + 1, x + 1, 3] = ai;
                    ++x;
                }
                ++y;
            }
            #endregion

            CvInvoke.Imwrite("result1.png", result);

            #region Second Pass
            wp[0] = 2.0;
            wp[1] = 0.0;
            wp[2] = 0.0;
            wp[3] = 0.0;
            wp[4] = 0.0;
            wp[5] = 0.0;

            for (int y = 0; y < result.Height; ++y)
            {
                for (int x = 0; x < result.Width; ++x)
                {
                    // sample supporting pixels in original image
                    for (int sx = -1; sx <= 2; ++sx)
                    {
                        for (int sy = -1; sy <= 2; ++sy)
                        {
                            // clamp pixel locations
                            int csy = (int)clamp(sx - sy + y, 0, factor * image.Height - 1);
                            int csx = (int)clamp(sx + sy + x, 0, factor * image.Width - 1);
                            // sample & add weighted components
                            byte bSample = result.Data[csy, csx, 0];
                            byte gSample = result.Data[csy, csx, 1];
                            byte rSample = result.Data[csy, csx, 2];
                            byte aSample = result.Data[csy, csx, 3];

                            r[sx + 1][sy + 1] = rSample;
                            g[sx + 1][sy + 1] = gSample;
                            b[sx + 1][sy + 1] = bSample;
                            a[sx + 1][sy + 1] = aSample;
                            Y[sx + 1][sy + 1] = (0.2126 * r[sx + 1][sy + 1] + 0.7152 * g[sx + 1][sy + 1] + 0.0722 * b[sx + 1][sy + 1]);
                        }
                    }

                    min_r_sample = getMin(r[1][1], r[2][1], r[1][2], r[2][2]);
                    min_g_sample = getMin(g[1][1], g[2][1], g[1][2], g[2][2]);
                    min_b_sample = getMin(b[1][1], b[2][1], b[1][2], b[2][2]);
                    min_a_sample = getMin(a[1][1], a[2][1], a[1][2], a[2][2]);
                    max_r_sample = getMax(r[1][1], r[2][1], r[1][2], r[2][2]);
                    max_g_sample = getMax(g[1][1], g[2][1], g[1][2], g[2][2]);
                    max_b_sample = getMax(b[1][1], b[2][1], b[1][2], b[2][2]);
                    max_a_sample = getMax(a[1][1], a[2][1], a[1][2], a[2][2]);
                    d_edge = diagonal_edge(Y, wp);
                    if (d_edge <= 0)
                    {
                        rf = w3 * (r[0][3] + r[3][0]) + w4 * (r[1][2] + r[2][1]);
                        gf = w3 * (g[0][3] + g[3][0]) + w4 * (g[1][2] + g[2][1]);
                        bf = w3 * (b[0][3] + b[3][0]) + w4 * (b[1][2] + b[2][1]);
                        af = w3 * (a[0][3] + a[3][0]) + w4 * (a[1][2] + a[2][1]);
                    }
                    else
                    {
                        rf = w3 * (r[0][0] + r[3][3]) + w4 * (r[1][1] + r[2][2]);
                        gf = w3 * (g[0][0] + g[3][3]) + w4 * (g[1][1] + g[2][2]);
                        bf = w3 * (b[0][0] + b[3][3]) + w4 * (b[1][1] + b[2][2]);
                        af = w3 * (a[0][0] + a[3][3]) + w4 * (a[1][1] + a[2][2]);
                    }

                    // anti-ringing, clamp.
                    rf = clamp(rf, min_r_sample, max_r_sample);
                    gf = clamp(gf, min_g_sample, max_g_sample);
                    bf = clamp(bf, min_b_sample, max_b_sample);
                    af = clamp(af, min_a_sample, max_a_sample);
                    ri = (byte)clamp(Math.Ceiling(rf), 0, 255);
                    gi = (byte)clamp(Math.Ceiling(gf), 0, 255);
                    bi = (byte)clamp(Math.Ceiling(bf), 0, 255);
                    ai = (byte)clamp(Math.Ceiling(af), 0, 255);
                    result.Data[y, x + 1, 0] = bi;
                    result.Data[y, x + 1, 1] = gi;
                    result.Data[y, x + 1, 2] = ri;
                    result.Data[y, x + 1, 3] = ai;

                    for (int sx = -1; sx <= 2; ++sx)
                    {
                        for (int sy = -1; sy <= 2; ++sy)
                        {
                            // clamp pixel locations
                            int csy = (int)clamp(sx - sy + 1 + y, 0, factor * image.Height - 1);
                            int csx = (int)clamp(sx + sy - 1 + x, 0, factor * image.Width - 1);
                            // sample & add weighted components
                            byte bSample = result.Data[csy, csx, 0];
                            byte gSample = result.Data[csy, csx, 1];
                            byte rSample = result.Data[csy, csx, 2];
                            byte aSample = result.Data[csy, csx, 3];

                            r[sx + 1][sy + 1] = rSample;
                            g[sx + 1][sy + 1] = gSample;
                            b[sx + 1][sy + 1] = bSample;
                            a[sx + 1][sy + 1] = aSample;
                            Y[sx + 1][sy + 1] = (0.2126 * r[sx + 1][sy + 1] + 0.7152 * g[sx + 1][sy + 1] + 0.0722 * b[sx + 1][sy + 1]);
                        }
                    }
                    d_edge = diagonal_edge(Y, wp);

                    if (d_edge <= 0)
                    {
                        rf = w3 * (r[0][3] + r[3][0]) + w4 * (r[1][2] + r[2][1]);
                        gf = w3 * (g[0][3] + g[3][0]) + w4 * (g[1][2] + g[2][1]);
                        bf = w3 * (b[0][3] + b[3][0]) + w4 * (b[1][2] + b[2][1]);
                        af = w3 * (a[0][3] + a[3][0]) + w4 * (a[1][2] + a[2][1]);
                    }
                    else
                    {
                        rf = w3 * (r[0][0] + r[3][3]) + w4 * (r[1][1] + r[2][2]);
                        gf = w3 * (g[0][0] + g[3][3]) + w4 * (g[1][1] + g[2][2]);
                        bf = w3 * (b[0][0] + b[3][3]) + w4 * (b[1][1] + b[2][2]);
                        af = w3 * (a[0][0] + a[3][3]) + w4 * (a[1][1] + a[2][2]);
                    }
                    // anti-ringing, clamp.
                    rf = clamp(rf, min_r_sample, max_r_sample);
                    gf = clamp(gf, min_g_sample, max_g_sample);
                    bf = clamp(bf, min_b_sample, max_b_sample);
                    af = clamp(af, min_a_sample, max_a_sample);
                    ri = (byte)clamp(Math.Ceiling(rf), 0, 255);
                    gi = (byte)clamp(Math.Ceiling(gf), 0, 255);
                    bi = (byte)clamp(Math.Ceiling(bf), 0, 255);
                    ai = (byte)clamp(Math.Ceiling(af), 0, 255);

                    result.Data[y + 1, x, 0] = bi;
                    result.Data[y + 1, x, 1] = gi;
                    result.Data[y + 1, x, 2] = ri;
                    result.Data[y + 1, x, 3] = ai;
                    ++x;
                }
                ++y;
            }
            #endregion

            CvInvoke.Imwrite("result2.png", result);

            #region Third Pass
            wp[0] = 2.0;
            wp[1] = 1.0;
            wp[2] = -1.0;
            wp[3] = 4.0;
            wp[4] = -1.0;
            wp[5] = 1.0;

            for (int y = result.Height - 1; y >= 0; --y)
            {
                for (int x = result.Width - 1; x >= 0; --x)
                {
                    for (int sx = -2; sx <= 1; ++sx)
                    {
                        for (int sy = -2; sy <= 1; ++sy)
                        {
                            // clamp pixel locations
                            int csy = (int)clamp(sy + y, 0, factor * image.Height - 1);
                            int csx = (int)clamp(sx + x, 0, factor * image.Width - 1);
                            // sample & add weighted components
                            byte bSample = result.Data[csy, csx, 0];
                            byte gSample = result.Data[csy, csx, 1];
                            byte rSample = result.Data[csy, csx, 2];
                            byte aSample = result.Data[csy, csx, 3];

                            r[sx + 2][sy + 2] = rSample;
                            g[sx + 2][sy + 2] = gSample;
                            b[sx + 2][sy + 2] = bSample;
                            a[sx + 2][sy + 2] = aSample;
                            Y[sx + 2][sy + 2] = (0.2126 * r[sx + 2][sy + 2] + 0.7152 * g[sx + 2][sy + 2] + 0.0722 * b[sx + 2][sy + 2]);
                        }
                    }

                    min_r_sample = getMin(r[1][1], r[2][1], r[1][2], r[2][2]);
                    min_g_sample = getMin(g[1][1], g[2][1], g[1][2], g[2][2]);
                    min_b_sample = getMin(b[1][1], b[2][1], b[1][2], b[2][2]);
                    min_a_sample = getMin(a[1][1], a[2][1], a[1][2], a[2][2]);
                    max_r_sample = getMax(r[1][1], r[2][1], r[1][2], r[2][2]);
                    max_g_sample = getMax(g[1][1], g[2][1], g[1][2], g[2][2]);
                    max_b_sample = getMax(b[1][1], b[2][1], b[1][2], b[2][2]);
                    max_a_sample = getMax(a[1][1], a[2][1], a[1][2], a[2][2]);
                    d_edge = diagonal_edge(Y, wp);
                    if (d_edge <= 0)
                    {
                        rf = w1 * (r[0][3] + r[3][0]) + w2 * (r[1][2] + r[2][1]);
                        gf = w1 * (g[0][3] + g[3][0]) + w2 * (g[1][2] + g[2][1]);
                        bf = w1 * (b[0][3] + b[3][0]) + w2 * (b[1][2] + b[2][1]);
                        af = w1 * (a[0][3] + a[3][0]) + w2 * (a[1][2] + a[2][1]);
                    }
                    else
                    {
                        rf = w1 * (r[0][0] + r[3][3]) + w2 * (r[1][1] + r[2][2]);
                        gf = w1 * (g[0][0] + g[3][3]) + w2 * (g[1][1] + g[2][2]);
                        bf = w1 * (b[0][0] + b[3][3]) + w2 * (b[1][1] + b[2][2]);
                        af = w1 * (a[0][0] + a[3][3]) + w2 * (a[1][1] + a[2][2]);
                    }

                    // anti-ringing, clamp.
                    rf = clamp(rf, min_r_sample, max_r_sample);
                    gf = clamp(gf, min_g_sample, max_g_sample);
                    bf = clamp(bf, min_b_sample, max_b_sample);
                    af = clamp(af, min_a_sample, max_a_sample);
                    ri = (byte)clamp(Math.Ceiling(rf), 0, 255);
                    gi = (byte)clamp(Math.Ceiling(gf), 0, 255);
                    bi = (byte)clamp(Math.Ceiling(bf), 0, 255);
                    ai = (byte)clamp(Math.Ceiling(af), 0, 255);

                    result.Data[y, x, 0] = bi;
                    result.Data[y, x, 1] = gi;
                    result.Data[y, x, 2] = ri;
                    result.Data[y, x, 3] = ai;
                }
            }
            #endregion

            return true;
        }
    }
}
