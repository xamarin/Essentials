using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        static Task<bool> PlatformCanOpenAsync(Uri uri) =>
            Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(WebUtils.GetNativeUrl(uri)));

        static Task PlatformOpenAsync(Uri uri) =>
            PlatformOpenAsync(WebUtils.GetNativeUrl(uri));

        internal static Task<bool> PlatformOpenAsync(NSUrl nativeUrl) =>
            Platform.HasOSVersion(10, 0)
                ? UIApplication.SharedApplication.OpenUrlAsync(nativeUrl, new UIApplicationOpenUrlOptions())
                : Task.FromResult(UIApplication.SharedApplication.OpenUrl(nativeUrl));

        static Task<bool> PlatformTryOpenAsync(Uri uri)
        {
            var nativeUrl = WebUtils.GetNativeUrl(uri);

            if (UIApplication.SharedApplication.CanOpenUrl(nativeUrl))
                return PlatformOpenAsync(nativeUrl);

            return Task.FromResult(false);
        }

#if __IOS__
        static UIDocumentInteractionController documentController;

        static Task PlatformOpenAsync(OpenFileRequest request)
        {
            documentController = new UIDocumentInteractionController()
            {
                Name = request.File.FileName,
                Url = NSUrl.FromFilename(request.File.FullPath),
                Uti = request.File.ContentType
            };

            var view = Platform.GetCurrentUIViewController().View;

            CGRect rect;

            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                rect = request.PresentationSourceBounds != Rectangle.Empty
                    ? request.PresentationSourceBounds.ToPlatformRectangle()
                    : new CGRect(new CGPoint(view.Bounds.Width / 2, view.Bounds.Height), CGRect.Empty.Size);
            }
            else
            {
                rect = view.Bounds;
            }

            documentController.PresentOpenInMenu(rect, view, true);
            return Task.CompletedTask;
        }

#else
        static Task PlatformOpenAsync(OpenFileRequest request) =>
            throw new FeatureNotSupportedException();
#endif
    }
}
