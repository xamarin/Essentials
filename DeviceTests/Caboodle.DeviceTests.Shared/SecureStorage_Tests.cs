using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Caboodle.DeviceTests
{
    public class SecureStorage_Tests
    {
#if __ANDROID__
        [Theory]
        [InlineData("test.txt", "data", true)]
        [InlineData("noextension", "data2", true)]
        [InlineData("funny*&$%@!._/\\chars", "data3", true)]
        [InlineData("test.txt2", "data2", false)]
        [InlineData("noextension2", "data22", false)]
        [InlineData("funny*&$%@!._/\\chars2", "data32", false)]
        public async Task Saves_And_Loads(string key, string data, bool emulatePreApi23)
        {
            SecureStorage.AlwaysUseAsymmetricKeyStorage = emulatePreApi23;

            await SecureStorage.SetAsync(key, data);

            var c = await SecureStorage.GetAsync(key);

            Assert.Equal(data, c);
        }
#else
        [Theory]
        [InlineData("test.txt", "data")]
        [InlineData("noextension", "data2")]
        [InlineData("funny*&$%@!._/\\chars", "data3")]
        public async Task Saves_And_Loads(string key, string data)
        {
#if __IOS__
            // Don't run this test on Simulator on iOS
            if (ObjCRuntime.Runtime.Arch != ObjCRuntime.Arch.DEVICE)
                return;
#endif

            await SecureStorage.SetAsync(key, data);

            var c = await SecureStorage.GetAsync(key);

            Assert.Equal(data, c);
        }
#endif
    }
}
