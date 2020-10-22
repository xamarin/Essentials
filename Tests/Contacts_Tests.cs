using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class Contacts_Tests
    {
        [Fact]
        public void Contacts_GetAll() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Contacts.GetAllAsync());
    }
}
