using Xamarin.Essentials;
using Xunit;

namespace Caboodle.Tests
{
    public class TextToSpeech_Tests
    {
        [Fact]
        public void TextToSpeech_Speak_Fail_On_NetStandard() =>
             Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => TextToSpeech.SpeakAsync("Xamarin Essentials!"));
    }
}
