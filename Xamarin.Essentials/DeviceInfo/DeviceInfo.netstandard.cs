namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        static string GetModel() => ThrowHelper.ThrowNotImplementedException<string>();

        static string GetManufacturer() => ThrowHelper.ThrowNotImplementedException<string>();

        static string GetDeviceName() => ThrowHelper.ThrowNotImplementedException<string>();

        static string GetVersionString() => ThrowHelper.ThrowNotImplementedException<string>();

        static DevicePlatform GetPlatform() => DevicePlatform.Unknown;

        static DeviceIdiom GetIdiom() => DeviceIdiom.Unknown;

        static DeviceType GetDeviceType() => DeviceType.Unknown;
    }
}
