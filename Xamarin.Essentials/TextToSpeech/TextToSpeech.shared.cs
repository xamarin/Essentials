using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        public static Task<IEnumerable<Locale>> GetLocalesAsync() =>
            PlatformGetLocalesAsync();

        public static Task SpeakAsync(string text, CancellationToken cancelToken = default) =>
            PlatformSpeakAsync(text, default, cancelToken);

        public static Task SpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default) =>
            PlatformSpeakAsync(text, settings, cancelToken);

        public enum Pitch
        {
            XLow,
            Low,
            Medium,
            High,
            XHigh
        }

        public enum SpeakRate
        {
            XSlow,
            Slow,
            Medium,
            Fast,
            Xast,
            XFast
        }

        public enum Volume
        {
            Silent,
            XSoft,
            Soft,
            Medium,
            Loud,
            XLoud
        }
    }

    public partial struct Locale
    {
        public string Language { get; }

        public string Country { get; }

        public string Name { get; }

        internal Locale(string language, string country, string name)
        {
            Language = language;
            Country = country;
            Name = name;
        }
    }

    public partial class SpeakSettings
    {
        public Locale Locale
        {
            get;
            set;
        }

        float? pitch;

        public float? Pitch
        {
            get
            {
                return pitch;
            }

            set
            {
                if (value < 0 || value > 2)
                    throw new ArgumentOutOfRangeException("Pitch must be >= 0.0f and <= 2.0f");
                pitch = value;
            }
        }

        float? speakRate;

        public float? SpeakRate
        {
            get
            {
                return speakRate;
            }

            set
            {
                if (value < 0 || value > 2)
                    throw new ArgumentOutOfRangeException("SpeakRate must be >= 0.0f and <= 2.0f");

                speakRate = value;
            }
        }

        float? volume;

        public float? Volume
        {
            get
            {
                return volume;
            }

            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Volume must be >= 0.0f and <= 1.0f");

                volume = value;
            }
        }

        public SpeakSettings SetVolume(TextToSpeech.Volume volume)
        {
            if (volume == TextToSpeech.Volume.Silent)
                Volume = 0f;
            if (volume == TextToSpeech.Volume.XSoft)
                Volume = 0.1f;
            else if (volume == TextToSpeech.Volume.Soft)
                Volume = 0.25f;
            else if (volume == TextToSpeech.Volume.Medium)
                Volume = 0.5f;
            else if (volume == TextToSpeech.Volume.Loud)
                Volume = 0.75f;
            else if (volume == TextToSpeech.Volume.XLoud)
                Volume = 1.0f;
            else
                Volume = 0.5f;

            return this;
        }

        public SpeakSettings SetSpeakRate(TextToSpeech.SpeakRate speakRate) =>
            PlatformSetSpeakRate(speakRate);

        public SpeakSettings SetPitch(TextToSpeech.Pitch pitch) =>
            PlatformSetPitch(pitch);
    }
}
