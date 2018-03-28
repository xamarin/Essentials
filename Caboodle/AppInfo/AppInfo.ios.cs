using System;
using System.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Foundation;

namespace Microsoft.Caboodle
{
    public static partial class AppInfo
    {
        static string GetPackageName() => GetBundleValue("CFBundleIdentifier");

        static string GetName() => GetBundleValue("CFBundleDisplayName") ?? GetBundleValue("CFBundleName");

        static string GetVersionString() => GetBundleValue("CFBundleShortVersionString");

        static string GetBuild() => GetBundleValue("CFBundleVersion");

        static string GetBundleValue(string key)
           => NSBundle.MainBundle.ObjectForInfoDictionary(key).ToString();

        static async Task<string> PlatformGetLatestVersionStringAsync()
        {
            var version = string.Empty;
            var url = $"http://itunes.apple.com/lookup?bundleId={GetPackageName()}";

            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(url);

                var appStoreItem = JsonValue.Parse(content);
                version = appStoreItem["results"][0]["version"];
            }

            return version;
        }
    }
}
