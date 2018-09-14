using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// https://stackoverflow.com/questions/38772158/how-to-take-screen-shot-in-windows-store-apps
namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        public static async Task<string> PlataformCaptureAsync()
        {
            var rootFrame = (Window.Current.Content as Frame).Content as Page;

            var render = new RenderTargetBitmap();
            await render.RenderAsync(rootFrame, (int)rootFrame.ActualWidth, (int)rootFrame.ActualHeight);

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("screeshot.jpg", CreationCollisionOption.GenerateUniqueName);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                var pixelBuffer = await render.GetPixelsAsync();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream.AsRandomAccessStream());
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    (uint)render.PixelWidth,
                    (uint)render.PixelHeight,
                    logicalDpi,
                    logicalDpi,
                    pixelBuffer.ToArray());

                await encoder.FlushAsync();
            }

            return file.Path;
        }
    }
}
