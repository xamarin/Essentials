using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public partial class Browser
    {
#pragma warning disable CS1998
        public static async Task OpenAsync(string uri) =>
            throw new NotImplentedInReferenceAssembly();

        public static async Task OpenAsync(System.Uri uri) =>
            throw new NotImplentedInReferenceAssembly();

        public static async Task OpenAsync(string uri, BrowserLaunchingType launch_type) =>
            throw new NotImplentedInReferenceAssembly();

        public static async Task OpenAsync(System.Uri uri, BrowserLaunchingType launch_type) =>
            throw new NotImplentedInReferenceAssembly();
#pragma warning restore CS1998
    }
}
