using System;
using System.Threading.Tasks;
using AppKit;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        static void PlatformPerform(HapticFeedbackType type)
        {
            if (type == HapticFeedbackType.LongPress)
                Performer.PerformFeedback(NSHapticFeedbackPattern.Generic, NSHapticFeedbackPerformanceTime.Default);
        }

        public static HapticFeedbackGenerator PlatformGetGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
            => new HapticFeedbackGenerator(() => PlatformPerform(type));

        internal static INSHapticFeedbackPerformer Performer => NSHapticFeedbackManager.DefaultPerformer;
    }

    public partial class HapticFeedbackGenerator
    {
        Action perform;

        internal HapticFeedbackGenerator(Action perform)
            => this.perform = perform;

        public virtual void PlatformPerform()
            => perform.Invoke();

        public virtual void PlatformDispose()
            => perform = null;
    }
}
