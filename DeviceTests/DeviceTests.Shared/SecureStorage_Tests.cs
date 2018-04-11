using System.Threading.Tasks;
using Xamarin.Essentials;
using Xunit;

namespace DeviceTests
{
    // TEST NOTES:
    //   - TODO: the iOS simulators have issues with the way we set up our tests... we need to fix that
    public class SecureStorage_Tests
    {
        [Theory]
        [InlineData("test.txt", "data", true)]
        [InlineData("noextension", "data2", true)]
        [InlineData("funny*&$%@!._/\\chars", "data3", true)]
        [InlineData("test.txt2", "data2", false)]
        [InlineData("noextension2", "data22", false)]
        [InlineData("funny*&$%@!._/\\chars2", "data32", false)]
#if __IOS__
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Physical)]
#endif
        public async Task Saves_And_Loads(string key, string data, bool emulatePreApi23)
        {
#if __ANDROID__
            SecureStorage.AlwaysUseAsymmetricKeyStorage = emulatePreApi23;
#endif

            await SecureStorage.SetAsync(key, data);

            var c = await SecureStorage.GetAsync(key);

            Assert.Equal(data, c);
        }
    }
}
