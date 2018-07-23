using System.Threading.Tasks;
using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class Maps_Tests
    {
        const double testLatitude = 52.51852;
        const double testLongitude = 13.37621;
        const string mapName = "Bundestag";

        [Fact]
        public async Task Open_Map_LatLong_NetStandard() =>
          await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(
              () => Map.OpenMapsAsync(
                  testLatitude,
                  testLongitude,
                  new MapsOpenOptions { Name = mapName }));

        [Fact]
        public async Task Open_Map_Location_NetStandard() =>
          await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(
              () => Map.OpenMapsAsync(
                  new Location(testLatitude, testLongitude),
                  new MapsOpenOptions { Name = mapName }));

        [Fact]
        public async Task Open_Map_Placemark_NetStandard() =>
          await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(
              () => Map.OpenMapsAsync(
                  new Placemark(),
                  new MapsOpenOptions { Name = mapName }));
    }
}
