using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Xamarin.Essentials.SaveToGallery
{
    public static partial class SaveToGallery
    {
        static Task PlatformSaveImageAsync(byte[] data, string filename, string albumName)
        {
            var image = new UIImage(NSData.FromArray(data));
            image.SaveToPhotosAlbum((UIImage img, NSError error) => { });
            return Task.CompletedTask;
        }
    }
}
