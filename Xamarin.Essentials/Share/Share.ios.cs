﻿using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Share
    {
        static async Task PlatformRequestAsync(ShareTextRequest request)
        {
            var src = new TaskCompletionSource<bool>();
            var items = new List<NSObject>();
            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                items.Add(new ShareActivityItemSource(new NSString(request.Text), request.Title));
            }

            if (!string.IsNullOrWhiteSpace(request.Uri))
            {
                items.Add(new ShareActivityItemSource(NSUrl.FromString(request.Uri), request.Title));
            }

            var activityController = new UIActivityViewController(items.ToArray(), null)
            {
                CompletionWithItemsHandler = (a, b, c, d) =>
                {
                    src.TrySetResult(true);
                }
            };

            var vc = Platform.GetCurrentViewController();

            if (activityController.PopoverPresentationController != null)
            {
                activityController.PopoverPresentationController.SourceView = vc.View;

                if (request.PresentationSourceBounds != Rectangle.Empty || Platform.HasOSVersion(13, 0))
                    activityController.PopoverPresentationController.SourceRect = request.PresentationSourceBounds.ToPlatformRectangle();
            }

            await vc.PresentViewControllerAsync(activityController, true);
            await src.Task;
        }

        static async Task PlatformRequestAsync(ShareMultipleFilesRequest request)
        {
            var src = new TaskCompletionSource<bool>();

            var items = new List<NSObject>();

            var hasTitel = !string.IsNullOrWhiteSpace(request.Title);
            foreach (var file in request.Files)
            {
                var fileUrl = NSUrl.FromFilename(file.FullPath);
                if (hasTitel)
                    items.Add(new ShareActivityItemSource(fileUrl, request.Title)); // Share with title (subject)
                else
                    items.Add(fileUrl); // No title specified
            }

            var activityController = new UIActivityViewController(items.ToArray(), null)
            {
                CompletionWithItemsHandler = (a, b, c, d) =>
                {
                    src.TrySetResult(true);
                }
            };

            var vc = Platform.GetCurrentViewController();

            if (activityController.PopoverPresentationController != null)
            {
                activityController.PopoverPresentationController.SourceView = vc.View;

                if (request.PresentationSourceBounds != Rectangle.Empty || Platform.HasOSVersion(13, 0))
                    activityController.PopoverPresentationController.SourceRect = request.PresentationSourceBounds.ToPlatformRectangle();
            }

            await vc.PresentViewControllerAsync(activityController, true);
            await src.Task;
        }
    }

    class ShareActivityItemSource : UIActivityItemSource
    {
        readonly NSObject item;
        readonly string subject;

        internal ShareActivityItemSource(NSObject item, string subject)
        {
            this.item = item;
            this.subject = subject;
        }

        public override NSObject GetItemForActivity(UIActivityViewController activityViewController, NSString activityType) => item;

        public override NSObject GetPlaceholderData(UIActivityViewController activityViewController) => item;

        public override string GetSubjectForActivity(UIActivityViewController activityViewController, NSString activityType) => subject;
    }
}
