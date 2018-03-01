﻿using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;

namespace Microsoft.Caboodle
{
    public partial class FileSystem
    {
        private static string cache;
        private static string appData;
        private static string userData;

        public static string CacheDirectory
            => cache ?? (cache = Application.Context.CacheDir.AbsolutePath);

        public static string AppDataDirectory
            => appData ?? (appData = Application.Context.FilesDir.AbsolutePath);

        public static string UserDataDirectory
            => userData ?? (userData = Application.Context.FilesDir.AbsolutePath);

        public static Task<Stream> OpenAppPackageFileAsync(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            filename = filename.Replace("\\", "/");
            try
            {
                return Task.FromResult(Application.Context.Assets.Open(filename));
            }
            catch (Java.IO.FileNotFoundException ex)
            {
                throw new FileNotFoundException(ex.Message, filename, ex);
            }
        }
    }
}
