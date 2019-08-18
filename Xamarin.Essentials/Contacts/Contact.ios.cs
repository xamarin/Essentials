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

        static Task<PhoneContact> PlatformPickContactAsync()
        {
            var picker = new CNContactPickerViewController
            {
                Delegate = new ContactPickerDelegate()
            };

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
                var p = (Lookup<ContactType, string>)emails.ToLookup(k => k.Value, v => v.Key);
                return new PhoneContact(
                                       name,
                                       (Lookup<ContactType, string>)phones.ToLookup(k => k.Value, v => v.Key),
                                       (Lookup<ContactType, string>)emails.ToLookup(k => k.Value, v => v.Key),
                                       birthday);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static Task PlatformSaveContactAsync(string name, string phone, string email)
        {
            var contact = new CNMutableContact();

            if (!string.IsNullOrEmpty(name))
            {
                var nameSplit = name.Split(' ');
                contact.GivenName = nameSplit[0];
                contact.FamilyName = nameSplit.Length > 1 ? nameSplit[nameSplit.Length - 1] : " ";
            }

            if (!string.IsNullOrEmpty(email))
            {
                contact.EmailAddresses = new CNLabeledValue<NSString>[1]
                {
                   new CNLabeledValue<NSString>(CNLabelKey.EmailiCloud, new NSString(email))
                };
            }

            if (!string.IsNullOrEmpty(phone))
            {
                contact.PhoneNumbers = new CNLabeledValue<CNPhoneNumber>[1]
                {
                    new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.Main, new CNPhoneNumber(phone))
                };
            }

            try
            {
                var view = CNContactViewController.FromNewContact(contact);
                view.Delegate = new ContactSaveDelegate();
                var nav = new UINavigationController(view);
                UIView.PresentModalViewController(nav, true);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
            finally
            {
                contact.Dispose();
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
    }
}
