using System.Threading.Tasks;
using AddressBook;
using Contacts;
using ContactsUI;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static Task<PhoneContact> PlataformPickContactAsync()
        {
            return null;
        }

        static Task PlataformSaveContactAsync(string name, string phone, string email)
        {
            var store = new CNContactStore();
            var contact = new CNMutableContact
            {
                GivenName = name
            };

            var cellPhone = new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.Mobile, new CNPhoneNumber(phone));
            var phoneNumber = new[] { cellPhone };
            contact.PhoneNumbers = phoneNumber;

            var personalEmail = new CNLabeledValue<NSString>(CNLabelKey.Home, new NSString(email));
            var emailC = new[] { personalEmail };
            contact.EmailAddresses = emailC;

            var parent = Platform.GetCurrentViewController(true);
            var view = CNContactViewController.FromNewContact(contact);
            view.Delegate = new ContactSaveDelegate();
            parent.PresentViewController(view, true, null);
            return Task.CompletedTask;
        }
    }
}
