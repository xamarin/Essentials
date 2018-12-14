using System;
using System.Drawing;
using iOSRectangle = CoreGraphics.CGRect;

namespace Xamarin.Essentials
{
    public static class RectangleExtensions
    {
        public static Rectangle ToSystemRectangle(this iOSRectangle rect) =>
            new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        public static iOSRectangle ToPlatformRectangle(this Rectangle rect) =>
            new iOSRectangle((nfloat)rect.X, (nfloat)rect.Y, (nfloat)rect.Width, (nfloat)rect.Height);

        public static RectangleF ToSystemRectangleF(this iOSRectangle rect) =>
            new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);

        public static iOSRectangle ToPlatformRectangle(this RectangleF rect) =>
            new iOSRectangle((nfloat)rect.X, (nfloat)rect.Y, (nfloat)rect.Width, (nfloat)rect.Height);
    }
}
