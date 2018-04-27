using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        const string enUS = "en-US";
        const float platformPitchMin = 0f;
        const float platformPitchMax = 2f;
        const float platformPitchDefault = 1f;
        const float platformVolumeDefault = 1f;

        static AVSpeechSynthesizer speechSynthesizer;
        static TaskCompletionSource<object> currentSpeak;
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
            AVSpeechUtterance speechUtterance;

            if (settings == null)
            {
                speechUtterance = new AVSpeechUtterance(text)
                {
                    Voice = AVSpeechSynthesisVoice.FromLanguage(enUS),
                    Volume = platformVolumeDefault,
                    PitchMultiplier = platformPitchDefault,
                    Rate = AVSpeechUtterance.DefaultSpeechRate,
                };
            }
            else
            {
                var voice = AVSpeechSynthesisVoice.FromLanguage(settings.Locale.Language) ??
                    AVSpeechSynthesisVoice.FromLanguage(AVSpeechSynthesisVoice.CurrentLanguageCode);

                if (voice == null)
                    voice = AVSpeechSynthesisVoice.FromLanguage(enUS);

                var pitch = settings.Pitch.HasValue ?
                    PlatformNormalize(platformPitchMin, platformPitchMax, settings.Pitch.Value / platformPitchMax) : platformPitchDefault;

                var speechrate = settings.SpeakRate.HasValue ?
                    PlatformNormalize(AVSpeechUtterance.MinimumSpeechRate, AVSpeechUtterance.MaximumSpeechRate, settings.SpeakRate.Value / SpeakRateMax) :
                    AVSpeechUtterance.DefaultSpeechRate;

                var volume = settings.Volume ?? platformVolumeDefault;

                speechUtterance = new AVSpeechUtterance(text)
                {
                    Voice = voice,
                    Rate = speechrate,
                    PitchMultiplier = pitch,
                    Volume = volume,
                };
            }

            return speechUtterance;
        }

        internal static async Task SpeakUtterance(AVSpeechUtterance speechUtterance, CancellationToken cancelToken)
        {
            try
            {
                currentSpeak = new TaskCompletionSource<object>();

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
            currentSpeak?.TrySetResult(null);

        static void TryCancel()
        {
            speechSynthesizer?.StopSpeaking(AVSpeechBoundary.Word);
            currentSpeak?.TrySetCanceled();
        }
    }
}
