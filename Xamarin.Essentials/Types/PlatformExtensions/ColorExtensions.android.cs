using System.Drawing;
using AColor = Android.Graphics.Color;

namespace Xamarin.Forms.Platform.Android
{
    public static class ColorExtensions
    {
        public static AColor ToAndroidColor(this Color color) =>
            new AColor(color.R, color.G, color.B, color.A);

        public static Color ToColor(this AColor color) =>
            Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
