using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public class Contact
    {
        internal Contact(string name, IEnumerable<ContactPhone> phones, IEnumerable<ContactEmail> email)
        {
            Name = name;
            Emails = email?.ToList();
            Phones = phones?.ToList();
        }

        public string Name { get; }

        public List<ContactPhone> Phones { get; }

        public List<ContactEmail> Emails { get; }
    }

    public class ContactEmail
    {
        internal ContactEmail(string value, ContactType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }

        public ContactType Type { get; }
    }

    public class ContactPhone
    {
        internal ContactPhone(string value, ContactType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }

        public ContactType Type { get; }
    }
}
