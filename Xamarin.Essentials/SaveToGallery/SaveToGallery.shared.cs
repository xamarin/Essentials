using System;
using System.IO;
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
        public static Task SaveAsync(MediaFileType type, Stream fileStream, string fileName, string albumName)
        {
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));
            CheckParameters(albumName, fileName);

            return PlatformSaveAsync(type, fileStream, fileName, albumName);
        }

        public static Task SaveAsync(MediaFileType type, byte[] data, string fileName, string albumName)
        {
            if (data == null || !(data.Length > 0))
                throw new ArgumentNullException(nameof(data));
            CheckParameters(albumName, fileName);

            return PlatformSaveAsync(type, data, fileName, albumName);
        }

        public static Task SaveAsync(MediaFileType type, string filePath, string albumName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(nameof(filePath));
            CheckParameters(albumName);

            return PlatformSaveAsync(type, filePath, albumName);
        }

        static void CheckParameters(string albumName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(nameof(fileName));
            CheckParameters(albumName);
        }

        static void CheckParameters(string albumName)
        {
            if (string.IsNullOrWhiteSpace(albumName))
                throw new ArgumentException(nameof(albumName));
        }
    }
}
