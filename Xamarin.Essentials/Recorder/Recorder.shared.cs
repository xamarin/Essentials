using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Recorder
    {
        static Task<bool> CanRecordAudio { get; }

        static bool IsRecording { get; }

        static Task RecordAsync()
        {
            if (IsRecording)
                throw new InvalidOperationException("Already recording.");

            if (!CanRecordAudio)
                throw new InvalidOperationException("Can nt record on this device.");

            return PlatformRecordAsync();
        }

        static Task<RecordedAudio> StopAsync();
    }
}
