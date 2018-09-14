using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        public static async Task<string> PlataformCaptureAsync()
        {
            var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(file, "screen.png");

            using (var stream = new FileStream(path, FileMode.CreateNew))
            using (var imgData = image.AsPNG())
            {
                var img = imgData.ToArray();
                await stream.WriteAsync(img, 0, img.Length);
            }

            return path;
        }
    }
}
