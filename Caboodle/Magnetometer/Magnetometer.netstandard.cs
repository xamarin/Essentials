using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    public static partial class Magnetometer
    {
        internal static bool IsSupported =>
            throw new NotImplementedInReferenceAssemblyException();

        internal static void PlatformStart(SensorSpeed sensorSpeed, Action<MagnetometerData> handler) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
