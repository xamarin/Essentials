using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        static Task<bool> PlatformCanOpenAsync(Uri uri) =>
            Task.FromResult(UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri.OriginalString)));

        static Task PlatformOpenAsync(Uri uri) =>
            UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(uri.OriginalString), new UIApplicationOpenUrlOptions());

        static Task PlatformOpenAsync(OpenFileRequest request)
        {
            var fileUrl = NSUrl.FromFilename(request.File.FullPath);

            var documentController = UIDocumentInteractionController.FromUrl(fileUrl);
            documentController.Uti = request.File.ContentType;

            var vc = Platform.GetCurrentViewController();

            documentController.PresentOpenInMenu(vc.View.Frame, vc.View, true);
            return Task.CompletedTask;
        }
    }
}
