using System;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Browser
    {
        public static Task OpenAsync(string uri) =>
            OpenAsync(uri, BrowserLaunchingType.SystemBrowser);

        public static Task OpenAsync(string uri, BrowserLaunchingType launchingType)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri), $"Uri cannot be empty.");
            }

            return OpenAsync(new Uri(uri), launchingType);
        }

        public static Task OpenAsync(Uri uri) =>
          OpenAsync(uri, BrowserLaunchingType.SystemBrowser);
    }
}
