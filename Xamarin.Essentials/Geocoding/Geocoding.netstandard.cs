using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Geocoding
    {
        static Task<IEnumerable<Placemark>> PlatformGetPlacemarksAsync(double latitude, double longitude) =>
            ThrowHelper.ThrowNotImplementedException<Task<IEnumerable<Placemark>>>();

        static Task<IEnumerable<Location>> PlatformGetLocationsAsync(string address) =>
            ThrowHelper.ThrowNotImplementedException<Task<IEnumerable<Location>>>();
    }
}
