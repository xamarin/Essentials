using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Recorder
    {
        public static Task<bool> PlatformCanRecordAudio
        => throw new NotImplementedInReferenceAssemblyException();

        static Task PlatformRecordAsync() => throw new NotImplementedInReferenceAssemblyException();

        static Task<RecordedAudio> PlatformStopAsync() => throw new NotImplementedInReferenceAssemblyException();
    }
}
