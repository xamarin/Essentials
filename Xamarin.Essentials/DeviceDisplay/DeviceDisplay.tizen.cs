using System;
using System.Runtime.InteropServices;
using ElmSharp;

namespace Xamarin.Essentials
{
    public static partial class DeviceDisplay
    {
        [DllImport("libcapi-system-device.so.0", EntryPoint = "device_power_request_lock", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ScreenLockDevicePowerRequestLock(int type, int timeout);

        [DllImport("libcapi-system-device.so.0", EntryPoint = "device_power_release_lock", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ScreenLockDevicePowerReleaseLock(int type);

        static bool screenLock = false;

        static bool PlatformKeepScreenOn
        {
            get
            {
                if (!screenLock)
                    ScreenLockDevicePowerReleaseLock(1);
                return screenLock;
            }

            set
            {
                if (value)
                    ScreenLockDevicePowerRequestLock(1, 0);
                else
                    ScreenLockDevicePowerReleaseLock(1);
                screenLock = value;
            }
        }

        [DllImport("libcapi-appfw-application.so.0", EntryPoint = "app_get_device_orientation")]
        internal static extern int AppGetDeviceOrientation();

        static DisplayInfo GetMainDisplayInfo()
        {
            return new DisplayInfo(
                width: Platform.MainWindow != null ? Platform.MainWindow.ScreenSize.Width : Platform.GetFeatureInfo<int>("screen.width"),
                height: Platform.MainWindow != null ? Platform.MainWindow.ScreenSize.Height : Platform.GetFeatureInfo<int>("screen.height"),
                density: GetDensity(),
                orientation: GetOrientation(),
                rotation: GetRotation());
        }

        static double GetDensity()
        {
            var dpi = Platform.MainWindow != null ? Platform.MainWindow.ScreenDpi.X : Platform.GetFeatureInfo<int>("screen.dpi");
            var unit = DeviceInfo.Idiom == DeviceIdiom.TV ? 72.0 : 160.0;
            return dpi / unit;
        }

        static DisplayOrientation GetOrientation()
        {
            var orientation = Platform.MainWindow != null ? Platform.MainWindow.Rotation : AppGetDeviceOrientation();
            return orientation switch
            {
                0 => DisplayOrientation.Portrait,
                90 => DisplayOrientation.Landscape,
                180 => DisplayOrientation.Portrait,
                270 => DisplayOrientation.Landscape,
                _ => DisplayOrientation.Unknown,
            };
        }

        static DisplayRotation GetRotation()
        {
            var orientation = Platform.MainWindow != null ? Platform.MainWindow.Rotation : AppGetDeviceOrientation();
            return orientation switch
            {
                0 => DisplayRotation.Rotation0,
                90 => DisplayRotation.Rotation90,
                180 => DisplayRotation.Rotation180,
                270 => DisplayRotation.Rotation270,
                _ => DisplayRotation.Unknown,
            };
        }

        static void StartScreenMetricsListeners()
        {
            if (Platform.MainWindow != null)
                Platform.MainWindow.RotationChanged += OnRotationChanged;
        }

        static void StopScreenMetricsListeners()
        {
            if (Platform.MainWindow != null)
                Platform.MainWindow.RotationChanged -= OnRotationChanged;
        }

        static void OnRotationChanged(object s, EventArgs e)
        {
            var metrics = GetMainDisplayInfo();
            OnMainDisplayInfoChanged(metrics);
        }
    }
}
