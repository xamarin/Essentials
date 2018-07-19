using Tizen.Sensor;
using TizenOrientationSensor = Tizen.Sensor.OrientationSensor;

namespace Xamarin.Essentials
{
    public static partial class Compass
    {
        internal const uint GameInterval = 20;
        internal const uint UiInterval = 60;

        internal static TizenOrientationSensor DefaultSensor =>
            (TizenOrientationSensor)Platform.GetDefaultSensor(SensorType.OrientationSensor);

        internal static bool IsSupported =>
            TizenOrientationSensor.IsSupported;

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

        static void DataUpdated(object sender, OrientationSensorDataUpdatedEventArgs e)
        {
            OnChanged(new CompassData(e.Azimuth));
        }
    }
}
