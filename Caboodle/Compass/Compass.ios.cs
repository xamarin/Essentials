using System;
using CoreLocation;

namespace Microsoft.Caboodle
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            CLLocationManager.HeadingAvailable;

        internal static void PlatformMonitor(SensorSpeed sensorSpeed, Action<CompassData> handler)
        {
            var useSyncContext = false;

            var locationManager = new CLLocationManager();
            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    locationManager.HeadingFilter = .1;
                    locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
                    break;
                case SensorSpeed.Game:
                    locationManager.HeadingFilter = .5;
                    locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
                    break;
                case SensorSpeed.Normal:
                    locationManager.HeadingFilter = 1;
                    locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
                    useSyncContext = true;
                    break;
                case SensorSpeed.Ui:
                    locationManager.HeadingFilter = 1.5;
                    locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
                    useSyncContext = true;
                    break;
            }

            MonitorCTS.Token.Register(CancelledToken, useSyncContext);

            locationManager.UpdatedHeading += LocationManagerUpdatedHeading;
            locationManager.StartUpdatingHeading();

            void CancelledToken()
            {
                locationManager.UpdatedHeading -= LocationManagerUpdatedHeading;
                locationManager.StopUpdatingHeading();
                locationManager?.Dispose();
                locationManager = null;
                DisposeToken();
            }

            void LocationManagerUpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
            {
                var data = new CompassData(e.NewHeading.MagneticHeading);
                if (useSyncContext)
                {
                    Platform.BeginInvokeOnMainThread(() => handler?.Invoke(data));
                }
                else
                {
                    handler?.Invoke(data);
                }
            }
        }
    }
}
