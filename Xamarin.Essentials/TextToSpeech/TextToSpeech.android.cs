using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.OS;
using Android.Speech.Tts;
using AndroidTextToSpeech = Android.Speech.Tts.TextToSpeech;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        const int maxSpeechInputLengthDefault = 4000;

        static WeakReference<TextToSpeechImplementation> textToSpeechRef = null;

        static TextToSpeechImplementation GetTextToSpeech()
        {
            TextToSpeechImplementation tts = null;
            if (textToSpeechRef == null || !textToSpeechRef.TryGetTarget(out tts))
            {
                tts = new TextToSpeechImplementation();
                textToSpeechRef = new WeakReference<TextToSpeechImplementation>(tts);
            }

            return tts;
        }

        internal static async Task PlatformSpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");

            using (var textToSpeech = GetTextToSpeech())
            {
                var max = maxSpeechInputLengthDefault;
                if (Platform.HasApiLevel(BuildVersionCodes.JellyBeanMr2))
                    max = AndroidTextToSpeech.MaxSpeechInputLength;

                // We need to await this otherwise
                await textToSpeech.SpeakAsync(text, max, settings, cancelToken);
            }
        }

        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync()
        {
            using (var textToSpeech = GetTextToSpeech())
                return textToSpeech.GetLocalesAsync();
        }

        internal static Dictionary<string, string> ISO3166Alpha2ToAlpha3Mapping =>
            CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(region => new RegionInfo(region.LCID))
                .GroupBy(region => region.ThreeLetterISORegionName)
                .ToDictionary(group => group.Key, group => group.First().TwoLetterISORegionName);
    }

    internal class TextToSpeechImplementation : Java.Lang.Object, AndroidTextToSpeech.IOnInitListener,
#pragma warning disable CS0618
        AndroidTextToSpeech.IOnUtteranceCompletedListener
#pragma warning restore CS0618
    {
        AndroidTextToSpeech tts;
        TaskCompletionSource<bool> tcs;
        TaskCompletionSource<object> tcsUtterances;

        public TextToSpeechImplementation()
        {
        }

        Task<bool> Initialize()
        {
            if (tcs != null && tts != null)
                return tcs.Task;

            tcs = new TaskCompletionSource<bool>();
            try
            {
                // set up the TextToSpeech object
                tts = new AndroidTextToSpeech(Platform.CurrentContext, this);
#pragma warning disable CS0618
                tts.SetOnUtteranceCompletedListener(this);
#pragma warning restore CS0618

            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        public new void Dispose()
        {
            tts?.Stop();
            tts?.Shutdown();
            tts = null;
        }

        int numExpectedUtterances = 0;
        int numCompletedUtterances = 0;

        public async Task SpeakAsync(string text, int max, SpeakSettings settings, CancellationToken cancelToken)
        {
            await Initialize();

            // Wait for any previous calls to finish up
            if (tcsUtterances?.Task != null)
                await tcsUtterances.Task;

            if (cancelToken != null)
            {
                cancelToken.Register(() =>
                {
                    try
                    {
                        tts?.Stop();

                        tcsUtterances?.TrySetResult(null);
                    }
                    catch
                    {
                    }
                });
            }

            if (settings != null)
            {
                if (settings.Locale.Language != null)
                {
                    var locale = new Java.Util.Locale(settings.Locale.Language);
                    tts.SetLanguage(locale);
                }

                if (settings.Pitch.HasValue)
                    tts.SetPitch(settings.Pitch.Value);

                if (settings.SpeakRate.HasValue)
                    tts.SetSpeechRate(settings.SpeakRate.Value);
            }

            var parts = text.Split(max);

            numExpectedUtterances = parts.Count;
            tcsUtterances = new TaskCompletionSource<object>();

            var guid = Guid.NewGuid().ToString();

            for (var i = 0; i < parts.Count; i++)
            {
                if (cancelToken.IsCancellationRequested)
                    break;

                // We require the utterance id to be set if we want the completed listener to fire
                var map = new Dictionary<string, string>
                {
                    { AndroidTextToSpeech.Engine.KeyParamUtteranceId, $"{guid}.{i}" }
                };

                if (settings != null && settings.Volume.HasValue)
                    map.Add(AndroidTextToSpeech.Engine.KeyParamVolume, settings.Volume.Value.ToString());

#pragma warning disable CS0618

                // We use an obsolete overload here so it works on older API levels at runtime
                // Flush on first entry and add (to not flush our own previous) subsequent entries
                tts.Speak(parts[i], i == 0 ? QueueMode.Flush : QueueMode.Add, map);
#pragma warning restore CS0618
            }

            await tcsUtterances.Task;
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                tcs.TrySetResult(true);
            }
            else
            {
                tcs.TrySetException(new ArgumentException("Failed to initialize TTS engine"));
            }

            return;
        }

        public async Task<IEnumerable<Locale>> GetLocalesAsync()
        {
            await Initialize();

            if (Platform.HasApiLevel(BuildVersionCodes.Lollipop))
            {
                try
                {
                    return tts.AvailableLanguages.Select(a => new Locale(a.Language, a.Country, a.DisplayName));
                }
                catch
                {
                }
            }

            var locales = new List<Locale>();
            var availableLocales = Java.Util.Locale.GetAvailableLocales();

            foreach (var l in availableLocales)
            {
                try
                {
                    var r = tts.IsLanguageAvailable(l);

                    if (r == LanguageAvailableResult.Available
                        || r == LanguageAvailableResult.CountryAvailable
                        || r == LanguageAvailableResult.CountryVarAvailable)
                    {
                        locales.Add(new Locale(l.Language, l.Country, l.DisplayName));
                    }
                }
                catch
                {
                }
            }

            return locales
                .GroupBy(c => c.ToString())
                .Select(g => g.First());
        }

        public void OnUtteranceCompleted(string utteranceId)
        {
            numCompletedUtterances++;
            if (numCompletedUtterances >= numExpectedUtterances)
                tcsUtterances?.TrySetResult(null);
        }
    }

    public partial class SpeakSettings
    {
        internal SpeakSettings PlatformSetSpeakRate(SpeakRate speakRate)
        {
            switch (speakRate)
            {
                case Essentials.SpeakRate.XSlow:
                    Pitch = 0.3f;
                    break;
                case Essentials.SpeakRate.Slow:
                    Pitch = 0.7f;
                    break;
                case Essentials.SpeakRate.Medium:
                    Pitch = 1.0f;
                    break;
                case Essentials.SpeakRate.Fast:
                    Pitch = 1.3f;
                    break;
                case Essentials.SpeakRate.XFast:
                    Pitch = 2.0f;
                    break;
                default:
                    Pitch = 1.0f;
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
                    Pitch = 1.0f;
                    break;
            }

            return this;
        }
    }
}
