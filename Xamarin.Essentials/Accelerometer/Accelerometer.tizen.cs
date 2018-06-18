using Tizen.Sensor;
using TizenAccelerometer = Tizen.Sensor.Accelerometer;

namespace Xamarin.Essentials
{
    public static partial class Accelerometer
    {
        internal const uint FastestInterval = 20;
        internal const uint GameInterval = 40;
        internal const uint UiInterval = 80;

        static TizenAccelerometer sensor;

        internal static bool IsSupported
        {
            get
            {
                return TizenAccelerometer.IsSupported;
            }
        }

        static void PlatformStart(SensorSpeed sensorSpeed)
        {
            sensor = new TizenAccelerometer();

            uint interval = 0;

            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    interval = FastestInterval;
                    break;
                case SensorSpeed.Game:
                    interval = GameInterval;
                    break;
                case SensorSpeed.Ui:
                    interval = UiInterval;
                    break;
            }

            sensor.Interval = interval;
            sensor.DataUpdated += DataUpdated;
            sensor.Start();
        }

        static void PlatformStop()
        {
            sensor.DataUpdated -= DataUpdated;
            sensor.Stop();
            sensor.Dispose();
        }

        static void DataUpdated(object sender, AccelerometerDataUpdatedEventArgs e)
        {
            var data = new AccelerometerData(e.X, e.Y, e.Z);
            OnChanged(data);
        }
    }
}
