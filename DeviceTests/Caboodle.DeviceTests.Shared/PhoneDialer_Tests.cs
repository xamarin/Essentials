using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class PhoneDialer_Tests
    {
        [Fact]
        public void Dialer_Is_Supported() => Assert.True(PhoneDialer.IsSupported);
    }
}
