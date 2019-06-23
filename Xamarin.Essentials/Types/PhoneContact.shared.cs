using System;
using System.Collections.Generic;

namespace Xamarin.Essentials
{
    public readonly struct PhoneContact : IEquatable<PhoneContact>
    {
        public string Name { get; }

        readonly Dictionary<string, ContactType> numbers;
        readonly Dictionary<string, ContactType> emails;

        public IReadOnlyDictionary<string, ContactType> Numbers => numbers;

        public IReadOnlyDictionary<string, ContactType> Emails => emails;

        public DateTime? Birthday { get; }

        internal PhoneContact(
            string name,
            Dictionary<string, ContactType> numbers,
            Dictionary<string, ContactType> emails,
            DateTime? bd)
        {
            Name = name;
            Birthday = bd;
            this.numbers = new Dictionary<string, ContactType>(numbers);
            this.emails = new Dictionary<string, ContactType>(emails);
        }

        public static bool operator ==(PhoneContact left, PhoneContact right) =>
            Equals(left, right);

        public static bool operator !=(PhoneContact left, PhoneContact right) =>
            !Equals(left, right);

        public override bool Equals(object obj) =>
            (obj is PhoneContact contact) && Equals(contact);

        public bool Equals(PhoneContact other) =>
            Name.Equals(other.Name) &&
            Numbers.Equals(other.Numbers) &&
            Emails.Equals(other.Emails) &&
            Birthday.Equals(other.Birthday) &&
            emails.Equals(other.emails) &&
            numbers.Equals(other.numbers);

        public override int GetHashCode() =>
            (Name, Numbers, Emails, Birthday, emails, numbers).GetHashCode();
    }
}
