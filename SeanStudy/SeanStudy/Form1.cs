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
                    Stopwatch sw = new Stopwatch();
                    var inputImage = new Image<Gray, byte>(ofd.FileName);
                    inputImage = inputImage.ThresholdBinary(new Gray(127), new Gray(255));

                    MeasureAngle.Parameter parameter = new MeasureAngle.Parameter();
                    parameter.ROI1 = new ROIOption(ROIOption.ROIType.SpecROI);
                    parameter.ROI1.SpecROI = SpecROI.NewLeftTopModeROI(55, 25, 200, 120);
                    parameter.ROI2 = new ROIOption(ROIOption.ROIType.SpecROI);
                    parameter.ROI2.SpecROI = SpecROI.NewLeftTopModeROI(255, 140, 135, 220);
                    parameter.AnglePoint = new Point(315, 75);

                    MeasureAngle.Result result = new MeasureAngle.Result(0);
                    sw.Start();
                    FindRowCol(inputImage, parameter, out result);
                    sw.Stop();

                    TimeSpan time = sw.Elapsed;
                    Console.WriteLine(time.TotalMilliseconds.ToString() + "ms");
                }
            }
        }

        public static bool FindRowCol(Image<Gray, byte> image, MeasureAngle.Parameter parameter, out MeasureAngle.Result result)
        {
            var img1 = ROIOption.GetImage(image, parameter.ROI1);
            var img2 = ROIOption.GetImage(image, parameter.ROI2);

            int row = horizontalProjection(img1);
            int col = verticalProjection(img2);

            List<Point> rowLinePts = new List<Point>();
            Point firstPt = new Point(0, row);
            Point lastPt = new Point(parameter.ROI1.SpecROI.Width - 1, row);
            firstPt = ROIOption.GetOriginPoint(image.Size, firstPt, parameter.ROI1);
            lastPt = ROIOption.GetOriginPoint(image.Size, lastPt, parameter.ROI1);
            rowLinePts.Add(firstPt);
            rowLinePts.Add(lastPt);


            List<Point> colLinePts = new List<Point>();
            firstPt = new Point(col, 0);
            lastPt = new Point(col, parameter.ROI2.SpecROI.Height - 1);
            firstPt = ROIOption.GetOriginPoint(image.Size, firstPt, parameter.ROI2);
            lastPt = ROIOption.GetOriginPoint(image.Size, lastPt, parameter.ROI2);
            colLinePts.Add(firstPt);
            colLinePts.Add(lastPt);

            result = new MeasureAngle.Result(0);
            if (MeasureAngle.Find(rowLinePts, colLinePts, parameter.AnglePoint, out result))
            {
                #region [DEBUG] Draw
                var imgBGRA = image.Convert<Bgra, byte>();
                // draw horizontal line
                LineSegment2D rowLine = new LineSegment2D(result.ROI1Points[0], result.ROI1Points[result.ROI1Points.Count - 1]);
                imgBGRA.Draw(rowLine, new Bgra(0, 0, 255, 255), 1);

                // draw vertical line
                LineSegment2D colLine = new LineSegment2D(result.ROI2Points[0], result.ROI2Points[result.ROI2Points.Count - 1]);
                imgBGRA.Draw(colLine, new Bgra(0, 255, 0, 255), 1);

                CvInvoke.PutText(imgBGRA, result.Angle.ToString(), parameter.AnglePoint, FontFace.HersheyComplex, 1, new MCvScalar(255, 0, 0, 255));
                CvInvoke.Imshow("imgBGRA", imgBGRA);
                #endregion

                return true;
            }
            return false;
        }

        private static int verticalProjection(Image<Gray, byte> image)
        {
            // vertical projection
            int width = image.Width;
            int height = image.Height;
            int maxCol = 0;

            // reduce to a single column by sum
            Mat mat = new Mat();
            CvInvoke.Reduce(image, mat, ReduceDimension.SingleRow, ReduceType.ReduceSum, DepthType.Cv32S);
            int[] projSums = new int[width];
            System.Runtime.InteropServices.Marshal.Copy(mat.DataPointer, projSums, 0, width);
            int minVal = projSums.Min();
            maxCol = projSums.ToList().IndexOf(minVal);

            return maxCol;
        }

        private static int horizontalProjection(Image<Gray, byte> image)
        {
            // horizontal projection
            int width = image.Width;
            int height = image.Height;
            int maxRow = 0;

            // reduce to a single column by sum
            Mat mat = new Mat();
            CvInvoke.Reduce(image, mat, ReduceDimension.SingleCol, ReduceType.ReduceSum, DepthType.Cv32S);
            int[] projSums = new int[height];
            System.Runtime.InteropServices.Marshal.Copy(mat.DataPointer, projSums, 0, height);
            int minVal = projSums.Min();
            maxRow = projSums.ToList().IndexOf(minVal);

            return maxRow;
        }

    }
}
