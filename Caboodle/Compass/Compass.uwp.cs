using System;
using System.Threading;
using Windows.Devices.Sensors;

namespace Microsoft.Caboodle
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            Windows.Devices.Sensors.Compass.GetDefault() != null;

        internal static void PlatformMonitor(SensorSpeed sensorSpeed, Action<CompassData> handler)
        {
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

            MonitorCTS.Token.Register(CancelledToken);

            void CancelledToken()
            {
                Platform.BeginInvokeOnMainThread(() =>
                {
                    compass.ReadingChanged -= CompassReportedInterval;
                    DisposeToken();
                });
            }

            compass.ReadingChanged += CompassReportedInterval;

            void CompassReportedInterval(object sender, CompassReadingChangedEventArgs e)
            {
                var data = new CompassData(e.Reading.HeadingMagneticNorth);
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
