using System;
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
                throw new InvalidOperationException("Can nt record on this device.");

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

    public class RecordedAudio : IDisposable
    {
        public string Filepath { get; }

        public bool Disposed { get; private set; }

        internal RecordedAudio(string filePath)
        {
            Filepath = filePath;
        }

#if !NETSTANDARD1_0
        public Stream AsStream() => File.Open(Filepath, FileMode.Open, FileAccess.Read);
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
            if (isDisposing)
                File.Delete(Filepath);
#endif
            Disposed = true;
        }
    }
}
