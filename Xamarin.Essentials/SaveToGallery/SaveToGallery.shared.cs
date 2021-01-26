using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public enum MediaFileType
    {
        Image,
        Video
    }

    public static partial class SaveToGallery
    {
        public static Task SaveAsync(MediaFileType type, byte[] data, string fileName, string albumName)
        {
            if (data == null || !(data.Length > 0))
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrWhiteSpace(albumName))
                throw new ArgumentNullException(nameof(albumName));

            return PlatformSaveAsync(type, data, fileName, albumName);
        }

        public static Task SaveAsync(MediaFileType type, string filePath, string albumName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (string.IsNullOrWhiteSpace(albumName))
                throw new ArgumentNullException(nameof(albumName));

            return PlatformSaveAsync(type, filePath, albumName);
        }
    }
}
