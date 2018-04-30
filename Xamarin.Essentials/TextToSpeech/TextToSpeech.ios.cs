using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        static AVSpeechSynthesizer speechSynthesizer;
        static TaskCompletionSource<bool> currentSpeak;
        static SemaphoreSlim semaphore;

        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync() =>
            Task.FromResult(AVSpeechSynthesisVoice.GetSpeechVoices()
                .Select(v => new Locale(v.Language, null, v.Language)));

        internal static async Task PlatformSpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default)
        {
            if (speechSynthesizer == null)
                speechSynthesizer = new AVSpeechSynthesizer();

            if (semaphore == null)
                semaphore = new SemaphoreSlim(1, 1);

            try
            {
                await semaphore.WaitAsync(cancelToken);
                var speechUtterance = GetSpeechUtterance(text, settings);
                await SpeakUtterance(speechUtterance, cancelToken);
            }
            finally
            {
                if (semaphore.CurrentCount == 0)
                    semaphore.Release();
            }
        }

        private static AVSpeechUtterance GetSpeechUtterance(string text, SpeakSettings settings)
        {
            var speechUtterance = new AVSpeechUtterance(text);

            if (settings != null)
            {
                // null voice if fine - it is the default
                speechUtterance.Voice =
                    AVSpeechSynthesisVoice.FromLanguage(settings.Locale.Language) ??
                    AVSpeechSynthesisVoice.FromLanguage(AVSpeechSynthesisVoice.CurrentLanguageCode);

                // the platform has a range of 0.5 - 2.0
                // anything lower than 0.5 is set to 0.5
                if (settings.Pitch.HasValue)
                    speechUtterance.PitchMultiplier = settings.Pitch.Value;

                // TODO: this is nothing useful
                speechUtterance.Rate = settings.SpeakRate.HasValue ?
                    PlatformNormalize(AVSpeechUtterance.MinimumSpeechRate, AVSpeechUtterance.MaximumSpeechRate, settings.SpeakRate.Value / SpeakRateMax) :
                    AVSpeechUtterance.DefaultSpeechRate;

                if (settings.Volume.HasValue)
                    speechUtterance.Volume = settings.Volume.Value;
            }

            return speechUtterance;
        }

        internal static async Task SpeakUtterance(AVSpeechUtterance speechUtterance, CancellationToken cancelToken)
        {
            try
            {
                currentSpeak = new TaskCompletionSource<bool>();

                speechSynthesizer.DidFinishSpeechUtterance += OnFinishedSpeechUtterance;
                speechSynthesizer.SpeakUtterance(speechUtterance);
                using (cancelToken.Register(TryCancel))
                {
                    await currentSpeak.Task;
                }
            }
            finally
            {
                speechSynthesizer.DidFinishSpeechUtterance -= OnFinishedSpeechUtterance;
            }
        }

        static void OnFinishedSpeechUtterance(object sender, AVSpeechSynthesizerUteranceEventArgs args) =>
            currentSpeak?.TrySetResult(true);

        static void TryCancel()
        {
            speechSynthesizer?.StopSpeaking(AVSpeechBoundary.Word);
            currentSpeak?.TrySetCanceled();
        }
    }
}
