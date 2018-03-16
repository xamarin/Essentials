using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Caboodle.Tests
{
    public class Sms_Tests
    {
        [Fact]
        public void Sms_Fail_On_NetStandard()
        {
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Sms.IsComposeSupported);
        }
    }
}
