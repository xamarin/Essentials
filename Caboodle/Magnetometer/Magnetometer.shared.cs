using System;

namespace Microsoft.Caboodle
{
    public static partial class Magnetometer
    {
        public static event MagnetometerChangedEventHandler ReadingChanged;

        public static bool IsMonitoring => isMonitoring;

        public static void Start(SensorSpeed sensorSpeed)
        {
            if (!IsSupported)
            {
                throw new FeatureNotSupportedException();
            }

            if (IsMonitoring)
            {
                return;
            }

            UseSyncContext = sensorSpeed == SensorSpeed.Normal || sensorSpeed == SensorSpeed.Ui;
            PlatformStart(sensorSpeed);
            isMonitoring = true;
        }

        public static void Stop()
        {
            PlatformStop();
            isMonitoring = false;
        }

        static bool isMonitoring;

        internal static bool UseSyncContext { get; set; }

        internal static void OnChanged(MagnetometerData reading)
            => OnChanged(new MagnetometerChangedEventArgs(reading));

        internal static void OnChanged(MagnetometerChangedEventArgs e)
        {
            if (ReadingChanged == null)
                return;

            if (UseSyncContext)
            {
                Platform.BeginInvokeOnMainThread(() => ReadingChanged?.Invoke(e));
            }
            else
            {
                ReadingChanged?.Invoke(e);
            }
        }
    }

    public delegate void MagnetometerChangedEventHandler(MagnetometerChangedEventArgs e);

    public class MagnetometerChangedEventArgs : EventArgs
    {
        internal MagnetometerChangedEventArgs(MagnetometerData reading) => Reading = reading;

        public MagnetometerData Reading { get; }
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
