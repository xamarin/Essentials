using System.Collections.Generic;

namespace Xamarin.Essentials
{
    public static partial class Preferences
    {
        static Dictionary<string, object> preferences = new Dictionary<string, object>();

        static bool PlatformContainsKey(string key, string sharedName) =>
            preferences.ContainsKey(key);

        static void PlatformRemove(string key, string sharedName) =>
            preferences.Remove(key);

        static void PlatformClear(string sharedName) =>
            preferences.Clear();

        static void PlatformSet<T>(string key, T value, string sharedName)
        {
            if (preferences.ContainsKey(key))
            {
                preferences[key] = value;
            }
            else
            {
                preferences.Add(key, value);
            }
        }

        static T PlatformGet<T>(string key, T defaultValue, string sharedName)
        {
            if (preferences.ContainsKey(key))
            {
                return (T)preferences[key];
            }

            return default;
        }
    }
}
