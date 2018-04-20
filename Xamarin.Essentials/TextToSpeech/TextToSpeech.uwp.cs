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
        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync() =>
            Task.FromResult(SpeechSynthesizer.AllVoices.Select(v => new Locale(v.Language, null, v.DisplayName)));

        internal static async Task PlatformSpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");

            var ssml = GetSpeakParametersSSMLProsody(text, settings);

            var mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();
            var stream = await synth.SynthesizeSsmlToStreamAsync(ssml.ToString());

            if (cancelToken != null)
            {
                cancelToken.Register(() =>
                {
                    try
                    {
                        mediaElement?.Stop();
                    }
                    catch
                    {
                    }
                });
            }

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }

        static string GetSpeakParametersSSMLProsody(string text, SpeakSettings settings)
        {
            var v = "default";
            var p = "default";
            var r = "default";

            // Look for the specified language, otherwise the default voice
            var lc = settings?.Locale.Language ?? SpeechSynthesizer.DefaultVoice.Language;

            if (settings != null)
            {
                if (settings.Volume.HasValue)
                    v = ProsodyVolume(settings.Volume);

                if (settings.Pitch.HasValue)
                    p = ProsodyPitch(settings.Pitch);

                if (settings.SpeakRate.HasValue)
                    r = ProsodySpeakRate(settings.SpeakRate);
            }

            // SSML generation
            var sbssml = new StringBuilder();
            sbssml.AppendLine($"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{lc}'>");
            sbssml.AppendLine($"<prosody pitch='{p}' rate='{r}' volume='{v}'>{text}</prosody> ");
            sbssml.AppendLine($"</speak>");

            return sbssml.ToString();
        }

        static string ProsodyPitch(float? pitch)
        {
            if (!pitch.HasValue)
                return "default";
            else if (pitch.Value >= 1.6f)
                return "x-high";
            else if (pitch.Value >= 1.1f)
                return "high";
            else if (pitch.Value >= .9f)
                return "medium";
            else if (pitch.Value >= .4f)
                return "low";

            return "x-low";
        }

        static string ProsodySpeakRate(float? speakRate)
        {
            var r = "default";

            if (!speakRate.HasValue)
                return r;
            else if (speakRate <= 0.3f)
                return "x-slow";
            else if (speakRate > 0.3f && speakRate <= 0.7f)
                return "slow";
            else if (speakRate > 0.7f && speakRate <= 1.0f)
                return "medium";
            else if (speakRate > 1.0f && speakRate <= 1.5f)
                return "fast";
            else if (speakRate > 1.5f)
                return "x-fast";

            return r;
        }

        static string ProsodyVolume(float? volume)
        {
            if (volume.Value > .8f)
                return "x-loud";
            else if (volume.Value > 0.6f && volume.Value <= 0.8f)
                return "loud";
            else if (volume.Value > 0.4f && volume.Value <= 0.6f)
                return "medium";
            else if (volume.Value > 0.2f && volume.Value <= 0.4f)
                return "soft";
            else if (volume.Value > 0.0f && volume.Value <= 0.2f)
                return "x-soft";
            else if (volume.Value < 0.0f)
                return "silent";

            return "default";
        }
    }

    public partial class SpeakSettings
    {
        internal SpeakSettings PlatformSetSpeakRate(SpeakRate speakRate)
        {
            switch (speakRate)
            {
                case Essentials.SpeakRate.XSlow:
                    SpeakRate = 0.3f;
                    break;
                case Essentials.SpeakRate.Slow:
                    SpeakRate = 0.7f;
                    break;
                case Essentials.SpeakRate.Medium:
                    SpeakRate = 1.0f;
                    break;
                case Essentials.SpeakRate.Fast:
                    SpeakRate = 1.5f;
                    break;
                case Essentials.SpeakRate.XFast:
                    SpeakRate = 2.0f;
                    break;
                default:
                    SpeakRate = 1.0f;
                    break;
            }

            return this;
        }

        internal SpeakSettings PlatformSetPitch(Pitch pitch)
        {
            switch (pitch)
            {
                case Essentials.Pitch.XLow:
                    Pitch = 0.3f;
                    break;
                case Essentials.Pitch.Low:
                    Pitch = 0.7f;
                    break;
                case Essentials.Pitch.Medium:
                    Pitch = 1.0f;
                    break;
                case Essentials.Pitch.High:
                    Pitch = 1.3f;
                    break;
                case Essentials.Pitch.XHigh:
                    Pitch = 1.6f;
                    break;
                default:
                    Pitch = null;
                    break;
            }

            return this;
        }
    }
}
