using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public partial class Geocoding
    {
        static AsyncLocal<IGeocoding> current;

        public static IGeocoding Current
        {
            get => current.Value;
            set => current.Value = value;
        }

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
