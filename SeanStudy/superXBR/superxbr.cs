using System;
using System.Diagnostics;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Corex.VisionLib_410.Algorithm;

namespace superXBR
{
    public partial class xBRForm : Form
    {
        public xBRForm()
        {
            InitializeComponent();
        }

        Image<Bgra, byte> inputImage;

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    inputImage = new Image<Bgra, byte>(ofd.FileName);
                    picSrc.Image = inputImage.Bitmap;
                }
            }
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            int factor = int.Parse(txtScale.Text);
            Image<Bgra, byte> result;

            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            bool isOk = SuperxBR.SuperxbrScaling(inputImage, factor, out result);
            sw.Stop();
            string costTime = sw.Elapsed.TotalMilliseconds.ToString();

            if (isOk)
            {
                picDst.Image = result.Bitmap;
                lblCost.Text = "Cost time: " + costTime + " ms";
            }
            else
            {
                lblCost.Text = "Resize failed";
            }
        }
    }
}
