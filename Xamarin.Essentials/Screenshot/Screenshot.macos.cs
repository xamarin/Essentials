using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using ImageIO;
using MobileCoreServices;

namespace Xamarin.Essentials
{
    public static partial class Screenshot
    {
        internal static bool PlatformIsCaptureSupported => true;

        static Task<ScreenshotResult> PlatformCaptureAsync()
        {
            var image = ApplicationServices.GetScreenshot();
            var result = new ScreenshotResult(image);
            return Task.FromResult(result);
        }
    }

    public partial class ScreenshotResult
    {
        readonly CGImage cgImage;

        internal ScreenshotResult(CGImage image)
        {
            cgImage = image;
            Width = (int)cgImage.Width;
            Height = (int)cgImage.Height;
        }

        internal Task<Stream> PlatformOpenReadAsync(ScreenshotFormat format)
        {
            NSString utType;
            switch (format)
            {
                case ScreenshotFormat.Png:
                    utType = UTType.PNG;
                    break;
                case ScreenshotFormat.Jpeg:
                    utType = UTType.JPEG;
                    break;
                default:
                    throw new NotImplementedException("The ScreenshotFormat is not supported");
            }

            var data = new NSMutableData();
            var destination = CGImageDestination.Create(data, utType, imageCount: 1);
            destination.AddImage(cgImage);
            destination.Close();

            return Task.FromResult(data.AsStream());
        }
    }
}
