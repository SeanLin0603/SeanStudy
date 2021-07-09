using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using static subpixel.sub;
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

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var inputImage = new Image<Gray, byte>(ofd.FileName);
                    picSrc.Image = inputImage.Bitmap;
                    DoSubPix(inputImage);
                }
            }
        }

        public bool DoSubPix(Image<Gray, byte> image)
        {
            int scale = 11;

            double sigma = double.Parse(txtSigma.Text);
            double high = double.Parse(txtCannyH.Text);
            double low = double.Parse(txtCannyL.Text);

            PointF noSubPixOfCenter = new PointF();
            PointF subPixOfCenter = new PointF();
            Parameter parameter = new Parameter(sigma, high, low);
            Result result = new Result();

            SubPixel(image, parameter, out result);
            Image<Gray, byte> srcImage = image.Resize(scale, Inter.Nearest);
            picSrc.Image = srcImage.Bitmap;
            Image<Bgra, byte> drawImage = image.Resize(scale, Inter.Nearest).Convert<Bgra, byte>();
            
            // find the biggest contour
            int maxIdx = -1;
            int maxSize = -1;
            for (int i = 0; i < result.SubContours.Capacity; i++)
            {
                int size = result.SubContours[i].points.Length;
                if (size > maxSize)
                {
                    maxIdx = i;
                    maxSize = size;
                }
            }

            VectorOfPoint contour = result.FoundContours[maxIdx];
            for (int i = 0; i < contour.Size; i++)
            {
                Point pt = contour[i];
                noSubPixOfCenter.X += pt.X;
                noSubPixOfCenter.Y += pt.Y;

                // draw in center
                int r = pt.Y * scale + scale / 2;
                int c = pt.X * scale + scale / 2;

                // green means contour points
                drawImage.Data[r, c, 0] = 0;
                drawImage.Data[r, c, 1] = 255;
                drawImage.Data[r, c, 2] = 0;
            }

            noSubPixOfCenter.X = noSubPixOfCenter.X / contour.Size * scale;
            noSubPixOfCenter.Y = noSubPixOfCenter.Y / contour.Size * scale;
            // cyan means average point
            drawImage.Data[(int)noSubPixOfCenter.Y, (int)noSubPixOfCenter.X, 0] = 255;
            drawImage.Data[(int)noSubPixOfCenter.Y, (int)noSubPixOfCenter.X, 1] = 255;
            drawImage.Data[(int)noSubPixOfCenter.Y, (int)noSubPixOfCenter.X, 2] = 0;
            CvInvoke.Imwrite("withoutSubPix.png", drawImage);

            PointF[] subPixContour = result.SubContours[maxIdx].points;
            for (int i = 0; i < subPixContour.Length; i++)
            {
                PointF pt = subPixContour[i];
                subPixOfCenter.X += pt.X;
                subPixOfCenter.Y += pt.Y;

                // draw in center
                int r = (int)(pt.Y * scale + scale / 2);
                int c = (int)(pt.X * scale + scale / 2);

                // red means subpix points
                drawImage.Data[r, c, 0] = 0;
                drawImage.Data[r, c, 1] = 0;
                drawImage.Data[r, c, 2] = 255;
            }

            subPixOfCenter.X = subPixOfCenter.X / contour.Size * scale;
            subPixOfCenter.Y = subPixOfCenter.Y / contour.Size * scale;
            // yellow means average point
            drawImage.Data[(int)subPixOfCenter.Y, (int)subPixOfCenter.X, 0] = 0;
            drawImage.Data[(int)subPixOfCenter.Y, (int)subPixOfCenter.X, 1] = 255;
            drawImage.Data[(int)subPixOfCenter.Y, (int)subPixOfCenter.X, 2] = 255;
            CvInvoke.Imwrite("withSubPix.png", drawImage);
            //CvInvoke.Imshow("drawImg", drawImage);

            picDst.Image = drawImage.Bitmap;
            PointF diff = new PointF(subPixOfCenter.X- noSubPixOfCenter.X, subPixOfCenter.Y - noSubPixOfCenter.Y);
            lblSrcCenter.Text = "Center point of FindContour: " + noSubPixOfCenter.ToString();
            lblDstCenter.Text = "Center point of SubPix: " + subPixOfCenter.ToString();
            lblDiff.Text = "Difference: " + diff.ToString();

            return true;
        }

        private void SubPixel(Image<Gray, byte> image, Parameter parameter, out Result result)
        {
            throw new NotImplementedException();
        }

        // ******************************algorithm******************************************


    }
}
