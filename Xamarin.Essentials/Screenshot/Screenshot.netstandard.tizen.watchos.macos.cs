using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Screenshot
    {
        static bool PlatformIsCaptureSupported =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        static Task<ScreenshotResult> PlatformCaptureAsync() =>
            ThrowHelper.ThrowNotImplementedException<Task<ScreenshotResult>>();
    }

    public partial class ScreenshotResult
    {
        ScreenshotResult()
        {
        }

        internal Task<Stream> PlatformOpenReadAsync(ScreenshotFormat format) =>
            ThrowHelper.ThrowNotImplementedException<Task<Stream>>();
    }
}
