using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using MobileCoreServices;
using Photos;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class MediaPicker
    {
        static UIImagePickerController picker;

        static bool PlatformIsCaptureSupported
            => UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);

        static Task<FileResult> PlatformPickPhotoAsync(MediaPickerOptions options)
            => PhotoAsync(options, true, true);

        static Task<FileResult> PlatformCapturePhotoAsync(MediaPickerOptions options)
            => PhotoAsync(options, true, false);

        static Task<FileResult> PlatformPickVideoAsync(MediaPickerOptions options)
            => PhotoAsync(options, false, true);

        static Task<FileResult> PlatformCaptureVideoAsync(MediaPickerOptions options)
            => PhotoAsync(options, false, false);

        static async Task<FileResult> PhotoAsync(MediaPickerOptions options, bool photo, bool pickExisting)
        {
            var sourceType = pickExisting ? UIImagePickerControllerSourceType.PhotoLibrary : UIImagePickerControllerSourceType.Camera;
            var mediaType = photo ? UTType.Image : UTType.Movie;

            if (!UIImagePickerController.IsSourceTypeAvailable(sourceType))
                throw new FeatureNotSupportedException();
            if (!UIImagePickerController.AvailableMediaTypes(sourceType).Contains(mediaType))
                throw new FeatureNotSupportedException();

            if (!photo)
                await Permissions.EnsureGrantedAsync<Permissions.Microphone>();

            // permission is not required on iOS 11 for the picker
            if (!Platform.HasOSVersion(11, 0))
            {
                await Permissions.EnsureGrantedAsync<Permissions.Photos>();
            }

            var vc = Platform.GetCurrentViewController(true);

            picker = new UIImagePickerController();
            picker.SourceType = sourceType;
            picker.MediaTypes = new string[] { mediaType };
            picker.AllowsEditing = false;
            if (!photo && !pickExisting)
                picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;

            if (!string.IsNullOrWhiteSpace(options?.Title))
                picker.Title = options.Title;

            if (DeviceInfo.Idiom == DeviceIdiom.Tablet && picker.PopoverPresentationController != null && vc.View != null)
                picker.PopoverPresentationController.SourceRect = vc.View.Bounds;

            var tcs = new TaskCompletionSource<FileResult>(picker);
            picker.Delegate = new PhotoPickerDelegate
            {
                CompletedHandler = info =>
                    tcs.TrySetResult(DictionaryToMediaFile(info))
            };

            await vc.PresentViewControllerAsync(picker, true);

            var result = await tcs.Task;

            await vc.DismissViewControllerAsync(true);

            picker?.Dispose();
            picker = null;

            return result;
        }

        static FileResult DictionaryToMediaFile(NSDictionary info)
        {
            if (info == null)
                return null;

            PHAsset phAsset = null;
            NSUrl assetUrl = null;

            if (Platform.HasOSVersion(11, 0))
            {
                assetUrl = info[UIImagePickerController.ImageUrl] as NSUrl;

                // Try the MediaURL sometimes used for videos
                if (assetUrl == null)
                    assetUrl = info[UIImagePickerController.MediaURL] as NSUrl;

                if (assetUrl != null)
                {
                    if (!assetUrl.Scheme.Equals("assets-library", StringComparison.InvariantCultureIgnoreCase))
                        return new UIDocumentFileResult(assetUrl);

                    phAsset = info.ValueForKey(UIImagePickerController.PHAsset) as PHAsset;
                }
            }

            if (phAsset == null)
            {
                assetUrl = info[UIImagePickerController.ReferenceUrl] as NSUrl;

                if (assetUrl != null)
                    phAsset = PHAsset.FetchAssets(new NSUrl[] { assetUrl }, null)?.LastObject as PHAsset;
            }

            if (phAsset == null || assetUrl == null)
            {
                var img = info.ValueForKey(UIImagePickerController.OriginalImage) as UIImage;
                var imgUrl = info.ValueForKey(UIImagePickerController.ImageUrl) as NSString;

                if (img != null)
                {
                    var rotatedImg = CorrectImageRotation(img);

                    return new UIImageFileResult(rotatedImg ?? img);
                }
            }

            if (phAsset == null || assetUrl == null)
                return null;

            string originalFilename;

            if (Platform.HasOSVersion(9, 0))
                originalFilename = PHAssetResource.GetAssetResources(phAsset).FirstOrDefault()?.OriginalFilename;
            else
                originalFilename = phAsset.ValueForKey(new NSString("filename")) as NSString;

            return new PHAssetFileResult(assetUrl, phAsset, originalFilename);
        }

        public static UIImage CorrectImageRotation(UIImage image)
        {
            UIImage imageToReturn = null;
            if (image.Orientation == UIImageOrientation.Up)
            {
                imageToReturn = image;
            }
            else
            {
                var transform = CGAffineTransform.MakeIdentity();

                switch (image.Orientation)
                {
                    case UIImageOrientation.Down:
                    case UIImageOrientation.DownMirrored:
                        transform.Rotate((float)Math.PI);
                        transform.Translate(image.Size.Width, image.Size.Height);
                        break;

                    case UIImageOrientation.Left:
                    case UIImageOrientation.LeftMirrored:
                        transform.Rotate((float)Math.PI / 2);
                        transform.Translate(image.Size.Width, 0);
                        break;

                    case UIImageOrientation.Right:
                    case UIImageOrientation.RightMirrored:
                        transform.Rotate(-(float)Math.PI / 2);
                        transform.Translate(0, image.Size.Height);
                        break;
                    case UIImageOrientation.Up:
                    case UIImageOrientation.UpMirrored:
                        break;
                }

                switch (image.Orientation)
                {
                    case UIImageOrientation.UpMirrored:
                    case UIImageOrientation.DownMirrored:
                        transform.Translate(image.Size.Width, 0);
                        transform.Scale(-1, 1);
                        break;

                    case UIImageOrientation.LeftMirrored:
                    case UIImageOrientation.RightMirrored:
                        transform.Translate(image.Size.Height, 0);
                        transform.Scale(-1, 1);
                        break;
                    case UIImageOrientation.Up:
                    case UIImageOrientation.Down:
                    case UIImageOrientation.Left:
                    case UIImageOrientation.Right:
                        break;
                }

                using var context = new CGBitmapContext(
                    IntPtr.Zero,
                    (int)image.Size.Width,
                    (int)image.Size.Height,
                    image.CGImage.BitsPerComponent,
                    image.CGImage.BytesPerRow,
                    image.CGImage.ColorSpace,
                    image.CGImage.BitmapInfo);

                context.ConcatCTM(transform);
                switch (image.Orientation)
                {
                    case UIImageOrientation.Left:
                    case UIImageOrientation.LeftMirrored:
                    case UIImageOrientation.Right:
                    case UIImageOrientation.RightMirrored:
                        context.DrawImage(new CGRect(0, 0, image.Size.Height, image.Size.Width), image.CGImage);
                        break;
                    default:
                        context.DrawImage(new CGRect(0, 0, image.Size.Width, image.Size.Height), image.CGImage);
                        break;
                }

                using var imageRef = context.ToImage();
                imageToReturn = new UIImage(imageRef, 1, UIImageOrientation.Up);
            }

            return imageToReturn;
        }

        class PhotoPickerDelegate : UIImagePickerControllerDelegate
        {
            public Action<NSDictionary> CompletedHandler { get; set; }

            public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info) =>
                CompletedHandler?.Invoke(info);

            public override void Canceled(UIImagePickerController picker) =>
                CompletedHandler?.Invoke(null);
        }
    }
}
