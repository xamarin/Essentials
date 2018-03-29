using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;

namespace Microsoft.Caboodle
{
    public static partial class Geolocation
    {
        static async Task<Location> PlatformLastKnownLocationAsync()
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse).ConfigureAwait(false);

            return null;
        }

        static async Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken cancellationToken)
        {
            await Permissions.RequireAsync(PermissionType.LocationWhenInUse).ConfigureAwait(false);

            return null;
        }
    }
}
