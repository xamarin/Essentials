using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public class Contact
    {
        string displayName;

        public Contact()
        {
        }

        public Contact(
            string id,
            string namePrefix,
            string givenName,
            string middleName,
            string familyName,
            string nameSuffix,
            IEnumerable<ContactPhone> phones,
            IEnumerable<ContactEmail> email,
            string displayName = null)
        {
            Id = id;
            NamePrefix = namePrefix;
            GivenName = givenName;
            MiddleName = middleName;
            FamilyName = familyName;
            NameSuffix = nameSuffix;
            Phones.AddRange(phones?.ToList());
            Emails.AddRange(email?.ToList());
            DisplayName = displayName;
        }

        public string Id { get; private set; }

        public string DisplayName
        {
            get => !string.IsNullOrWhiteSpace(displayName) ? displayName : $"{GivenName}{GetName(FamilyName)}";
            private set => displayName = value;
        }

        public string NamePrefix { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string FamilyName { get; set; }

        public string NameSuffix { get; set; }

        public List<ContactPhone> Phones { get; set; } = new List<ContactPhone>();

        public List<ContactEmail> Emails { get; set; } = new List<ContactEmail>();

        public override string ToString() => DisplayName;

        string GetName(string name)
            => string.IsNullOrWhiteSpace(name) ? string.Empty : $" {name}";
    }

    public class ContactEmail
    {
        public ContactEmail()
        {
        }

        public ContactEmail(string emailAddress)
            => EmailAddress = emailAddress;

        public string EmailAddress { get; set; }

        public override string ToString() => EmailAddress;
    }

    public class ContactPhone
    {
        public ContactPhone()
        {
        }

        public ContactPhone(string phoneNumber)
            => PhoneNumber = phoneNumber;

        public string PhoneNumber { get; set; }

        public override string ToString() => PhoneNumber;
    }
}
