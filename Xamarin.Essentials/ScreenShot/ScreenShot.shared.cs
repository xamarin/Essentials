using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        public static Task<string> CaptureAsync(ScreenOutputType type = ScreenOutputType.PNG, string fileName = null) =>
            PlataformCaptureAsync(type, fileName);

        public static Task<byte[]> GetImageBytesAsync(ScreenOutputType type = ScreenOutputType.PNG) =>
            PlataformGetImageBytesAsync(type);
    }
}
