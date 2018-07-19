using Tizen.Sensor;
using TizenAccelerometer = Tizen.Sensor.Accelerometer;

namespace Xamarin.Essentials
{
    public static partial class Accelerometer
    {
        internal const uint GameInterval = 20;
        internal const uint UiInterval = 60;

        internal static TizenAccelerometer DefaultSensor =>
            (TizenAccelerometer)Platform.GetDefaultSensor(SensorType.Accelerometer);

        internal static bool IsSupported =>
            TizenAccelerometer.IsSupported;

        static void PlatformStart(SensorSpeed sensorSpeed)
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
                case SensorSpeed.UI:
                    interval = UiInterval;
                    break;
            }

            DefaultSensor.Interval = interval;
            DefaultSensor.DataUpdated += DataUpdated;
            DefaultSensor.Start();
        }

        static void PlatformStop()
        {
            DefaultSensor.DataUpdated -= DataUpdated;
            DefaultSensor.Stop();
        }

        static void DataUpdated(object sender, AccelerometerDataUpdatedEventArgs e)
        {
            OnChanged(new AccelerometerData(e.X, e.Y, e.Z));
        }
    }
}
