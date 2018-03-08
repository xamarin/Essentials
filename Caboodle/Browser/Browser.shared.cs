using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// Shared code between Browser implementations
    /// Contains static methods and shared members
    /// </summary>
    public partial class Browser
    {
        /// <summary>
        /// </summary>
        /// TODO: naming discussion
        /// Simple Launching
        /// Android     - intent.StartActivity(..., Uri)
        /// iOS         - SharedApplcation.OpenUrl(NSUrl)
        /// UWP         - Launcher.LaunchUriAsync(Uri)
        /// suggestion for naming
        /// moljac
        /// UseUriLauncher / UseLanucher
        /// UseSystemBrowser
        public static bool AlwaysUseExternal
        {
            get;
            set;
        }
    }
}
