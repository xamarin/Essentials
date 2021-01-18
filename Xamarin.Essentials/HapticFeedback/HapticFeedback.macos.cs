using System;
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

        static HapticFeedbackGenerator PlatformPrepareGenerator(HapticFeedbackType type)
            => new HapticFeedbackGenerator(() => PlatformPerform(type));

        internal static INSHapticFeedbackPerformer Performer => NSHapticFeedbackManager.DefaultPerformer;
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
        Action perform;

        internal HapticFeedbackGenerator(Action perform)
            => this.perform = perform;

        void PlatformPerform()
            => perform.Invoke();

        void PlatformDispose()
            => perform = null;
    }
}
