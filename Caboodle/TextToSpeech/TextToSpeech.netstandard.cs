using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class TextToSpeech
    {
        public static int MaxSpeechInputLength { get; }

        public static Task SpeakAsync(string text, CancellationToken cancelToken = default(CancellationToken)) =>
            throw new NotImplementedInReferenceAssemblyException();

        public static Task SpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default(CancellationToken)) =>
            throw new NotImplementedInReferenceAssemblyException();

        public static Task GetLocalesAsync() =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
