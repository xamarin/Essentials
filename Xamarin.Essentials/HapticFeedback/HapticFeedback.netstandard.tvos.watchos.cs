using System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        static void PlatformPerform(HapticFeedbackType type)
            => throw ExceptionUtils.NotSupportedOrImplementedException;

        static HapticFeedbackGenerator PlatformPrepareGenerator(HapticFeedbackType type)
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
        void PlatformPerform()
            => throw ExceptionUtils.NotSupportedOrImplementedException;

        void PlatformDispose()
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
