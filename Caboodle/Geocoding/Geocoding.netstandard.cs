using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public partial class Geocoding
    {
        public static IGeocoding Current { get; set; }

        public static Task<IEnumerable<Placemark>> GetPlacemarksAsync(double latitude, double longitude) =>
            Current?.GetPlacemarksAsync(latitude, longitude) ?? throw new NotImplentedInReferenceAssembly();

        public static Task<IEnumerable<Location>> GetLocationsAsync(string address) =>
            Current?.GetLocationsAsync(address) ?? throw new NotImplentedInReferenceAssembly();
    }

    public interface IGeocoding
    {
        Task<IEnumerable<Placemark>> GetPlacemarksAsync(double latitude, double longitude);

        Task<IEnumerable<Location>> GetLocationsAsync(string address);
    }
}
