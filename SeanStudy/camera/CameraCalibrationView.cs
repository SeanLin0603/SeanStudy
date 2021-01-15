using System;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

using Corex.VisionLib.Algorithm;
using System.IO;

namespace camera
{
    public partial class CameraCalibrationView : Form
    {
        private Image<Bgr, byte> image;
        private CameraCalib.Parameter param = new CameraCalib.Parameter();
        private CameraCalib.FindCornerResult fcResult = new CameraCalib.FindCornerResult();
        private CameraCalib.GetMatrixResult gmResult = new CameraCalib.GetMatrixResult();
        private CameraCalib.StatisticResult stResult = new CameraCalib.StatisticResult();

        public CameraCalibrationView()
        {
            InitializeComponent();

            btnfindcorner.Enabled = false;
            btngetmatrix.Enabled = false;
            btnundistortion.Enabled = false;
        }

        private void btnbrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image|*.jpg;*.bmp;*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filepath.Text = ofd.FileName;
                    image = new Image<Bgr, byte>(ofd.FileName);
                    srcpic.Image = image.ToBitmap();
                    btnfindcorner.Enabled = true;
                }
            }
        }

        private void btnfindcorner_Click(object sender, EventArgs e)
        {
            param.BoardSize = new Size((int)setwidth.Value, (int)setheight.Value);
            param.DrawOutput = true;

            if (CameraCalib.FindCorner(image, param, out fcResult))
            {
                lbltotalpts.Text = fcResult.FindCorners.Size.ToString();
                btngetmatrix.Enabled = true;

                dstpic.Image = fcResult.ImageWithCorner.ToBitmap();
            }
            else
            {
                btngetmatrix.Enabled = false;
                MessageBox.Show("Corners searching failed!");
                dstpic.Image = null;
            }          
        }

        private void btngetmatrix_Click(object sender, EventArgs e)
        {
            if (CameraCalib.GetMatrix(image, param, fcResult, out gmResult))
            {
                displayMatrix(gmResult);



                btnundistortion.Enabled = true;
            }
        }

        private void btnundistortion_Click(object sender, EventArgs e)
        {
            if (CameraCalib.Undistortion(image, gmResult, out var udImage))
            {
                dstpic.Image = udImage.ToBitmap();
            }

            if (CameraCalib.Statistic(fcResult, gmResult, out stResult))
            {
                lblDTV.Text = stResult.TvDistortion.ToString() + "%";
                lblsumoffset.Text = stResult.SumOffset.ToString();
                lblaveoffset.Text = stResult.AveOffset.ToString();
                lblmaxindex.Text = "[" + stResult.MaxOffsetIndex.ToString() + "]";
                lblminindex.Text = "[" + stResult.MinOffsetIndex.ToString() + "]";
                lblmaxoffset.Text = stResult.MaxOffset.ToString();
                lblminoffset.Text = stResult.MinOffset.ToString();
            }
        }


        private bool saveAs(CameraCalib.GetMatrixResult result, string filename)
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

        private bool readMatrix(string filename, out CameraCalib.GetMatrixResult result)
        {
            result = new CameraCalib.GetMatrixResult();
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

        private void displayMatrix(CameraCalib.GetMatrixResult result)
        {
            //cameraMat00.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[0, 0], 1).ToString();
            //cameraMat01.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[0, 1], 1).ToString();
            //cameraMat02.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[0, 2], 1).ToString();
            //cameraMat10.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[1, 0], 1).ToString();
            //cameraMat11.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[1, 1], 1).ToString();
            //cameraMat12.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[1, 2], 1).ToString();
            //cameraMat20.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[2, 0], 1).ToString();
            //cameraMat21.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[2, 1], 1).ToString();
            //cameraMat22.Text = Math.Round(result.Intrinsic.IntrinsicMatrix[2, 2], 1).ToString();
            //coef0.Text = Math.Round(result.Intrinsic.DistortionCoeffs[0, 0], 6).ToString();
            //coef1.Text = Math.Round(result.Intrinsic.DistortionCoeffs[1, 0], 6).ToString();
            //coef2.Text = Math.Round(result.Intrinsic.DistortionCoeffs[2, 0], 6).ToString();
            //coef3.Text = Math.Round(result.Intrinsic.DistortionCoeffs[3, 0], 6).ToString();
            //coef4.Text = Math.Round(result.Intrinsic.DistortionCoeffs[4, 0], 6).ToString();

            // New Version

            var cameraMatrixData = gmResult.CameraMatrix.GetData();
            var distCoeffData = gmResult.DistCoeffs.GetData();
            cameraMat00.Text = Math.Round((double)cameraMatrixData.GetValue(0, 0), 1).ToString();
            cameraMat01.Text = Math.Round((double)cameraMatrixData.GetValue(0, 1), 1).ToString();
            cameraMat02.Text = Math.Round((double)cameraMatrixData.GetValue(0, 2), 1).ToString();
            cameraMat10.Text = Math.Round((double)cameraMatrixData.GetValue(1, 0), 1).ToString();
            cameraMat11.Text = Math.Round((double)cameraMatrixData.GetValue(1, 1), 1).ToString();
            cameraMat12.Text = Math.Round((double)cameraMatrixData.GetValue(1, 2), 1).ToString();
            cameraMat20.Text = Math.Round((double)cameraMatrixData.GetValue(2, 0), 1).ToString();
            cameraMat21.Text = Math.Round((double)cameraMatrixData.GetValue(2, 1), 1).ToString();
            cameraMat22.Text = Math.Round((double)cameraMatrixData.GetValue(2, 2), 1).ToString();
            coef0.Text = Math.Round((double)distCoeffData.GetValue(0, 0), 6).ToString();
            coef1.Text = Math.Round((double)distCoeffData.GetValue(1, 0), 6).ToString();
            coef2.Text = Math.Round((double)distCoeffData.GetValue(2, 0), 6).ToString();
            coef3.Text = Math.Round((double)distCoeffData.GetValue(3, 0), 6).ToString();
            coef4.Text = Math.Round((double)distCoeffData.GetValue(4, 0), 6).ToString();
        }


        private void btnSaveMat_Click(object sender, EventArgs e)
        {
            if (saveAs(gmResult, "mat.txt"))
            {
                lblStatus.Text = "Status: Save matrix successfully.";
            }
            else
            {
                lblStatus.Text = "Status: Save matrix failed.";
            }
        }

        private void btnLoadMat_Click(object sender, EventArgs e)
        {
            btnundistortion.Enabled = true;
            CameraCalib.GetMatrixResult tmpResult = new CameraCalib.GetMatrixResult();

            if (readMatrix("mat.txt", out tmpResult))
            {
                gmResult = tmpResult;
                displayMatrix(gmResult);
                lblStatus.Text = "Status: Load matrix successfully.";
            }
            else
            {
                lblStatus.Text = "Status: Load matrix failed.";
            }

        }
    }
}
