using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        internal static bool IsSupported
            => ThrowHelper.ThrowNotImplementedException<bool>();

        static void PlatformPerform(HapticFeedbackType type)
            => ThrowHelper.ThrowNotImplementedException();
    }
}
