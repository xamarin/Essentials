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
        public async Task Can_Open_Browser_Uri_As_String(string uri)
        {
            Assert.True(await Launcher.CanOpenAsync(uri));
        }

        [Theory]
        [InlineData("http://www.example.com")]
        public async Task Can_Open_Browser_Uri_As_Uri(string uri)
        {
            Assert.True(await Launcher.CanOpenAsync(new Uri(uri)));
        }

        [Theory]
        [InlineData("LITERAL GARBAGE")]
        public async Task Can_Not_Open_Uri_As_String_Throws_InvalidUriException(string uri)
        {
            await Assert.ThrowsAsync<UriFormatException>(() => Launcher.CanOpenAsync(uri));
        }

        [Theory]
        [InlineData("ms-invalidurifortest:abc")]
        public async Task Can_Not_Open_Uri_As_Uri_No_Target_Found(string uri)
        {
            Assert.False(await Launcher.CanOpenAsync(new Uri(uri)));
        }

        [Theory]
        [InlineData("ms-invalidurifortest:abc")]
        public async Task Can_Not_Open_Uri_As_string_No_Target_Found(string uri)
        {
            Assert.False(await Launcher.CanOpenAsync(uri));
        }
    }
}
