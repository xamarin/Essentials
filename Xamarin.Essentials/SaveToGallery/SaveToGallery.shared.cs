using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials.SaveToGallery
{
    public static partial class SaveToGallery
    {
        public static Task SaveImageAsync(byte[] data, string filename, string albumName = null)
            => PlatformSaveImageAsync(data, filename, albumName);
    }
}
