using System;
using Android.Hardware;
using Android.Runtime;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        internal static bool IsSupported =>
               Platform.SensorManager?.GetDefaultSensor(SensorType.Pressure) != null;

        static BarometerListener listener;
        static Sensor barometer;

        internal static void PlatformStart()
        {
            barometer = Platform.SensorManager.GetDefaultSensor(SensorType.Pressure);
            listener = new BarometerListener();

            Platform.SensorManager.RegisterListener(listener, barometer, SensorDelay.Normal);
        }

        internal static void PlatformStop()
        {
            if (listener == null)
                return;

            Platform.SensorManager.UnregisterListener(listener, barometer);
            listener.Dispose();
            listener = null;
        }
    }

    class BarometerListener : Java.Lang.Object, ISensorEventListener, IDisposable
    {
        void ISensorEventListener.OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

        void ISensorEventListener.OnSensorChanged(SensorEvent e)
        {
            Barometer.OnChanged(new BarometerData(e.Values[0]));
        }
    }
}
