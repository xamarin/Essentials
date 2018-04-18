using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        public static bool Initialized
        {
            get;
            set;
        }

        public static Locale Locale
        {
            get;
            set;
        }

        public static IEnumerable<Locale> Locales
        {
            get;
            set;
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
        Xast
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

    public partial struct SpeakSettings
    {
        public Locale Locale;
        public float? Pitch;
        public float? SpeakRate;
        public float? Volume;

        public float VolumeAsNumeric(Volume v)
        {
            var volume = 0.5f;

            if (v == Essentials.Volume.XSoft)
            {
                volume = 0.1f;
            }
            else if (v == Essentials.Volume.Soft)
            {
                volume = 0.25f;
            }
            else if (v == Essentials.Volume.Silent)
            {
                volume = 0.5f;
            }
            else if (v == Essentials.Volume.Loud)
            {
                volume = 0.75f;
            }
            else if (v == Essentials.Volume.XLoud)
            {
                volume = 1.0f;
            }

            return volume;
        }
    }
}
