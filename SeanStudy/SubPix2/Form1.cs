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

namespace SubPix2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Parameter
        {
            public double Sigma;

            public double Threshold_H;

            public double Threshold_L;

            public Parameter()
            {
                Sigma = 1.5;
                Threshold_H = 4.2;
                Threshold_L = 0.81;
            }

            public Parameter(double sigma, double high, double low)
            {
                Sigma = sigma;
                Threshold_H = high;
                Threshold_L = low;
            }
        }

        public class Result
        {
            public VectorOfVectorOfPointF Points;

            public Result()
            {
                Points = new VectorOfVectorOfPointF();
            }
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var inputImage = new Image<Gray, byte>(ofd.FileName);
                    picSrc.Image = inputImage.Bitmap;
                }
            }
        }

        public bool Devernay(Image<Gray, byte> image, Parameter parameter, out Result result)
        {
            result = new Result();

            if (parameter.Sigma == 0)
            {
                // compute gradient
            }
            else
            {
                Size kernel = new Size(3, 3);
                CvInvoke.GaussianBlur(image, image, kernel, parameter.Sigma);
                // compute gradient

            }



            //if (sigma == 0.0) compute_gradient(Gx, Gy, modG, image, X, Y);
            //else
            //{
            //    gaussian_filter(image, gauss, X, Y, sigma);
            //    compute_gradient(Gx, Gy, modG, gauss, X, Y);
            //}

            //compute_edge_points(Ex, Ey, modG, Gx, Gy, X, Y);

            //chain_edge_points(next, prev, Ex, Ey, Gx, Gy, X, Y);

            //thresholds_with_hysteresis(next, prev, modG, X, Y, th_h, th_l);

            //list_chained_edge_points(x, y, N, curve_limits, M, next, prev, Ex, Ey, X, Y);




            return true;
        }
    }
}
