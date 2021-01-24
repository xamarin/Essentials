using System;
using System.Threading.Tasks;
using CoreGraphics;
using CoreImage;
using Foundation;
using ImageIO;
using Photos;
using UIKit;

namespace Xamarin.Essentials.SaveToGallery
{
    public static partial class SaveToGallery
    {
        static async Task PlatformSaveImageAsync(byte[] data, string filename, string albumName)
        {
            albumName = "testAlbumName";
            await Permissions.EnsureGrantedAsync<Permissions.Photos>();

            var album = GetAlbum(albumName);

            if (album == null)
                album = await CreateAlbumAsync(albumName);
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

            var tcs = new TaskCompletionSource<NSError>();
            PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(
                () =>
                {
                    var request = PHAssetCollectionChangeRequest.CreateAssetCollection(albumName);
                    placeholder = request.PlaceholderForCreatedAssetCollection;
                },
                (success, error) => tcs.TrySetResult(error));

            var error = await tcs.Task;
            if (error != null)
                throw new NSErrorException(error);

            var id = placeholder?.LocalIdentifier;
            var fetchResult = PHAssetCollection.FetchAssetCollections(new string[] { id }, null);
            return (PHAssetCollection)fetchResult.firstObject;
        }
    }
}
