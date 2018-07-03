using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xunit;

namespace DeviceTests
{
    // TEST NOTES:
    //   - a human needs to close the browser window
    public class Launcher_Tests
    {
        [Theory]
        [InlineData("http://www.example.com")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public Task Open_Run_On_UiThread(string uri)
        {
            return Utils.OnMainThread(() => Launcher.OpenAsync(uri));
        }

        [Theory]
        [InlineData("http://www.example.com")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public async Task Can_Open_Browser_Uri_As_String(string uri)
        {
            var success = await Launcher.CanOpenAsync(uri).ConfigureAwait(false);
            Assert.True(success);
        }

        [Theory]
        [InlineData("http://www.example.com")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public async Task Can_Open_Browser_Uri_As_Uri(string uri)
        {
            var success = await Launcher.CanOpenAsync(new Uri(uri));
            Assert.True(success);
        }

        [Theory]
        [InlineData("LITERAL GARBAGE")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public async Task Can_Not_Open_Browser_Uri_As_String(string uri)
        {
            var success = await Launcher.CanOpenAsync(uri).ConfigureAwait(false);
            Assert.False(success);
        }

        [Theory]
        [InlineData("LITERAL GARBAGE")]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public async Task Can_Not_Open_Browser_Uri_As_Uri(string uri)
        {
            await Assert.ThrowsAsync<UriFormatException>(() => Launcher.CanOpenAsync(new Uri(uri))).ConfigureAwait(false);
        }
    }
}
