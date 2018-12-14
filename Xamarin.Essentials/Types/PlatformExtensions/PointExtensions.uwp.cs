using System.Drawing;
using WindowsPoint = Windows.Foundation.Point;

namespace Xamarin.Essentials
{
    public static class PointExtensions
    {
        public static Point ToSystemPoint(this WindowsPoint point) =>
            new Point((int)point.X, (int)point.Y);

        public static WindowsPoint ToPlatformPoint(this Point point) =>
            new WindowsPoint(point.X, point.Y);

        public static PointF ToSystemPointF(this WindowsPoint point) =>
            new PointF((float)point.X, (float)point.Y);

        public static WindowsPoint ToPlatformPoint(this PointF point) =>
            new WindowsPoint(point.X, point.Y);
    }
}
