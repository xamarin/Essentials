using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public class Contact
    {
        public Contact(string name, IEnumerable<ContactPhone> phones, IEnumerable<ContactEmail> email)
        {
            Name = name;
            Emails = email?.ToList();
            Phones = phones?.ToList();
        }

        public string Name { get; }

        public List<ContactPhone> Phones { get; } = new List<ContactPhone>();

        public List<ContactEmail> Emails { get; } = new List<ContactEmail>();
    }

    public class ContactEmail
    {
        public ContactEmail(string value, ContactType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }

        public ContactType Type { get; }
    }

    public class ContactPhone
    {
        public ContactPhone(string value, ContactType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }

        public ContactType Type { get; }
    }
}
