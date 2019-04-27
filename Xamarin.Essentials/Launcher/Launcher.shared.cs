using System;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Launcher
    {
        public static Task<bool> CanOpenAsync(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentNullException(nameof(uri));

            return PlatformCanOpenAsync(new Uri(uri));
        }

        public static Task<bool> CanOpenAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return PlatformCanOpenAsync(uri);
        }

        public static Task OpenAsync(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentNullException(nameof(uri));

            return PlatformOpenAsync(new Uri(uri));
        }

        public static Task OpenAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return PlatformOpenAsync(uri);
        }

        public static Task OpenAsync(OpenFileRequest request)
        {
            ExperimentalFeatures.VerifyEnabled(ExperimentalFeatures.ShareFileRequest);

            return PlatformOpenAsync(request);
        }
    }

    public class OpenFileRequest
    {
        public OpenFileRequest()
        {
            ExperimentalFeatures.VerifyEnabled(ExperimentalFeatures.OpenFileRequest);
        }

        public OpenFileRequest(LauncherFile file)
        {
            ExperimentalFeatures.VerifyEnabled(ExperimentalFeatures.OpenFileRequest);
            File = file;
        }

        public OpenFileRequest(FileBase file)
        {
            ExperimentalFeatures.VerifyEnabled(ExperimentalFeatures.OpenFileRequest);
            File = new LauncherFile(file);
        }

        public LauncherFile File { get; set; }
    }

    public class LauncherFile : FileBase
    {
        public LauncherFile(string fullPath)
            : base(fullPath)
        {
            ExperimentalFeatures.VerifyEnabled(ExperimentalFeatures.OpenFileRequest);
        }

        public LauncherFile(string fullPath, string contentType)
            : base(fullPath, contentType)
        {
            ExperimentalFeatures.VerifyEnabled(ExperimentalFeatures.OpenFileRequest);
        }

        public LauncherFile(FileBase file)
            : base(file)
        {
            ExperimentalFeatures.VerifyEnabled(ExperimentalFeatures.OpenFileRequest);
        }

        string attachmentName;

        public string AttachmentName
        {
            get => GetAttachmentName();
            set => attachmentName = value;
        }

        internal string GetAttachmentName()
        {
            // try the provided file name
            if (!string.IsNullOrWhiteSpace(attachmentName))
                return attachmentName;

            // try get from the path
            if (!string.IsNullOrWhiteSpace(FullPath))
                return Path.GetFileName(FullPath);

            // this should never happen as the path is validated in the constructor
            throw new InvalidOperationException($"Unable to determine the attachment file name from '{FullPath}'.");
        }
    }
}
