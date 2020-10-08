using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using Core.Graphics;
using ImageIO;
using MobileCoreServices;

namespace Xamarin.Essentials
{
    public static partial class Screenshot
    {
        internal static bool PlatformIsCaptureSupported => true;

        const string appServicesPath =
    "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";

        // https://developer.apple.com/documentation/coregraphics/1454595-cgdisplaycreateimage
        [DllImport(appServicesPath, EntryPoint = "CGDisplayCreateImage")]
        static extern /* CGImageRef */ IntPtr CGDisplayCreateImage(int displayId, CGRect rect);

        static Task<ScreenshotResult> PlatformCaptureAsync()
        {
            var displayId = CGDisplay.MainDisplayID;// todo: Not working on second screen
            var rect = NSApplication.SharedApplication.KeyWindow.Frame; // todo: Full screen, not only app
            var handle = CGDisplayCreateImage(displayId, rect);

            var image = new CGImage(handle);
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
            var filename = Path.Combine(FileSystem.CacheDirectory, "screenshot." + format.ToString().ToLower());
            Save(filename, format);

            var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            var r = new StreamReader(fs);
            return Task.FromResult(r.BaseStream);
        }

        internal void Save(string filename, ScreenshotFormat format)
        {
            var utType = UTType.JPEG;
            switch (format)
            {
                case ScreenshotFormat.Png:
                    utType = UTType.PNG;
                    break;
            }
            var fileURL = new NSUrl(filename, false);

            using (var dataConsumer = new CGDataConsumer(fileURL))
            {
                var imageDestination = CGImageDestination.Create(dataConsumer, utType, 1);
                imageDestination.AddImage(cgImage);
                imageDestination.Close();
            }
        }
    }
}
