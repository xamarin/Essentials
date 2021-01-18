using System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        public static void Perform(HapticFeedbackType type = HapticFeedbackType.Click)
            => PlatformPerform(type);

        public static HapticFeedbackGenerator PrepareGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
            => PlatformPrepareGenerator(type);
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
        protected internal HapticFeedbackGenerator(HapticFeedbackType type)
            => Type = type;

        public HapticFeedbackType Type { get; }

        public void Perform()
            => PlatformPerform();

        public void Dispose()
        {
            PlatformDispose();
            GC.SuppressFinalize(this);
        }
    }
}
