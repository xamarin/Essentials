using System;
using Android.Views;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        internal static bool IsSupported => true;

        static void PlatformPerform(HapticFeedbackType type)
        {
            Permissions.EnsureDeclared<Permissions.Vibrate>();
            Platform.CurrentActivity?.Window?.DecorView?.PerformHapticFeedback(ConvertType(type));
        }

        static HapticFeedbackGenerator PlatformPrepareGenerator(HapticFeedbackType type)
            => new HapticFeedbackGenerator(() => PlatformPerform(type));

        static FeedbackConstants ConvertType(HapticFeedbackType type) =>
            type switch
            {
                HapticFeedbackType.LongPress => FeedbackConstants.LongPress,
                _ => FeedbackConstants.ContextClick
            };
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
