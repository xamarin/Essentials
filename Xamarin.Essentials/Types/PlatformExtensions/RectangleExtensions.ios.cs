using System;
using System.Drawing;
using RectangleF = CoreGraphics.CGRect;

namespace Xamarin.Essentials
{
    public static class RectangleExtensions
    {
        public static Rectangle ToRectangle(this RectangleF rect) =>
            new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        public static RectangleF ToRectangleF(this Rectangle rect) =>
            new RectangleF((nfloat)rect.X, (nfloat)rect.Y, (nfloat)rect.Width, (nfloat)rect.Height);
    }
}
