using System;
using Windows.Devices.Sensors;
using WinBarometer = Windows.Devices.Sensors.Barometer;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        static readonly Lazy<WinBarometer> sensor = new Lazy<WinBarometer>(() => WinBarometer.GetDefault());

        static bool PlatformIsSupported =>
            sensor.Value != null;

        static void PlatformStart()
            => sensor.Value.ReadingChanged += BarometerReportedInterval;

        static void BarometerReportedInterval(object sender, BarometerReadingChangedEventArgs e)
            => OnChanged(new BarometerData(e.Reading.StationPressureInHectopascals));

        static void PlatformStop()
            => sensor.Value.ReadingChanged -= BarometerReportedInterval;
    }
}
