namespace Xamarin.Essentials
{
    public static partial class Accelerometer
    {
        internal static bool IsSupported =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        static void PlatformStart(SensorSpeed sensorSpeed) =>
            ThrowHelper.ThrowNotImplementedException();

        static void PlatformStop() =>
            ThrowHelper.ThrowNotImplementedException();
    }
}
