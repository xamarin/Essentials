using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Xamarin.Essentials
{
    public static partial class WebAuthenticator
    {
        static Uri redirectUri;

        static TaskCompletionSource<AuthResult> tcsResponse = null;

        static Task<AuthResult> PlatformAuthenticateAsync(Uri url, Uri callbackUrl)
        {
            // Cancel any previous task that's still pending
            if (tcsResponse?.Task != null && !tcsResponse.Task.IsCompleted)
                tcsResponse.TrySetCanceled();

            redirectUri = callbackUrl;

            tcsResponse = new TaskCompletionSource<AuthResult>();

            if (!IsUriProtocolDeclared(callbackUrl.Scheme))
                throw new InvalidOperationException($"You need to declare the windows.protocol usage of the protocol/scheme `{callbackUrl.Scheme}` in your AppxManifest.xml file");

            Windows.System.Launcher.LaunchUriAsync(url).AsTask();

            return tcsResponse.Task;
        }

        internal static bool OnActivated(Uri uri)
        {
            // If we aren't waiting on a task, don't handle the url
            if (tcsResponse?.Task?.IsCompleted ?? true)
                return false;

            // Only handle the url with our callback uri scheme
            if (!uri.Scheme.Equals(redirectUri.Scheme))
                return false;

            try
            {
                // Parse the account from the url the app opened with
                var r = new AuthResult(uri);

                // Set our account result
                tcsResponse.TrySetResult(r);
                return true;
            }
            catch (Exception ex)
            {
                tcsResponse.TrySetException(ex);
                return false;
            }
        }

        public static bool IsUriProtocolDeclared(string scheme)
        {
            var doc = XDocument.Load(Platform.AppManifestFilename, LoadOptions.None);
            var reader = doc.CreateReader();
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("x", Platform.AppManifestXmlns);
            namespaceManager.AddNamespace("uap", "http://schemas.microsoft.com/appx/manifest/uap/windows10");

            // Check if the protocol was declared
            var decl = doc.Root.XPathSelectElements($"//uap:Extension[@Category='windows.protocol']/uap:Protocol[@Name='{scheme}']", namespaceManager);

            return decl != null && decl.Any();
        }
    }
}
