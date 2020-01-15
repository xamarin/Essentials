using Windows.ApplicationModel.Activation;

namespace Xamarin.Essentials
{
    public static partial class Platform
    {
        internal const string AppManifestFilename = "AppxManifest.xml";
        internal const string AppManifestXmlns = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";

        public static string MapServiceToken { get; set; }

        public static void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                var ea = args as ProtocolActivatedEventArgs;

                WebAuthenticator.OnActivated(ea.Uri);
            }
        }
    }
}
