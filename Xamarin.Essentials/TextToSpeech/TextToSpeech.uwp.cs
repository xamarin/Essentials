using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        public static async Task SpeakAsync(string text, CancellationToken cancelToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");
            }

            var mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            var stream = await synth.SynthesizeTextToStreamAsync(text);

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();

            return;
        }

        public static async Task SpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");
            }

            var v = "default";
            if (settings.Volume.HasValue)
            {
                v = SpeakSettings.ProsodyVolume(settings.Volume);
            }

            var p = "default";
            if (settings.Pitch.HasValue)
            {
                p = SpeakSettings.ProsodyPitch(settings.Pitch);
            }

            var r = "default";
            if (settings.SpeakRate.HasValue)
            {
                r = SpeakSettings.ProsodySpeakRate(settings.SpeakRate);
            }

            var lc = SpeakSettings.LocalCode();

            // SSML generation
            var sbssml = new StringBuilder();
            sbssml.AppendLine($"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{lc}'>");
            sbssml.AppendLine($"<prosody pitch='{p}' rate='{r}' volume='{v}'>{text}</prosody> ");
            sbssml.AppendLine($"</speak>");

            System.Diagnostics.Debug.WriteLine(sbssml.ToString());

            var mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            var stream = await synth.SynthesizeSsmlToStreamAsync(sbssml.ToString());

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();

            return;
        }
    }

    public partial struct SpeakSettings
    {
        public static string LocalCode()
        {
            var lc = "en-US";

            var voices = from voice in SpeechSynthesizer.AllVoices
                         where voice.Language == lc && voice.Gender.Equals(VoiceGender.Female)
                         select voice;

            if (!voices.Any())
            {
                lc = SpeechSynthesizer.DefaultVoice.Language;
            }

            return lc;
        }

        public static string ProsodyPitch(float? pitch)
        {
            var p = "default";

            if (!pitch.HasValue)
            {
                p = "default";
            }
            else if (pitch.Value >= 1.6f)
            {
                p = "x-high";
            }
            else if (pitch.Value >= 1.1f)
            {
                p = "high";
            }
            else if (pitch.Value >= .9f)
            {
                p = "medium";
            }
            else if (pitch.Value >= .4f)
            {
                p = "low";
            }
            else
            {
                p = "x-low";
            }

            return p;
        }

        public static string ProsodySpeakRate(float? pitch)
        {
            var r = "default";

            return r;
        }

        public static string ProsodyVolume(float? volume)
        {
            var v = "default";

            if (volume.Value > .8f)
            {
                v = "x-loud";
            }
            else if (volume.Value > 0.6f && volume.Value <= 0.8f)
            {
                v = "loud";
            }
            else if (volume.Value > 0.4f && volume.Value <= 0.6f)
            {
                v = "medium";
            }
            else if (volume.Value > 0.2f && volume.Value <= 0.4f)
            {
                v = "soft";
            }
            else if (volume.Value > 0.0f && volume.Value <= 0.2f)
            {
                v = "x-soft";
            }
            else if (volume.Value < 0.0f)
            {
                v = "silent";
            }

            return v;
        }

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
