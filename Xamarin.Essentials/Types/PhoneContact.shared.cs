﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public readonly struct PhoneContact : IEquatable<PhoneContact>
    {
        public string Name { get; }

        public Lookup<ContactType, string> Numbers { get; }

        public Lookup<ContactType, string> Emails { get; }

        public DateTime? Birthday { get; }

        internal PhoneContact(
            string name,
            Lookup<ContactType, string> numbers,
            Lookup<ContactType, string> email,
            DateTime? bd)
        {
            Name = name;
            Birthday = bd;
            Emails = email;
            Numbers = numbers;
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
            Birthday.Equals(other.Birthday);

        public override int GetHashCode() =>
            (Name, Numbers, Emails, Birthday).GetHashCode();
    }
}
