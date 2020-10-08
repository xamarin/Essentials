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

        const string appServicesPath =
    "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";

        // https://developer.apple.com/documentation/coregraphics/1454595-cgdisplaycreateimage
        [DllImport(appServicesPath, EntryPoint = "CGDisplayCreateImage")]
        static extern /* CGImageRef */ IntPtr CGDisplayCreateImage(int displayId);

        static Task<ScreenshotResult> PlatformCaptureAsync()
        {
            using var pool = new NSAutoreleasePool();
            var location = NSApplication.SharedApplication.KeyWindow.Frame.Location;
            var screens = new List<NSScreen>(NSScreen.Screens);
            var screen = screens.First(obj => obj.Frame.Contains(location));
            var windowNumber = (NSNumber)screen.DeviceDescription["NSScreenNumber"];
            var displayId = windowNumber.Int32Value;

            var handle = CGDisplayCreateImage(displayId); // CGWindowListCreateImage not working
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

        // We have to save because MemoryStream is not working
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
