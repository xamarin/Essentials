using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contacts;
using ContactsUI;
using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        internal static Action<PhoneContact> CallBack { get; set; }

        internal static UIViewController UIView => Platform.GetCurrentViewController();

        static Task<PhoneContact> PlataformPickContactAsync()
        {
            var picker = new CNContactPickerViewController
            {
                // Select property to pick
                // DisplayedPropertyKeys = new NSString[] { CNContactKey.EmailAddresses },
                // PredicateForEnablingContact = NSPredicate.FromFormat("emailAddresses.@count > 0"),
                // PredicateForSelectionOfContact = NSPredicate.FromFormat("emailAddresses.@count == 1"),

                // Respond to selection
                Delegate = new ContactPickerDelegate()
            };

            // Display picker
            UIView.PresentViewController(picker, true, null);

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

            try
            {
                var contactType = ToPhoneContact(contact.ContactType);
                var phones = new Dictionary<string, ContactType>();

                foreach (var item in contact.PhoneNumbers)
                    phones.Add(item?.Value?.StringValue, contactType);

                var emails = new Dictionary<string, ContactType>();

                foreach (var item in contact.EmailAddresses)
                    emails.Add(item?.Value?.ToString(), contactType);

                var name = $"{contact.GivenName} {contact.MiddleName} {contact.FamilyName}";

                var birthday = contact.Birthday?.Date.ToDateTime().Date.ToShortDateString();

                return new PhoneContact(name, phones, emails, birthday);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        static ContactType ToPhoneContact(CNContactType type)
        {
            switch (type)
            {
                case CNContactType.Person:
                    return ContactType.Personal;
                case CNContactType.Organization:
                    return ContactType.Work;
                default:
                    return ContactType.Unknow;
            }
        }
    }
}
