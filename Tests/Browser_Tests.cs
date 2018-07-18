using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class BrowserTests
    {
        [Fact]
        public async Task Open_Uri_String_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => Browser.OpenAsync("http://xamarin.com"));

        [Fact]
        public async Task Open_Uri_String_Launch_NetStandard() =>
             await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => Browser.OpenAsync("http://xamarin.com", BrowserLaunchType.SystemPreferred));

        [Fact]
        public async Task Open_Uri_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => Browser.OpenAsync(new Uri("http://xamarin.com")));

        [Fact]
        public async Task Open_Uri_Launch_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => Browser.OpenAsync(new Uri("http://xamarin.com"), BrowserLaunchType.SystemPreferred));

        [Theory]
        [InlineData("https://xamarin.com", "https://xamarin.com")]
        [InlineData("http://xamarin.com", "http://xamarin.com")]
        [InlineData("https://mañana.com", "https://xn--maana-pta.com")]
        [InlineData("http://mañana.com", "http://xn--maana-pta.com")]
        [InlineData("https://mañana.com/?test=mañana", "https://xn--maana-pta.com/?test=ma%C3%B1ana")]
        [InlineData("http://mañana.com/?test=mañana", "http://xn--maana-pta.com/?test=ma%C3%B1ana")]
        public void Escape_Uri(string uri, string escaped)
        {
            var escapedUri = Browser.EscapeUri(new Uri(uri));

            Assert.Equal(escaped, escapedUri.AbsoluteUri.TrimEnd('/'));
        }
    }
}
