using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Sensors;
using WindowsMagnetometer = Windows.Devices.Sensors.Magnetometer;

namespace Microsoft.Caboodle
{
    public static partial class Magnetometer
    {
        // Magic numbers from https://docs.microsoft.com/en-us/uwp/api/windows.devices.sensors.compass.reportinterval#Windows_Devices_Sensors_Compass_ReportInterval
        internal const uint FastestInterval = 8;
        internal const uint GameInterval = 22;
        internal const uint NormalInterval = 33;

        internal static WindowsMagnetometer DefaultMagnetometer =>
            WindowsMagnetometer.GetDefault();

        internal static bool IsSupported =>
            DefaultMagnetometer != null;

        internal static void PlatformStart(SensorSpeed sensorSpeed, Action<MagnetometerData> handler)
        {
            var sensor = DefaultMagnetometer;
            var useSyncContext = false;
            var interval = NormalInterval;
            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    interval = FastestInterval;
                    break;
                case SensorSpeed.Game:
                    interval = GameInterval;
                    break;
                default:
                    useSyncContext = true;
                    break;
            }

            sensor.ReportInterval = sensor.MinimumReportInterval >= interval ? interval : sensor.MinimumReportInterval;

            MonitorCTS.Token.Register(CancelledToken);

            void CancelledToken()
            {
                Platform.BeginInvokeOnMainThread(() =>
                {
                    sensor.ReadingChanged -= DataUpdated;
                    DisposeToken();
                });
            }

            sensor.ReadingChanged += DataUpdated;

            void DataUpdated(object sender, MagnetometerReadingChangedEventArgs e)
            {
                var reading = e.Reading;
                var data = new MagnetometerData(reading.MagneticFieldX, reading.MagneticFieldY, reading.MagneticFieldZ);
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
