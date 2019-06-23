using System;
using System.Collections.Generic;
using System.Linq;
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

                var birthday = contact.Birthday?.Date.ToDateTime().Date;

                return new PhoneContact(name, phones, emails, birthday);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static Task PlataformSaveContact(PhoneContact phoneContact)
        {
            var contact = new CNMutableContact();
            var nameSplit = phoneContact.Name.Split(' ');

            // Set standard properties
            contact.GivenName = nameSplit[0];
            contact.FamilyName = nameSplit.Length > 1 ? nameSplit[nameSplit.Length] : " ";

            // Add email addresses
            var emails = PopulateEmail(phoneContact.Emails);
            contact.EmailAddresses = emails.ToArray();

            // Add phone numbers
            var phones = PopulatePhones(phoneContact.Numbers);
            contact.PhoneNumbers = phones.ToArray();

            // Add work address
            var workAddress = new CNMutablePostalAddress()
            {
                Street = "1 Infinite Loop",
                City = "Cupertino",
                State = "CA",
                PostalCode = "95014"
            };
            contact.PostalAddresses = new CNLabeledValue<CNPostalAddress>[] { new CNLabeledValue<CNPostalAddress>(CNLabelKey.Work, workAddress) };

            // Add birthday
            var birthday = new NSDateComponents()
            {
                Day = 2,
                Month = 4,
                Year = 1984
            };
            contact.Birthday = birthday;

            try
            {
                var view = CNContactViewController.FromNewContact(contact);
                view.Delegate = new ContactSaveDelegate();
                var nav = new UINavigationController(view);
                UIView.PresentModalViewController(nav, false);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }

            IEnumerable<CNLabeledValue<NSString>> PopulateEmail(IReadOnlyDictionary<string, ContactType> email)
            {
                foreach (var item in email)
                {
                    switch (item.Value)
                    {
                        case ContactType.Unknow:
                        case ContactType.Personal:
                            yield return new CNLabeledValue<NSString>(CNLabelKey.Home, new NSString(item.Key));
                            break;
                        case ContactType.Work:
                            yield return new CNLabeledValue<NSString>(CNLabelKey.Work, new NSString(item.Key));
                            break;
                        default:
                            break;
                    }
                }
            }
            IEnumerable<CNLabeledValue<CNPhoneNumber>> PopulatePhones(IReadOnlyDictionary<string, ContactType> phone)
            {
                foreach (var item in phone)
                {
                    switch (item.Value)
                    {
                        case ContactType.Unknow:
                        case ContactType.Personal:
                            yield return new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.iPhone, new CNPhoneNumber(item.Key));
                            break;
                        case ContactType.Work:
                            yield return new CNLabeledValue<CNPhoneNumber>("Work", new CNPhoneNumber(item.Key));
                            break;
                        default:
                            break;
                    }
                }
            }
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

        static Task PlataformSaveContactAsync(string name, string phone, string email) => Task.CompletedTask;
    }
}
