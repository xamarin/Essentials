using System;

namespace Xamarin.Essentials
{
    public static partial class Vibration
    {
        internal static bool IsSupported
            => throw new NotImplementedInReferenceAssemblyException();

        private static void PlatformVibrate(TimeSpan duration)
            => throw new NotImplementedInReferenceAssemblyException();

        private static void PlatformCancel()
            => throw new NotImplementedInReferenceAssemblyException();
    }
}
