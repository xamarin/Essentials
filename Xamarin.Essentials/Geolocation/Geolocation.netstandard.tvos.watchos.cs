using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Geolocation
    {
        static Task<Location> PlatformLastKnownLocationAsync() =>
            ThrowHelper.ThrowNotImplementedException<Task<Location>>();

        static Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken cancellationToken) =>
            ThrowHelper.ThrowNotImplementedException<Task<Location>>();
    }
}
