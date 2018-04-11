using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;

namespace Microsoft.Caboodle
{
    public static partial class TextToSpeech
    {
        public static async Task SpeakAsync(string text, CancellationToken cancelToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                System.Diagnostics.Debug.WriteLine($"{text}");

                var speechSynthesizer = new AVSpeechSynthesizer();
                var speechUtterance = new AVSpeechUtterance(text)
                {
                    Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                    Volume = 0.75f,
                    PitchMultiplier = 1.0f,
                    Rate = AVSpeechUtterance.MaximumSpeechRate / 5,
                };

                speechSynthesizer.SpeakUtterance(speechUtterance);
            });
        }

        public static async Task SpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");
            }

            await Task.Run(() =>
            {
                System.Diagnostics.Debug.WriteLine($"{text}");
                System.Diagnostics.Debug.WriteLine($"     Volume    = {settings.Volume}");
                System.Diagnostics.Debug.WriteLine($"     SpeakRate = {settings.SpeakRate}");
                System.Diagnostics.Debug.WriteLine($"     Pitch     = {settings.Pitch}");

                var speechSynthesizer = new AVSpeechSynthesizer();
                var speechUtterance = GetSpeechUtterance(text, settings);
                speechSynthesizer.SpeakUtterance(speechUtterance);
            });

            return;
        }

        private static AVSpeechUtterance GetSpeechUtterance(string text, SpeakSettings settings)
        {
            var voice = GetVoiceFromLanguage(settings.Locale.Language);

            if (voice == null)
            {
                voice = AVSpeechSynthesisVoice.FromLanguage("en-US");
            }

            float pitch;
            if (settings.Pitch.HasValue)
            {
                pitch = PlatformSpecificPitch(settings.Pitch.Value);
            }
            else
            {
                pitch = 1.0f;
            }

            float speechrate;
            if (settings.SpeakRate.HasValue)
            {
                speechrate = PlatformSpecificRate(settings.SpeakRate.Value);
            }
            else
            {
                speechrate = AVSpeechUtterance.MaximumSpeechRate / 4;
            }

            float volume;
            if (settings.Volume.HasValue)
            {
                volume = PlatformSpecificVolume(settings.Volume.Value);
            }
            else
            {
                volume = 0.5f;
            }

            var speechUtterance = new AVSpeechUtterance(text)
            {
                Voice = voice,
                Rate = speechrate,
                PitchMultiplier = pitch,
                Volume = volume,
            };

            return speechUtterance;
        }

        private static float PlatformSpecificPitch(float pitch)
        {
            var p = 1.0f;

            if (pitch < 0.0f || pitch > 1.0)
            {
                p = 1.0f;
            }
            else
            {
                p = pitch;
            }

            return p;
        }

        private static float PlatformSpecificRate(float rate)
        {
            var r = 1.0f;

            if (rate < 0.0f || rate > 1.0)
            {
                r = 1.0f;
            }
            else
            {
                r = rate;
            }

            return r;
        }

        private static float PlatformSpecificVolume(float volume)
        {
            var v = 1.0f;

            if (volume < 0.0f || volume > 1.0)
            {
                v = 1.0f;
            }
            else
            {
                v = volume;
            }

            return v;
        }

        private static AVSpeechSynthesisVoice GetVoiceFromLanguage(string language)
        {
            var voice = AVSpeechSynthesisVoice.FromLanguage(language);
            if (voice == null)
            {
                var sb = new StringBuilder($"TextToSpeech");
                sb.Append($"Locale not found for voice: {language} Using default.");
                Debug.WriteLine(sb.ToString());
                voice = AVSpeechSynthesisVoice.FromLanguage(AVSpeechSynthesisVoice.CurrentLanguageCode);
            }

            return voice;
        }

        public static Task<List<Locale>> GetLocalesAsync()
        {
            var voices = AVSpeechSynthesisVoice.GetSpeechVoices();
            var locales = new List<Locale>();

            foreach (var v in voices)
            {
                locales.Add(new Locale(v.Language, null, v.Language));
            }

            return Task.FromResult(locales);
        }
    }

    public partial struct SpeakSettings
    {
        public float PitchToPlatformSpecific(float pitch)
        {
            if (pitch < 0.0 || pitch > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(pitch));
            }

            return pitch;
        }

        public float PitchFromPlatformSpecific(float pitch)
        {
            return pitch;
        }

        public float VolumeToPlatformSpecific(float volume)
        {
            return volume;
        }

        public float VolumeFromPlatformSpecific(float volume)
        {
            return volume;
        }

        public float SpeakRateToPlatformSpecific(float rate)
        {
            return rate;
        }

        public float SpeakRateFromPlatformSpecific(float rate)
        {
            return rate;
        }
    }
}
