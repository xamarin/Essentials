using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using Photos;

namespace Xamarin.Essentials
{
    public static partial class SaveToGallery
    {
        static readonly string cacheDir = "EssentialsPhotosCacheDir";

        static async Task PlatformSaveAsync(MediaFileType type, byte[] data, string fileName, string albumName)
        {
            string filePath = null;

            try
            {
                filePath = GetFilePath(fileName);
                await File.WriteAllBytesAsync(filePath, data);

                await PlatformSaveAsync(type, filePath, albumName);
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        static async Task PlatformSaveAsync(MediaFileType type, Stream fileStream, string fileName, string albumName)
        {
            string filePath = null;

            try
            {
                filePath = GetFilePath(fileName);
                using var stream = File.Create(filePath);
                await fileStream.CopyToAsync(stream);
                stream.Close();

                await PlatformSaveAsync(type, filePath, albumName);
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        static async Task PlatformSaveAsync(MediaFileType type, string filePath, string albumName)
        {
            using var album = PHPhotoLibrary.AuthorizationStatus == PHAuthorizationStatus.Authorized
                ? GetAlbum(albumName) ?? await CreateAlbumAsync(albumName)
                : null;

            using var fileUri = new NSUrl(filePath);

            await PhotoLibraryPerformChanges(() =>
            {
                using var request = type == MediaFileType.Video
                ? PHAssetChangeRequest.FromVideo(fileUri)
                : PHAssetChangeRequest.FromImage(fileUri);

                if (album != null)
                {
                    using var addRequest = PHAssetCollectionChangeRequest.ChangeRequest(album);
                    addRequest.AddAssets(new PHObject[] { request.PlaceholderForCreatedAsset });
                }
            });
        }

        static PHAssetCollection GetAlbum(string albumName)
        {
            var options = new PHFetchOptions();
            options.Predicate = NSPredicate.FromFormat(
                    "title=%@",
                    new NSObject[] { NSObject.FromObject(albumName) });

            var collection = PHAssetCollection.FetchAssetCollections(
                PHAssetCollectionType.Album,
                PHAssetCollectionSubtype.Any,
                options);

            return (PHAssetCollection)collection?.firstObject;
        }

        static async Task<PHAssetCollection> CreateAlbumAsync(string albumName)
        {
            PHObjectPlaceholder placeholder = null;

            await PhotoLibraryPerformChanges(() =>
            {
                using var request = PHAssetCollectionChangeRequest.CreateAssetCollection(albumName);
                placeholder = request.PlaceholderForCreatedAssetCollection;
            });

            var id = placeholder?.LocalIdentifier;
            var fetchResult = PHAssetCollection.FetchAssetCollections(new string[] { id }, null);
            return (PHAssetCollection)fetchResult.firstObject;
        }

        static async Task PhotoLibraryPerformChanges(Action action)
        {
            var tcs = new TaskCompletionSource<Exception>();

            PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(
                () =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetResult(ex);
                    }
                },
                (success, error) =>
                    tcs.TrySetResult(error != null ? new NSErrorException(error) : null));

            var exception = await tcs.Task;
            if (exception != null)
                throw exception;
        }

        static void DeleteFile(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                File.Delete(filePath);
        }

        static string GetFilePath(string fileName)
        {
            fileName = fileName.Trim();
            var dirPath = Path.Combine(FileSystem.CacheDirectory, cacheDir);
            var filePath = Path.Combine(dirPath, fileName);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return filePath;
        }
    }
}
