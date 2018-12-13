using System.Drawing;
using APoint = Android.Graphics.Point;

namespace Xamarin.Essentials
{
    public static class PointExtensions
    {
        public static Point ToPoint(this APoint point) =>
            new Point(point.X, point.Y);

        public static APoint ToAndroidPoint(this Point point) =>
            new APoint(point.X, point.Y);
    }
}
