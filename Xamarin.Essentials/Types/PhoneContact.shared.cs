using System;
using System.Collections.Generic;

namespace Xamarin.Essentials
{
    public struct PhoneContact : IEquatable<PhoneContact>
    {
        public string Name { get; }

        public Dictionary<string, ContactType> Numbers { get; }

        public Dictionary<string, ContactType> Emails { get; }

        public string Birthday { get; }

        internal PhoneContact(
            string name,
            Dictionary<string, ContactType> numbers,
            Dictionary<string, ContactType> emails,
            string bd)
        {
            Name = name;
            Birthday = bd;
            Numbers = new Dictionary<string, ContactType>(numbers);
            Emails = new Dictionary<string, ContactType>(emails);
        }

        public static bool operator ==(PhoneContact left, PhoneContact right) =>
            Equals(left, right);

        public static bool operator !=(PhoneContact left, PhoneContact right) =>
            !Equals(left, right);

        public override bool Equals(object obj) =>
            (obj is PhoneContact contact) && Equals(contact);

        public bool Equals(PhoneContact other) =>
            (Name, Numbers, Emails, Birthday) == (other.Name, other.Numbers, other.Emails, other.Birthday);

        public override int GetHashCode() =>
            (Name, Numbers, Emails, Birthday).GetHashCode();
    }
}
