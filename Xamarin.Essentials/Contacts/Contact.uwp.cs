using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static async Task<PhoneContact> PlataformPickContact()
        {
            var contactPicker = new ContactPicker();

            var contactSelected = await contactPicker.PickContactAsync();

            if (contactSelected is null)
                throw new Exception("We can't get the contact!");

            var contactManager = await ContactManager.RequestStoreAsync();
            var contact = await contactManager.FindContactsAsync(contactSelected.Name);

            var eu = contact[0];

            return new PhoneContact(
                                    contact[0].Name,
                                    eu.Phones.Select(p => p.Number),
                                    eu.Emails.Select(e => e.Address),
                                    string.Empty);
        }
    }
}
