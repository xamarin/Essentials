using Tizen.Sensor;
using TizenBarometerSensor = Tizen.Sensor.PressureSensor;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        internal const uint GameInterval = 20;
        internal const uint UiInterval = 60;

        static TizenBarometerSensor DefaultSensor
            => (TizenBarometerSensor)Platform.GetDefaultSensor(SensorType.Barometer);

        internal static bool IsSupported
            => TizenBarometerSensor.IsSupported;

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

        static void DataUpdated(object sender, PressureSensorDataUpdatedEventArgs e)
        {
            OnChanged(new BarometerData(e.Pressure));
        }
    }
}
