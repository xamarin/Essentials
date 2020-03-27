using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MobileCoreServices;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class FilePicker
    {
        static DocumentPicker CreatePicker(PickOptions options, bool multipleSelect, bool save)
        {
            if (multipleSelect && !save && !Platform.HasOSVersion(11, 0))
                throw new FeatureNotSupportedException("Picking multiple files is only available on iOS 11 or later.");

            if (save && string.IsNullOrEmpty(options.SuggestedFileName))
                throw new ArgumentException("PickOptions.SuggestedFileName is required and must point to an existing file.");

            var allowedUtis = options?.FileTypes?.Value?.ToArray() ?? new string[]
            {
                UTType.Content,
                UTType.Item,
                "public.data"
            };

            DocumentPicker documentPicker;
            if (save)
            {
                // Note: When exporting (UIDocumentPickerMode.ExportToService) an original file is
                // required which the OS will then copy to the user's choice of location. So the
                // user must first save the file in a temporary location then pass in it's path as
                // the SuggestFileName in the PickOptions.

                var pathToTempFile = NSUrl.CreateFileUrl(options.SuggestedFileName, false, null);
                documentPicker = new DocumentPicker(pathToTempFile, UIDocumentPickerMode.ExportToService);
            }
            else
            {
                // Note: Importing (UIDocumentPickerMode.Import) makes a local copy of the document,
                // while opening (UIDocumentPickerMode.Open) opens the document directly. We do the
                // latter so the user accesses the original file, not a copy, which is more performant
                // with larger files. This also means we don't have to open a file stream straight away
                // to keep the temporary "imported" file from being cleaned up.

                documentPicker = new DocumentPicker(allowedUtis, UIDocumentPickerMode.Open);
                documentPicker.AllowsMultipleSelection = multipleSelect;
            }

            return documentPicker;
        }

        static Task<FilePickerResult> PlatformPickFileAsync(PickOptions options)
        {
            var documentPicker = CreatePicker(options, false, false);

            var tcs = new TaskCompletionSource<FilePickerResult>();

            documentPicker.Picked += (sender, e) =>
            {
                // there was a cancellation
                if (e == null || e.Urls.Length == 0)
                {
                    tcs.TrySetResult(null);
                    return;
                }

                try
                {
                    var result = new FilePickerResult(e.Urls[0]);
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    // pass exception to task so that it doesn't get lost in the UI main loop
                    tcs.SetException(ex);
                }
            };

            var parentController = Platform.GetCurrentViewController();

            parentController.PresentViewController(documentPicker, true, null);

            return tcs.Task;
        }

        static Task<FilePickerResult> PlatformPickFileToSaveAsync(PickOptions options)
        {
            var documentPicker = CreatePicker(options, false, true);

            var tcs = new TaskCompletionSource<FilePickerResult>();

            documentPicker.Picked += (sender, e) =>
            {
                // there was a cancellation
                if (e == null || e.Urls.Length == 0)
                {
                    tcs.TrySetResult(null);
                    return;
                }

                try
                {
                    var result = new FilePickerResult(e.Urls[0]);
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    // pass exception to task so that it doesn't get lost in the UI main loop
                    tcs.SetException(ex);
                }
            };

            var parentController = Platform.GetCurrentViewController();

            parentController.PresentViewController(documentPicker, true, null);

            return tcs.Task;
        }

        static Task<IEnumerable<FilePickerResult>> PlatformPickMultipleFilesAsync(PickOptions options)
        {
            var documentPicker = CreatePicker(options, true, false);

            var tcs = new TaskCompletionSource<IEnumerable<FilePickerResult>>();

            documentPicker.Picked += (sender, e) =>
            {
                // there was a cancellation
                if (e == null || e.Urls.Length == 0)
                {
                    tcs.TrySetResult(null);
                    return;
                }

                try
                {
                    var resultList = e.Urls.Select(url => new FilePickerResult(url));
                    tcs.TrySetResult(resultList);
                }
                catch (Exception ex)
                {
                    // pass exception to task so that it doesn't get lost in the UI main loop
                    tcs.SetException(ex);
                }
            };

            var parentController = Platform.GetCurrentViewController();

            parentController.PresentViewController(documentPicker, true, null);

            return tcs.Task;
        }

        class DocumentPicker : UIDocumentPickerViewController
        {
            public DocumentPicker(string[] allowedUTIs, UIDocumentPickerMode mode)
                : base(allowedUTIs, mode)
            {
                // this is called starting from iOS 11.
                DidPickDocumentAtUrls += OnUrlsPicked;
                DidPickDocument += OnDocumentPicked;
                WasCancelled += OnCancelled;
            }

            public DocumentPicker(NSUrl tempFile, UIDocumentPickerMode mode)
                : base(tempFile, mode)
            {
                // this is called starting from iOS 11.
                DidPickDocumentAtUrls += OnUrlsPicked;
                DidPickDocument += OnDocumentPicked;
                WasCancelled += OnCancelled;
            }

            public event EventHandler<UIDocumentPickedAtUrlsEventArgs> Picked;

            void OnUrlsPicked(object sender, UIDocumentPickedAtUrlsEventArgs e) =>
                Picked?.Invoke(this, e);

            void OnDocumentPicked(object sender, UIDocumentPickedEventArgs e) =>
                Picked?.Invoke(this, new UIDocumentPickedAtUrlsEventArgs(new NSUrl[] { e.Url }));

            void OnCancelled(object sender, EventArgs args) =>
                Picked?.Invoke(this, null);
        }
    }

    public partial class FilePickerFileType
    {
        public static FilePickerFileType PlatformImageFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { (string)UTType.Image } }
            });

        public static FilePickerFileType PlatformPngFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { (string)UTType.PNG } }
            });
    }

    public partial class FilePickerResult
    {
        internal FilePickerResult(NSUrl url)
            : base()
        {
            url.StartAccessingSecurityScopedResource();

            var doc = new UIDocument(url);
            FullPath = doc.FileUrl?.Path;
            FileName = doc.LocalizedName ?? Path.GetFileName(FullPath);

            url.StopAccessingSecurityScopedResource();
        }

        Task<Stream> PlatformOpenReadStreamAsync()
        {
            var stream = File.Open(FullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult<Stream>(stream);
        }

        Task<Stream> PlatformOpenWriteStreamAsync()
        {
            var stream = File.Open(FullPath, FileMode.OpenOrCreate, FileAccess.Write);
            return Task.FromResult<Stream>(stream);
        }
    }
}
