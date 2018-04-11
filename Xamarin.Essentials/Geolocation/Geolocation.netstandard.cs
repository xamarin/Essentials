using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Geolocation
    {
        private static Task<Location> PlatformLastKnownLocationAsync() =>
            throw new NotImplementedInReferenceAssemblyException();

        private static Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken cancellationToken) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
