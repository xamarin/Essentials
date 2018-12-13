using System.Drawing;
using AColor = Android.Graphics.Color;

namespace Xamarin.Forms.Platform.Android
{
    public static class ColorExtensions
    {
        public static AColor ToAndroidColor(this Color color) =>
            new AColor((byte)(byte.MaxValue * color.R), (byte)(byte.MaxValue * color.G), (byte)(byte.MaxValue * color.B), (byte)(byte.MaxValue * color.A));

        public static Color ToColor(this AColor color) =>
            Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}