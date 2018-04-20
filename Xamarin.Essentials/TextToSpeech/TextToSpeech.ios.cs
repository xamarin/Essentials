using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync() =>
            Task.FromResult(AVSpeechSynthesisVoice.GetSpeechVoices()
                .Select(v => new Locale(v.Language, null, v.Language)));

        internal static Task PlatformSpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");

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

                var volume = settings.Volume.HasValue ? settings.Volume.Value : platformVolumeDefault;

                speechUtterance = new AVSpeechUtterance(text)
                {
                    Voice = voice,
                    Rate = speechrate,
                    PitchMultiplier = pitch,
                    Volume = volume,
                };
            }

            var speechSynthesizer = new AVSpeechSynthesizer();

            if (cancelToken != null)
            {
                cancelToken.Register(() =>
                {
                    try
                    {
                        speechSynthesizer?.StopSpeaking(AVSpeechBoundary.Immediate);
                    }
                    catch
                    {
                    }
                });
            }

            speechSynthesizer.SpeakUtterance(speechUtterance);

            return Task.CompletedTask;
        }
    }
}
