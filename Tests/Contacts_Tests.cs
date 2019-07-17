using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class Contacts_Tests
    {
        [Fact]
        public async Task Contact_PickContact_Fail_On_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => Contact.PickContactAsync());

        [Fact]
        public async Task Contact_SaveContact_Fail_On_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(
                () => Contact.SaveContactAsync("Xamarin", "55555555", "test@test.com"));

        [Fact]
        public void PhoneContact_Equals_Comparation()
        {
            var emails = GenerateEmails();
            var phones = GeneratePhones();
            var bd = DateTime.Now;
            var contact1 = new PhoneContact("Xamarin", phones, emails, bd);
            var contact2 = new PhoneContact("Xamarin", phones, emails, bd);

            Assert.True(contact1.Equals(contact2));
            Assert.True(contact1 == contact2);
            Assert.False(contact1 != contact2);
            Assert.Equal(contact1, contact2);
            Assert.Equal(contact1.GetHashCode(), contact2.GetHashCode());
        }

        [Fact]
        public void PhoneContact_Not_Equals_Comparation()
        {
            var emails = GenerateEmails();
            var phones = GeneratePhones();
            var contact1 = new PhoneContact("Xamarin", phones, emails, DateTime.Now);
            var contact2 = new PhoneContact("Essentials", phones, emails, DateTime.Now);

            Assert.False(contact1.Equals(contact2));
            Assert.False(contact1 == contact2);
            Assert.True(contact1 != contact2);
            Assert.NotEqual(contact1, contact2);
            Assert.NotEqual(contact1.GetHashCode(), contact2.GetHashCode());
        }

        static Lookup<ContactType, string> GeneratePhones()
        {
            var phonesDictionary = new Dictionary<string, ContactType>
            {
                { "98888888", ContactType.Personal },
                { "89999999", ContactType.Work }
            };

            return (Lookup<ContactType, string>)phonesDictionary.ToLookup(k => k.Value, v => v.Key);
        }

        static Lookup<ContactType, string> GenerateEmails()
        {
            var emailsDictionary = new Dictionary<string, ContactType>
            {
                { "test@test.com", ContactType.Personal },
                { "anothertest@test.com", ContactType.Work }
            };

            return (Lookup<ContactType, string>)emailsDictionary.ToLookup(k => k.Value, v => v.Key);
        }
    }
}
