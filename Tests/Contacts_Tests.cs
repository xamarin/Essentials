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
            var contact1 = new PhoneContact("Xamarin", phones, emails, bd, ContactType.Personal);
            var contact2 = new PhoneContact("Xamarin", phones, emails, bd, ContactType.Personal);

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
            var contact1 = new PhoneContact("Xamarin", phones, emails, DateTime.Now, ContactType.Personal);
            var contact2 = new PhoneContact("Essentials", phones, emails, DateTime.Now, ContactType.Personal);

            Assert.False(contact1.Equals(contact2));
            Assert.False(contact1 == contact2);
            Assert.True(contact1 != contact2);
            Assert.NotEqual(contact1, contact2);
            Assert.NotEqual(contact1.GetHashCode(), contact2.GetHashCode());
        }

        static Lookup<string, string> GeneratePhones()
        {
            var phonesDictionary = new Dictionary<string, string>
            {
                { "98888888", ContactType.Personal.ToString() },
                { "89999999", ContactType.Work.ToString() }
            };

            return (Lookup<string, string>)phonesDictionary.ToLookup(k => k.Value, v => v.Key);
        }

        static Lookup<string, string> GenerateEmails()
        {
            var emailsDictionary = new Dictionary<string, string>
            {
                { "test@test.com", ContactType.Personal.ToString() },
                { "anothertest@test.com", ContactType.Work.ToString() }
            };

            return (Lookup<string, string>)emailsDictionary.ToLookup(k => k.Value, v => v.Key);
        }
    }
}
