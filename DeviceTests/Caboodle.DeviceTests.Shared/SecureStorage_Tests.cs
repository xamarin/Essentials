using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Caboodle.DeviceTests
{
    public class SecureStorage_Tests
    {
        [Theory]
        [InlineData("test.txt", "data")]
        [InlineData("noextension", "data2")]
        [InlineData("funny*&$%@!._/\\chars", "data3")]
        public async Task Saves_And_Loads(string key, string data)
        {
            await SecureStorage.SetAsync(key, data);

            var c = await SecureStorage.GetAsync(key);

            Assert.Equal(data, c);
        }
    }
}
