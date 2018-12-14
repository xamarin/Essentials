using System.Drawing;
using AndroidColor = Android.Graphics.Color;

namespace Xamarin.Forms.Platform.Android
{
    public static class ColorExtensions
    {
        public static AndroidColor ToPlatformColor(this Color color) =>
            new AndroidColor(color.R, color.G, color.B, color.A);

        public static Color ToSystemColor(this AndroidColor color) =>
            Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
