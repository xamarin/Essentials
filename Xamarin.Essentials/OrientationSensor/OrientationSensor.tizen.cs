using Tizen.Sensor;
using TizenRotationVectorSensor = Tizen.Sensor.RotationVectorSensor;

namespace Xamarin.Essentials
{
    public static partial class OrientationSensor
    {
        internal const uint GameInterval = 20;
        internal const uint UiInterval = 60;

        static TizenRotationVectorSensor DefaultSensor
            => (TizenRotationVectorSensor)Platform.GetDefaultSensor(SensorType.OrientationSensor);

        internal static bool IsSupported
            => TizenRotationVectorSensor.IsSupported;

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

        static void DataUpdated(object sender, RotationVectorSensorDataUpdatedEventArgs e)
        {
            OnChanged(new OrientationSensorData(e.X, e.Y, e.Z, e.W));
        }
    }
}
