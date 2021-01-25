using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class SaveToGallery
    {
        public static Task SaveImageAsync(byte[] data, string filename, string albumName = null)
            => PlatformSaveImageAsync(data, filename, albumName);

        public static Task SaveVideoAsync(byte[] data, string filename, string albumName = null)
            => PlatformSaveVideoAsync(data, filename, albumName);
    }
}
