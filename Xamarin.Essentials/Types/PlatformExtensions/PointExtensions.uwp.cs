using System.Drawing;
using WindowsPoint = Windows.Foundation.Point;

namespace Xamarin.Essentials
{
    public static class PointExtensions
    {
        public static Point ToPoint(this WindowsPoint point) =>
            new Point((int)point.X, (int)point.Y);

        public static WindowsPoint ToWindowsPoint(this Point point) =>
            new WindowsPoint(point.X, point.Y);
    }
}
