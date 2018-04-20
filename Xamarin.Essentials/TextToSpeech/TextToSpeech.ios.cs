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

        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync() =>
            Task.FromResult(AVSpeechSynthesisVoice.GetSpeechVoices()
                .Select(v => new Locale(v.Language, null, v.Language)));

        internal static Task PlatformSpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");

            var rateMin = AVSpeechUtterance.MinimumSpeechRate;
            var rateMax = AVSpeechUtterance.MaximumSpeechRate;
            var rateDef = AVSpeechUtterance.DefaultSpeechRate;

            AVSpeechUtterance speechUtterance;

            if (settings == null)
            {
                speechUtterance = new AVSpeechUtterance(text)
                {
                    Voice = AVSpeechSynthesisVoice.FromLanguage(enUS),
                    Volume = 0.75f,
                    PitchMultiplier = 1f,
                    Rate = rateDef,
                };
            }
            else
            {
                var voice = AVSpeechSynthesisVoice.FromLanguage(settings.Locale.Language) ??
                    AVSpeechSynthesisVoice.FromLanguage(AVSpeechSynthesisVoice.CurrentLanguageCode);

                if (voice == null)
                    voice = AVSpeechSynthesisVoice.FromLanguage(enUS);

                var pitch = settings.Pitch.HasValue ? settings.Pitch.Value : 1.0f;
                var speechrate = settings.SpeakRate.HasValue ? NormalizeSpeakRate(settings.SpeakRate.Value) : 0.3f;
                var volume = settings.Volume.HasValue ? settings.Volume.Value : 0.5f;

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

        static float NormalizeSpeakRate(float rate)
        {
            var min = AVSpeechUtterance.MinimumSpeechRate;
            var max = AVSpeechUtterance.MaximumSpeechRate;
            var def = AVSpeechUtterance.DefaultSpeechRate;

            if (rate < min)
                return min;
            if (rate > max)
                return max;

            // TODO: We need to normalize the rate on the same scale of min to max from AV

            return rate;
        }
    }

    public partial class SpeakSettings
    {
        internal SpeakSettings PlatformSetSpeakRate(TextToSpeech.SpeakRate speakRate)
        {
            var min = AVSpeechUtterance.MinimumSpeechRate;
            var max = AVSpeechUtterance.MaximumSpeechRate;
            var def = AVSpeechUtterance.DefaultSpeechRate;

            switch (speakRate)
            {
                case TextToSpeech.SpeakRate.XSlow:
                    SpeakRate = min;
                    break;
                case TextToSpeech.SpeakRate.Slow:
                    SpeakRate = min + ((def - min) / 2.0f);
                    break;
                case TextToSpeech.SpeakRate.Medium:
                    SpeakRate = def;
                    break;
                case TextToSpeech.SpeakRate.Fast:
                    SpeakRate = def + ((max - def) / 2.0f);
                    break;
                case TextToSpeech.SpeakRate.XFast:
                    SpeakRate = max;
                    break;
                default:
                    SpeakRate = def;
                    break;
            }

            return this;
        }

        internal SpeakSettings PlatformSetPitch(TextToSpeech.Pitch pitch)
        {
            switch (pitch)
            {
                case TextToSpeech.Pitch.XLow:
                    Pitch = 0.5f;
                    break;
                case TextToSpeech.Pitch.Low:
                    Pitch = 0.7f;
                    break;
                case TextToSpeech.Pitch.Medium:
                    Pitch = 1.0f;
                    break;
                case TextToSpeech.Pitch.High:
                    Pitch = 1.5f;
                    break;
                case TextToSpeech.Pitch.XHigh:
                    Pitch = 2.0f;
                    break;
                default:
                    Pitch = 1.0f;
                    break;
            }

            return this;
        }
    }
}
