using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public partial class Browser
    {
        public static async Task OpenAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException($"Uri cannot be null or empty.");
            }

            await Browser.OpenAsync(new System.Uri(uri));

            return;
        }

        public static async Task OpenAsync(System.Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException($"Uri cannot be null.");
            }

            await Windows.System.Launcher.LaunchUriAsync(uri);

            return;
        }
    }
}
