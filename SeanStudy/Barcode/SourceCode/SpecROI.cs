using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

using Corex.Core.Mini.PropertyGrid;
using Corex.Mini;
using Corex.Mini.PropertyGrid;
using Corex.Mini.DB;
using Corex.Util;

namespace Barcode.SourceCode
{
    public class SpecROI
    {
        // Coordinate system start from <Mode>
        public enum Mode : int
        {
            Center, LeftTop
        }

        public Mode Mode_ { get; protected set; }

        // Note: In Center mode, the value is offset to the center
        private PointD center { get { return new PointD(CenterX, CenterY); } }

        public double CenterX { get; set; }

        public double CenterY { get; set; }

        public Size Size { get { return new Size(Width, Height); } }

        public int Width { get; set; }

        public int Height { get; set; }

        public double Angle { get; set; }

        public SpecROI() { }


        public SpecROI Clone()
        {
            SpecROI specROI = new SpecROI();
            specROI.Mode_ = Mode_;
            specROI.CenterX = CenterX;
            specROI.CenterY = CenterY;
            specROI.Width = Width;
            specROI.Height = Height;
            specROI.Angle = Angle;
            return specROI;
        }

        // Note: Not support rotation
        public Rectangle GetROI(Size imageSize)
        {
            PointD corner = GetCorner(imageSize);
            return new Rectangle((int)corner.X, (int)corner.Y, Width, Height);
        }

        public PointD GetCorner(Size imageSize)
        {
            if (Mode_ == Mode.Center)
            {
                double cornerX = (imageSize.Width - Width) / 2.0;
                double cornerY = (imageSize.Height - Height) / 2.0;

                return new PointD(cornerX + CenterX, cornerY + CenterY);
            }
            else
            {
                return CenterToCorner(center, Size);
            }
        }

        public void Shift(PointD shift)
        {
            CenterX += shift.X;
            CenterY += shift.Y;
        }

        // Note: Suppose rCenter is defined according to the center mode.
        public void Rotate(PointD rCenter, double angle)
        {
            PointD newCenter = Geometry.RotateXY(center, rCenter, angle);
            CenterX = newCenter.X;
            CenterY = newCenter.Y;
            Angle += angle;
        }

        public static SpecROI NewCenterModeROI(int width, int height)
        {
            SpecROI roi = new SpecROI();
            roi.Mode_ = Mode.Center;
            roi.CenterX = 0;
            roi.CenterY = 0;
            roi.Width = width;
            roi.Height = height;
            return roi;
        }

        public static SpecROI NewLeftTopModeROI(Rectangle roi)
        {
            return NewLeftTopModeROI(roi.X, roi.Y, roi.Width, roi.Height);
        }

        public static SpecROI NewLeftTopModeROI(int x, int y, int width, int height)
        {
            SpecROI roi = new SpecROI();
            roi.Mode_ = Mode.LeftTop;
            PointD center = CornerToCenter(new PointD(x, y), new Size(width, height));
            roi.CenterX = center.X;
            roi.CenterY = center.Y;
            roi.Width = width;
            roi.Height = height;
            return roi;
        }

        // Corner: ROI's left top corner
        // Center: ROI's center
        public static PointD CenterToCorner(PointD center, Size size)
        {
            return new PointD(
                center.X - size.Width / 2.0 + 0.5,
                center.Y - size.Height / 2.0 + 0.5);
        }

        public static PointD CornerToCenter(PointD corner, Size size)
        {
            return new PointD(
                (corner.X + (corner.X + size.Width - 1)) / 2.0,
                (corner.Y + (corner.Y + size.Height - 1)) / 2.0);
        }

        // End points sequence
        // 1===2
        // =   =
        // 3===4
        public static void GetEndPoint(SpecROI roi, Size imageSize, double angle,
            out PointD p1, out PointD p2, out PointD p3, out PointD p4)
        {
            PointD corner = roi.GetCorner(imageSize);
            p1 = new PointD(corner);
            p2 = new PointD(corner.X + roi.Width - 1, corner.Y);
            p3 = new PointD(corner.X, corner.Y + roi.Height - 1);
            p4 = new PointD(corner.X + roi.Width - 1, corner.Y + roi.Height - 1);

            if (!double.IsNaN(angle) && angle != 0)
            {
                PointD center = CornerToCenter(corner, roi.Size);
                p1 = Geometry.RotateXY(p1, center, angle);
                p2 = Geometry.RotateXY(p2, center, angle);
                p3 = Geometry.RotateXY(p3, center, angle);
                p4 = Geometry.RotateXY(p4, center, angle);
            }
        }
    }
}

