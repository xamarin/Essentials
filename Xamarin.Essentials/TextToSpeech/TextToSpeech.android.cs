﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            if (textToSpeechRef == null || !textToSpeechRef.TryGetTarget(out var tts))
            {
                tts = new TextToSpeechImplementation();
                textToSpeechRef = new WeakReference<TextToSpeechImplementation>(tts);
            }

            return tts;
        }

        internal static Task PlatformSpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default)
        {
            using (var textToSpeech = GetTextToSpeech())
            {
                var max = maxSpeechInputLengthDefault;
                if (Platform.HasApiLevel(BuildVersionCodes.JellyBeanMr2))
                    max = AndroidTextToSpeech.MaxSpeechInputLength;

                return textToSpeech.SpeakAsync(text, max, settings, cancelToken);
            }
        }

        internal static Task<IEnumerable<Locale>> PlatformGetLocalesAsync()
        {
            using (var textToSpeech = GetTextToSpeech())
                return textToSpeech.GetLocalesAsync();
        }
    }

    [Preserve(AllMembers = true)]
    internal class TextToSpeechImplementation : Java.Lang.Object, AndroidTextToSpeech.IOnInitListener,
#pragma warning disable CS0618
        AndroidTextToSpeech.IOnUtteranceCompletedListener
#pragma warning restore CS0618
    {
        AndroidTextToSpeech tts;
        TaskCompletionSource<bool> tcsInitialize;
        TaskCompletionSource<bool> tcsUtterances;

        public TextToSpeechImplementation()
        {
        }

        Task<bool> Initialize()
        {
            if (tcsInitialize != null && tts != null)
                return tcsInitialize.Task;

            tcsInitialize = new TaskCompletionSource<bool>();
            try
            {
                // set up the TextToSpeech object
                tts = new AndroidTextToSpeech(Platform.AppContext, this);
#pragma warning disable CS0618
                tts.SetOnUtteranceCompletedListener(this);
#pragma warning restore CS0618

            }
            catch (Exception e)
            {
                tcsInitialize.SetException(e);
            }

            return tcsInitialize.Task;
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

                        tcsUtterances?.TrySetResult(true);
                    }
                    catch
                    {
                    }
                });
            }

            if (settings?.Locale.Language != null)
            {
                var locale = new Java.Util.Locale(settings.Locale.Language);
                tts.SetLanguage(locale);
            }

            if (settings?.Pitch.HasValue ?? false)
            {
                var pitch = settings.Pitch.Value;
                tts.SetPitch(pitch);
            }

            var parts = text.Split(max);

            numExpectedUtterances = parts.Count;
            tcsUtterances = new TaskCompletionSource<bool>();

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
                tcsInitialize.TrySetResult(true);
            }
            else
            {
                tcsInitialize.TrySetException(new ArgumentException("Failed to initialize Text to Speech engine."));
            }
        }

        public async Task<IEnumerable<Locale>> GetLocalesAsync()
        {
            await Initialize();

            if (Platform.HasApiLevel(BuildVersionCodes.Lollipop))
            {
                try
                {
                    return tts.AvailableLanguages.Select(a => new Locale(a.Language, a.Country, a.DisplayName, string.Empty));
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
                        locales.Add(new Locale(l.Language, l.Country, l.DisplayName, string.Empty));
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
                tcsUtterances?.TrySetResult(true);
        }
    }
}
