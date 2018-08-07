using System;
using System.Linq;
using System.Threading.Tasks;
using Tizen.Applications;

namespace Xamarin.Essentials
{
    public static partial class Browser
    {
        static Task PlatformOpenAsync(Uri uri, BrowserLaunchMode launchType)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            Permissions.EnsureDeclared(PermissionType.LaunchApp);

            var appControl = new AppControl
            {
                Operation = AppControlOperations.View,
                Uri = uri.AbsoluteUri
            };

            var hasMatches = AppControl.GetMatchedApplicationIds(appControl).Any();

            if (hasMatches)
                AppControl.SendLaunchRequest(appControl);

            return Task.FromResult(hasMatches);
        }
    }
}
