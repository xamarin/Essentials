using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public class Contact
    {
        internal Contact(string name, IEnumerable<ContactProperty> numbers, IEnumerable<ContactProperty> email)
        {
            Name = name;
            Emails = email?.ToList();
            Numbers = numbers?.ToList();
        }

        public string Name { get; }

        public IReadOnlyList<ContactProperty> Numbers { get; }

        public IReadOnlyList<ContactProperty> Emails { get; }

        public override string ToString() => Name;
    }

    public class ContactProperty
    {
        internal ContactProperty(string value, ContactType type, string platformSpecificType)
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
