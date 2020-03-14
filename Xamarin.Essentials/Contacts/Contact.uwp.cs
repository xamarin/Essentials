using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static async Task<PhoneContact> PlatformPickContactAsync()
        {
            var contactPicker = new ContactPicker();

            try
            {
                var contactSelected = await contactPicker.PickContactAsync();

                if (contactSelected == null)
                    throw new Exception("We can't get the contact!");

                var contactManager = await ContactManager.RequestStoreAsync();
                var contact = await contactManager.FindContactsAsync(contactSelected.Name);

                var phoneContact = contact[0];
                var phones = new Dictionary<string, ContactType>();
                var emails = new Dictionary<string, ContactType>();

                foreach (var item in phoneContact.Phones)
                {
                    if (!phones.ContainsKey(item.Number))
                        phones.Add(item.Number, GetPhoneContactType(item.Kind));
                }

                foreach (var item in phoneContact.Emails)
                {
                    if (!emails.ContainsKey(item.Address))
                        emails.Add(item.Address, GetEmailContactType(item.Kind));
                }
                var b = phoneContact.ImportantDates.FirstOrDefault(x => x.Kind == ContactDateKind.Birthday);

                var birthday = (b == null) ? default :
                    new DateTime((int)b?.Year, (int)b?.Month, (int)b?.Day, 0, 0, 0);
                return new PhoneContact(
                                        phoneContact.Name,
                                        (Lookup<ContactType, string>)phones.ToLookup(k => k.Value, v => v.Key),
                                        (Lookup<ContactType, string>)emails.ToLookup(k => k.Value, v => v.Key),
                                        birthday);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static async Task PlatformSaveContactAsync(string name, string phone, string email)
        {
            try
            {
                var uriPeople = new Uri($"ms-people:savetocontact?PhoneNumber={phone}&ContactName={name}&Email={email}");

                await Windows.System.Launcher.LaunchUriAsync(uriPeople);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static ContactType GetPhoneContactType(ContactPhoneKind type)
        {
            switch (type)
            {
                case ContactPhoneKind.Home:
                case ContactPhoneKind.HomeFax:
                case ContactPhoneKind.Mobile:
                    return ContactType.Personal;
                case ContactPhoneKind.Work:
                case ContactPhoneKind.Pager:
                case ContactPhoneKind.BusinessFax:
                case ContactPhoneKind.Company:
                    return ContactType.Work;
                default:
                    return ContactType.Unknown;
            }
        }

        static ContactType GetEmailContactType(ContactEmailKind type)
        {
            switch (type)
            {
                case ContactEmailKind.Personal:
                    return ContactType.Personal;
                case ContactEmailKind.Work:
                    return ContactType.Work;
                default:
                    return ContactType.Unknown;
            }
        }
    }
}
