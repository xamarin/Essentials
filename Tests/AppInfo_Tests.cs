using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class AppInfo_Tests
    {
        void AppStateDelegate(AppStateChangedEventArgs args)
        {
        }

        [Fact]
        public void State_On_NetStandard()
            => Assert.Throws<NotImplementedInReferenceAssemblyException>(() => AppInfo.State);

        [Fact]
        public void AppStateChanged_Subscribe_Unsubscribe_On_NetStandard()
        {
            Assert.Throws<NotImplementedInReferenceAssemblyException>(
                () => AppInfo.AppStateChanged += AppStateDelegate);
            Assert.Throws<NotImplementedInReferenceAssemblyException>(
                () => AppInfo.AppStateChanged -= AppStateDelegate);
        }
    }
}
