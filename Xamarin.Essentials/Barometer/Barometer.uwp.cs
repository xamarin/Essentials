using Windows.Devices.Sensors;
using WinBarometer = Windows.Devices.Sensors.Barometer;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        static WinBarometer sensor;

        internal static WinBarometer DefaultBarometer =>
            WinBarometer.GetDefault();

        internal static bool IsSupported =>
            DefaultBarometer != null;

        internal static void PlatformStart()
        {
            sensor = DefaultBarometer;

            sensor.ReadingChanged += BarometerReportedInterval;
        }

        static void BarometerReportedInterval(object sender, BarometerReadingChangedEventArgs e)
        {
            var data = new BarometerData(e.Reading.StationPressureInHectopascals);
            OnChanged(data);
        }

        internal static void PlatformStop()
        {
            sensor.ReadingChanged -= BarometerReportedInterval;
        }
    }
}
