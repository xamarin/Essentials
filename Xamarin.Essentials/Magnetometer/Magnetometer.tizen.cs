using Tizen.Sensor;
using TizenMagnetometer = Tizen.Sensor.Magnetometer;

namespace Xamarin.Essentials
{
    public static partial class Magnetometer
    {
        internal const uint GameInterval = 20;
        internal const uint UiInterval = 60;

        internal static TizenMagnetometer DefaultSensor =>
            (TizenMagnetometer)Platform.GetDefaultSensor(SensorType.Magnetometer);

        internal static bool IsSupported =>
            TizenMagnetometer.IsSupported;

        internal static void PlatformStart(SensorSpeed sensorSpeed)
        {
            uint interval = 0;

            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    interval = (uint)DefaultSensor.MinInterval;
                    break;
                case SensorSpeed.Game:
                    interval = GameInterval;
                    break;
                case SensorSpeed.Ui:
                    interval = UiInterval;
                    break;
            }

            DefaultSensor.Interval = interval;
            DefaultSensor.DataUpdated += DataUpdated;
            DefaultSensor.Start();
        }

        internal static void PlatformStop()
        {
            DefaultSensor.DataUpdated -= DataUpdated;
            DefaultSensor.Stop();
        }

        static void DataUpdated(object sender, MagnetometerDataUpdatedEventArgs e)
        {
            OnChanged(new MagnetometerData(e.X, e.Y, e.Z));
        }
    }
}
