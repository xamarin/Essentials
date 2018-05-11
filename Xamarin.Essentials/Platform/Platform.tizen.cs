using System;
using ElmSharp;
using Tizen.Applications;
using Tizen.System;

namespace Xamarin.Essentials
{
    public static partial class Platform
    {
        internal static Package CurrentPackage
        {
            get
            {
                var packageId = Application.Current.ApplicationInfo.PackageId;
                return PackageManager.GetPackage(packageId);
            }
        }

        static void PlatformBeginInvokeOnMainThread(Action action)
        {
            if (EcoreMainloop.IsMainThread)
                action();
            else
                EcoreMainloop.PostAndWakeUp(action);
        }

        internal static string GetSystemInfo(string item) => GetSystemInfo<string>(item);

        internal static T GetSystemInfo<T>(string item)
        {
            Information.TryGetValue<T>($"http://tizen.org/system/{item}", out var value);
            return value;
        }

        internal static string GetFeatureInfo(string item) => GetFeatureInfo<string>(item);

        internal static T GetFeatureInfo<T>(string item)
        {
            Information.TryGetValue<T>($"http://tizen.org/feature/{item}", out var value);
            return value;
        }
    }
}
