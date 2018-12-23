using System;

namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        public static string Model => GetModel();

        public static string Manufacturer => GetManufacturer();

        public static string Name => GetDeviceName();

        public static string VersionString => GetVersionString();

        public static Version Version => Utils.ParseVersion(VersionString);

        public static DevicePlatform Platform => GetPlatform();

        public static DeviceIdiom Idiom => GetIdiom();

        public static DeviceType DeviceType => GetDeviceType();

        const double accelerationThreshold = 2;

        const int shakenInterval = 500;

        static bool shakenLister;

        static DateTime shakenTimeSpan = DateTime.Now;

        public static bool ShakenLister
        {
            get => shakenLister;
            set
            {
                if (shakenLister != value)
                {
                    if (value)
                    {
                        Accelerometer.Start(SensorSpeed.Default);
                        Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                    }
                    else
                    {
                        Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                        Accelerometer.Stop();
                    }
                }
                shakenLister = value;
            }
        }

        static void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var g = Math.Round(e.Reading.Acceleration.X.Square() + e.Reading.Acceleration.Y.Square() + e.Reading.Acceleration.Z.Square());
            if (g > accelerationThreshold && DateTime.Now.Subtract(shakenTimeSpan).Milliseconds > shakenInterval)
            {
                shakenTimeSpan = DateTime.Now;
                OnShaked?.Invoke(null, EventArgs.Empty);
            }
        }

        static double Square(this float q) => q * q;

        public static event EventHandler OnShaked;
    }

    public enum DeviceType
    {
        Unknown = 0,
        Physical = 1,
        Virtual = 2
    }
}
