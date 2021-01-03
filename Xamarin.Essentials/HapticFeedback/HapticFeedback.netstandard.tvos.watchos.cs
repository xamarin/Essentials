using System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        static void PlatformPerform(HapticFeedbackType type)
            => throw ExceptionUtils.NotSupportedOrImplementedException;

        static HapticFeedbackGenerator PlatformGetGenerator(HapticFeedbackType type)
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
        public void PlatformPerform()
            => throw ExceptionUtils.NotSupportedOrImplementedException;

        public void PlatformDispose()
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
