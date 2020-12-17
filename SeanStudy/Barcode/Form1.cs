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
using ZXing;

namespace Barcode
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
                    var inputImage = new Image<Gray, byte>(ofd.FileName);
                    ZXing.BarcodeReader reader = new ZXing.BarcodeReader();
                    reader.Options.TryHarder = true;
                    reader.Options.PossibleFormats = new List<BarcodeFormat>();
                    reader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
                    ZXing.Result decodeResult = reader.Decode(inputImage.Bitmap);

                    int a = 0;

                }
            }

            

        }
    }
}
