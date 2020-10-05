using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class FileSystem
    {
        static string PlatformCacheDirectory
            => ThrowHelper.ThrowNotImplementedException<string>();

        static string PlatformAppDataDirectory
            => ThrowHelper.ThrowNotImplementedException<string>();

        static Task<Stream> PlatformOpenAppPackageFileAsync(string filename)
             => ThrowHelper.ThrowNotImplementedException<Task<Stream>>();
    }

    public partial class FileBase
    {
        static string PlatformGetContentType(string extension) =>
            ThrowHelper.ThrowNotImplementedException<string>();

        internal void PlatformInit(FileBase file) =>
            ThrowHelper.ThrowNotImplementedException();

        internal virtual Task<Stream> PlatformOpenReadAsync()
            => ThrowHelper.ThrowNotImplementedException<Task<Stream>>();
    }
}
