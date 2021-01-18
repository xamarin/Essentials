﻿using UIKit;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        static void PlatformPerform(HapticFeedbackType type)
        {
            using var generator = PlatformPrepareGenerator(type);
            generator?.Perform();
        }

        static HapticFeedbackGenerator PlatformPrepareGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
        {
            if (!Platform.HasOSVersion(10, 0))
                return null;

            return new ImpactHapticFeedbackGenerator(type);
        }
    }

    public partial class HapticFeedbackGenerator
    {
        protected internal virtual void PlatformPerform()
        {
        }

        protected internal virtual void PlatformDispose()
        {
        }
    }

    class ImpactHapticFeedbackGenerator : HapticFeedbackGenerator
    {
        UIImpactFeedbackGenerator impact;

        internal ImpactHapticFeedbackGenerator(HapticFeedbackType type)
            : base(type)
        {
            impact = new UIImpactFeedbackGenerator(ConvertType(type));
            impact.Prepare();
        }

        protected internal override void PlatformPerform()
            => impact.ImpactOccurred();

        protected internal override void PlatformDispose()
        {
            impact?.Dispose();
            impact = null;
        }

        UIImpactFeedbackStyle ConvertType(HapticFeedbackType type)
            => type switch
            {
                HapticFeedbackType.LongPress => UIImpactFeedbackStyle.Medium,
                _ => UIImpactFeedbackStyle.Light
            };
    }
}
