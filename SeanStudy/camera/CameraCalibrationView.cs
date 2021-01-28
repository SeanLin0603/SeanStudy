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
        private bool loadMat = false;

        public CameraCalibrationView()
        {
            InitializeComponent();

            loadMat = false;
            btnfindcorner.Enabled = false;
            btngetmatrix.Enabled = false;
            btnundistortion.Enabled = false;
            btnSaveMat.Enabled = false;
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
                    picSrc.Image = image.ToBitmap();
                    btnfindcorner.Enabled = true;
                    lblStatus.Text = "Status: Load image successfully.";
                }
            }
        }

        private void btnFindcorner_Click(object sender, EventArgs e)
        {
            param.BoardSize = new Size((int)setwidth.Value, (int)setheight.Value);
            param.DrawOutput = true;

            if (CameraCalib.FindCorner(image, param, out fcResult))
            {
                lbltotalpts.Text = fcResult.FindCorners.Size.ToString();
                btngetmatrix.Enabled = true;

                picDst.Image = fcResult.ImageWithCorner.ToBitmap();
                lblStatus.Text = "Status: Finding corner successfully.";
            }
            else
            {
                btngetmatrix.Enabled = false;
                MessageBox.Show("Corners searching failed!");
                lblStatus.Text = "Status: Finding corner failed.";
                picDst.Image = null;
            }          
        }

        private void btnGetmatrix_Click(object sender, EventArgs e)
        {
            if (CameraCalib.GetMatrix(image, param, fcResult, out gmResult))
            {
                displayMatrix(gmResult);
                btnundistortion.Enabled = true;
                btnSaveMat.Enabled = true;
                lblStatus.Text = "Status: Geting matrix successfully.";
            }
            else
            {
                lblStatus.Text = "Status: Geting matrix failed.";
            }
        }

        private void btnUndistortion_Click(object sender, EventArgs e)
        {
            if (CameraCalib.Undistortion(image, gmResult, out var udImage))
            {
                picDst.Image = udImage.ToBitmap();
            }

            // there is no need to do statistic, because it don't have fcResult
            if (!loadMat && CameraCalib.Statistic(fcResult, gmResult, out stResult))
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

        private void displayMatrix(CameraCalib.GetMatrixResult result)
        {
            #region EMGU3.0.0
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
            #endregion

            #region EMGU4.1.0
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
            #endregion
        }

        private void btnSaveMat_Click(object sender, EventArgs e)
        {
            if (CameraCalib.SaveMatrix(gmResult, "mat.txt"))
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
            loadMat = true;
            btnundistortion.Enabled = true;
            CameraCalib.GetMatrixResult tmpResult = new CameraCalib.GetMatrixResult();

            if (CameraCalib.ReadMatrix("mat.txt", out tmpResult))
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
