using System.Linq;
using Xamarin.Essentials;
using Xunit;

namespace DeviceTests
{
    public class Connectivity_Tests
    {
        [Fact]
        public void Network_Access() =>
            Assert.Equal(NetworkAccess.Internet, Connectivity.NetworkAccess);

        [Fact]
        public void ConnectionProfiles() =>
            Assert.True(Connectivity.ConnectionProfiles.Count() > 0);

        [Fact]
        public void WiFiSignale()
        {
            var strength = Connectivity.WiFi.SignalStrength;
            if (Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi))
                Assert.True(strength != SignalStrength.None && strength != SignalStrength.Unknown);
            else
                Assert.True(strength == SignalStrength.None || strength == SignalStrength.Unknown);
        }
    }
}
