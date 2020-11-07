using System;
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

        public IReadOnlyList<ContactPropertyBase> Phones { get; }

        public IReadOnlyList<ContactEmail> Emails { get; }

        public override string ToString() => Name;
    }

    public class ContactEmail : ContactPropertyBase
    {
        internal ContactEmail(string value, ContactType type, string platformSpecificType)
            : base(value, type, platformSpecificType)
        {
        }
    }

    public class ContactPhone : ContactPropertyBase
    {
        internal ContactPhone(string value, ContactType type, string platformSpecificType)
            : base(value, type, platformSpecificType)
        {
        }
    }

    public class ContactPropertyBase
    {
        internal ContactPropertyBase(string value, ContactType type, string platformSpecificType)
        {
            Value = value;
            Type = type;
            PlatformSpecificType = platformSpecificType;
        }

        public string Value { get; }

        public ContactType Type { get; }

        public string PlatformSpecificType { get; }

        public override string ToString() => Value;
    }
}
