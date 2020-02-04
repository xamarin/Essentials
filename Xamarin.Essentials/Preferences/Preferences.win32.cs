using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Xamarin.Essentials
{
    public static partial class Preferences
    {
        static readonly string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppInfo.Name, "settings.dat");

        static Dictionary<string, Dictionary<string, string>> preferences = new Dictionary<string, Dictionary<string, string>>();

        static Preferences()
        {
            if (File.Exists(settingsPath))
            {
                var json = File.ReadAllText(settingsPath);
                try
                {
                    preferences = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
                }
                catch
                {
                    File.Delete(settingsPath);
                }
            }
            else
            {
                preferences.Add(string.Empty, new Dictionary<string, string>());

                // create folder for app settings
                Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
            }
        }

        static void Save()
        {
            var json = JsonConvert.SerializeObject(preferences);
            File.WriteAllText(settingsPath, json);
        }

        static string CleanSharedName(string sharedName)
        {
            if (string.IsNullOrEmpty(sharedName))
            {
                return string.Empty;
            }

            return sharedName;
        }

        static bool PlatformContainsKey(string key, string sharedName)
        {
            if (preferences.ContainsKey(CleanSharedName(sharedName)))
            {
                return preferences[sharedName].ContainsKey(key);
            }

            return false;
        }

        static void PlatformRemove(string key, string sharedName)
        {
            if (preferences.ContainsKey(CleanSharedName(sharedName)))
            {
                preferences[CleanSharedName(sharedName)].Remove(key);
                Save();
            }
        }

        static void PlatformClear(string sharedName)
        {
            if (preferences.ContainsKey(CleanSharedName(sharedName)))
            {
                preferences[CleanSharedName(sharedName)].Clear();
                Save();
            }
        }

        static void PlatformSet<T>(string key, T value, string sharedName)
        {
            if (!preferences.ContainsKey(CleanSharedName(sharedName)))
            {
                preferences.Add(CleanSharedName(sharedName), new Dictionary<string, string>());
            }

            if (preferences[CleanSharedName(sharedName)].ContainsKey(key))
            {
                preferences[CleanSharedName(sharedName)][key] = JsonConvert.SerializeObject(value);
            }
            else
            {
                preferences[CleanSharedName(sharedName)].Add(key, JsonConvert.SerializeObject(value));
            }

            Save();
        }

        static T PlatformGet<T>(string key, T defaultValue, string sharedName)
        {
            if (preferences.ContainsKey(CleanSharedName(sharedName)))
            {
                if (preferences[CleanSharedName(sharedName)].ContainsKey(key))
                {
                    return JsonConvert.DeserializeObject<T>(preferences[CleanSharedName(sharedName)][key]);
                }
            }

            return defaultValue;
        }
    }
}
