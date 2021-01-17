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

        internal static INSHapticFeedbackPerformer Performer => NSHapticFeedbackManager.DefaultPerformer;
    }
}
