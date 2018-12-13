using System;
using System.Drawing;
using PointF = CoreGraphics.CGPoint;

namespace Xamarin.Essentials
{
    public static class PointExtensions
    {
        public static Point ToPoint(this PointF point) =>
            new Point((int)point.X, (int)point.Y);

        public static PointF ToPointF(this Point point) =>
            new PointF((nfloat)point.X, (nfloat)point.Y);
    }
}
