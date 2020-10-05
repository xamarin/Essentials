using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        internal static Task PlatformSpeakAsync(string text, SpeechOptions options, CancellationToken cancelToken = default) =>
            ThrowHelper.ThrowNotImplementedException<Task>();

        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync() =>
            ThrowHelper.ThrowNotImplementedException<Task<IEnumerable<Locale>>>();
    }
}
