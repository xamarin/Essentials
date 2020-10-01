using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xunit;

namespace DeviceTests
{
    public class Share_Tests
    {
        [Fact]
        public async Task Share_NullText()
        {
            string text = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => Share.RequestAsync(text));
        }

        [Fact]
        public async Task Share_NullRequest()
        {
            ShareTextRequest request = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => Share.RequestAsync(request));
        }
    }
}
