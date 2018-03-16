using System;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Browser
    {
        public static Task OpenAsync(Uri uri, BrowserLaunchingType launchType)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri), "Uri cannot be null.");
            }

            return Windows.System.Launcher.LaunchUriAsync(uri).AsTask();
        }
    }
}
