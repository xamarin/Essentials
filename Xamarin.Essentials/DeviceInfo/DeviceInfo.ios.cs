using Foundation;
using ObjCRuntime;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        private static NSObject observer;

        private static string GetModel() => UIDevice.CurrentDevice.Model;

        private static string GetManufacturer() => "Apple";

        private static string GetDeviceName() => UIDevice.CurrentDevice.Name;

        private static string GetVersionString() => UIDevice.CurrentDevice.SystemVersion;

        private static string GetPlatform() => Platforms.iOS;

        private static string GetIdiom()
        {
            switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
            {
                case UIUserInterfaceIdiom.Pad:
                    return Idioms.Tablet;
                case UIUserInterfaceIdiom.Phone:
                    return Idioms.Phone;
                case UIUserInterfaceIdiom.TV:
                    return Idioms.TV;
                case UIUserInterfaceIdiom.CarPlay:
                case UIUserInterfaceIdiom.Unspecified:
                default:
                    return Idioms.Unsupported;
            }
        }

        private static DeviceType GetDeviceType()
            => Runtime.Arch == Arch.DEVICE ? DeviceType.Physical : DeviceType.Virtual;

        private static ScreenMetrics GetScreenMetrics()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            var scale = UIScreen.MainScreen.Scale;

            return new ScreenMetrics
            {
                Width = bounds.Width * scale,
                Height = bounds.Height * scale,
                Density = scale,
                Orientation = CalculateOrientation(),
                Rotation = CalculateRotation()
            };
        }

        private static void StartScreenMetricsListeners()
        {
            var notificationCenter = NSNotificationCenter.DefaultCenter;
            var notification = UIApplication.DidChangeStatusBarOrientationNotification;
            observer = notificationCenter.AddObserver(notification, OnScreenMetricsChanaged);
        }

        private static void StopScreenMetricsListeners()
        {
            observer?.Dispose();
            observer = null;
        }

        private static void OnScreenMetricsChanaged(NSNotification obj)
        {
            var metrics = GetScreenMetrics();
            OnScreenMetricsChanaged(metrics);
        }

        private static ScreenOrientation CalculateOrientation()
        {
            var orientation = UIApplication.SharedApplication.StatusBarOrientation;

            if (orientation.IsLandscape())
                return ScreenOrientation.Landscape;

            return ScreenOrientation.Portrait;
        }

        private static ScreenRotation CalculateRotation()
        {
            var orientation = UIApplication.SharedApplication.StatusBarOrientation;

            switch (orientation)
            {
                case UIInterfaceOrientation.Portrait:
                    return ScreenRotation.Rotation0;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return ScreenRotation.Rotation180;
                case UIInterfaceOrientation.LandscapeLeft:
                    return ScreenRotation.Rotation270;
                case UIInterfaceOrientation.LandscapeRight:
                    return ScreenRotation.Rotation90;
            }

            return ScreenRotation.Rotation0;
        }
    }
}
