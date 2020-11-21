using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public class Contact
    {
        public Contact()
        {
        }

        public Contact(string name, IEnumerable<ContactPhone> phones, IEnumerable<ContactEmail> email)
        {
            Name = name;
            Emails = email?.ToList();
            Phones = phones?.ToList();
        }

        public string Name { get; set; }

        public List<ContactPhone> Phones { get; set; } = new List<ContactPhone>();

        public List<ContactEmail> Emails { get; set; } = new List<ContactEmail>();
    }

    public class ContactEmail
    {
        public ContactEmail()
        {
        }

        public ContactEmail(string value, ContactType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }

        public ContactType Type { get; set; }
    }

    public class ContactPhone
    {
        public ContactPhone()
        {
        }

        public ContactPhone(string value, ContactType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }

        public ContactType Type { get; set; }
    }
}
