using System;
using System.Threading;
using System.Threading.Tasks;
using AddressBook;
using Contacts;
using ContactsUI;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        internal static Action<PhoneContact> CallBack { get; set; }

        static Task<PhoneContact> PlataformPickContactAsync()
        {
            var source = new TaskCompletionSource<PhoneContact>();
            try
            {
                CallBack = (phoneContact) =>
                {
                    var tcs = Interlocked.Exchange(ref source, null);
                    tcs?.SetResult(phoneContact);
                };
            }
            catch (Exception ex)
            {
                source.SetException(ex);
            }
            return source.Task;
        }

        internal static PhoneContact GetContact(CNContact contact)
        {
            if (contact == null)
                return default;

            return default;
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
