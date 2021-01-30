using System;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class SaveToGallery
    {
        static Task PlatformSaveAsync(MediaFileType type, byte[] data, string fileName, string albumName)
            => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task PlatformSaveAsync(MediaFileType type, string filePath, string albumName)
            => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task PlatformSaveAsync(MediaFileType type, Stream fileStream, string fileName, string albumName)
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
