using System;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class MediaPicker
    {
        static bool PlatformIsCaptureSupported =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        static Task<FileResult> PlatformPickPhotoAsync(MediaPickerOptions options) =>
            ThrowHelper.ThrowNotImplementedException<Task<FileResult>>();

        static Task<FileResult> PlatformCapturePhotoAsync(MediaPickerOptions options) =>
            ThrowHelper.ThrowNotImplementedException<Task<FileResult>>();

        static Task<FileResult> PlatformPickVideoAsync(MediaPickerOptions options) =>
            ThrowHelper.ThrowNotImplementedException<Task<FileResult>>();

        static Task<FileResult> PlatformCaptureVideoAsync(MediaPickerOptions options) =>
            ThrowHelper.ThrowNotImplementedException<Task<FileResult>>();
    }
}
