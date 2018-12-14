using System.Drawing;
using Windows.Foundation;

namespace Xamarin.Essentials
{
    public static class RectangleExtensions
    {
        public static Rectangle ToSystemRectangle(this Rect rect) =>
            new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        public static Rect ToPlatformRectangle(this Rectangle rect) =>
            new Rect(rect.X, rect.Y, rect.Width, rect.Height);

        public static RectangleF ToSystemRectangleF(this Rect rect) =>
            new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);

        public static Rect ToPlatformRectangle(this RectangleF rect) =>
            new Rect(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
