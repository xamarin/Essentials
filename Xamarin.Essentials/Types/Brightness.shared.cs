using System;
#if WINDOWS_UWP
using UwpBrightness = Windows.Graphics.Display.BrightnessOverride;
#endif

namespace Xamarin.Essentials
{
    [Preserve(AllMembers = true)]
    public readonly partial struct BrightnessOverride : IDisposable, IEquatable<BrightnessOverride>
    {
        internal BrightnessOverride(Brightness oldBrightness, Brightness appliedBrightness)
        {
            OldBrightness = oldBrightness;
            AppliedBrightness = appliedBrightness;
#if WINDOWS_UWP
            @override = null;
#endif
        }

#if WINDOWS_UWP
        readonly UwpBrightness @override;

        public BrightnessOverride(Brightness oldBrightness, Brightness newBrightness, UwpBrightness @override)
            : this(oldBrightness, newBrightness)
        {
            this.@override = @override;
        }

        public void Dispose()
        {
            @override.StopOverride();
        }

        public bool Equals(BrightnessOverride other)
        {
            return OldBrightness.Equals(other.OldBrightness) && AppliedBrightness.Equals(other.AppliedBrightness) && @override.Equals(other.@override);
        }
#endif

        public Brightness OldBrightness { get; }

        public Brightness AppliedBrightness { get; }

#if !WINDOWS_UWP
        public void Dispose()
        {
            AppInfo.SetBrightness(OldBrightness);
        }
#endif

#if !WINDOWS_UWP
        public bool Equals(BrightnessOverride other)
        {
            return OldBrightness.Equals(other.OldBrightness) && AppliedBrightness.Equals(other.AppliedBrightness);
        }
#endif
    }

    [Preserve(AllMembers = true)]
    public readonly struct Brightness
    {
        public double Value { get; }

        public Brightness(double value)
        {
            const double minValue = 0d;
#if __ANDROID__
            const double minValue = -1d;
#endif
            if (value < minValue || value > 1d)
                throw new ArgumentException("Value has to be between (0,1) or less than one on Android", nameof(value));
            Value = value;
        }
    }
}
