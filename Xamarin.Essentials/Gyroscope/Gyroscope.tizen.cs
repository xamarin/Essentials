using Tizen.Sensor;
using TizenGyroscope = Tizen.Sensor.Gyroscope;

namespace Xamarin.Essentials
{
    public static partial class Gyroscope
    {
        internal const uint GameInterval = 20;
        internal const uint UiInterval = 60;

        internal static TizenGyroscope DefaultSensor =>
            (TizenGyroscope)Platform.GetDefaultSensor(SensorType.Gyroscope);

        internal static bool IsSupported =>
            TizenGyroscope.IsSupported;

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
                case SensorSpeed.UI:
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

        static void DataUpdated(object sender, GyroscopeDataUpdatedEventArgs e)
        {
            OnChanged(new GyroscopeData(e.X, e.Y, e.Z));
        }
    }
}
