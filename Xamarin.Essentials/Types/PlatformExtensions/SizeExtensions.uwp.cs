using System;
using System.Drawing;
using WindowsSize = Windows.Foundation.Size;

namespace Xamarin.Essentials
{
    public static class SizeExtensions
    {
        public static Size ToSystemSize(this WindowsSize size)
        {
            if (size.Width > int.MaxValue)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(size.Width));

            if (size.Height > int.MaxValue)
                ThrowHelper.ThrowIndexOutOfRangeException(nameof(size.Height));

            return new Size((int)size.Width, (int)size.Height);
        }

        public static SizeF ToSystemSizFe(this WindowsSize size) =>
            new SizeF((float)size.Width, (float)size.Height);

        public static WindowsSize ToPlatformSize(this Size size) =>
            new WindowsSize(size.Width, size.Height);

        public static WindowsSize ToPlatformSize(this SizeF size) =>
            new WindowsSize(size.Width, size.Height);
    }
}
