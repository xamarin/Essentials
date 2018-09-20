using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;

namespace Xamarin.Essentials
{
    [ContentProvider(
        new[] { "${applicationId}.fileProvider" },
        Name = "xamarin.essentials.fileProvider",
        Exported = false,
        GrantUriPermissions = true)]
    [MetaData(
        "android.support.FILE_PROVIDER_PATHS",
        Resource = "@xml/xamarin_essentials_fileprovider_file_paths")]
    public class FileProvider : global::Android.Support.V4.Content.FileProvider
    {
        public static FileProviderLocation TemporaryLocation { get; set; }

        internal static Java.IO.File GetTemporaryDirectory()
        {
            var root = GetTemporaryRootDirectory();
            var dir = new Java.IO.File(root, "2203693cc04e0be7f4f024d5f9499e13");
            dir.Mkdirs();
            dir.DeleteOnExit();
            return dir;
        }

        internal static Java.IO.File GetTemporaryRootDirectory()
        {
            // we want the internal storage
            if (TemporaryLocation == FileProviderLocation.Internal)
                return Platform.AppContext.CacheDir;

            // KitKat no longer needs permission
            var requiresPermission = !Platform.HasApiLevel(BuildVersionCodes.Kitkat);

            // if we need permission, then check
            // if we can fall back, then don't throw
            var externalOnly = TemporaryLocation == FileProviderLocation.External;
            var hasPermission = !requiresPermission || Permissions.EnsureDeclared(PermissionType.ExternalStorage, externalOnly);

            // based on permssions, return the correct directory
            // if permission were required, then it would have already thrown
            return hasPermission ? Platform.AppContext.ExternalCacheDir : Platform.AppContext.CacheDir;
        }
    }

    public enum FileProviderLocation
    {
        PreferExternal,
        Internal,
        External,
    }
}
