using System;
using System.Threading.Tasks;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        static void PlatformPerform(HapticFeedbackType type)
        {
            var generator = PlatformGetGenerator(type);
            generator?.Perform();
            generator?.Dispose();
        }

        public static HapticFeedbackGenerator PlatformGetGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
        {
            if (!Platform.HasOSVersion(10, 0))
                return null;

            return new ImpactHapticFeedbackGenerator(ConvertType(type));
        }

        static UIImpactFeedbackStyle ConvertType(HapticFeedbackType type) =>
            type switch
            {
                HapticFeedbackType.LongPress => UIImpactFeedbackStyle.Medium,
                _ => UIImpactFeedbackStyle.Light
            };
    }

    public partial class ImpactHapticFeedbackGenerator : HapticFeedbackGenerator
    {
        UIImpactFeedbackGenerator impact;

        internal ImpactHapticFeedbackGenerator(UIImpactFeedbackStyle style)
            : base()
        {
            impact = new UIImpactFeedbackGenerator(style);
            impact.Prepare();
        }

        public override void PlatformPerform()
            => impact.ImpactOccurred();

        public override void PlatformDispose()
        {
            impact?.Dispose();
            impact = null;
        }
    }

    public partial class HapticFeedbackGenerator
    {
        public virtual void PlatformPerform()
        {
        }

        public virtual void PlatformDispose()
        {
        }
    }
}
