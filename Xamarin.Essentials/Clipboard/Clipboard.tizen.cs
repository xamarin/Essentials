using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Clipboard
    {
        static Task PlatformSetTextAsync(string text)
            => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");

        static bool PlatformHasText
            => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");

        static Task<string> PlatformGetTextAsync()
            => throw new PlatformNotSupportedException("This API is not currently supported on Tizen.");
    }
}
