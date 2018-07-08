using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        internal static bool IsSupported =>
            throw new NotImplementedInReferenceAssemblyException();

        internal static void PlatformStart() =>
            throw new NotImplementedInReferenceAssemblyException();

        internal static void PlatformStop() =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
