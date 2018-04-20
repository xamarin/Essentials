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
                    v = (settings.Volume.Value * 100f).ToString();

                if (settings.Pitch.HasValue)
                    p = ProsodyPitch(settings.Pitch);

                if (settings.SpeakRate.HasValue)
                    r = settings.SpeakRate.Value.ToString();
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

            if (pitch.Value <= 0.25f)
                return "x-low";
            else if (pitch.Value > 0.25f && pitch.Value <= 0.75f)
                return "low";
            else if (pitch.Value > 0.75f && pitch.Value <= 1.25f)
                return "medium";
            else if (pitch.Value > 1.25f && pitch.Value <= 1.75f)
                return "high";
            else if (pitch.Value > 1.75f)
                return "x-high";

            return "default";
        }
    }
}
