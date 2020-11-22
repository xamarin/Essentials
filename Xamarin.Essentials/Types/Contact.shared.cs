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

        public Contact(string id, string displayName, IEnumerable<ContactPhone> phones, IEnumerable<ContactEmail> email)
        {
            Id = id;
            DisplayName = displayName;
            Phones.AddRange(phones?.ToList());
            Emails.AddRange(email?.ToList());
        }

        public string Id { get; private set; }

        public string DisplayName { get => displayName; private set => displayName = value; }

        public List<ContactPhone> Phones { get; set; } = new List<ContactPhone>();

        public List<ContactEmail> Emails { get; set; } = new List<ContactEmail>();
    }

    public class ContactEmail
    {
        public ContactEmail()
        {
        }

        public ContactEmail(string value, ContactEmailType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }

        public ContactEmailType Type { get; set; }
    }

    public class ContactPhone
    {
        public ContactPhone()
        {
        }

        public ContactPhone(string value, ContactPhoneType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }

        public ContactPhoneType Type { get; set; }
    }
}
