﻿namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        static string GetDeviceId() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static string GetModel() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static string GetManufacturer() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static string GetDeviceName() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static string GetVersionString() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static DevicePlatform GetPlatform() => DevicePlatform.Unknown;

        static DeviceIdiom GetIdiom() => DeviceIdiom.Unknown;

        static DeviceType GetDeviceType() => DeviceType.Unknown;
    }
}
