using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Recorder
    {
        public static Task<bool> CanRecordAudio => PlatformCanRecordAudio;

        public static bool IsRecording { get; private set; }

        public static async Task RecordAsync()
        {
            if (IsRecording)
                throw new InvalidOperationException("Already recording.");

            if (!await CanRecordAudio)
                throw new InvalidOperationException("Can not record on this device.");
            await Permissions.RequestAsync(PermissionType.RecordAudio);
            await Permissions.RequestAsync(PermissionType.WriteExternalStorage);
            await PlatformRecordAsync();

            IsRecording = true;
        }

        public static async Task<RecordedAudio> StopAsync()
        {
            var recording = await PlatformStopAsync();
            IsRecording = false;
            return recording;
        }
    }

    public class RecordedAudio : FileBase, IDisposable
    {
        static readonly string wavMimeType = "audio/wav";

        public bool Disposed { get; private set; }

#if !NETSTANDARD1_0
        public Stream AsStream() => System.IO.File.Open(FullPath, FileMode.Open, FileAccess.Read);
#endif

        ~RecordedAudio()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (Disposed)
                return;
#if !NETSTANDARD1_0
            if (isDisposing && FullPath != null)
                System.IO.File.Delete(FullPath);
#endif
            Disposed = true;
        }

        internal RecordedAudio(string fullPath)
            : base(fullPath, wavMimeType)
        {
        }
    }
}
