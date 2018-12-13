using System.Drawing;
using Windows.Foundation;

namespace Xamarin.Essentials
{
    public static class RectangleExtensions
    {
        public static Rectangle ToRectangle(this Rect rect) =>
            new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        public static Rect ToRect(this Rectangle rect) =>
            new Rect(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
