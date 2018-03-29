using System;
using System.Collections.Generic;
using System.Text;
using CoreMotion;
using Foundation;

namespace Microsoft.Caboodle
{
    public static partial class Magnetometer
    { // Timing intervales to match android sensor speeds in seconds
        // https://stackoverflow.com/questions/10044158/android-sensors
        internal const double FastestInterval = .02;
        internal const double GameInterval = .04;
        internal const double UiInterval = .08;
        internal const double NormalInterval = .225;

        internal static bool IsSupported =>
            Platform.MotionManager?.MagnetometerAvailable ?? false;

        internal static void PlatformStart(SensorSpeed sensorSpeed, Action<MagnetometerData> handler)
        {
            var useSyncContext = false;

            var manager = Platform.MotionManager;
            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    manager.MagnetometerUpdateInterval = FastestInterval;
                    break;
                case SensorSpeed.Game:
                    manager.MagnetometerUpdateInterval = GameInterval;
                    break;
                case SensorSpeed.Normal:
                    manager.MagnetometerUpdateInterval = NormalInterval;
                    useSyncContext = true;
                    break;
                case SensorSpeed.Ui:
                    manager.MagnetometerUpdateInterval = UiInterval;
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
