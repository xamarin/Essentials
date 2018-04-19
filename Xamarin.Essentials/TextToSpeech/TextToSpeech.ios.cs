using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        static AVSpeechSynthesizer speechSynthesizer = null;

        public static Task<bool> Initialize()
        {
            if (Initialized)
            {
                return Task<bool>.FromResult(true);
            }

            var tcs = new TaskCompletionSource<bool>();
            try
            {
                var speechSynthesizer = new AVSpeechSynthesizer();

                Initialized = true;
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        public static async Task SpeakAsync(string text, CancellationToken cancelToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                System.Diagnostics.Debug.WriteLine($"{text}");
                var speechUtterance = GetSpeechUtterance(text, null);

                speechUtterance.Voice = AVSpeechSynthesisVoice.FromLanguage("en-US");

                System.Diagnostics.Debug.WriteLine($"     Volume    = {speechUtterance.Volume}");
                System.Diagnostics.Debug.WriteLine($"     SpeakRate = {speechUtterance.Rate}");
                System.Diagnostics.Debug.WriteLine($"     Pitch     = {speechUtterance.PitchMultiplier}");

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
                var speechSynthesizer = new AVSpeechSynthesizer();
                var speechUtterance = GetSpeechUtterance(text, settings);

                System.Diagnostics.Debug.WriteLine($"     Volume    = {speechUtterance.Volume}");
                System.Diagnostics.Debug.WriteLine($"     SpeakRate = {speechUtterance.Rate}");
                System.Diagnostics.Debug.WriteLine($"     Pitch     = {speechUtterance.PitchMultiplier}");

                speechSynthesizer.SpeakUtterance(speechUtterance);
            });

            return;
        }

        static AVSpeechUtterance GetSpeechUtterance(string text, SpeakSettings settings)
        {
            if (settings == null)
            {
                return new AVSpeechUtterance(text)
                {
                    Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                    Volume = 2.0f,
                    PitchMultiplier = 1.0f,
                    Rate = AVSpeechUtterance.MaximumSpeechRate / 5
                };
            }

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
                speechrate = 0.3f;

                // speechrate = AVSpeechUtterance.MaximumSpeechRate / 4;
                // (AVSpeechUtterance.MinimumSpeechRate + AVSpeechUtterance.DefaultSpeechRate)*0.5;
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

        static float PlatformSpecificPitch(float pitch)
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

        static float PlatformSpecificRate(float rate)
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

        static float PlatformSpecificVolume(float volume)
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

        static AVSpeechSynthesisVoice GetVoiceFromLanguage(string language)
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

    public partial class SpeakSettings
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
