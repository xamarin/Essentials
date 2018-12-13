using System.Drawing;
using ASize = Android.Util.Size;

namespace Xamarin.Essentials
{
    public static class SizeExtensions
    {
        public static ASize ToAndroidSize(this Size size) =>
            new ASize(size.Width, size.Height);

        public static Size ToSize(this ASize size) =>
            new Size(size.Width, size.Height);
    }
}
