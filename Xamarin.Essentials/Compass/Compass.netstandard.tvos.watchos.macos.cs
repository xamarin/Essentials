namespace Xamarin.Essentials
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        internal static void PlatformStart(SensorSpeed sensorSpeed, bool applyLowPassFilter) =>
            ThrowHelper.ThrowNotImplementedException();

        internal static void PlatformStop() =>
            ThrowHelper.ThrowNotImplementedException();
    }
}
