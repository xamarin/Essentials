﻿using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Xamarin.Essentials
{
    public static partial class FileSystem
    {
        static string PlatformCacheDirectory
            => ApplicationData.Current.LocalCacheFolder.Path;

        static string PlatformAppDataDirectory
            => ApplicationData.Current.LocalFolder.Path;

        static Task<Stream> PlatformOpenAppPackageFileAsync(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            return Package.Current.InstalledLocation.OpenStreamForReadAsync(NormalizePath(filename));
        }

        internal static string NormalizePath(string path)
            => path.Replace('/', Path.DirectorySeparatorChar);
    }

    public partial class FileBase
    {
        internal FileBase(IStorageFile file)
            : this(file?.Path) => ContentType = file?.ContentType;

        // we can't do anything here, but Windows will take care of it
        internal static string PlatformGetContentType(string extension) => null;
    }
}
