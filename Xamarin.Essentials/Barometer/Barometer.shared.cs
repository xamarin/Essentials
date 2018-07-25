using System;

namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        public static event BarometerChangedEventHandler ReadingChanged;

        public static bool IsMonitoring { get; private set; }

        public static bool IsSupported => PlatformIsSupported;

        public static void Start()
        {
            if (!IsSupported)
               throw new FeatureNotSupportedException();

            if (IsMonitoring)
                return;

            IsMonitoring = true;
            try
            {
                PlatformStart();
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

        internal static void OnChanged(BarometerData reading)
                => OnChanged(new BarometerChangedEventArgs(reading));

        static void OnChanged(BarometerChangedEventArgs e)
            => ReadingChanged?.Invoke(e);
    }

    public delegate void BarometerChangedEventHandler(BarometerChangedEventArgs e);

    public struct BarometerData
    {
        internal BarometerData(double hPAPressure) =>
            Pressure = hPAPressure;

        public double Pressure { get; }
    }

    public class BarometerChangedEventArgs : EventArgs
    {
        internal BarometerChangedEventArgs(BarometerData pressureData)
            => BarometerData = pressureData;

        public BarometerData BarometerData { get; } // In Hectopascals (hPA)
    }
}
