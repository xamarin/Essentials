using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static async Task<PhoneContact> PlataformPickContactAsync()
        {
            var contactPicker = new ContactPicker();

            try
            {
                var contactSelected = await contactPicker.PickContactAsync();

                if (contactSelected is null)
                    throw new Exception("We can't get the contact!");

                var contactManager = await ContactManager.RequestStoreAsync();
                var contact = await contactManager.FindContactsAsync(contactSelected.Name);
                var phoneContact = contact[0];
                var phones = new Dictionary<string, ContactType>();
                var emails = new Dictionary<string, ContactType>();
                foreach (var item in phoneContact.Phones)
                {
                    if (item.Kind == ContactPhoneKind.Home || item.Kind == ContactPhoneKind.HomeFax)
                        phones.Add(item.Number, ContactType.Personal);
                    else if (item.Kind == ContactPhoneKind.Company || item.Kind == ContactPhoneKind.BusinessFax)
                        phones.Add(item.Number, ContactType.Work);
                    else
                        phones.Add(item.Number, ContactType.Unknow);
                }

                foreach (var item in phoneContact.Emails)
                {
                    if (item.Kind == ContactEmailKind.Personal)
                        emails.Add(item.Address, ContactType.Personal);
                    else if (item.Kind == ContactEmailKind.Work)
                        emails.Add(item.Address, ContactType.Work);
                    else
                        phones.Add(item.Address, ContactType.Unknow);
                }

                var birthday = phoneContact.ImportantDates.First(x => x.Kind == ContactDateKind.Birthday).ToString();

                return new PhoneContact(
                                        phoneContact.Name,
                                        phones,
                                        emails,
                                        string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static async Task PlataformSaveContactAsync(string name, string phone, string email)
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

        static Task PlataformSaveContact(PhoneContact phoneContact) => Task.CompletedTask;
    }
}
