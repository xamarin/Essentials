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

        static async Task PlatformSaveImageAsync(byte[] data, string fileName, string albumName)
            => await SaveMediaFileAsync(data, fileName, albumName, false);

        static async Task PlatformSaveVideoAsync(byte[] data, string fileName, string albumName)
            => await SaveMediaFileAsync(data, fileName, albumName, true);

        static async Task SaveMediaFileAsync(byte[] data, string fileName, string albumName, bool isVideo)
        {
            await Permissions.EnsureGrantedAsync<Permissions.Photos>();

            var album = GetAlbum(albumName);

            if (album == null)
                album = await CreateAlbumAsync(albumName);

            await SaveMediaFileAsync(data, fileName, album, isVideo);
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

        static async Task SaveMediaFileAsync(byte[] data, string fileName, PHAssetCollection album, bool isVideo)
        {
            string filePath = null;

            try
            {
                filePath = await CreateFile(data, fileName);
                using var fileUri = new NSUrl(filePath);

                await PhotoLibraryPerformChanges(() =>
                {
                    using var request = isVideo
                        ? PHAssetChangeRequest.FromVideo(fileUri)
                        : PHAssetChangeRequest.FromImage(fileUri);

                    using var addRequest = PHAssetCollectionChangeRequest.ChangeRequest(album);
                    addRequest.AddAssets(new PHObject[] { request.PlaceholderForCreatedAsset });
                });
            }
            finally
            {
                DeleteFile(filePath);
            }
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

        static async Task<string> CreateFile(byte[] data, string fileName)
        {
            fileName = fileName.Trim();
            var dirPath = Path.Combine(FileSystem.CacheDirectory, cacheDir);
            var filePath = Path.Combine(dirPath, fileName);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            await File.WriteAllBytesAsync(filePath, data);
            return filePath;
        }

        static void DeleteFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var path = Path.Combine(FileSystem.CacheDirectory, cacheDir, fileName);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
