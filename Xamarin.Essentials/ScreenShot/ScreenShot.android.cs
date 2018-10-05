using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        public static Activity Activity { get; set; }

        static async Task<string> PlataformCaptureAsync(ScreenOutputType type, string fileName = null)
        {
            var bitmap = GetScreen();

            var (format, extention) = ConfigureFile(type);

            var file = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);

            if (string.IsNullOrEmpty(fileName))
                fileName = "screen";

            fileName += extention;

            var path = System.IO.Path.Combine(file, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
                await bitmap.CompressAsync(format, 100, stream);

            return path;
        }

        static async Task<byte[]> PlataformGetImageBytesAsync(ScreenOutputType type)
        {
            var bitmap = GetScreen();

            byte[] data;

            using (var stream = new MemoryStream())
            {
                await bitmap.CompressAsync(Bitmap.CompressFormat.Png, 100, stream);
                data = stream.ToArray();
            }

            return data;
        }

        static Bitmap GetScreen()
        {
            if (Activity == null)
                Activity = Platform.GetCurrentActivity(true);

            var view = Activity.Window.DecorView;
            view.DrawingCacheEnabled = true;

            var bitmap = view.GetDrawingCache(true);
            return bitmap;
        }

        static (Bitmap.CompressFormat format, string extention) ConfigureFile(ScreenOutputType type)
        {
            if (type is ScreenOutputType.JPEG)
                return (Bitmap.CompressFormat.Jpeg, ".jpg");
            else
                return (Bitmap.CompressFormat.Png, ".png");
        }
    }
}
