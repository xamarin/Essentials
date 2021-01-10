using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials.SaveToGallery
{
    public static partial class SaveToGallery
    {
        static Task PlatformSaveImageAsync(byte[] data, string filename, string albumName)
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
