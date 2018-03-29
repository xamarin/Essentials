using System;
using System.Collections.Generic;
using System.Text;
using Android.Hardware;
using Android.Runtime;

namespace Microsoft.Caboodle
{
    public static partial class Magnetometer
    {
        internal static bool IsSupported =>
               Platform.SensorManager?.GetDefaultSensor(SensorType.MagneticField) != null;

        internal static void PlatformStart(SensorSpeed sensorSpeed, Action<MagnetometerData> handler)
        {
            var delay = SensorDelay.Normal;
            var useSyncContext = false;
            switch (sensorSpeed)
            {
                case SensorSpeed.Normal:
                    delay = SensorDelay.Normal;
                    break;
                case SensorSpeed.Fastest:
                    delay = SensorDelay.Fastest;
                    break;
                case SensorSpeed.Game:
                    delay = SensorDelay.Game;
                    useSyncContext = true;
                    break;
                case SensorSpeed.Ui:
                    delay = SensorDelay.Ui;
                    useSyncContext = true;
                    break;
            }

            var sensorListener = new MagnetometerListener(useSyncContext, handler, delay);

            MonitorCTS.Token.Register(CancelledToken, useSyncContext);

            void CancelledToken()
            {
                sensorListener.Dispose();
                sensorListener = null;
                DisposeToken();
            }
        }
    }

    internal class MagnetometerListener : Java.Lang.Object, ISensorEventListener, IDisposable
    {
        Action<MagnetometerData> handler;
        Sensor magnetometer;
        bool useSyncContext;

        internal MagnetometerListener(bool useSyncContext, Action<MagnetometerData> handler, SensorDelay delay)
        {
            this.useSyncContext = useSyncContext;
            this.handler = handler;
            magnetometer = Platform.SensorManager.GetDefaultSensor(SensorType.MagneticField);
            Platform.SensorManager.RegisterListener(this, magnetometer, delay);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

        public void OnSensorChanged(SensorEvent e)
        {
            var data = new MagnetometerData(e.Values[0], e.Values[1], e.Values[2]);
            if (useSyncContext)
            {
                Platform.BeginInvokeOnMainThread(() => handler?.Invoke(data));
            }
            else
            {
                handler?.Invoke(data);
            }
        }

        bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposed)
            {
                if (disposing)
                {
                    Platform.SensorManager.UnregisterListener(this, magnetometer);
                }

                disposed = true;
            }
        }
    }
}
