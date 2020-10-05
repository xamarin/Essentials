namespace Xamarin.Essentials
{
    public static partial class Magnetometer
    {
        internal static bool IsSupported =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        internal static void PlatformStart(SensorSpeed sensorSpeed) =>
            ThrowHelper.ThrowNotImplementedException();

        internal static void PlatformStop() =>
            ThrowHelper.ThrowNotImplementedException();
    }
}
