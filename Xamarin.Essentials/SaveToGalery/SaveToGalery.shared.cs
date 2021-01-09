using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials.SaveToGalery
{
    public static partial class SaveToGalery
    {
        public static Task SaveImageAsync(byte[] data, string filename, string albumName = null)
            => PlatformSaveImageAsync(data, filename, albumName);
    }
}
