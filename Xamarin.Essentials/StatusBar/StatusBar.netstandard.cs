#if !NETSTANDARD1_0
using System.Drawing;

namespace Xamarin.Essentials
{
    public static partial class StatusBar
    {
        internal static void PlatformSetColor(Color color, StatusBarTint tint)
        {
        }
    }
}
#endif
