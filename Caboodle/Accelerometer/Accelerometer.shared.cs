using System;

namespace Microsoft.Caboodle
{
    public static partial class Accelerometer
    {
        public static event AccelerometerChangedEventHandler ReadingChanged;

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

        internal static void OnChanged(AccelerometerData reading)
            => OnChanged(new AccelerometerChangedEventArgs(reading));

        internal static void OnChanged(AccelerometerChangedEventArgs e)
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

    public delegate void AccelerometerChangedEventHandler(AccelerometerChangedEventArgs e);

    public class AccelerometerChangedEventArgs : EventArgs
    {
        internal AccelerometerChangedEventArgs(AccelerometerData reading) => Reading = reading;

        public AccelerometerData Reading { get; }
    }

    public struct AccelerometerData
    {
        internal AccelerometerData(double x, double y, double z)
        {
            AccelerometerX = x;
            AccelerometerY = y;
            AccelerometerZ = z;
        }

        public double AccelerometerX { get; }

        public double AccelerometerY { get; }

        public double AccelerometerZ { get; }
    }
}
