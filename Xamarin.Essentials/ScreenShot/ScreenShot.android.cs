using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Provider;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        public static Activity Activity { get; set; }

        public static async Task<string> PlataformCaptureAsync()
        {
            Activity = Platform.GetCurrentActivity(true);

            if (Activity == null)
                throw new Exception("You have to set ScreenshotManager.Activity in your project");

            var view = Activity.Window.DecorView;
            view.DrawingCacheEnabled = true;

            var bitmap = view.GetDrawingCache(true);

            var file = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
            var path = System.IO.Path.Combine(file, "screen.png");

            using (var stream = new FileStream(path, FileMode.Create))
                await bitmap.CompressAsync(Bitmap.CompressFormat.Png, 100, stream);

            return path;
        }
    }
}
