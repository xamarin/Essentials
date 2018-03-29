using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Microsoft.Caboodle
{
    public static partial class Magnetometer
    {
        public static bool IsMonitoring =>
               MonitorCTS != null && !MonitorCTS.IsCancellationRequested;

        public static void Start(SensorSpeed sensorSpeed, Action<MagnetometerData> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            PreMonitorValidation();
            CreateToken();
            PlatformStart(sensorSpeed, handler);
        }

        public static void Stop()
        {
            if (MonitorCTS == null)
                return;

            if (!MonitorCTS.Token.CanBeCanceled || MonitorCTS.Token.IsCancellationRequested)
                return;

            MonitorCTS.Cancel();
        }

        internal static CancellationTokenSource MonitorCTS { get; set; }

        internal static void PreMonitorValidation()
        {
            if (!IsSupported)
            {
                throw new FeatureNotSupportedException();
            }

            if (IsMonitoring)
            {
                throw new InvalidOperationException("Magnetometer is already being monitored. Please stop to start a new session.");
            }
        }

        internal static void CreateToken()
        {
            DisposeToken();
            MonitorCTS = new CancellationTokenSource();
        }

        internal static void DisposeToken()
        {
            if (MonitorCTS == null)
                return;

            MonitorCTS.Dispose();
            MonitorCTS = null;
        }
    }

    public struct MagnetometerData
    {
        internal MagnetometerData(double x, double y, double z)
        {
            MagneticFieldX = x;
            MagneticFieldY = y;
            MagneticFieldZ = z;
        }

        public double MagneticFieldX { get; }

        public double MagneticFieldY { get; }

        public double MagneticFieldZ { get; }
    }
}
