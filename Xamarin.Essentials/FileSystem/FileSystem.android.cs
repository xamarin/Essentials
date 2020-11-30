using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Android.Provider;
using Android.Webkit;
using AndroidUri = Android.Net.Uri;

namespace Xamarin.Essentials
{
    public partial class FileSystem
    {
        static string PlatformCacheDirectory
            => Platform.AppContext.CacheDir.AbsolutePath;

        static string PlatformAppDataDirectory
            => Platform.AppContext.FilesDir.AbsolutePath;

        static Task<Stream> PlatformOpenAppPackageFileAsync(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            filename = filename.Replace('\\', Path.DirectorySeparatorChar);
            try
            {
                return Task.FromResult(Platform.AppContext.Assets.Open(filename));
            }
            catch (Java.IO.FileNotFoundException ex)
            {
                throw new FileNotFoundException(ex.Message, filename, ex);
            }
        }

        internal static Java.IO.File GetEssentialsTemporaryFile(Java.IO.File root, string fileName)
        {
            // create the directory for all Essentials files
            var rootDir = new Java.IO.File(root, "2203693cc04e0be7f4f024d5f9499e13");
            rootDir.Mkdirs();
            rootDir.DeleteOnExit();

            // create a unique directory just in case there are multiple file with the same name
            var tmpDir = new Java.IO.File(rootDir, Guid.NewGuid().ToString("N"));
            tmpDir.Mkdirs();
            tmpDir.DeleteOnExit();

            // create the new temporary file
            var tmpFile = new Java.IO.File(tmpDir, fileName);
            tmpFile.DeleteOnExit();

            return tmpFile;
        }

