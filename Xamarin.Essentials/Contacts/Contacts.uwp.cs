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

            try
            {
                var contactSelected = await contactPicker.PickContactAsync();
                return ConvertContact(contactSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<Task<IEnumerable<Contact>>> PlatformGetAllTasks()
            => new List<Task<IEnumerable<Contact>>>() { PlatformGetAllAsync() };

        static async Task<IEnumerable<Contact>> PlatformGetAllAsync()
        {
            var contactStore = await ContactManager.RequestStoreAsync();

            if (contactStore == null)
                return null;
            return (await contactStore.FindContactsAsync())?.Select(a => ConvertContact(a));
        }

        internal static Contact ConvertContact(Windows.ApplicationModel.Contacts.Contact contact)
        {
            if (contact == null)
                return default;

            var phones = new List<ContactPhone>();
            var emails = new List<ContactEmail>();

            foreach (var item in contact.Phones)
                phones.Add(new ContactPhone(item.Number, GetPhoneContactType(item.Kind)));

            phones = phones.Distinct().ToList();

            foreach (var item in contact.Emails)
                emails.Add(new ContactEmail(item.Address, GetEmailContactType(item.Kind)));

            emails = emails.Distinct().ToList();

            return new Contact(contact.Name, phones, emails, ContactType.Unknown);
        }

        static ContactType GetPhoneContactType(ContactPhoneKind type)
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

        static ContactType GetEmailContactType(ContactEmailKind type) => type switch
        {
            ContactEmailKind.Personal => ContactType.Personal,
            ContactEmailKind.Work => ContactType.Work,
            _ => ContactType.Unknown,
        };
    }
}
