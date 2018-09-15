using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        static Task<string> PlataformCaptureAsync(ScreenOutputType type, string fileName = null) =>
            throw new NotImplementedInReferenceAssemblyException();

        static Task<byte[]> PlataformGetImageBytesAsync(ScreenOutputType type) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
