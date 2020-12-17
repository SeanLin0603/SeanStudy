using Corex.Mini;
using Corex.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace Barcode.SourceCode
{
    public class RoiOption
    {
        public enum Types
        {
            NoROI,
            SpecROI,
            ArcROI
        }

        private Types _Type;
        public Types Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                // Set SpecROI browsable.
                {
                    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(GetType())["SpecROI_"];
                    BrowsableAttribute attrib = (BrowsableAttribute)descriptor.Attributes[typeof(BrowsableAttribute)];
                    FieldInfo isBrow = attrib.GetType().GetField("browsable", BindingFlags.NonPublic | BindingFlags.Instance);
                    isBrow.SetValue(attrib, _Type == Types.SpecROI);
                }
                // Set ArcROI browsable.
                {
                    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(GetType())["ArcROI_"];
                    BrowsableAttribute attrib = (BrowsableAttribute)descriptor.Attributes[typeof(BrowsableAttribute)];
                    FieldInfo isBrow = attrib.GetType().GetField("browsable", BindingFlags.NonPublic | BindingFlags.Instance);
                    isBrow.SetValue(attrib, _Type == Types.ArcROI);
                }
            }
        }

        public SpecROI SpecROI_ { get; set; }

        public ArcROI ArcROI_ { get; set; }

        public RoiOption()
        {
            _Type = Types.NoROI;
            SpecROI_ = SpecROI.NewLeftTopModeROI(0, 0, 10, 10);
            ArcROI_ = new ArcROI();
        }

        public static Image<Bgra, byte> GetImage(Image<Bgra, byte> image, RoiOption roi)
        {
            if (roi.Type == Types.SpecROI)
            {
                return Barcode.SourceCode.Tool.GetImage(image, roi.SpecROI_);
            }
            else if (roi.Type == Types.ArcROI)
            {
                return ArcROI.GetImage(image, roi.ArcROI_);
            }
            else
                return image.Clone();
        }

        public static Point GetOriginPoint(Size imgSize, Point point, RoiOption roi)
        {
            if (roi.Type == Types.SpecROI)
            {
                PointD pd = Barcode.SourceCode.Tool.GetOriginPoint(roi.SpecROI_, imgSize, roi.SpecROI_.Angle, new PointD(point.X, point.Y));
                return new Point((int)pd.X, (int)pd.Y);
            }
            else if (roi.Type == Types.ArcROI)
            {
                return ArcROI.GetOriginPoint(point, roi.ArcROI_);
            }
            else
                return point;
        }

        public static Image<Bgra, byte> DrawROI(Image<Bgra, byte> image, RoiOption roi, Bgra color, int thickness = 1)
        {
            if (roi.Type == Types.SpecROI)
            {
                return Barcode.SourceCode.Tool.GetDrawImage(image, roi.SpecROI_, color, thickness);
            }
            else if (roi.Type == Types.ArcROI)
            {
                return ArcROI.GetDrawImage(image, roi.ArcROI_, color.MCvScalar, thickness);
            }
            else
                return image.Clone();
        }
    }

    class Tool
    {
        public static PointD RotatePoint(PointD center, PointD point, double angle)
        {
            double dx = point.X - center.X;
            double dy = point.Y - center.Y;

            double rdx = dx * Math.Cos(angle * Math.PI / 180) - dy * Math.Sin(angle * Math.PI / 180);
            double rdy = dx * Math.Sin(angle * Math.PI / 180) + dy * Math.Cos(angle * Math.PI / 180);

            return new PointD(center.X + rdx, center.Y + rdy);
        }

        private static void updateMinMax(PointD point, ref double xMin, ref double xMax, ref double yMin, ref double yMax)
        {
            if (point.X < xMin || double.IsNaN(xMin))
                xMin = point.X;
            if (point.X > xMax || double.IsNaN(xMax))
                xMax = point.X;
            if (point.Y < yMin || double.IsNaN(yMin))
                yMin = point.Y;
            if (point.Y > yMax || double.IsNaN(yMax))
                yMax = point.Y;
        }

        public static Image<Bgra, byte> GetImage(Image<Bgra, byte> image, SpecROI specROI)
        {
            // Note: Use _image instead of using image.
            // TODO: Prevent set ROI crash issue usage, implement crop image func without using image.ROI.
            Image<Bgra, byte> _image = new Image<Bgra, byte>(image.ToBitmap());

            // Init crop range
            double xMax = double.NaN;
            double xMin = double.NaN;
            double yMax = double.NaN;
            double yMin = double.NaN;

            // Consider roi range
            PointD p1_1, p2_1, p3_1, p4_1;
            SpecROI.GetEndPoint(specROI, _image.Size, 0, out p1_1, out p2_1, out p3_1, out p4_1);
            updateMinMax(p1_1, ref xMin, ref xMax, ref yMin, ref yMax);
            updateMinMax(p2_1, ref xMin, ref xMax, ref yMin, ref yMax);
            updateMinMax(p3_1, ref xMin, ref xMax, ref yMin, ref yMax);
            updateMinMax(p4_1, ref xMin, ref xMax, ref yMin, ref yMax);

            // Consider rotated roi range
            PointD p1_2, p2_2, p3_2, p4_2;
            SpecROI.GetEndPoint(specROI, _image.Size, specROI.Angle, out p1_2, out p2_2, out p3_2, out p4_2);
            updateMinMax(p1_2, ref xMin, ref xMax, ref yMin, ref yMax);
            updateMinMax(p2_2, ref xMin, ref xMax, ref yMin, ref yMax);
            updateMinMax(p3_2, ref xMin, ref xMax, ref yMin, ref yMax);
            updateMinMax(p4_2, ref xMin, ref xMax, ref yMin, ref yMax);

            Rectangle srcROI = new Rectangle(
                (int)xMin, (int)yMin,
                (int)(xMax - xMin + 1), (int)(yMax - yMin + 1));
            _image.ROI = srcROI;
            Rectangle dstROI = new Rectangle(0, 0, _image.ROI.Width, _image.ROI.Height);

            if (srcROI.X < 0)
                dstROI.X = -srcROI.X;
            if (srcROI.Y < 0)
                dstROI.Y = -srcROI.Y;

            Image<Bgra, byte> srcImg = new Image<Bgra, byte>(srcROI.Size);
            srcImg.ROI = dstROI;
            CvInvoke.cvCopy(_image, srcImg, IntPtr.Zero);
            srcImg.ROI = Rectangle.Empty;

            PointD cropCenter = SpecROI.CornerToCenter(specROI.GetCorner(_image.Size), specROI.Size);
            cropCenter.X -= xMin;
            cropCenter.Y -= yMin;

            srcImg = srcImg.Rotate(-specROI.Angle,
                new PointF((float)cropCenter.X, (float)cropCenter.Y),
                Emgu.CV.CvEnum.Inter.Linear, new Bgra(), true);

            PointD corner = SpecROI.CenterToCorner(cropCenter, specROI.Size);

            srcImg.ROI = new Rectangle((int)corner.X, (int)corner.Y, specROI.Width, specROI.Height);
            Image<Bgra, byte> dstImage = srcImg.Copy();

            return dstImage;
        }

        public static PointD GetOriginPoint(SpecROI roi, Size imageSize, double angle, PointD p)
        {
            PointD corner = roi.GetCorner(imageSize);
            PointD oriP = new PointD(corner.X + p.X, corner.Y + p.Y);

            if (!double.IsNaN(angle) && angle != 0)
            {
                PointD center = new PointD(roi.CenterX, roi.CenterY);
                oriP = Geometry.RotateXY(oriP, center, angle);
            }

            return oriP;
        }

        public static Image<Bgra, byte> GetDrawImage(Image<Bgra, byte> image, SpecROI roi, Bgra color, int thickness = 1)
        {
            var drawImage = image.Clone();

            SpecROI.GetEndPoint(roi, image.Size, roi.Angle, out PointD p1, out PointD p2, out PointD p3, out PointD p4);
            Point[] ps = new Point[4];
            ps[0] = new Point((int)p1.X, (int)p1.Y);
            ps[1] = new Point((int)p2.X, (int)p2.Y);
            ps[2] = new Point((int)p4.X, (int)p4.Y);
            ps[3] = new Point((int)p3.X, (int)p3.Y);
            drawImage.Draw(ps, color, thickness);

            return drawImage;
        }
    }
}
