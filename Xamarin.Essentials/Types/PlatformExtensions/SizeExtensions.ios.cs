using System;
using System.Drawing;
using SizeF = CoreGraphics.CGSize;

namespace Xamarin.Essentials
{
    public static class SizeExtensions
    {
        public static SizeF ToSizeF(this Size size) =>
            new SizeF((nfloat)size.Width, (nfloat)size.Height);

        public static Size ToSize(this SizeF size) =>
            new Size((int)size.Width, (int)size.Height);
    }
}
