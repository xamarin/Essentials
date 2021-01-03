using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Views;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        internal static bool IsSupported => true;

        static void PlatformPerform(HapticFeedbackType type)
        {
            Permissions.EnsureDeclared<Permissions.Vibrate>();

            try
            {
                Platform.CurrentActivity?.Window?.DecorView?.PerformHapticFeedback(ConvertType(type));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HapticFeedback Exception: {ex.Message}");
            }
        }

        public static HapticFeedbackGenerator PlatformGetGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
            => new HapticFeedbackGenerator(() => PlatformPerform(type));

        static FeedbackConstants ConvertType(HapticFeedbackType type) =>
            type switch
            {
                HapticFeedbackType.LongPress => FeedbackConstants.LongPress,
                _ => FeedbackConstants.ContextClick
            };
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
