#if !NETSTANDARD1_0
using System.Drawing;

namespace Xamarin.Essentials
{
    public static partial class StatusBar
    {
        public static void SetColor(Color color, StatusBarTint tint) =>
            PlatformSetColor(color, tint);
    }

    public enum StatusBarTint
    {
        Light = 0,
        Dark = 1
    }
}
#endif
