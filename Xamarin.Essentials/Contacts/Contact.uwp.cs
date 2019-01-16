using System;
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

                return new PhoneContact(
                                        phoneContact.Name,
                                        phoneContact.Phones.Select(p => p.Number),
                                        phoneContact.Emails.Select(e => e.Address),
                                        string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static async Task PlataformSaveContactAsync(string name, string phone, string email)
        {
            var uriPeople = new Uri($"ms-people:savetocontact?PhoneNumber={phone}&ContactName={name}&Email={email}");

            var success = await Windows.System.Launcher.LaunchUriAsync(uriPeople);
        }
    }
}
