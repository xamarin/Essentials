using System;
using System.Numerics;

namespace Xamarin.Essentials
{
    public static partial class Accelerometer
    {
        const int accelerationThreshold = 2;

        const double gravity = 9.81;

        static readonly AccelerometerQueue queue = new AccelerometerQueue();

        static bool useSyncContext;

        public static event EventHandler<AccelerometerChangedEventArgs> ReadingChanged;

        public static event EventHandler ShakeDetected;

        public static bool IsMonitoring { get; private set; }

        public static void Start(SensorSpeed sensorSpeed)
        {
            if (!IsSupported)
                throw new FeatureNotSupportedException();

            if (IsMonitoring)
                throw new InvalidOperationException("Accelerometer has already been started.");

            IsMonitoring = true;
            useSyncContext = sensorSpeed == SensorSpeed.Default || sensorSpeed == SensorSpeed.UI;

            try
            {
                PlatformStart(sensorSpeed);
            }
            catch
            {
                IsMonitoring = false;
                throw;
            }
        }

        public static void Stop()
        {
            if (!IsSupported)
                throw new FeatureNotSupportedException();

            if (!IsMonitoring)
                return;

            IsMonitoring = false;

            try
            {
                PlatformStop();
            }
            catch
            {
                IsMonitoring = true;
                throw;
            }
        }

        internal static void OnChanged(AccelerometerData reading) =>
            OnChanged(new AccelerometerChangedEventArgs(reading));

        internal static void OnChanged(AccelerometerChangedEventArgs e)
        {
            if (useSyncContext)
                MainThread.BeginInvokeOnMainThread(() => ReadingChanged?.Invoke(null, e));
            else
                ReadingChanged?.Invoke(null, e);

            if (ShakeDetected != null)
                ProcessShakeEvent(e.Reading.Acceleration);
        }

        static void ProcessShakeEvent(Vector3 acceleration)
        {
            var x = acceleration.X * -1;
            var y = acceleration.Y * -1;
            var z = acceleration.Z * -1;

            var g = Math.Round(x.Square() + y.Square() + z.Square());
            System.Diagnostics.Debug.WriteLine($"g: {g}");

            var isAccelerating = g > accelerationThreshold;
            System.Diagnostics.Debug.WriteLine($"isAccelerating: {isAccelerating}");
            queue.Add(DateTime.UtcNow.Ticks * 100, isAccelerating);

            if (queue.IsShaking)
            {
                System.Diagnostics.Debug.WriteLine($"IsShaking");
                queue.Clear();
                var args = new EventArgs();

                if (useSyncContext)
                    MainThread.BeginInvokeOnMainThread(() => ShakeDetected?.Invoke(null, args));
                else
                    ShakeDetected?.Invoke(null, args);
            }
        }

        static double Square(this float q) => q * q;
    }

    public class AccelerometerChangedEventArgs : EventArgs
    {
        public AccelerometerChangedEventArgs(AccelerometerData reading) => Reading = reading;

        public AccelerometerData Reading { get; }
    }

    public readonly struct AccelerometerData : IEquatable<AccelerometerData>
    {
        public AccelerometerData(double x, double y, double z)
            : this((float)x, (float)y, (float)z)
        {
        }

        public AccelerometerData(float x, float y, float z) =>
            Acceleration = new Vector3(x, y, z);

        public Vector3 Acceleration { get; }

        public override bool Equals(object obj) =>
            (obj is AccelerometerData data) && Equals(data);

        public bool Equals(AccelerometerData other) =>
            Acceleration.Equals(other.Acceleration);

        public static bool operator ==(AccelerometerData left, AccelerometerData right) =>
            Equals(left, right);

        public static bool operator !=(AccelerometerData left, AccelerometerData right) =>
           !Equals(left, right);

        public override int GetHashCode() =>
            Acceleration.GetHashCode();

        public override string ToString() =>
            $"{nameof(Acceleration.X)}: {Acceleration.X}, " +
            $"{nameof(Acceleration.Y)}: {Acceleration.Y}, " +
            $"{nameof(Acceleration.Z)}: {Acceleration.Z}";
    }
}
