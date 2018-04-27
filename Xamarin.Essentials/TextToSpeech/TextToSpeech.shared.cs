using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class TextToSpeech
    {
        internal const float SpeakRateMax = 2.0f;
        internal const float SpeakRateDefault = 1.0f;
        internal const float SpeakRateMin = 0.0f;
        internal const float PitchMax = 2.0f;
        internal const float PitchDefault = 1.0f;
        internal const float PitchMin = 0.0f;
        internal const float VolumeMax = 1.0f;
        internal const float VolumeDefault = 0.5f;
        internal const float VolumeMin = 0.0f;

        public static Task<IEnumerable<Locale>> GetLocalesAsync() =>
            PlatformGetLocalesAsync();

        public static Task SpeakAsync(string text, CancellationToken cancelToken = default) =>
            SpeakAsync(text, default, cancelToken);

        public static Task SpeakAsync(string text, SpeakSettings settings, CancellationToken cancelToken = default)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty string");

            if (settings?.SpeakRate.HasValue ?? false)
            {
                if (settings.SpeakRate.Value < SpeakRateMin || settings.SpeakRate.Value > SpeakRateMax)
                    throw new ArgumentOutOfRangeException($"SpeakRate must be >= {SpeakRateMin} and <= {SpeakRateMax}");
            }

            if (settings?.Volume.HasValue ?? false)
            {
                if (settings.Volume.Value < VolumeMin || settings.Volume.Value > VolumeMax)
                    throw new ArgumentOutOfRangeException($"Volume must be >= {VolumeMin} and <= {VolumeMax}");
            }

            if (settings?.Pitch.HasValue ?? false)
            {
                if (settings.Pitch.Value < PitchMin || settings.Pitch.Value > PitchMax)
                    throw new ArgumentOutOfRangeException($"Pitch must be >= {PitchMin} and <= {PitchMin}");
            }

            return PlatformSpeakAsync(text, settings, cancelToken);
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

        internal static float PlatformNormalize(float min, float max, float percent)
        {
            var range = max - min;
            var add = range * percent;
            return min + add;
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
        public Locale Locale { get; set; }

        public float? Pitch { get; set; }

        public float? SpeakRate { get; set; }

        public float? Volume { get; set; }

        public SpeakSettings SetVolume(TextToSpeech.Volume volume)
        {
            if (volume == TextToSpeech.Volume.Silent)
                Volume = TextToSpeech.VolumeMin;
            if (volume == TextToSpeech.Volume.XSoft)
                Volume = 0.1f;
            else if (volume == TextToSpeech.Volume.Soft)
                Volume = 0.25f;
            else if (volume == TextToSpeech.Volume.Medium)
                Volume = 0.5f;
            else if (volume == TextToSpeech.Volume.Loud)
                Volume = 0.75f;
            else if (volume == TextToSpeech.Volume.XLoud)
                Volume = TextToSpeech.VolumeMax;
            else
                Volume = 0.5f;

            return this;
        }

        public SpeakSettings SetSpeakRate(TextToSpeech.SpeakRate speakRate)
        {
            var ratePercent = 0.5f;

            switch (speakRate)
            {
                case TextToSpeech.SpeakRate.XSlow:
                    ratePercent = 0f;
                    break;
                case TextToSpeech.SpeakRate.Slow:
                    ratePercent = 0.25f;
                    break;
                case TextToSpeech.SpeakRate.Medium:
                    ratePercent = 0.5f;
                    break;
                case TextToSpeech.SpeakRate.Fast:
                    ratePercent = 0.75f;
                    break;
                case TextToSpeech.SpeakRate.XFast:
                    ratePercent = 1.0f;
                    break;
            }

            SpeakRate = ratePercent * TextToSpeech.SpeakRateMax;

            return this;
        }

        public SpeakSettings SetPitch(TextToSpeech.Pitch pitch)
        {
            var pitchPercent = 0.5f;

            switch (pitch)
            {
                case TextToSpeech.Pitch.XLow:
                    pitchPercent = 0f;
                    break;
                case TextToSpeech.Pitch.Low:
                    pitchPercent = 0.25f;
                    break;
                case TextToSpeech.Pitch.Medium:
                    pitchPercent = 0.5f;
                    break;
                case TextToSpeech.Pitch.High:
                    pitchPercent = 0.75f;
                    break;
                case TextToSpeech.Pitch.XHigh:
                    pitchPercent = 1f;
                    break;
            }

            Pitch = pitchPercent * TextToSpeech.PitchMax;

            return this;
        }
    }
}
