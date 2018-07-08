using CoreMotion;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        internal static bool IsSupported =>
            CMAltimeter.IsRelativeAltitudeAvailable;

        static CMAltimeter altitudeManager;

        internal static void PlatformStart()
        {
            altitudeManager = new CMAltimeter();
            altitudeManager.StartRelativeAltitudeUpdates(NSOperationQueue.CurrentQueue, LocationManagerUpdatedHeading);
        }

        static void LocationManagerUpdatedHeading(CMAltitudeData e, NSError error)
        {
            var data = new BarometerData(e.Pressure.DoubleValue / 10d); // Convert to HectoPascal from KiloPascal
            OnChanged(data);
        }

        internal static void PlatformStop()
        {
            if (altitudeManager == null)
                return;
            altitudeManager.StopRelativeAltitudeUpdates();
            altitudeManager.Dispose();
            altitudeManager = null;
        }
    }
}
