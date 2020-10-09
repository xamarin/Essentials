namespace Xamarin.Essentials
{
    public static partial class Barometer
    {
        internal static bool IsSupported =>
            false;

        internal static void PlatformStart(SensorSpeed sensorSpeed) =>
            throw ExceptionUtils.NotSupportedOrImplementedException;

        internal static void PlatformStop() =>
            throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
