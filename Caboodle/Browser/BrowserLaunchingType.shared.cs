using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// BrowserLaunchingType - enum for opening Uri links
    /// </summary>
    /// <remarks>
    /// Simple Launching
    ///     Android     - intent.StartActivity(..., Uri)
    ///     iOS         - SharedApplcation.OpenUrl(NSUrl)
    ///     UWP         - Launcher.LaunchUriAsync(Uri)
    /// TODO: naming discussion
    /// </remarks>
    public enum BrowserLaunchingType
    {
        UriLauncher,

        // Android (CustomTabs) iOS (SFSafariViewController)
        SystemBrowser
    }
}
