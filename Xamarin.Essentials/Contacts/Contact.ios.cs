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
        internal static Action<PhoneContact?> CallBack { get; set; }

        internal static Action<Exception> ErrorCallBack { get; set; }

        static Task<PhoneContact?> PlatformPickContactAsync()
        {
            var uiView = Platform.GetCurrentViewController();
            if (uiView == null)
                throw new ArgumentNullException($"The View Controller can't be null.");

            using var picker = new CNContactPickerViewController
            {
                Delegate = new ContactPickerDelegate()
            };

            uiView.PresentViewController(picker, true, null);
            var source = new TaskCompletionSource<PhoneContact?>();
            try
            {
                CallBack = (phoneContact) =>
                {
                    var tcs = Interlocked.Exchange(ref source, null);
                    tcs?.SetResult(phoneContact);
                };

                ErrorCallBack = (ex) =>
                {
                    var tcs = Interlocked.Exchange(ref source, null);
                    tcs?.SetException(ex);
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
                var phones = new Dictionary<string, string>();

                foreach (var item in contact.PhoneNumbers)
                    phones.Add(item?.Value?.StringValue, NormalizeString(item?.Label));

                var emails = new Dictionary<string, string>();

                foreach (var item in contact.EmailAddresses)
                    emails.Add(item?.Value?.ToString(), NormalizeString(item?.Label));

                var name = string.Empty;

                if (string.IsNullOrEmpty(contact.MiddleName))
                    name = $"{contact.GivenName} {contact.FamilyName}";
                else
                    name = $"{contact.GivenName} {contact.MiddleName} {contact.FamilyName}";

                var birthday = contact.Birthday?.Date.ToDateTime().Date;
                return new PhoneContact(
                                       name,
                                       (Lookup<string, string>)phones.ToLookup(k => k.Value, v => v.Key),
                                       (Lookup<string, string>)emails.ToLookup(k => k.Value, v => v.Key),
                                       birthday,
                                       contactType);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contact.Dispose();
            }
        }

        static Task PlatformSaveContactAsync(string name, string phone, string email)
        {
            var uiView = Platform.GetCurrentViewController();

            if (uiView == null)
                throw new ArgumentNullException($"The View Controller can't be null.");

            using var contact = new CNMutableContact();

            if (!string.IsNullOrEmpty(name))
            {
                var nameSplit = name.Split(' ');
                contact.GivenName = nameSplit[0];
                contact.FamilyName = nameSplit.Length > 1 ? nameSplit[^1] : " ";
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
                using var view = CNContactViewController.FromNewContact(contact);
                view.Delegate = new ContactSaveDelegate();
                using var nav = new UINavigationController(view);
                uiView.PresentModalViewController(nav, true);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        static ContactType ToPhoneContact(CNContactType type) => type switch
        {
            CNContactType.Person => ContactType.Personal,
            CNContactType.Organization => ContactType.Work,
            _ => ContactType.Unknown,
        };

        static string NormalizeString(string contactType)
        {
            // _$!<Work>!$_

            if (!contactType.StartsWith("_$!<"))
            {
                return contactType;
            }

            var type = contactType.Split('<')[1];
            type = type.Split('>')[0];
            return type;
        }
     }
}
