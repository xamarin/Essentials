using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        static async Task<string> PlataformCaptureAsync(ScreenOutputType type, string fileName = null)
        {
            var render = await GetRendererAsync();

            if (string.IsNullOrEmpty(fileName))
                fileName = "screeshot";

            var (bitmapId, extention) = GetEnconder(type);

            fileName += extention;

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;

                var pixelBuffer = await render.GetPixelsAsync();

                var encoder = await BitmapEncoder.CreateAsync(bitmapId, stream.AsRandomAccessStream());
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

        static async Task<byte[]> PlataformGetImageBytesAsync(ScreenOutputType type)
        {
            var render = await GetRendererAsync();

            using (var stream = new InMemoryRandomAccessStream())
            {
                var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                var pixelBuffer = await render.GetPixelsAsync();

                var (bitmapId, _) = GetEnconder(type);

                var encoder = await BitmapEncoder.CreateAsync(bitmapId, stream);
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    (uint)render.PixelWidth,
                    (uint)render.PixelHeight,
                    logicalDpi,
                    logicalDpi,
                    pixelBuffer.ToArray());

                await encoder.FlushAsync();

                var bytes = new byte[stream.Size];

                await stream.ReadAsync(bytes.AsBuffer(), (uint)bytes.Length, InputStreamOptions.None);

                return bytes;
            }
        }

        static (Guid encoder, string extention) GetEnconder(ScreenOutputType type)
        {
            if (type is ScreenOutputType.PNG)
                return (BitmapEncoder.PngEncoderId, ".png");
            else
                return (BitmapEncoder.JpegEncoderId, ".jpg");
        }

        static async Task<RenderTargetBitmap> GetRendererAsync()
        {
            var rootFrame = (Window.Current.Content as Frame).Content as Page;

            var render = new RenderTargetBitmap();
            await render.RenderAsync(rootFrame, (int)rootFrame.ActualWidth, (int)rootFrame.ActualHeight);

            return render;
        }
    }
}
