namespace Xamarin.Essentials
{
    public static partial class DeviceDisplay
    {
        static bool PlatformKeepScreenOn
        {
            get => ThrowHelper.ThrowNotImplementedException<bool>();
            set => ThrowHelper.ThrowNotImplementedException();
        }

        static DisplayInfo GetMainDisplayInfo() => ThrowHelper.ThrowNotImplementedException<DisplayInfo>();

        static void StartScreenMetricsListeners() => ThrowHelper.ThrowNotImplementedException();

        static void StopScreenMetricsListeners() => ThrowHelper.ThrowNotImplementedException();
    }
}
