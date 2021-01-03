using System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        public static void Perform(HapticFeedbackType type = HapticFeedbackType.Click)
            => PlatformPerform(type);

        public static HapticFeedbackGenerator GetGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
            => PlatformGetGenerator(type);
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
        protected HapticFeedbackGenerator()
        {
        }

        public void Perform()
            => PlatformPerform();

        public void Dispose()
        {
            PlatformDispose();
            GC.SuppressFinalize(this);
        }
    }
}
