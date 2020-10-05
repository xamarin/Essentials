using System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        public static void Perform(HapticFeedbackType type = HapticFeedbackType.Click)
        {
            if (!IsSupported)
               ThrowHelper.ThrowNotImplementedException();
            PlatformPerform(type);
        }
    }
}
