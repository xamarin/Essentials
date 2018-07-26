using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;

namespace Xamarin.Essentials.Types
{
    [ContentProvider(
        new[] { "${applicationId}.fileProvider" },
        Name = FileProviderJavaName,
        Exported = false,
        GrantUriPermissions = true)]
    [MetaData(
        "android.support.FILE_PROVIDER_PATHS",
        Resource = "@xml/xamarin_essentials_fileprovider_file_paths")]
    public class FileProvider : Android.Support.V4.Content.FileProvider
    {
        internal const string FileProviderJavaName = "xamarin.essentials.fileProvider";
    }
}
