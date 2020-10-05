using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Clipboard
    {
        static Task PlatformSetTextAsync(string text)
            => ThrowHelper.ThrowNotImplementedException<Task>();

        static bool PlatformHasText
            => ThrowHelper.ThrowNotImplementedException<bool>();

        static Task<string> PlatformGetTextAsync()
            => ThrowHelper.ThrowNotImplementedException<Task<string>>();

        static void StartClipboardListeners()
            => ThrowHelper.ThrowNotImplementedException();

        static void StopClipboardListeners()
            => ThrowHelper.ThrowNotImplementedException();
    }
}
