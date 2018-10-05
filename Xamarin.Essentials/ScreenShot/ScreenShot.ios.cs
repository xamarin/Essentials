using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        static async Task<string> PlataformCaptureAsync(ScreenOutputType type, string fileName = null)
        {
            var image = GetScreen();
            UIGraphics.EndImageContext();

            var (format, extention) = ConfigureFile(type, image);

            if (string.IsNullOrEmpty(fileName))
                fileName = "screen";

            fileName += extention;

            var file = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
            var path = Path.Combine(file, fileName);
            using (var stream = new FileStream(path, FileMode.CreateNew))
            using (format)
            {
                var img = format.ToArray();
                await stream.WriteAsync(img, 0, img.Length);
            }

            return path;
        }

        static Task<byte[]> PlataformGetImageBytesAsync(ScreenOutputType type)
        {
            var image = GetScreen();
            UIGraphics.EndImageContext();

            var (format, _) = ConfigureFile(type, image);

            using (format)
            {
                var bytes = new byte[format.Length];
                System.Runtime.InteropServices.Marshal.Copy(format.Bytes, bytes, 0, Convert.ToInt32(format.Length));
                return Task.FromResult(bytes);
            }
        }

        static UIImage GetScreen()
        {
            var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();

            return image;
        }

        static (Foundation.NSData data, string extention) ConfigureFile(ScreenOutputType type, UIImage image)
        {
            if (type is ScreenOutputType.JPEG)
                return (image.AsJPEG(), ".jpeg");
            else
                return (image.AsPNG(), ".png");
        }
    }
}
