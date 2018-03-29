using System;
using System.Collections.Generic;
using System.Text;
using CoreMotion;
using Foundation;

namespace Microsoft.Caboodle
{
    public static partial class Magnetometer
    { // The angular distance is measured relative to the last delivered heading event. Align with UWP numbers
        internal const double FastestFilter = .01;
        internal const double GameFilter = .5;
        internal const double NormalFilter = 1;
        internal const double UiFilter = 2;

        internal static bool IsSupported =>
            Platform.MotionManager?.MagnetometerAvailable ?? false;

        internal static void PlatformStart(SensorSpeed sensorSpeed, Action<MagnetometerData> handler)
        {
            var useSyncContext = false;

            var manager = Platform.MotionManager;
            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    manager.MagnetometerUpdateInterval = FastestFilter;
                    break;
                case SensorSpeed.Game:
                    manager.MagnetometerUpdateInterval = GameFilter;
                    break;
                case SensorSpeed.Normal:
                    manager.MagnetometerUpdateInterval = NormalFilter;
                    useSyncContext = true;
                    break;
                case SensorSpeed.Ui:
                    manager.MagnetometerUpdateInterval = UiFilter;
                    useSyncContext = true;
                    break;
            }

            MonitorCTS.Token.Register(CancelledToken, useSyncContext);

            manager.StartMagnetometerUpdates(NSOperationQueue.CurrentQueue, DataUpdated);

            void CancelledToken()
            {
                manager?.StopMagnetometerUpdates();
                DisposeToken();
            }

            void DataUpdated(CMMagnetometerData data, NSError error)
            {
                if (data == null)
                    return;

                var field = data.MagneticField;
                var magnetometerData = new MagnetometerData(field.X, field.Y, field.Z);
                if (useSyncContext)
                {
                    Platform.BeginInvokeOnMainThread(() => handler?.Invoke(magnetometerData));
                }
                else
                {
                    handler?.Invoke(magnetometerData);
                }
            }
        }
    }
}
