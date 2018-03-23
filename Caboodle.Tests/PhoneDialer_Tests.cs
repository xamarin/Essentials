using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class PhoneDialer_Tests
    {
        [Fact]
        public void Dialer_Supported_Fail_On_NetStandard() =>
            Assert.Throws<NotImplentedInReferenceAssembly>(() => PhoneDialer.IsSupported);

        [Fact]
        public void Dialer_Open_Fail_On_NetStandard() =>
            Assert.Throws<NotImplentedInReferenceAssembly>(() => PhoneDialer.Open("1234567890"));
    }
}
