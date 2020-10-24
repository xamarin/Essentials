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
                var phones = contact.PhoneNumbers?.Select(
                   item => new ContactPhone(
                       item?.Value?.StringValue,
                       TypeConvert(item.Label?.ToString()),
                       item.Label?.ToString()));

                var emails = contact.EmailAddresses?.Select(
                   item => new ContactEmail(
                       item?.Value?.ToString(),
                       TypeConvert(item.Label?.ToString()),
                       item.Label?.ToString()));

                var name = $"{contact.NamePrefix} {contact.GivenName} {contact.MiddleName} {contact.FamilyName} {contact.NameSuffix}"
                    .Replace("  ", " ").TrimEnd();

                return new Contact(name, phones, emails);
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

        static ContactType TypeConvert(string type)
        {
            if (type == CNLabelKey.Work || type == CNLabelPhoneNumberKey.WorkFax)
                return ContactType.Work;
            else if (
                type == CNLabelPhoneNumberKey.Main ||
                type == CNLabelPhoneNumberKey.Mobile ||
                type == CNLabelPhoneNumberKey.HomeFax ||
                type == CNLabelPhoneNumberKey.iPhone ||
                type == CNLabelKey.Home)
                return ContactType.Personal;
            else
                return ContactType.Unknown;
        }
    }
}
