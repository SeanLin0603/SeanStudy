﻿using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Corex.VisionLib_410.Algorithm
{
    public class SuperxBR
    {
        private static double df(double a, double b)
        {
            return Math.Abs(a - b);
        }

        private static int getMin(int a, int b, int c, int d)
        {
            //int minVal = int.MaxValue;
            //if (a < minVal) minVal = a;
            //if (b < minVal) minVal = b;
            //if (c < minVal) minVal = c;
            //if (d < minVal) minVal = d;

            int minVal = a;
            if (b < a) minVal = b;
            if (c < minVal) minVal = c;
            if (d < minVal) minVal = d;
            return minVal;
        }

        private static int getMax(int a, int b, int c, int d)
        {
            //int maxVal = int.MinValue;
            //if (a > maxVal) maxVal = a;
            //if (b > maxVal) maxVal = b;
            //if (c > maxVal) maxVal = c;
            //if (d > maxVal) maxVal = d;

            int maxVal = a;
            if (b > a) maxVal = a;
            if (c > maxVal) maxVal = c;
            if (d > maxVal) maxVal = d;
            return maxVal;
        }

        private static int[][] fourByfourIntMat()
        {
            int[][] array = new int[4][];
            array[0] = new int[4];
            array[1] = new int[4];
            array[2] = new int[4];
            array[3] = new int[4];
            return array;
        }

        private static double[][] fourByfourDoubleMat()
        {
            double[][] array = new double[4][];
            array[0] = new double[4];
            array[1] = new double[4];
            array[2] = new double[4];
            array[3] = new double[4];
            return array;
        }

        private static double diagonal_edge(double[][] mat1, double[] wp)
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

        public static bool SuperxbrScaling(Image<Bgra, byte> image, int factor, out Image<Bgra, byte> result)
        {
            byte minByte = 0;
            byte maxByte = 255;
            int smallW = image.Width;
            int smallH = image.Height;
            int bigW = smallW * factor;
            int bigH = smallH * factor;

            result = new Image<Bgra, byte>(bigW, bigH);

            // weight
            double wgt1 = 0.129633;
            double wgt2 = 0.129633;
            double w1 = -wgt1;
            double w2 = wgt1 + 0.5;
            double w3 = -wgt2;
            double w4 = wgt2 + 0.5;

            // initialization
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

            byte[,,] imageData = image.Data;
            byte[,,] resultData = result.Data;

            double[] rWeight = new double[256];
            double[] gWeight = new double[256];
            double[] bWeight = new double[256];

            for (int i = 0; i < 256; i++)
            {
                rWeight[i] = 0.2126 * i;
                gWeight[i] = 0.7152 * i;
                bWeight[i] = 0.0722 * i;
            }

            #region First Pass
            double[] wp = new double[] { 2.0, 1.0, -1.0, 4.0, -1.0, 1.0 };
            for (int y = 0; y < bigH; ++y)
            {
                for (int x = 0; x < bigW; ++x)
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
                            int csy = sy + cy;
                            int csx = sx + cx;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > smallH - 1) ? smallH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > smallW - 1) ? smallW - 1 : csx;

                            // sample & add weighted components
                            byte bSample = imageData[csy, csx, 0];
                            byte gSample = imageData[csy, csx, 1];
                            byte rSample = imageData[csy, csx, 2];
                            byte aSample = imageData[csy, csx, 3];

                            r[sx + 1][sy + 1] = rSample;
                            g[sx + 1][sy + 1] = gSample;
                            b[sx + 1][sy + 1] = bSample;
                            a[sx + 1][sy + 1] = aSample;
                            Y[sx + 1][sy + 1] = rWeight[rSample] + gWeight[gSample] + bWeight[bSample];
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
                    rf = (rf < min_r_sample) ? min_r_sample : rf;
                    rf = (rf > max_r_sample) ? max_r_sample : rf;
                    gf = (gf < min_g_sample) ? min_g_sample : gf;
                    gf = (gf > max_g_sample) ? max_g_sample : gf;
                    bf = (bf < min_b_sample) ? min_b_sample : bf;
                    bf = (bf > max_b_sample) ? max_b_sample : bf;
                    af = (af < min_a_sample) ? min_a_sample : af;
                    af = (af > max_a_sample) ? max_a_sample : af;

                    ri = (byte)((rf - (byte)rf == 0) ? rf : rf + 1);
                    gi = (byte)((gf - (byte)gf == 0) ? gf : gf + 1);
                    bi = (byte)((bf - (byte)bf == 0) ? bf : bf + 1);
                    ai = (byte)((af - (byte)af == 0) ? af : af + 1);

                    ri = (ri < minByte) ? minByte : ri;
                    ri = (ri > maxByte) ? maxByte : ri;
                    gi = (gi < minByte) ? minByte : gi;
                    gi = (gi > maxByte) ? maxByte : gi;
                    bi = (bi < minByte) ? minByte : bi;
                    bi = (bi > maxByte) ? maxByte : bi;
                    ai = (ai < minByte) ? minByte : ai;
                    ai = (ai > maxByte) ? maxByte : ai;

                    resultData[y, x, 0] = resultData[y, x + 1, 0] = resultData[y + 1, x, 0] = imageData[cy, cx, 0];
                    resultData[y, x, 1] = resultData[y, x + 1, 1] = resultData[y + 1, x, 1] = imageData[cy, cx, 1];
                    resultData[y, x, 2] = resultData[y, x + 1, 2] = resultData[y + 1, x, 2] = imageData[cy, cx, 2];
                    resultData[y, x, 3] = resultData[y, x + 1, 3] = resultData[y + 1, x, 3] = imageData[cy, cx, 3];
                    resultData[y + 1, x + 1, 0] = bi;
                    resultData[y + 1, x + 1, 1] = gi;
                    resultData[y + 1, x + 1, 2] = ri;
                    resultData[y + 1, x + 1, 3] = ai;
                    ++x;
                }
                ++y;
            }
            #endregion

            #region Second Pass
            wp[0] = 2.0;
            wp[1] = 0.0;
            wp[2] = 0.0;
            wp[3] = 0.0;
            wp[4] = 0.0;
            wp[5] = 0.0;

            for (int y = 0; y < bigH; ++y)
            {
                for (int x = 0; x < bigW; ++x)
                {
                    // sample supporting pixels in original image
                    for (int sx = -1; sx <= 2; ++sx)
                    {
                        for (int sy = -1; sy <= 2; ++sy)
                        {
                            // clamp pixel locations
                            int csy = sx - sy + y;
                            int csx = sx + sy + x;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > bigH - 1) ? bigH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > bigW - 1) ? bigW - 1 : csx;

                            // sample & add weighted components
                            byte bSample = resultData[csy, csx, 0];
                            byte gSample = resultData[csy, csx, 1];
                            byte rSample = resultData[csy, csx, 2];
                            byte aSample = resultData[csy, csx, 3];

                            r[sx + 1][sy + 1] = rSample;
                            g[sx + 1][sy + 1] = gSample;
                            b[sx + 1][sy + 1] = bSample;
                            a[sx + 1][sy + 1] = aSample;
                            Y[sx + 1][sy + 1] = rWeight[rSample] + gWeight[gSample] + bWeight[bSample];
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
                    rf = (rf < min_r_sample) ? min_r_sample : rf;
                    rf = (rf > max_r_sample) ? max_r_sample : rf;
                    gf = (gf < min_g_sample) ? min_g_sample : gf;
                    gf = (gf > max_g_sample) ? max_g_sample : gf;
                    bf = (bf < min_b_sample) ? min_b_sample : bf;
                    bf = (bf > max_b_sample) ? max_b_sample : bf;
                    af = (af < min_a_sample) ? min_a_sample : af;
                    af = (af > max_a_sample) ? max_a_sample : af;

                    ri = (byte)((rf - (byte)rf == 0) ? rf : rf + 1);
                    gi = (byte)((gf - (byte)gf == 0) ? gf : gf + 1);
                    bi = (byte)((bf - (byte)bf == 0) ? bf : bf + 1);
                    ai = (byte)((af - (byte)af == 0) ? af : af + 1);

                    ri = (ri < minByte) ? minByte : ri;
                    ri = (ri > maxByte) ? maxByte : ri;
                    gi = (gi < minByte) ? minByte : gi;
                    gi = (gi > maxByte) ? maxByte : gi;
                    bi = (bi < minByte) ? minByte : bi;
                    bi = (bi > maxByte) ? maxByte : bi;
                    ai = (ai < minByte) ? minByte : ai;
                    ai = (ai > maxByte) ? maxByte : ai;

                    resultData[y, x + 1, 0] = bi;
                    resultData[y, x + 1, 1] = gi;
                    resultData[y, x + 1, 2] = ri;
                    resultData[y, x + 1, 3] = ai;

                    for (int sx = -1; sx <= 2; ++sx)
                    {
                        for (int sy = -1; sy <= 2; ++sy)
                        {
                            // clamp pixel locations
                            int csy = sx - sy + 1 + y;
                            int csx = sx + sy - 1 + x;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > bigH - 1) ? bigH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > bigW - 1) ? bigW - 1 : csx;

                            // sample & add weighted components
                            byte bSample = resultData[csy, csx, 0];
                            byte gSample = resultData[csy, csx, 1];
                            byte rSample = resultData[csy, csx, 2];
                            byte aSample = resultData[csy, csx, 3];

                            r[sx + 1][sy + 1] = rSample;
                            g[sx + 1][sy + 1] = gSample;
                            b[sx + 1][sy + 1] = bSample;
                            a[sx + 1][sy + 1] = aSample;
                            Y[sx + 1][sy + 1] = rWeight[rSample] + gWeight[gSample] + bWeight[bSample];
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
                    rf = (rf < min_r_sample) ? min_r_sample : rf;
                    rf = (rf > max_r_sample) ? max_r_sample : rf;
                    gf = (gf < min_g_sample) ? min_g_sample : gf;
                    gf = (gf > max_g_sample) ? max_g_sample : gf;
                    bf = (bf < min_b_sample) ? min_b_sample : bf;
                    bf = (bf > max_b_sample) ? max_b_sample : bf;
                    af = (af < min_a_sample) ? min_a_sample : af;
                    af = (af > max_a_sample) ? max_a_sample : af;

                    ri = (byte)((rf - (byte)rf == 0) ? rf : rf + 1);
                    gi = (byte)((gf - (byte)gf == 0) ? gf : gf + 1);
                    bi = (byte)((bf - (byte)bf == 0) ? bf : bf + 1);
                    ai = (byte)((af - (byte)af == 0) ? af : af + 1);

                    ri = (ri < minByte) ? minByte : ri;
                    ri = (ri > maxByte) ? maxByte : ri;
                    gi = (gi < minByte) ? minByte : gi;
                    gi = (gi > maxByte) ? maxByte : gi;
                    bi = (bi < minByte) ? minByte : bi;
                    bi = (bi > maxByte) ? maxByte : bi;
                    ai = (ai < minByte) ? minByte : ai;
                    ai = (ai > maxByte) ? maxByte : ai;

                    resultData[y + 1, x, 0] = bi;
                    resultData[y + 1, x, 1] = gi;
                    resultData[y + 1, x, 2] = ri;
                    resultData[y + 1, x, 3] = ai;
                    ++x;
                }
                ++y;
            }
            #endregion

            #region Third Pass
            wp[0] = 2.0;
            wp[1] = 1.0;
            wp[2] = -1.0;
            wp[3] = 4.0;
            wp[4] = -1.0;
            wp[5] = 1.0;

            for (int y = bigH - 1; y >= 0; --y)
            {
                for (int x = bigW - 1; x >= 0; --x)
                {
                    for (int sx = -2; sx <= 1; ++sx)
                    {
                        for (int sy = -2; sy <= 1; ++sy)
                        {
                            // clamp pixel locations
                            int csy = sy + y;
                            int csx = sx + x;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > bigH - 1) ? bigH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > bigW - 1) ? bigW - 1 : csx;


                            // sample & add weighted components
                            byte bSample = resultData[csy, csx, 0];
                            byte gSample = resultData[csy, csx, 1];
                            byte rSample = resultData[csy, csx, 2];
                            byte aSample = resultData[csy, csx, 3];

                            r[sx + 2][sy + 2] = rSample;
                            g[sx + 2][sy + 2] = gSample;
                            b[sx + 2][sy + 2] = bSample;
                            a[sx + 2][sy + 2] = aSample;
                            Y[sx + 2][sy + 2] = rWeight[rSample] + gWeight[gSample] + bWeight[bSample];
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
                    rf = (rf < min_r_sample) ? min_r_sample : rf;
                    rf = (rf > max_r_sample) ? max_r_sample : rf;
                    gf = (gf < min_g_sample) ? min_g_sample : gf;
                    gf = (gf > max_g_sample) ? max_g_sample : gf;
                    bf = (bf < min_b_sample) ? min_b_sample : bf;
                    bf = (bf > max_b_sample) ? max_b_sample : bf;
                    af = (af < min_a_sample) ? min_a_sample : af;
                    af = (af > max_a_sample) ? max_a_sample : af;

                    ri = (byte)((rf - (byte)rf == 0) ? rf : rf + 1);
                    gi = (byte)((gf - (byte)gf == 0) ? gf : gf + 1);
                    bi = (byte)((bf - (byte)bf == 0) ? bf : bf + 1);
                    ai = (byte)((af - (byte)af == 0) ? af : af + 1);

                    ri = (ri < minByte) ? minByte : ri;
                    ri = (ri > maxByte) ? maxByte : ri;
                    gi = (gi < minByte) ? minByte : gi;
                    gi = (gi > maxByte) ? maxByte : gi;
                    bi = (bi < minByte) ? minByte : bi;
                    bi = (bi > maxByte) ? maxByte : bi;
                    ai = (ai < minByte) ? minByte : ai;
                    ai = (ai > maxByte) ? maxByte : ai;

                    resultData[y, x, 0] = bi;
                    resultData[y, x, 1] = gi;
                    resultData[y, x, 2] = ri;
                    resultData[y, x, 3] = ai;
                }
            }
            #endregion
            result.Data = resultData;

            //Console.WriteLine("preprocessing-xBR takes :" + preprocessing + "ms");
            //Console.WriteLine("firstPass takes :" + firstPass + "ms");
            //Console.WriteLine("secondPass takes :" + secondPass + "ms");
            //Console.WriteLine("thirdPass takes :" + thirdPass + "ms");

            return true;
        }

        public static bool SuperxbrScalingGray(Image<Gray, byte> image, int factor, out Image<Gray, byte> result)
        {
            byte minByte = 0;
            byte maxByte = 255;

            double wgt1 = 0.129633;
            double wgt2 = 0.129633;
            double w1 = -wgt1;
            double w2 = wgt1 + 0.5;
            double w3 = -wgt2;
            double w4 = wgt2 + 0.5;

            int smallW = image.Width;
            int smallH = image.Height;
            int bigW = smallW * factor;
            int bigH = smallH * factor;
            result = new Image<Gray, byte>(bigW, bigH);

            int[][] pixel = fourByfourIntMat();
            double[][] Y = fourByfourDoubleMat();

            double pixelF, pixelMin, pixelMax;
            byte pixelByte;
            double d_edge;

            byte[,,] imageData = image.Data;
            byte[,,] resultData = result.Data;


            #region First Pass
            double[] wp = new double[] { 2.0, 1.0, -1.0, 4.0, -1.0, 1.0 };
            for (int y = 0; y < bigH; ++y)
            {
                for (int x = 0; x < bigW; ++x)
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
                            int csy = sy + cy;
                            int csx = sx + cx;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > smallH - 1) ? smallH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > smallW - 1) ? smallW - 1 : csx;

                            // sample & add weighted components
                            byte sample = imageData[csy, csx, 0];

                            pixel[sx + 1][sy + 1] = sample;
                            Y[sx + 1][sy + 1] = sample;
                        }
                    }

                    pixelMin = getMin(pixel[1][1], pixel[2][1], pixel[1][2], pixel[2][2]);
                    pixelMax = getMax(pixel[1][1], pixel[2][1], pixel[1][2], pixel[2][2]);
                    d_edge = diagonal_edge(Y, wp);

                    if (d_edge <= 0)
                    {
                        pixelF = w1 * (pixel[0][3] + pixel[3][0]) + w2 * (pixel[1][2] + pixel[2][1]);
                    }
                    else
                    {
                        pixelF = w1 * (pixel[0][0] + pixel[3][3]) + w2 * (pixel[1][1] + pixel[2][2]);
                    }
                    // anti-ringing, clamp.
                    pixelF = (pixelF < pixelMin) ? pixelMin : pixelF;
                    pixelF = (pixelF > pixelMax) ? pixelMax : pixelF;

                    pixelByte = (byte)((pixelF - (byte)pixelF == 0) ? pixelF : pixelF + 1);

                    pixelByte = (pixelByte < minByte) ? minByte : pixelByte;
                    pixelByte = (pixelByte > maxByte) ? maxByte : pixelByte;

                    resultData[y, x, 0] = resultData[y, x + 1, 0] = resultData[y + 1, x, 0] = imageData[cy, cx, 0];
                    resultData[y + 1, x + 1, 0] = pixelByte;
                    ++x;
                }
                ++y;
            }
            #endregion

            #region Second Pass
            wp[0] = 2.0;
            wp[1] = 0.0;
            wp[2] = 0.0;
            wp[3] = 0.0;
            wp[4] = 0.0;
            wp[5] = 0.0;

            for (int y = 0; y < bigH; ++y)
            {
                for (int x = 0; x < bigW; ++x)
                {
                    // sample supporting pixels in original image
                    for (int sx = -1; sx <= 2; ++sx)
                    {
                        for (int sy = -1; sy <= 2; ++sy)
                        {
                            // clamp pixel locations
                            int csy = sx - sy + y;
                            int csx = sx + sy + x;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > bigH - 1) ? bigH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > bigW - 1) ? bigW - 1 : csx;

                            // sample & add weighted components
                            byte sample = resultData[csy, csx, 0];

                            pixel[sx + 1][sy + 1] = sample;
                            Y[sx + 1][sy + 1] = sample;
                        }
                    }

                    pixelMin = getMin(pixel[1][1], pixel[2][1], pixel[1][2], pixel[2][2]);
                    pixelMax = getMax(pixel[1][1], pixel[2][1], pixel[1][2], pixel[2][2]);
                    d_edge = diagonal_edge(Y, wp);

                    if (d_edge <= 0)
                    {
                        pixelF = w3 * (pixel[0][3] + pixel[3][0]) + w4 * (pixel[1][2] + pixel[2][1]);
                    }
                    else
                    {
                        pixelF = w3 * (pixel[0][0] + pixel[3][3]) + w4 * (pixel[1][1] + pixel[2][2]);
                    }

                    // anti-ringing, clamp.
                    pixelF = (pixelF < pixelMin) ? pixelMin : pixelF;
                    pixelF = (pixelF > pixelMax) ? pixelMax : pixelF;

                    pixelByte = (byte)((pixelF - (byte)pixelF == 0) ? pixelF : pixelF + 1);
                    pixelByte = (pixelByte < minByte) ? minByte : pixelByte;
                    pixelByte = (pixelByte > maxByte) ? maxByte : pixelByte;
                    resultData[y, x + 1, 0] = pixelByte;

                    for (int sx = -1; sx <= 2; ++sx)
                    {
                        for (int sy = -1; sy <= 2; ++sy)
                        {
                            // clamp pixel locations
                            int csy = sx - sy + 1 + y;
                            int csx = sx + sy - 1 + x;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > bigH - 1) ? bigH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > bigW - 1) ? bigW - 1 : csx;

                            // sample & add weighted components
                            byte sample = resultData[csy, csx, 0];

                            pixel[sx + 1][sy + 1] = sample;
                            Y[sx + 1][sy + 1] = sample;
                        }
                    }
                    d_edge = diagonal_edge(Y, wp);

                    if (d_edge <= 0)
                    {
                        pixelF = w3 * (pixel[0][3] + pixel[3][0]) + w4 * (pixel[1][2] + pixel[2][1]);
                    }
                    else
                    {
                        pixelF = w3 * (pixel[0][0] + pixel[3][3]) + w4 * (pixel[1][1] + pixel[2][2]);
                    }
                    // anti-ringing, clamp.
                    pixelF = (pixelF < pixelMin) ? pixelMin : pixelF;
                    pixelF = (pixelF > pixelMax) ? pixelMax : pixelF;

                    pixelByte = (byte)((pixelF - (byte)pixelF == 0) ? pixelF : pixelF + 1);
                    pixelByte = (pixelByte < minByte) ? minByte : pixelByte;
                    pixelByte = (pixelByte > maxByte) ? maxByte : pixelByte;

                    resultData[y + 1, x, 0] = pixelByte;
                    ++x;
                }
                ++y;
            }
            #endregion

            #region Third Pass
            wp[0] = 2.0;
            wp[1] = 1.0;
            wp[2] = -1.0;
            wp[3] = 4.0;
            wp[4] = -1.0;
            wp[5] = 1.0;

            for (int y = bigH - 1; y >= 0; --y)
            {
                for (int x = bigW - 1; x >= 0; --x)
                {
                    for (int sx = -2; sx <= 1; ++sx)
                    {
                        for (int sy = -2; sy <= 1; ++sy)
                        {
                            // clamp pixel locations
                            int csy = sy + y;
                            int csx = sx + x;
                            csy = (csy < 0) ? 0 : csy;
                            csy = (csy > bigH - 1) ? bigH - 1 : csy;
                            csx = (csx < 0) ? 0 : csx;
                            csx = (csx > bigW - 1) ? bigW - 1 : csx;

                            // sample & add weighted components
                            byte sample = resultData[csy, csx, 0];

                            pixel[sx + 2][sy + 2] = sample;
                            Y[sx + 2][sy + 2] = sample;
                        }
                    }

                    pixelMin = getMin(pixel[1][1], pixel[2][1], pixel[1][2], pixel[2][2]);
                    pixelMax = getMax(pixel[1][1], pixel[2][1], pixel[1][2], pixel[2][2]);
                    d_edge = diagonal_edge(Y, wp);

                    if (d_edge <= 0)
                    {
                        pixelF = w1 * (pixel[0][3] + pixel[3][0]) + w2 * (pixel[1][2] + pixel[2][1]);
                    }
                    else
                    {
                        pixelF = w1 * (pixel[0][0] + pixel[3][3]) + w2 * (pixel[1][1] + pixel[2][2]);
                    }

                    // anti-ringing, clamp.
                    pixelF = (pixelF < pixelMin) ? pixelMin : pixelF;
                    pixelF = (pixelF > pixelMax) ? pixelMax : pixelF;

                    pixelByte = (byte)((pixelF - (byte)pixelF == 0) ? pixelF : pixelF + 1);
                    pixelByte = (pixelByte < minByte) ? minByte : pixelByte;
                    pixelByte = (pixelByte > maxByte) ? maxByte : pixelByte;

                    resultData[y, x, 0] = pixelByte;
                }
            }
            #endregion
            result.Data = resultData;

            return true;
        }

    }
}
