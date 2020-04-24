using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;

namespace Xamarin.Essentials
{
    public static partial class FilePicker
    {
        const int requestCodeFilePicker = 12345;

        static async Task<Intent> CreateIntent(PickOptions options, bool multipleSelect, bool save)
        {
            // Save takes precedence over multi-select

            if (multipleSelect && !save && (int)global::Android.OS.Build.VERSION.SdkInt < 18)
                throw new FeatureNotSupportedException("Picking multiple files is only available on API level 18 (Android 4.3) or later.");

            // we only need the permission when accessing the file, but it's more natural
            // to ask the user first, then show the picker.
            if (save)
                await Permissions.RequestAsync<Permissions.StorageWrite>();
            else
                await Permissions.RequestAsync<Permissions.StorageRead>();

            Intent intent;
            if (save)
                intent = new Intent(Intent.ActionCreateDocument);
            else if ((int)global::Android.OS.Build.VERSION.SdkInt < BuildVersionCodes.Kitkat)
                intent = new Intent(Intent.ActionGetContent);
            else
                intent = new Intent(Intent.ActionOpenDocument);

            intent.SetType("*/*");

            if (Build.VERSION.SdkInt < BuildVersionCodes.Kitkat)
                intent.AddCategory(Intent.CategoryOpenable);
            else
                intent.AddFlags(ActivityFlags.GrantPersistableUriPermission);

            if (multipleSelect && !save)
                intent.PutExtra(Intent.ExtraAllowMultiple, true);
            else if (save && !string.IsNullOrEmpty(options.SuggestedFileName))
                intent.PutExtra(Intent.ExtraTitle, options.SuggestedFileName);

            var allowedTypes = options?.FileTypes?.Value?.ToArray();
            if (allowedTypes?.Length > 0)
                intent.PutExtra(Intent.ExtraMimeTypes, allowedTypes);

            return Intent.CreateChooser(intent, options?.PickerTitle ?? (multipleSelect && !save ? "Select files" : "Select file"));
        }

        static async Task<FilePickerResult> PlatformPickFileAsync(PickOptions options)
        {
            var pickerIntent = await CreateIntent(options, false, false);

            try
            {
                var result = await IntermediateActivity.StartAsync(pickerIntent, requestCodeFilePicker);
                var contentUri = result.Data;

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                {
                    Platform.AppContext.ContentResolver.TakePersistableUriPermission(
                        contentUri,
                        ActivityFlags.GrantReadUriPermission);
                }

                return new FilePickerResult(contentUri);
            }
            catch (System.OperationCanceledException)
            {
                return null;
            }
        }

        static async Task<FilePickerResult> PlatformPickFileToSaveAsync(PickOptions options)
        {
            var pickerIntent = await CreateIntent(options, false, true);

            try
            {
                var result = await IntermediateActivity.StartAsync(pickerIntent, requestCodeFilePicker);
                var contentUri = result.Data;
                return new FilePickerResult(contentUri);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        static async Task<IEnumerable<FilePickerResult>> PlatformPickMultipleFilesAsync(PickOptions options)
        {
            var pickerIntent = await CreateIntent(options, true, false);

            try
            {
                var result = await IntermediateActivity.StartAsync(pickerIntent, requestCodeFilePicker);

                var resultList = new List<FilePickerResult>();

                if (result.ClipData == null)
                {
                    var contentUri = result.Data;

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                    {
                        Platform.AppContext.ContentResolver.TakePersistableUriPermission(
                            contentUri,
                            ActivityFlags.GrantReadUriPermission);
                    }

                    resultList.Add(new FilePickerResult(contentUri));
                }
                else
                {
                    for (var index = 0; index < result.ClipData.ItemCount; index++)
                    {
                        var data = result.ClipData.GetItemAt(index);

                        var contentUri = data.Uri;

                        if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                        {
                            Platform.AppContext.ContentResolver.TakePersistableUriPermission(
                                contentUri,
                                ActivityFlags.GrantReadUriPermission);
                        }

                        resultList.Add(new FilePickerResult(contentUri));
                    }
                }

                return resultList;
            }
            catch (System.OperationCanceledException)
            {
                return null;
            }
        }
    }

    public partial class FilePickerFileType
    {
        public static FilePickerFileType PlatformImageFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "image/png", "image/jpeg" } }
            });

        public static FilePickerFileType PlatformPngFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "image/png" } }
            });
    }

    public partial class FilePickerResult
    {
        internal FilePickerResult(global::Android.Net.Uri contentUri)
            : base(GetFullPath(contentUri))
        {
            this.contentUri = contentUri;
            FileName = GetFileName(contentUri);
        }

        readonly global::Android.Net.Uri contentUri;

        static string GetFullPath(global::Android.Net.Uri contentUri)
        {
            // if this is a file, use that
            if (contentUri.Scheme == "file")
                return contentUri.Path;

            // ask the content provider for the data column, which may contain the actual file path
#pragma warning disable CS0618 // Type or member is obsolete
            var path = QueryContentResolverColumn(contentUri, MediaStore.Files.FileColumns.Data);
#pragma warning restore CS0618 // Type or member is obsolete

            if (!string.IsNullOrEmpty(path) && Path.IsPathRooted(path))
                return path;

            // fallback: use content URI
            return contentUri.ToString();
        }

        static string GetFileName(global::Android.Net.Uri contentUri)
        {
            // resolve file name by querying content provider for display name
            var filename = QueryContentResolverColumn(contentUri, MediaStore.MediaColumns.DisplayName);

            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = Path.GetFileName(WebUtility.UrlDecode(contentUri.ToString()));
            }

            if (!Path.HasExtension(filename))
                filename = filename.TrimEnd('.') + '.' + GetFileExtensionFromUri(contentUri);

            return filename;
        }

        static string QueryContentResolverColumn(global::Android.Net.Uri contentUri, string columnName)
        {
            string text = null;

            var projection = new[] { columnName };
            using var cursor = Application.Context.ContentResolver.Query(contentUri, projection, null, null, null);
            if (cursor?.MoveToFirst() == true)
            {
                var columnIndex = cursor.GetColumnIndex(columnName);
                if (columnIndex != -1)
                    text = cursor.GetString(columnIndex);
            }

            return text;
        }

        static string GetFileExtensionFromUri(global::Android.Net.Uri uri)
        {
            var mimeType = Application.Context.ContentResolver.GetType(uri);
            return mimeType != null ? global::Android.Webkit.MimeTypeMap.Singleton.GetExtensionFromMimeType(mimeType) : string.Empty;
        }

        Task<Stream> PlatformOpenReadStreamAsync()
        {
            if (contentUri.Scheme == "content")
            {
                var content = Application.Context.ContentResolver.OpenInputStream(contentUri);
                return Task.FromResult(content);
            }

            var stream = File.Open(FullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult<Stream>(stream);
        }

        Task<Stream> PlatformOpenWriteStreamAsync()
        {
            if (contentUri.Scheme == "content")
            {
                var content = Application.Context.ContentResolver.OpenOutputStream(contentUri);
                return Task.FromResult(content);
            }

            var stream = File.Open(FullPath, FileMode.OpenOrCreate, FileAccess.Write);
            return Task.FromResult<Stream>(stream);
        }
    }
}
