using System;
using System.Threading;

namespace Microsoft.Caboodle
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            throw new NotImplementedInReferenceAssemblyException();

        public static void Monitor(SensorSpeed sensorSpeed, CancellationToken cancellationToken, Action<double> handler) =>
            throw new NotImplementedInReferenceAssemblyException();

        static void StartListeners() =>
               throw new NotImplementedInReferenceAssemblyException();

        static void StopListeners() =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