        internal static string EnsurePhysicalPath(AndroidUri uri)
        {
            // if this is a file, use that
            if (uri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
                return uri.Path;

            // try resolve using the content provider
            var absolute = ResolvePhysicalPath(uri);
            if (!string.IsNullOrWhiteSpace(absolute) && Path.IsPathRooted(absolute))
                return absolute;

            // fall back to just copying it
            absolute = CacheContentFile(uri);
            if (!string.IsNullOrWhiteSpace(absolute) && Path.IsPathRooted(absolute))
                return absolute;

            throw new FileNotFoundException($"Unable to resolve absolute path or retrieve contents of URI '{uri}'.");
        }

        static string ResolvePhysicalPath(AndroidUri uri)
        {
            if (Platform.HasApiLevelKitKat && DocumentsContract.IsDocumentUri(Platform.AppContext, uri))
            {
                var resolved = ResolveDocumentPath(uri);
                if (File.Exists(resolved))
                    return resolved;
            }

            if (uri.Scheme.Equals("content", StringComparison.OrdinalIgnoreCase))
            {
                var resolved = ResolveContentPath(uri);
                if (File.Exists(resolved))
                    return resolved;
            }
            else if (uri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
            {
                var resolved = uri.Path;
                if (File.Exists(resolved))
                    return resolved;
            }

            return null;
        }

        static string ResolveDocumentPath(AndroidUri uri)
        {
            Debug.WriteLine($"Trying to resolve document URI: '{uri}'");

            var docId = DocumentsContract.GetDocumentId(uri);

            var docIdParts = docId?.Split(':');
            if (docIdParts == null || docIdParts.Length == 0)
                return null;

            if (uri.Authority.Equals("com.android.externalstorage.documents", StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine($"Resolving external storage URI: '{uri}'");

                if (docIdParts.Length == 2)
                {
                    var storageType = docIdParts[0];
                    var uriPath = docIdParts[1];

                    // This is the internal "external" memory, NOT the SD Card
                    if (storageType.Equals("primary", StringComparison.OrdinalIgnoreCase))
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        var root = global::Android.OS.Environment.ExternalStorageDirectory.Path;
#pragma warning restore CS0618 // Type or member is obsolete

                        return Path.Combine(root, uriPath);
                    }

                    // TODO: support other types, such as actual SD Cards
                }
            }
            else if (uri.Authority.Equals("com.android.providers.downloads.documents", StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine($"Resolving downloads URI: '{uri}'");

                // NOTE: This only really applies to older Android vesions since the privacy changes

                if (docIdParts.Length == 2)
                {
                    var storageType = docIdParts[0];
                    var uriPath = docIdParts[1];

                    if (storageType.Equals("raw", StringComparison.OrdinalIgnoreCase))
                        return uriPath;
                }

                var contentUriPrefixes = new[]
                {
                    "content://downloads/public_downloads",
                    "content://downloads/my_downloads",
                    "content://downloads/all_downloads",
                };

                // ID could be "###" or "msf:###"
                var fileId = docIdParts.Length == 2
                    ? docIdParts[1]
                    : docIdParts[0];

                foreach (var prefix in contentUriPrefixes)
                {
                    var uriString = prefix + "/" + fileId;
                    var contentUri = AndroidUri.Parse(uriString);

                    if (GetDataFilePath(contentUri) is string filePath)
                        return filePath;
                }
            }
            else if (uri.Authority.Equals("com.android.providers.media.documents", StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine($"Resolving media URI: '{uri}'");

                if (docIdParts.Length == 2)
                {
                    var storageType = docIdParts[0];
                    var uriPath = docIdParts[1];

                    AndroidUri contentUri = null;
                    if (storageType.Equals("image", StringComparison.OrdinalIgnoreCase))
                        contentUri = MediaStore.Images.Media.ExternalContentUri;
                    else if (storageType.Equals("video", StringComparison.OrdinalIgnoreCase))
                        contentUri = MediaStore.Video.Media.ExternalContentUri;
                    else if (storageType.Equals("audio", StringComparison.OrdinalIgnoreCase))
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;

                    if (contentUri != null && GetDataFilePath(contentUri, "_id=?", new[] { uriPath }) is string filePath)
                        return filePath;
                }
            }

            Debug.WriteLine($"Unable to resolve document URI: '{uri}'");

            return null;
        }

        static string ResolveContentPath(AndroidUri uri)
        {
            Debug.WriteLine($"Trying to resolve content URI: '{uri}'");

            if (GetDataFilePath(uri) is string filePath)
                return filePath;

            // TODO: support some additional things, like Google Photos if that is possible

            Debug.WriteLine($"Unable to resolve content URI: '{uri}'");

            return null;
        }

        static string CacheContentFile(AndroidUri uri)
        {
            if (!uri.Scheme.Equals("content", StringComparison.OrdinalIgnoreCase))
                return null;

            Debug.WriteLine($"Copying content URI to local cache: '{uri}'");

            // open the source stream
            using var srcStream = OpenContentStream(uri, out var extension);
            if (srcStream == null)
                return null;

            // resolve or generate a valid destination path
            var filename = GetColumnValue(uri, MediaStore.Files.FileColumns.DisplayName) ?? Guid.NewGuid().ToString("N");
            if (!Path.HasExtension(filename) && !string.IsNullOrEmpty(extension))
                filename = Path.ChangeExtension(filename, extension);

            // create a temporary file
            var tmpFile = GetEssentialsTemporaryFile(Platform.AppContext.CacheDir, filename);

            // copy to the destination
            using var dstStream = File.Create(tmpFile.CanonicalPath);
            srcStream.CopyTo(dstStream);

            return tmpFile.CanonicalPath;
        }

        static Stream OpenContentStream(AndroidUri uri, out string extension)
        {
            var isVirtual = IsVirtualFile(uri);
            if (isVirtual)
            {
                Debug.WriteLine($"Content URI was virtual: '{uri}'");
                return GetVirtualFileStream(uri, out extension);
            }

            extension = GetFileExtension(uri);
            return Platform.ContentResolver.OpenInputStream(uri);
        }

        static bool IsVirtualFile(AndroidUri uri)
        {
            if (!DocumentsContract.IsDocumentUri(Platform.AppContext, uri))
                return false;

            var value = GetColumnValue(uri, DocumentsContract.Document.ColumnFlags);
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out var flagsInt))
            {
                var flags = (DocumentContractFlags)flagsInt;
                return flags.HasFlag(DocumentContractFlags.VirtualDocument);
            }

            return false;
        }

        static Stream GetVirtualFileStream(AndroidUri uri, out string extension)
        {
            var mimeTypes = Platform.ContentResolver.GetStreamTypes(uri, "*/*");
            if (mimeTypes?.Length >= 1)
            {
                var mimeType = mimeTypes[0];

                var stream = Platform.ContentResolver
                    .OpenTypedAssetFileDescriptor(uri, mimeType, null)
                    .CreateInputStream();

                extension = MimeTypeMap.Singleton.GetExtensionFromMimeType(mimeType);

                return stream;
            }

            extension = null;
            return null;
        }

        static string GetColumnValue(AndroidUri contentUri, string column, string selection = null, string[] selectionArgs = null)
        {
            try
            {
                var value = QueryContentResolverColumn(contentUri, column, selection, selectionArgs);
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            catch
            {
                // Ignore all exceptions and use null for the error indicator
            }

            return null;
        }

        static string GetDataFilePath(AndroidUri contentUri, string selection = null, string[] selectionArgs = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            const string column = MediaStore.Files.FileColumns.Data;
#pragma warning restore CS0618 // Type or member is obsolete

            // ask the content provider for the data column, which may contain the actual file path
            var path = GetColumnValue(contentUri, column, selection, selectionArgs);
            if (!string.IsNullOrEmpty(path) && Path.IsPathRooted(path))
                return path;

            return null;
        }

        static string GetFileExtension(AndroidUri uri)
        {
            var mimeType = Platform.ContentResolver.GetType(uri);

            return mimeType != null
                ? MimeTypeMap.Singleton.GetExtensionFromMimeType(mimeType)
                : null;
        }

        static string QueryContentResolverColumn(AndroidUri contentUri, string columnName, string selection = null, string[] selectionArgs = null)
        {
            string text = null;

            var projection = new[] { columnName };
            using var cursor = Platform.ContentResolver.Query(contentUri, projection, selection, selectionArgs, null);
            if (cursor?.MoveToFirst() == true)
            {
                var columnIndex = cursor.GetColumnIndex(columnName);
                if (columnIndex != -1)
                    text = cursor.GetString(columnIndex);
            }

            return text;
        }
    }

    public partial class FileBase
    {
        internal FileBase(Java.IO.File file)
            : this(file?.Path)
        {
        }

        internal static string PlatformGetContentType(string extension) =>
            MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension.TrimStart('.'));

        internal void PlatformInit(FileBase file)
        {
        }

        internal virtual Task<Stream> PlatformOpenReadAsync()
        {
            var stream = File.OpenRead(FullPath);
            return Task.FromResult<Stream>(stream);
        }
    }
}
