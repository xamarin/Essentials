using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Xamarin.Essentials
{
    public static partial class Preferences
    {
        static readonly string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppInfo.Name, "settings.dat");

        static readonly Dictionary<string, Dictionary<string, object>> preferences = new Dictionary<string, Dictionary<string, object>>();

        static Preferences()
        {
            if (File.Exists(settingsPath))
            {
                using (var stream = File.OpenRead(settingsPath))
                {
                    try
                    {
                        var formatter = new BinaryFormatter();
                        var readPreferences = (Dictionary<string, Dictionary<string, object>>)formatter.Deserialize(stream);
                        stream.Close();

                        if (readPreferences is object)
                        {
                            preferences = readPreferences;
                        }
                    }
                    catch (SerializationException)
                    {
                        // if deserialization fails proceed with empty settings
                    }
                }
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(settingsPath)))
                {
                    // create folder for app settings
                    Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
                }
            }

            if (!preferences.ContainsKey(string.Empty))
            {
                preferences.Add(string.Empty, new Dictionary<string, object>());
            }
        }

        static void Save()
        {
            using (var stream = File.OpenWrite(settingsPath))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, preferences);
                stream.Close();
            }
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
                preferences.Add(CleanSharedName(sharedName), new Dictionary<string, object>());
            }

            if (preferences[CleanSharedName(sharedName)].ContainsKey(key))
            {
                preferences[CleanSharedName(sharedName)][key] = value;
            }
            else
            {
                preferences[CleanSharedName(sharedName)].Add(key, value);
            }

            Save();
        }

        static T PlatformGet<T>(string key, T defaultValue, string sharedName)
        {
            if (preferences.ContainsKey(CleanSharedName(sharedName)))
            {
                if (preferences[CleanSharedName(sharedName)].ContainsKey(key))
                {
                    return (T)preferences[CleanSharedName(sharedName)][key];
                }
            }

            return defaultValue;
        }
    }
}
