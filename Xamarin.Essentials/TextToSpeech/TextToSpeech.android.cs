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

        public static int MaxSpeechInputLength
        {
            get
            {
                if ((int)Build.VERSION.SdkInt < 18)
                {
                    return maxSpeechInputLengthDefault;
                }
                else
                {
                    return global::Android.Speech.Tts.TextToSpeech.MaxSpeechInputLength;
                }
            }
        }

        static global::Android.Speech.Tts.TextToSpeech textToSpeech;

        static TextToSpeech()
        {
            Initialize();

            return;
        }

        static Java.Util.Locale localedefault = null;

        static async void Initialize()
        {
            // set up the TextToSpeech object
            // third parameter is the speech engine to use
            textToSpeech = new global::Android.Speech.Tts.TextToSpeech(Platform.CurrentContext, default(TextToSpeechListener), "com.google.android.tts");

            var locales = await GetLocalesAsync();

            // set up the speech to use the default langauge
            // if a language is not available, then the default language is used.
            localedefault = Java.Util.Locale.Default;
            textToSpeech.SetLanguage(localedefault);

            return;
        }

        public static async Task SpeakAsync(string text, CancellationToken cancelToken = default(CancellationToken)) =>
             await SpeakAsync(text, default(SpeakSettings), cancelToken);

        public static async Task SpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");
            }

            await Task.Run(() =>
            {
                // set the speed and pitch
                textToSpeech.SetPitch(settings.Pitch.Value);
                textToSpeech.SetSpeechRate(settings.SpeakRate.Value);

                var bundle = Bundle.Empty;
                string stringutteranceid = null;

                if (settings.Volume.HasValue)
                {
                }

                textToSpeech.Speak(text, QueueMode.Flush, bundle, stringutteranceid);
            });

            return;
        }

        public static async Task SpeakAsync1(string text, SpeakSettings settings, CancellationToken cancelToken = default(CancellationToken))
        {
            if (text.Length >= TextToSpeech.MaxSpeechInputLength)
            {
                throw new ArgumentException(nameof(text), "Text length too long!");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(settings.Locale.Language))
            {
                Java.Util.Locale locale = null;
                if (!string.IsNullOrWhiteSpace(settings.Locale.Country))
                {
                    locale = new Java.Util.Locale(settings.Locale.Language, settings.Locale.Country, null);
                }
                else
                {
                    locale = new Java.Util.Locale(settings.Locale.Language, null, null);
                }

                var result = textToSpeech.IsLanguageAvailable(locale);
                if (result == LanguageAvailableResult.CountryAvailable)
                {
                    textToSpeech.SetLanguage(locale);
                }
                else
                {
                    Console.WriteLine("Locale: " + locale + " was not valid, setting to default.");
                }
            }
            else
            {
            }

            await Task.Run(() => { });

            return;
        }

        public static Task<List<Locale>> GetLocalesAsync()
        {
            var locales = new List<Locale>();

            if (textToSpeech != null && TextToSpeech.Initialized)
            {
                var sdk = (int)global::Android.OS.Build.VERSION.SdkInt;
                if (sdk < 21)
                {
                    return Task.FromResult<List<Locale>>(locales);
                }
                else if (sdk == 21)
                {
                    var locales_21 = from l in textToSpeech.AvailableLanguages
                                     select new Locale(l.Language, l.Country, l.DisplayName);

                    locales = locales_21.ToList();
                }
                else if (sdk > 21)
                {
                    try
                    {
                        var localesavailable = Java.Util.Locale.GetAvailableLocales();
                        foreach (var l in localesavailable)
                        {
                            try
                            {
                                var result = textToSpeech.IsLanguageAvailable(l);
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
            else
            {
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

    internal class TextToSpeechListener : Java.Lang.Object, global::Android.Speech.Tts.TextToSpeech.IOnInitListener, IDisposable
    {
        public global::Android.Speech.Tts.TextToSpeech TextToSpeechNative
        {
            get;
            set;
        }

        public Java.Util.Locale LocaleNative
        {
            get;
            set;
        }

        public new void Dispose()
        {
            TextToSpeechNative.Stop();
            TextToSpeechNative = null;
        }

        public void OnInit(OperationResult status)
        {
            if (status == OperationResult.Success)
            {
                TextToSpeechNative.SetLanguage(LocaleNative);
                TextToSpeech.Initialized = true;
            }
            else if (status == OperationResult.Error)
            {
                TextToSpeechNative.SetLanguage(Java.Util.Locale.Default);
            }
            else
            {
                throw new InvalidOperationException("TextToSpeech could not be initialized");
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

    public partial struct SpeakSettings
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
