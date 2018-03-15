using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class Connectivity_Tests
    {
        [Fact]
        public void Network_Access_On_NetStandard() =>
            Assert.Throws<NotImplentedInReferenceAssembly>(() => Connectivity.NetworkAccess);

        [Fact]
        public void Profiles_On_NetStandard() =>
            Assert.Throws<NotImplentedInReferenceAssembly>(() => Connectivity.Profiles);

        [Fact]
        public void Connectivity_Changed_Event_On_NetStandard() =>
            Assert.Throws<NotImplentedInReferenceAssembly>(() => Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged);

        void Connectivity_ConnectivityChanged(ConnectivityChangedEventArgs e)
        {
        }
    }
}
