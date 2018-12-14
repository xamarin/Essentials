using System;
using System.Drawing;
using iOSSize = CoreGraphics.CGSize;

namespace Xamarin.Essentials
{
    public static class SizeExtensions
    {
        public static iOSSize ToPlatformSize(this Size size) =>
            new iOSSize((nfloat)size.Width, (nfloat)size.Height);

        public static Size ToSystemSize(this iOSSize size) =>
            new Size((int)size.Width, (int)size.Height);

        public static iOSSize ToPlatformSize(this SizeF size) =>
            new iOSSize((nfloat)size.Width, (nfloat)size.Height);

        public static SizeF ToSystemSizeF(this iOSSize size) =>
            new SizeF((float)size.Width, (float)size.Height);
    }
}
