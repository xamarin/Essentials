using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class TextToSpeech
    {
        public static bool Initialized
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

    public enum Pithch
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

            if (v == Caboodle.Volume.XSoft)
            {
                volume = 0.1f;
            }
            else if (v == Caboodle.Volume.Soft)
            {
                volume = 0.25f;
            }
            else if (v == Caboodle.Volume.Silent)
            {
                volume = 0.5f;
            }
            else if (v == Caboodle.Volume.Loud)
            {
                volume = 0.75f;
            }
            else if (v == Caboodle.Volume.XLoud)
            {
                volume = 1.0f;
            }

            return volume;
        }
    }
}
