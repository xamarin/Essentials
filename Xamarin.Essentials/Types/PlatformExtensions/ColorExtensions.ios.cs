using System.Drawing;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class ColorExtensions
    {
        public static Color ToSystemColor(this UIColor color)
        {
            color.GetRGBA(out var red, out var green, out var blue, out var alpha);
            return Color.FromArgb((int)alpha, (int)red, (int)green, (int)blue);
        }

        public static UIColor ToPlatformColor(this Color color) =>
            new UIColor((float)color.R, (float)color.G, (float)color.B, (float)color.A);
    }
}
