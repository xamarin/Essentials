using System.Collections.Generic;

namespace Xamarin.Essentials
{
    public static partial class Connectivity
    {
        static NetworkAccess PlatformNetworkAccess =>
            ThrowHelper.ThrowNotImplementedException<NetworkAccess>();

        static IEnumerable<ConnectionProfile> PlatformConnectionProfiles =>
            ThrowHelper.ThrowNotImplementedException<IEnumerable<ConnectionProfile>>();

        static void StartListeners() =>
            ThrowHelper.ThrowNotImplementedException();

        static void StopListeners() =>
            ThrowHelper.ThrowNotImplementedException();
    }
}
