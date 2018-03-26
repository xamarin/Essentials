using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CoreLocation;

namespace Microsoft.Caboodle
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            CLLocationManager.HeadingAvailable;

        public static void Monitor(SensorSpeed sensorSpeed, CancellationToken cancellationToken, Action<double> handler)
        {
            if (!IsSupported)
                throw new FeatureNotSupportedException();

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

            cancellationToken.Register(CancelledToken, useSyncContext);

            locationManager.UpdatedHeading += LocationManagerUpdatedHeading;
            locationManager.StartUpdatingHeading();

            void CancelledToken()
            {
                locationManager.UpdatedHeading -= LocationManagerUpdatedHeading;
                locationManager.StopUpdatingHeading();
                locationManager?.Dispose();
                locationManager = null;
            }

            void LocationManagerUpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
            {
                if (useSyncContext)
                {
                    Platform.BeginInvokeOnMainThread(() => handler?.Invoke(e.NewHeading.TrueHeading));
                }
                else
                {
                    handler?.Invoke(e.NewHeading.TrueHeading);
                }
            }
        }
    }
}
