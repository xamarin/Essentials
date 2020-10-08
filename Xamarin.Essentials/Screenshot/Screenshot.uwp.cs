using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Xamarin.Essentials
{
    public static partial class Screenshot
    {
        internal static bool PlatformIsCaptureSupported =>
            true;

        static async Task<ScreenshotResult> PlatformCaptureAsync()
        {
            var element = Window.Current?.Content as FrameworkElement;
            if (element == null)
                throw new InvalidOperationException("Unable to find main window content.");

            var bmp = new RenderTargetBitmap();

            // We must use ConfigureAwait(true), to ensure we keep the same execution context (thread); otherwise we'll get an exception
            // in the ScreenshotResult constructor, when trying to access the properties of the RendertargetBitmap instance
            await bmp.RenderAsync(element).AsTask().ConfigureAwait(true);

            return new ScreenshotResult(bmp);
        }
    }

    public partial class ScreenshotResult
    {
        readonly RenderTargetBitmap bmp;
        byte[] bytes;

        internal ScreenshotResult(RenderTargetBitmap bmp)
        {
            this.bmp = bmp;

            Width = bmp.PixelWidth;
            Height = bmp.PixelHeight;
        }

        internal async Task<Stream> PlatformOpenReadAsync(ScreenshotFormat format)
        {
            if (bytes == null)
            {
                var pixels = await bmp.GetPixelsAsync().AsTask().ConfigureAwait(false);
                bytes = pixels.ToArray();
            }

            var f = format switch
            {
                ScreenshotFormat.Jpeg => BitmapEncoder.JpegEncoderId,
                _ => BitmapEncoder.PngEncoderId
            };

            var ms = new InMemoryRandomAccessStream();

            var encoder = await BitmapEncoder.CreateAsync(f, ms).AsTask().ConfigureAwait(false);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)Width, (uint)Height, 96, 96, bytes);
            await encoder.FlushAsync().AsTask().ConfigureAwait(false);

            return ms.AsStreamForRead();
        }
    }
}
