namespace Xamarin.Essentials
{
    public static partial class Preferences
    {
        static bool PlatformContainsKey(string key, string sharedName) =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        static void PlatformRemove(string key, string sharedName) =>
            ThrowHelper.ThrowNotImplementedException();

        static void PlatformClear(string sharedName) =>
            ThrowHelper.ThrowNotImplementedException();

        static void PlatformSet<T>(string key, T value, string sharedName) =>
            ThrowHelper.ThrowNotImplementedException();

        static T PlatformGet<T>(string key, T defaultValue, string sharedName) =>
            ThrowHelper.ThrowNotImplementedException<T>();
    }
}
