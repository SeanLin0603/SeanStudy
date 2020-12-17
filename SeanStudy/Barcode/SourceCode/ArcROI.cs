using System;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;

using Corex.Mini.DB;
using Corex.Mini.PropertyGrid;
using System.ComponentModel;

namespace Barcode.SourceCode
{
    public class ArcROI
    {
        public enum InterplotMethods
        {
            Near,
            BiLinear
        }

        public InterplotMethods InterplotMethod { get; set; }
        public Point Center { get; set; }
        public double Radius { get; set; }
        public double Height { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }


        public ArcROI()
        {
            InterplotMethod = InterplotMethods.BiLinear;
            Center = new Point(0, 0);
            Radius = 20;
            Height = 5;
            StartAngle = 0;
            EndAngle = 180;
        }

        public static Image<Bgra, byte> GetImage(Image<Bgra, byte> image, ArcROI arcROI)
        {
            var startArc = Math.PI * arcROI.StartAngle / 180.0;
            var endArc = Math.PI * arcROI.EndAngle / 180.0;

            var width = (int)Math.Abs((arcROI.Radius * (endArc - startArc)));
            var height = (int)(arcROI.Height);
            var arcImg = new Image<Bgra, byte>(new Size(width, height));

            var innerRadius = arcROI.Radius - arcROI.Height / 2;
            var outerRadius = arcROI.Radius + arcROI.Height / 2;

            var stepArc = 1 / arcROI.Radius;
            if (arcROI.EndAngle < arcROI.StartAngle)
                stepArc *= -1;

            var nowArc = startArc;

            var imgWidth = image.Width;
            var imgHeight = image.Height;

            for (int i = 0; i < width; i++)
            {
                var stepX = Math.Cos(nowArc);
                var stepY = Math.Sin(nowArc);
                var nowRadius = outerRadius;

                var x = arcROI.Center.X + nowRadius * stepX;
                var y = arcROI.Center.Y + nowRadius * stepY;

                for (int j = 0; j < height; j++)
                {
                    int pixX = (int)x;
                    int pixY = (int)y;

                    if (pixX >= imgWidth - 1 || pixX < 0 || pixY >= imgHeight - 1 || pixY < 0)
                    {
                        arcImg.Data[j, i, 0] = 0;
                        arcImg.Data[j, i, 1] = 0;
                        arcImg.Data[j, i, 2] = 0;
                        arcImg.Data[j, i, 3] = 255;
                        //arcImg[j, i] = new Bgra(0, 0, 0, 255);
                    }
                    else
                    {
                        if (arcROI.InterplotMethod == InterplotMethods.Near)
                        {
                            arcImg.Data[j, i, 0] = image.Data[pixY, pixX, 0];
                            arcImg.Data[j, i, 1] = image.Data[pixY, pixX, 1];
                            arcImg.Data[j, i, 2] = image.Data[pixY, pixX, 2];
                            arcImg.Data[j, i, 3] = image.Data[pixY, pixX, 3];
                            //arcImg[j, i] = image[pixY, pixX];
                        }
                        else if (arcROI.InterplotMethod == InterplotMethods.BiLinear)
                        {
                            var ratioX = x - pixX;
                            var ratioY = y - pixY;

                            for (int c = 0; c < 4; c++)
                            {
                                double v1 = image.Data[pixY, pixX, c];
                                double v2 = image.Data[pixY, pixX + 1, c];
                                double v3 = image.Data[pixY + 1, pixX, c];
                                double v4 = image.Data[pixY + 1, pixX + 1, c];

                                double x1 = v1 + ratioX * (v2 - v1);
                                double x2 = v3 + ratioX * (v4 - v3);
                                double bl = x1 + ratioY * (x2 - x1);
                                arcImg.Data[j, i, c] = (byte)bl;
                            }
                        }
                    }

                    x -= stepX;
                    y -= stepY;
                }
                nowArc += stepArc;
            }

            return arcImg;
        }

