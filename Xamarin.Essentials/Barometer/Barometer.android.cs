using System;
using Android.Hardware;
using Android.Runtime;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        internal static bool PlatformIsSupported =>
               barometer.Value != null;

        static readonly Lazy<Sensor> barometer = new Lazy<Sensor>(
            () => Platform.SensorManager.GetDefaultSensor(SensorType.Pressure));

        static BarometerListener listener;

        static void PlatformStart()
        {
            listener = new BarometerListener();
            Platform.SensorManager.RegisterListener(listener, barometer.Value, SensorDelay.Normal);
        }

        static void PlatformStop()
        {
            if (listener == null)
                return;

            Platform.SensorManager.UnregisterListener(listener, barometer.Value);
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
            => Barometer.OnChanged(new BarometerData(e.Values[0]));
    }
}
