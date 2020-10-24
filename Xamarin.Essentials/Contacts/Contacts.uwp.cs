using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static async Task<Contact> PlatformPickContactAsync()
        {
            var contactPicker = new ContactPicker();
            var contactSelected = await contactPicker.PickContactAsync();
            return ConvertContact(contactSelected);
        }

        public static async IAsyncEnumerable<Contact> PlatformGetAllAsync()
        {
            var contactStore = await ContactManager.RequestStoreAsync();
            if (contactStore == null)
                yield break;

            var contacts = await contactStore.FindContactsAsync();
            if (contacts == null)
                yield break;

            foreach (var item in contacts)
                yield return ConvertContact(item);
        }

        internal static Contact ConvertContact(Windows.ApplicationModel.Contacts.Contact contact)
        {
            if (contact == null)
                return default;

            var phones = contact.Phones?.Select(item => new ContactProperty(item?.Number, GetPhoneContactType(item?.Kind), item?.Kind?.ToString()))?.Distinct()?.ToList();
            var emails = contact.Emails?.Select(item => new ContactProperty(item?.Address, GetEmailContactType(item?.Kind), item?.Kind?.ToString()))?.Distinct()?.ToList()

            return new Contact(contact.Name, phones, emails);
        }

        static ContactType GetPhoneContactType(ContactPhoneKind? type)
            => type switch
            {
                ContactPhoneKind.Home => ContactType.Personal,
                ContactPhoneKind.HomeFax => ContactType.Personal,
                ContactPhoneKind.Mobile => ContactType.Personal,
                ContactPhoneKind.Work => ContactType.Work,
                ContactPhoneKind.Pager => ContactType.Work,
                ContactPhoneKind.BusinessFax => ContactType.Work,
                ContactPhoneKind.Company => ContactType.Work,
                _ => ContactType.Unknown
            };

        static ContactType GetEmailContactType(ContactEmailKind? type)
            => type switch
            {
                ContactEmailKind.Personal => ContactType.Personal,
                ContactEmailKind.Work => ContactType.Work,
                _ => ContactType.Unknown,
            };
    }
}
