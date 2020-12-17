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

namespace ZXing
{
    public partial class Form1 : Form
    {
        List<float> angles = new List<float>();
        Image<Bgra, byte> SourceImg;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image|*.jpg;*.bmp;*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var inputImage = new Image<Bgra, byte>(ofd.FileName);
                    SourceImg = inputImage.Clone();
                    picSrc.Image = inputImage.ToBitmap();
                }
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            var img = new Image<Gray, byte>((Bitmap)picSrc.Image);
            CvInvoke.GaussianBlur(img, img, new Size(5, 5), 10);
            img = img.ThresholdBinary(new Gray(127), new Gray(255));
            var luminance = new BitmapLuminanceSource(img.Bitmap);

            var reader = new BarcodeReader();
            reader.Options.PossibleFormats = new List<BarcodeFormat>();
            reader.Options.PossibleFormats.Add(BarcodeFormat.DATA_MATRIX);
            Result result = reader.Decode(luminance);

            lblresult.Text = result.Text + "  via  " + result.BarcodeFormat.ToString();
            picSrc.Image = img.Bitmap;
        }


        private void btnFind_Click(object sender, EventArgs e)
        {
            angles.Clear();
            Console.Clear();
            Console.WriteLine("#,\tN,\tP,\tC,\tPa");
            var erodeImg = new Image<Gray, byte>(SourceImg.Bitmap);
            CvInvoke.GaussianBlur(erodeImg, erodeImg, new Size(5, 5), 10);
            erodeImg = erodeImg.ThresholdBinary(new Gray(127), new Gray(255));

            // 擴張
            //CvInvoke.Dilate(img, outImg, element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
            // 侵蝕
            Mat element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));
            CvInvoke.Erode(erodeImg, erodeImg, element, new Point(-1, -1), 10, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
            
            var contourImg = erodeImg.Convert<Bgr, byte>().Clone();
            VectorOfVectorOfPoint candidateContours = new VectorOfVectorOfPoint();
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                Mat hier = new Mat();
                CvInvoke.FindContours(erodeImg, contours, hier, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

                // find candidate contours
                for (int i = 0; i < contours.Size; i++)
                {
                    var arr = GetHierarchy(hier, i);
                    Console.Write("#" + i.ToString() + ",\t");
                    Console.Write(arr[0].ToString() + ",\t");
                    Console.Write(arr[1].ToString() + ",\t");
                    Console.Write(arr[2].ToString() + ",\t");
                    Console.WriteLine(arr[3].ToString());
                    
                    // eliminate contours which don't have child and parent
                    if (arr[2] != -1 && arr[3] != -1)
                    {
                        candidateContours.Push(contours[i]);
                    }
                }

                for (int i = 0; i < candidateContours.Size; i++)
                {
                    using (VectorOfPoint contour = candidateContours[i])
                    {
                        // MinAreaRect 是此版本找尋最小面積矩形的方法。
                        RotatedRect BoundingBox = CvInvoke.MinAreaRect(contour);
                        CvInvoke.Polylines(contourImg, Array.ConvertAll(BoundingBox.GetVertices(), Point.Round), true, new Bgr(Color.Red).MCvScalar, 3);
                        var cenPt = BoundingBox.Center;
                        angles.Add(BoundingBox.Angle);
                        CvInvoke.PutText(contourImg, i.ToString(), new Point((int)cenPt.X, (int)cenPt.Y), FontFace.HersheySimplex, 1.5, new MCvScalar(0, 0, 255), 2);
                    }
                }
            }
            Console.WriteLine("////////////////////////////////////////");
            if (candidateContours.Size != 0)
            {
                picSrc.Image = SourceImg.Rotate(angles[0] * (-1), new Bgra(0, 0, 0, 255)).Bitmap;
            }
            picDst.Image = contourImg.Bitmap;
        }

        private int[] GetHierarchy(Mat Hierarchy, int contourIdx)
        {
            int[] ret = new int[] { };

            if (Hierarchy.Depth != Emgu.CV.CvEnum.DepthType.Cv32S)
            {
                throw new ArgumentOutOfRangeException("ContourData must have Cv32S hierarchy element type.");
            }
            if (Hierarchy.Rows != 1)
            {
                throw new ArgumentOutOfRangeException("ContourData must have one hierarchy hierarchy row.");
            }
            if (Hierarchy.NumberOfChannels != 4)
            {
                throw new ArgumentOutOfRangeException("ContourData must have four hierarchy channels.");
            }
            if (Hierarchy.Dims != 2)
            {
                throw new ArgumentOutOfRangeException("ContourData must have two dimensional hierarchy.");
            }
            long elementStride = Hierarchy.ElementSize / sizeof(Int32);
            var offset0 = (long)0 + contourIdx * elementStride;
            if (0 <= offset0 && offset0 < Hierarchy.Total.ToInt64() * elementStride)
            {
                var offset1 = (long)1 + contourIdx * elementStride;
                var offset2 = (long)2 + contourIdx * elementStride;
                var offset3 = (long)3 + contourIdx * elementStride;

                ret = new int[4];
                unsafe
                {
                    //return *((Int32*)Hierarchy.DataPointer.ToPointer() + offset);

                    ret[0] = *((Int32*)Hierarchy.DataPointer.ToPointer() + offset0);
                    ret[1] = *((Int32*)Hierarchy.DataPointer.ToPointer() + offset1);
                    ret[2] = *((Int32*)Hierarchy.DataPointer.ToPointer() + offset2);
                    ret[3] = *((Int32*)Hierarchy.DataPointer.ToPointer() + offset3);
                }
            }
            //else
            //{
            //    return new int[] { };
            //}

            return ret;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            picSrc.Image = SourceImg.Bitmap;
        }
    }
}
