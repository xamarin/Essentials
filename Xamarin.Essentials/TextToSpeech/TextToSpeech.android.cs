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
using Xamarin.Essentials;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        const int maxSpeechInputLengthDefault = 4000;

        static TextToSpeechImplementation textToSpeech;

        public static async Task SpeakAsync(string text, CancellationToken cancelToken = default(CancellationToken)) =>
             await SpeakAsync(text, default(SpeakSettings), cancelToken);

        public static async Task SpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");
            }

            textToSpeech = new TextToSpeechImplementation();
            Initialized = await textToSpeech.Initialize();

            var max = maxSpeechInputLengthDefault;
            if ((int)Build.VERSION.SdkInt >= 18)
            {
                max = global::Android.Speech.Tts.TextToSpeech.MaxSpeechInputLength;
            }

            await textToSpeech.Speak(text, max);

            return;
        }

        public static Task<List<Locale>> GetLocalesAsync()
        {
            // set up the speech to use the default langauge
            // if a language is not available, then the default language is used.

            var locales = new List<Locale>();

            if (textToSpeech != null && TextToSpeech.Initialized)
            {
                if (!Platform.HasApiLevel(BuildVersionCodes.Lollipop))
                {
                    return Task.FromResult<List<Locale>>(locales);
                }
                else
                {
                    try
                    {
                        var localesavailable = Java.Util.Locale.GetAvailableLocales();
                        foreach (var l in localesavailable)
                        {
                            try
                            {
                                var result = textToSpeech.TextToSpeech.IsLanguageAvailable(l);
                                string name = null;

                                switch (result)
                                {
                                    case LanguageAvailableResult.Available:
                                        name = l.DisplayLanguage;
                                        break;
                                    case LanguageAvailableResult.CountryAvailable:
                                        name = l.DisplayLanguage;
                                        break;
                                    case LanguageAvailableResult.CountryVarAvailable:
                                        name = l.DisplayLanguage;
                                        break;
                                }

                                locales.Add(new Locale(l.Language, l.Country, l.DisplayName));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error checking language; " + l + " " + ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var sb = new StringBuilder("Something went horribly wrong, defaulting to old implementation to get languages: ");
                        sb.Append($"{ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"{sb.ToString()}");
                        throw new InvalidProgramException($"{sb.ToString()}");
                    }
                }
            }

            return Task.FromResult(locales);
        }

        static Dictionary<string, string> iso3166Alpha2ToAlpha3Mapping;

        public static Dictionary<string, string> ISO3166Alpha2ToAlpha3Mapping
        {
            get
            {
                if (iso3166Alpha2ToAlpha3Mapping == null)
                {
                    iso3166Alpha2ToAlpha3Mapping =
                    CultureInfo
                        .GetCultures(CultureTypes.SpecificCultures)
                        .Select(region => new RegionInfo(region.LCID))
                        .GroupBy(region => region.ThreeLetterISORegionName)
                        .ToDictionary(group => group.Key, group => group.First().TwoLetterISORegionName);
                }

                return iso3166Alpha2ToAlpha3Mapping;
            }
        }
    }

    internal class TextToSpeechImplementation : Java.Lang.Object, global::Android.Speech.Tts.TextToSpeech.IOnInitListener
    {
        static Java.Util.Locale localedefault = null;

        global::Android.Speech.Tts.TextToSpeech tts;

        public global::Android.Speech.Tts.TextToSpeech TextToSpeech
        {
            get
            {
                return tts;
            }
        }

        TaskCompletionSource<bool> tcs;

        public Locale Locale
        {
            get;
            set;
        }

        public TextToSpeechImplementation()
        {
        }

        public Task<bool> Initialize()
        {
            tcs = new TaskCompletionSource<bool>();
            try
            {
                localedefault = Java.Util.Locale.Default;

                // set up the TextToSpeech object
                // third parameter is the speech engine to use
                tts = new global::Android.Speech.Tts.TextToSpeech(Platform.CurrentContext, this, "com.google.android.tts");
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        public new void Dispose()
        {
            tts.Stop();
            tts.Shutdown();
            tts = null;
        }

        public async Task Speak(string text, int max)
        {
            var parts = text.Split(max);

            await Task.Run(() =>
            {
                foreach (var t in parts)
                {
                    tts.Speak(t, QueueMode.Flush, null, null);
                }
            });

            return;
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
    }

    public partial struct Locale
    {
        public static IEnumerable<Locale> GetLocalesAvailable(string language, string country)
        {
            return Java.Util.Locale.GetAvailableLocales()
                  .Where(a => !string.IsNullOrWhiteSpace(a.Language) && !string.IsNullOrWhiteSpace(a.Country))
                  .Select(a => new Locale(a.Language, a.Country, a.DisplayName))
                  .GroupBy(c => c.ToString())
                  .Select(g => g.First());
        }
    }

    public partial class SpeakSettings
    {
        public float PitchToPlatformSpecific(float pitch)
        {
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
