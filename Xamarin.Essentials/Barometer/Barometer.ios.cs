using CoreMotion;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        static bool PlatformIsSupported => CMAltimeter.IsRelativeAltitudeAvailable;

        static CMAltimeter altitudeManager;

        static void PlatformStart()
        {
            altitudeManager = new CMAltimeter();
            altitudeManager.StartRelativeAltitudeUpdates(NSOperationQueue.CurrentQueue, LocationManagerUpdatedHeading);
        }

        static void LocationManagerUpdatedHeading(CMAltitudeData e, NSError error)
            => OnChanged(new BarometerData(e.Pressure.DoubleValue / 10d)); // Convert to HectoPascal from KiloPascal

        static void PlatformStop()
        {
            if (altitudeManager == null)
                return;
            altitudeManager.StopRelativeAltitudeUpdates();
            altitudeManager.Dispose();
            altitudeManager = null;
        }
    }
}