        public static double[] GetProfileLine(Image<Gray, byte> image, ArcROI arcROI)
        {
            var startArc = Math.PI * arcROI.StartAngle / 180.0;
            var endArc = Math.PI * arcROI.EndAngle / 180.0;

            var width = (int)Math.Abs((arcROI.Radius * (endArc - startArc)));
            var height = (int)(arcROI.Height);
            double[] profileLine = new double[width];

            var innerRadius = arcROI.Radius - arcROI.Height / 2;
            var outerRadius = arcROI.Radius + arcROI.Height / 2;

            var stepArc = 1 / arcROI.Radius;
            if (arcROI.EndAngle < arcROI.StartAngle)
                stepArc *= -1;

            var nowArc = startArc;

            var imgWidth = image.Width;
            var imgHeight = image.Height;

            for (int i = 0; i < width; i++)
            {
                var stepX = Math.Cos(nowArc);
                var stepY = Math.Sin(nowArc);
                var nowRadius = outerRadius;

                var x = arcROI.Center.X + nowRadius * stepX;
                var y = arcROI.Center.Y + nowRadius * stepY;

                profileLine[i] = 0;
                for (int j = 0; j < height; j++)
                {
                    int pixX = (int)x;
                    int pixY = (int)y;

                    if (x >= imgWidth || x < 0 || y >= imgHeight || y < 0)
                    {

                    }
                    else
                    {
                        if (arcROI.InterplotMethod == InterplotMethods.Near)
                        {
                            profileLine[i] += image.Data[pixY, pixX, 0];
                        }
                        else if (arcROI.InterplotMethod == InterplotMethods.BiLinear)
                        {
                            var ratioX = x - pixX;
                            var ratioY = y - pixY;

                            double v1 = image.Data[pixY, pixX, 0];
                            double v2 = image.Data[pixY, pixX + 1, 0];
                            double v3 = image.Data[pixY + 1, pixX, 0];
                            double v4 = image.Data[pixY + 1, pixX + 1, 0];

                            double x1 = v1 + ratioX * (v2 - v1);
                            double x2 = v3 + ratioX * (v4 - v3);
                            double bl = x1 + ratioY * (x2 - x1);
                            profileLine[i] += (byte)bl;
                        }
                    }

                    x -= stepX;
                    y -= stepY;
                }

                profileLine[i] /= height;
                nowArc += stepArc;
            }

            return profileLine;
        }

        public static Point GetOriginPoint(Point arcPoint, ArcROI arcROI)
        {
            var startArc = Math.PI * arcROI.StartAngle / 180.0;

            var outerRadius = arcROI.Radius + arcROI.Height / 2;
            var stepArc = 1 / arcROI.Radius;
            if (arcROI.EndAngle < arcROI.StartAngle)
                stepArc *= -1;

            var dstArc = startArc + arcPoint.X * stepArc;

            var stepX = Math.Cos(dstArc);
            var stepY = Math.Sin(dstArc);

            var x = arcROI.Center.X + (outerRadius - arcPoint.Y) * stepX;
            var y = arcROI.Center.Y + (outerRadius - arcPoint.Y) * stepY;

            return new Point((int)x, (int)y);
        }

        public static Image<Bgra, byte> GetDrawImage(Image<Bgra, byte> image, ArcROI arcROI, MCvScalar color, int thickness = 1)
        {
            var drawImage = image.Clone();

            var innerRadius = arcROI.Radius - arcROI.Height / 2;
            var outerRadius = arcROI.Radius + arcROI.Height / 2;

            CvInvoke.Ellipse(drawImage, arcROI.Center, new Size((int)arcROI.Radius, (int)arcROI.Radius), 0, arcROI.StartAngle, arcROI.EndAngle, color, thickness);
            CvInvoke.Ellipse(drawImage, arcROI.Center, new Size((int)innerRadius, (int)innerRadius), 0, arcROI.StartAngle, arcROI.EndAngle, color, thickness);
            CvInvoke.Ellipse(drawImage, arcROI.Center, new Size((int)outerRadius, (int)outerRadius), 0, arcROI.StartAngle, arcROI.EndAngle, color, thickness);

            var startArc = Math.PI * arcROI.StartAngle / 180.0;
            var endArc = Math.PI * arcROI.EndAngle / 180.0;

            var startDownX = (int)(arcROI.Center.X + innerRadius * Math.Cos(startArc));
            var startDownY = (int)(arcROI.Center.Y + innerRadius * Math.Sin(startArc));
            var startTopX = (int)(arcROI.Center.X + outerRadius * Math.Cos(startArc));
            var startTopY = (int)(arcROI.Center.Y + outerRadius * Math.Sin(startArc));
            var endDownX = (int)(arcROI.Center.X + innerRadius * Math.Cos(endArc));
            var endDownY = (int)(arcROI.Center.Y + innerRadius * Math.Sin(endArc));
            var endTopX = (int)(arcROI.Center.X + outerRadius * Math.Cos(endArc));
            var endTopY = (int)(arcROI.Center.Y + outerRadius * Math.Sin(endArc));

            CvInvoke.Line(drawImage, new Point(startDownX, startDownY), new Point(startTopX, startTopY), color, thickness);
            CvInvoke.Line(drawImage, new Point(endDownX, endDownY), new Point(endTopX, endTopY), color, thickness);

            return drawImage;
        }
    }
}

