using System;
using System.Collections.Generic;
using Android.Hardware;
using Android.Runtime;

namespace Xamarin.Essentials
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            Platform.SensorManager?.GetDefaultSensor(SensorType.Accelerometer) != null &&
            Platform.SensorManager?.GetDefaultSensor(SensorType.MagneticField) != null;

        private static SensorListener listener;
        private static Sensor magnetometer;
        private static Sensor accelerometer;

        private static void PlatformStart(SensorSpeed sensorSpeed)
        {
            var delay = SensorDelay.Normal;
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
                    break;
                case SensorSpeed.Ui:
                    delay = SensorDelay.Ui;
                    break;
            }

            accelerometer = Platform.SensorManager.GetDefaultSensor(SensorType.Accelerometer);
            magnetometer = Platform.SensorManager.GetDefaultSensor(SensorType.MagneticField);
            listener = new SensorListener(accelerometer.Name, magnetometer.Name, delay);
            Platform.SensorManager.RegisterListener(listener, accelerometer, delay);
            Platform.SensorManager.RegisterListener(listener, magnetometer, delay);
        }

        private static void PlatformStop()
        {
            if (listener == null)
                return;

            Platform.SensorManager.UnregisterListener(listener, accelerometer);
            Platform.SensorManager.UnregisterListener(listener, magnetometer);
            listener.Dispose();
            listener = null;
        }
    }

    internal class SensorListener : Java.Lang.Object, ISensorEventListener, IDisposable
    {
        private float[] lastAccelerometer = new float[3];
        private float[] lastMagnetometer = new float[3];
        private bool lastAccelerometerSet;
        private bool lastMagnetometerSet;
        private float[] r = new float[9];
        private float[] orientation = new float[3];

        private string magnetometer;
        private string accelerometer;

        public SensorListener(string accelerometer, string magnetometer, SensorDelay delay)
        {
            this.magnetometer = magnetometer;
            this.accelerometer = accelerometer;
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Name == accelerometer && !lastAccelerometerSet)
            {
                CopyValues(e.Values, lastAccelerometer);
                lastAccelerometerSet = true;
            }
            else if (e.Sensor.Name == magnetometer && !lastMagnetometerSet)
            {
                CopyValues(e.Values, lastMagnetometer);
                lastMagnetometerSet = true;
            }

            if (lastAccelerometerSet && lastMagnetometerSet)
            {
                SensorManager.GetRotationMatrix(r, null, lastAccelerometer, lastMagnetometer);
                SensorManager.GetOrientation(r, orientation);
                var azimuthInRadians = orientation[0];
                var azimuthInDegress = (Java.Lang.Math.ToDegrees(azimuthInRadians) + 360.0) % 360.0;

                var data = new CompassData(azimuthInDegress);
                Compass.OnChanged(data);
                lastMagnetometerSet = false;
                lastAccelerometerSet = false;
            }
        }

        private void CopyValues(IList<float> source, float[] destination)
        {
            for (var i = 0; i < source.Count; ++i)
            {
                destination[i] = source[i];
            }
        }
    }
}
