using System.Drawing;
using WindowsColor = Windows.UI.Color;

namespace Xamarin.Essentials
{
    public static class ColorExtensions
    {
        public static WindowsColor ToWindowsColor(this Color color) =>
               WindowsColor.FromArgb(color.A, color.R, color.G, color.B);

        public static Color ToColor(this WindowsColor color) =>
            Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
