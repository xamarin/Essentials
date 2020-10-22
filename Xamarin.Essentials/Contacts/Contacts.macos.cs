using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static Task<Contact> PlatformPickContactAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static async IAsyncEnumerable<Contact> PlatformGetAllAsync()
        {
            await Task.CompletedTask;
            var keys = new[]
            {
                CNContactKey.NamePrefix,
                CNContactKey.GivenName,
                CNContactKey.MiddleName,
                CNContactKey.FamilyName,
                CNContactKey.NameSuffix,
                CNContactKey.EmailAddresses,
                CNContactKey.PhoneNumbers,
                CNContactKey.Type
            };

            using var store = new CNContactStore();
            var containers = store.GetContainers(null, out var error);
            if (containers == null)
                yield break;

            foreach (var container in containers)
            {
                using var pred = CNContact.GetPredicateForContactsInContainer(container.Identifier);
                var contacts = store.GetUnifiedContacts(pred, keys, out error);
                if (contacts == null)
                    continue;

                foreach (var contact in contacts)
                    yield return ConvertContact(contact);
            }
        }

        internal static Contact ConvertContact(CNContact contact)
        {
            if (contact == null)
                return default;

            try
            {
                var contactType = ToPhoneContact(contact.ContactType);
                var phones = contact.PhoneNumbers?.Select(item => new ContactPhone(item?.Value?.StringValue, contactType))?.ToList();
                var emails = contact.EmailAddresses?.Select(item => new ContactEmail(item?.Value?.ToString(), contactType))?.ToList();
                var name = $"{contact.NamePrefix} {contact.GivenName} {contact.MiddleName} {contact.FamilyName} {contact.NameSuffix}"
                    .Replace("  ", " ").TrimEnd();

                return new Contact(name, phones, emails, contactType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                contact.Dispose();
            }
        }

        static ContactType ToPhoneContact(CNContactType type) => type switch
        {
            CNContactType.Person => ContactType.Personal,
            CNContactType.Organization => ContactType.Work,
            _ => ContactType.Unknown,
        };
    }
}
