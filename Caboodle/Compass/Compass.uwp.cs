using System;
using System.Threading;
using Windows.Devices.Sensors;

namespace Microsoft.Caboodle
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            Windows.Devices.Sensors.Compass.GetDefault() != null;

        public static void Monitor(SensorSpeed sensorSpeed, CancellationToken cancellationToken, Action<double> handler)
        {
            if (!IsSupported)
                throw new FeatureNotSupportedException();

            var useSyncContext = false;

            var compass = Windows.Devices.Sensors.Compass.GetDefault();

            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    compass.ReportInterval = compass.MinimumReportInterval >= 8 ? 8 : compass.MinimumReportInterval;
                    break;
                case SensorSpeed.Game:
                    compass.ReportInterval = compass.MinimumReportInterval >= 22 ? 22 : compass.MinimumReportInterval;
                    break;
                case SensorSpeed.Normal:
                case SensorSpeed.Ui:
                    compass.ReportInterval = compass.MinimumReportInterval >= 33 ? 33 : compass.MinimumReportInterval;
                    useSyncContext = true;
                    break;
            }

            cancellationToken.Register(CancelledToken);

            void CancelledToken()
            {
                Platform.BeginInvokeOnMainThread(() =>
                {
                    compass.ReadingChanged -= CompassReportedInterval;
                });
            }

            compass.ReadingChanged += CompassReportedInterval;

            void CompassReportedInterval(object sender, CompassReadingChangedEventArgs e)
            {
                if (useSyncContext)
                {
                    Platform.BeginInvokeOnMainThread(() => handler?.Invoke(e.Reading.HeadingTrueNorth.Value));
                }
                else
                {
                    handler?.Invoke(e.Reading.HeadingTrueNorth.Value);
                }
            }
        }
    }
}
