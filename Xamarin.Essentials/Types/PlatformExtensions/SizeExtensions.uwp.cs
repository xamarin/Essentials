using System.Drawing;
using WindowsSize = Windows.Foundation.Size;

namespace Xamarin.Essentials
{
    public static class SizeExtensions
    {
        public static WindowsSize ToWindowsSize(this Size size) =>
            new WindowsSize(size.Width, size.Height);

        public static Size ToSize(this WindowsSize size) =>
            new Size((int)size.Width, (int)size.Height);
    }
}
