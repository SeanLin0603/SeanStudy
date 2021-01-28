using System;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Corex.VisionLib.Algorithm
{
    public class CameraCalib
    {
        public class Parameter
        {
            public Size BoardSize;
            public bool DrawOutput;

            public Parameter()
            { }
        }

        public class FindCornerResult
        {
            public VectorOfPointF FindCorners;
            public PointF[][] FoundRefined;
            public Image<Bgr, byte> ImageWithCorner;
        }

        public static bool FindCorner(Image<Bgr, byte> image, Parameter parameter, out FindCornerResult result)
        {
            result = new FindCornerResult();
            result.ImageWithCorner = image.Clone();

            Mat grayMat = image.Convert<Gray, byte>().Mat;

            VectorOfPointF findCorners = new VectorOfPointF();
            CvInvoke.FindChessboardCorners(grayMat, parameter.BoardSize, findCorners, CalibCbType.AdaptiveThresh);
            grayMat.Dispose();
            result.FindCorners = findCorners;

            // Check if find or not.
            int fcSize = findCorners.Size;
            if (fcSize == parameter.BoardSize.Width * parameter.BoardSize.Height)
            {
                // Refine position
                PointF[][] cornersPointF = new PointF[1][] { findCorners.ToArray() };
                result.ImageWithCorner.Convert<Gray, byte>().FindCornerSubPix(cornersPointF, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));
                result.FoundRefined = cornersPointF;

                // Draw on the image or not
                if (parameter.DrawOutput)
                {
                    CvInvoke.DrawChessboardCorners(result.ImageWithCorner.Mat, parameter.BoardSize, result.FindCorners, true);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public class GetMatrixResult
        {
            public Size BoardSize;
            public MCvPoint3D32f[][] idealpoints;
            //public IntrinsicCameraParameters Intrinsic;
            //public ExtrinsicCameraParameters[] Extrinsic;

            // New Version
            // internal parameter
            public Mat CameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
            public Mat DistCoeffs = new Mat(8, 1, DepthType.Cv64F, 1);
            // external parameter
            public Mat[] RotationVector, TranslationVector;


            public GetMatrixResult()
            {
                BoardSize = new Size(0, 0);
                idealpoints = new MCvPoint3D32f[1][];
            }
        }

        public static bool GetMatrix(Image<Bgr, byte> image, Parameter parameter, FindCornerResult findCornerResult, out GetMatrixResult result)
        {
            result = new GetMatrixResult();
            var board_size = parameter.BoardSize;
            var size = board_size.Height * board_size.Width;

            MCvPoint3D32f[][] idealpoints = new MCvPoint3D32f[1][];
            idealpoints[0] = new MCvPoint3D32f[size];
            for (int i = 0; i < board_size.Height; i++)
            {
                for (int j = 0; j < board_size.Width; j++)
                {
                    idealpoints[0][i * board_size.Width + j] = new MCvPoint3D32f(i, j, 0.0f);
                }
            }
            result.BoardSize = parameter.BoardSize;
            result.idealpoints = idealpoints;

            //var intrinsic = new IntrinsicCameraParameters();
            //PointF[][] cornersPointF = new PointF[1][] { findCornerResult.FindCorners.ToArray() };
            //ExtrinsicCameraParameters[] extrinsic;
            //CameraCalibration.CalibrateCamera(idealpoints,
            //    cornersPointF,
            //    new Size(image.Width, image.Height),
            //    intrinsic,
            //    CalibType.Default,
            //    new MCvTermCriteria(30, 0.1),
            //    out extrinsic);

            //result.Intrinsic = intrinsic;
            //result.Extrinsic = extrinsic;

            // New Version
            var error = CvInvoke.CalibrateCamera(idealpoints, findCornerResult.FoundRefined, image.Size,
                                    result.CameraMatrix, result.DistCoeffs, CalibType.RationalModel, new MCvTermCriteria(30, 0.1),
                                    out result.RotationVector, out result.TranslationVector);
            return true;
        }

        public static bool Undistortion(Image<Bgr, byte> image, GetMatrixResult parameter, out Image<Gray, byte> outImage)
        {
            Mat srcMat = image.Mat;
            Mat undistortedMat = srcMat.Clone();
            //CvInvoke.Undistort(srcMat, undistortedMat, parameter.Intrinsic.IntrinsicMatrix, parameter.Intrinsic.DistortionCoeffs);
            
            // New Version
            CvInvoke.Undistort(srcMat, undistortedMat, parameter.CameraMatrix, parameter.DistCoeffs);
            outImage = undistortedMat.ToImage<Gray, byte>();
            undistortedMat.Dispose();
            return true;
        }

        public class StatisticResult
        {
            public double TvDistortion;
            public double SumOffset;
            public double AveOffset;
            public int MaxOffsetIndex;
            public int MinOffsetIndex;
            public double MaxOffset;
            public double MinOffset;
        }

        public static bool Statistic(FindCornerResult parameter, GetMatrixResult getMatrixResult, out StatisticResult result)
        {
            result = new StatisticResult();
            var idealVector = getIdealVector(getMatrixResult);
            double[] distArray = new double[parameter.FindCorners.Size];
            int maxIndex = 0, minIndex = 0;
            double maxDist = distArray[0];
            double minDist = distArray[0];

            for (int i = 0; i < parameter.FindCorners.Size; i++)
            {
                distArray[i] = getDistance(parameter.FindCorners[i], idealVector[i]);
            }

            // get max and min
            for (int i = 0; i < distArray.Length; i++)
            {
                if (distArray[i] > maxDist)
                {
                    maxDist = distArray[i];
                    maxIndex = i;
                }
                if (distArray[i] < minDist)
                {
                    minDist = distArray[i];
                    minIndex = i;
                }
            }

            // save
            result.TvDistortion = getTVDistortion(parameter, idealVector);
            result.SumOffset = getTotalOffset(parameter.FindCorners, idealVector);
            result.AveOffset = Math.Round(result.SumOffset / parameter.FindCorners.Size, 5);
            result.MaxOffsetIndex = maxIndex;
            result.MinOffsetIndex = minIndex;
            result.MaxOffset = maxDist;
            result.MinOffset = minDist;
            return true;
        }

        private static VectorOfPointF getIdealVector(GetMatrixResult parameter)
        {
            //PointF[] idealpointf = new PointF[parameter.BoardSize.Width * parameter.BoardSize.Height];
            //idealpointf = CameraCalibration.ProjectPoints(parameter.idealpoints[0], parameter.Extrinsic[0], parameter.Intrinsic);
           
            // New Version
            PointF[] idealpointf = CvInvoke.ProjectPoints(parameter.idealpoints[0], parameter.RotationVector[0], parameter.TranslationVector[0],
                                                 parameter.CameraMatrix, parameter.DistCoeffs);
            return new VectorOfPointF(idealpointf);
        }

        private static double getTVDistortion(FindCornerResult parameter, VectorOfPointF idealvector)
        {
            int mid = 0;
            PointF midRealPoint, lastRealPoint;
            PointF midIdealPoint, lastIdealPoint;
            double realDist, idealDist, dtv;
            mid = parameter.FindCorners.Size / 2;

            midRealPoint = parameter.FindCorners[mid];
            lastRealPoint = parameter.FindCorners[parameter.FindCorners.Size - 1];
            midIdealPoint = idealvector[mid];
            lastIdealPoint = idealvector[idealvector.Size - 1];
            realDist = getDistance(midRealPoint, lastRealPoint);
            idealDist = getDistance(midIdealPoint, lastIdealPoint);

            dtv = Math.Abs(realDist - idealDist) / idealDist;
            dtv = Math.Round(dtv, 5) * 100;
            return dtv;
        }

        private static double getDistance(PointF ptf1, PointF ptf2)
        {
            double dist = 0;
            dist = Math.Sqrt(Math.Pow(Math.Abs(ptf1.X - ptf2.X), 2) + Math.Pow(Math.Abs(ptf1.Y - ptf2.Y), 2));
            dist = Math.Round(dist, 5);
            return dist;
        }

        private static double getTotalOffset(VectorOfPointF vpf1, VectorOfPointF vpf2)
        {
            double offsets = 0;
            for (int i = 0; i < vpf1.Size; i++)
            {
                offsets += getDistance(vpf1[i], vpf2[i]);
            }
            return offsets;
        }



        public static bool SaveMatrix(GetMatrixResult result, string filename)
        {
            try
            {
                var width = result.BoardSize.Width;
                var height = result.BoardSize.Height;
                var idealPts = result.idealpoints[0];
                var camaraMat = result.CameraMatrix;
                var distCoefMat = result.DistCoeffs;
                var rotaVector = result.RotationVector[0];
                var tranVector = result.TranslationVector[0];

                FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write);
                StreamWriter stream = new StreamWriter(file, System.Text.Encoding.UTF8);

                // BoardSize
                stream.Write(width);
                stream.Write(",");
                stream.WriteLine(height);

                // IdealPoints
                int size = width * height;
                for (int i = 0; i < size; i++)
                {
                    stream.Write(idealPts[i].X);
                    stream.Write(",");
                    stream.Write(idealPts[i].Y);
                    stream.Write(",");
                    stream.WriteLine(idealPts[i].Z);
                }

                // IntrinsicMatrix
                Array intrinsicMatrix = camaraMat.GetData();
                for (int i = 0; i < camaraMat.Rows; i++)
                {
                    if (i < camaraMat.Rows - 1)
                    {
                        stream.Write(intrinsicMatrix.GetValue(i, 0));
                        stream.Write(",");
                        stream.Write(intrinsicMatrix.GetValue(i, 1));
                        stream.Write(",");
                        stream.Write(intrinsicMatrix.GetValue(i, 2));
                        stream.Write(",");
                    }
                    else
                    {
                        stream.Write(intrinsicMatrix.GetValue(i, 0));
                        stream.Write(",");
                        stream.Write(intrinsicMatrix.GetValue(i, 1));
                        stream.Write(",");
                        stream.WriteLine(intrinsicMatrix.GetValue(i, 2));
                    }
                }

                // DistortionCoeffs
                Array intrinsicDistCoef = distCoefMat.GetData();
                for (int i = 0; i < distCoefMat.Rows; i++)
                {
                    if (i < distCoefMat.Rows - 1)
                    {
                        stream.Write(intrinsicDistCoef.GetValue(i, 0));
                        stream.Write(",");
                    }
                    else
                    {
                        stream.WriteLine(intrinsicDistCoef.GetValue(i, 0));
                    }
                }

                // rotationVector
                Array rVec = rotaVector.GetData();
                stream.Write(rVec.GetValue(0, 0));
                stream.Write(",");
                stream.Write(rVec.GetValue(1, 0));
                stream.Write(",");
                stream.WriteLine(rVec.GetValue(2, 0));

                // translationVector
                Array tVec = tranVector.GetData();
                stream.Write(tVec.GetValue(0, 0));
                stream.Write(",");
                stream.Write(tVec.GetValue(1, 0));
                stream.Write(",");
                stream.WriteLine(tVec.GetValue(2, 0));

                stream.Close();
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ReadMatrix(string filename, out GetMatrixResult result)
        {
            result = new GetMatrixResult();
            try
            {
                StreamReader stream = new StreamReader(filename);

                // BoardSize
                string strSize = stream.ReadLine();
                string[] wh = strSize.Split(',');
                int width = int.Parse(wh[0]);
                int height = int.Parse(wh[1]);
                result.BoardSize = new Size(width, height);

                // IdealPoints
                int size = width * height;
                result.idealpoints[0] = new MCvPoint3D32f[size];
                for (int i = 0; i < size; i++)
                {
                    string strPoint = stream.ReadLine();
                    string[] tmpXYZ = strPoint.Split(',');
                    int x = int.Parse(tmpXYZ[0]);
                    int y = int.Parse(tmpXYZ[1]);
                    int z = int.Parse(tmpXYZ[2]);
                    result.idealpoints[0][i] = new MCvPoint3D32f(x, y, z);
                }

                // IntrinsicMatrix
                string strMatrix = stream.ReadLine();
                string[] strMatVals = strMatrix.Split(',');
                double[,] tmpCamera = new double[3, 3];
                tmpCamera[0, 0] = double.Parse(strMatVals[0]);
                tmpCamera[0, 1] = double.Parse(strMatVals[1]);
                tmpCamera[0, 2] = double.Parse(strMatVals[2]);
                tmpCamera[1, 0] = double.Parse(strMatVals[3]);
                tmpCamera[1, 1] = double.Parse(strMatVals[4]);
                tmpCamera[1, 2] = double.Parse(strMatVals[5]);
                tmpCamera[2, 0] = double.Parse(strMatVals[6]);
                tmpCamera[2, 1] = double.Parse(strMatVals[7]);
                tmpCamera[2, 2] = double.Parse(strMatVals[8]);
                Matrix<double> tmpCameraMat = new Matrix<double>(tmpCamera);
                result.CameraMatrix = tmpCameraMat.Mat;

                // DistortionCoeffs
                string strDisCoef = stream.ReadLine();
                string[] strDisCoefVals = strDisCoef.Split(',');
                int length = strDisCoefVals.Length;
                double[,] tmpCoef = new double[length, 1];
                for (int i = 0; i < length; i++)
                {
                    tmpCoef[i, 0] = double.Parse(strDisCoefVals[i]);
                }
                Matrix<double> tmpCoefMat = new Matrix<double>(tmpCoef);
                result.DistCoeffs = tmpCoefMat.Mat;

                // rotationVector
                string strRotaVec = stream.ReadLine();
                string[] strRotaVecVals = strRotaVec.Split(',');
                double[,] rVector = new double[3, 1];
                rVector[0, 0] = double.Parse(strRotaVecVals[0]);
                rVector[1, 0] = double.Parse(strRotaVecVals[1]);
                rVector[2, 0] = double.Parse(strRotaVecVals[2]);
                Matrix<double> tmpRVecMat = new Matrix<double>(rVector);
                result.RotationVector = new Mat[1];
                result.RotationVector[0] = tmpRVecMat.Mat;

                // translationVector
                string strTransVec = stream.ReadLine();
                string[] strTransVecVals = strTransVec.Split(',');
                double[,] tVector = new double[3, 1];
                tVector[0, 0] = double.Parse(strTransVecVals[0]);
                tVector[1, 0] = double.Parse(strTransVecVals[1]);
                tVector[2, 0] = double.Parse(strTransVecVals[2]);
                Matrix<double> tmpTVecMat = new Matrix<double>(tVector);
                result.TranslationVector = new Mat[1];
                result.TranslationVector[0] = tmpTVecMat.Mat;

                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
