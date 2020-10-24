using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts;
using ContactsUI;
using Foundation;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static Task<Contact> PlatformPickContactAsync()
        {
            var uiView = Platform.GetCurrentViewController();
            if (uiView == null)
                throw new ArgumentNullException($"The View Controller can't be null.");

            var source = new TaskCompletionSource<Contact>();

            using var picker = new CNContactPickerViewController
            {
                Delegate = new ContactPickerDelegate(phoneContact =>
                    source?.TrySetResult(Contacts.ConvertContact(phoneContact)))
            };

            uiView.PresentViewController(picker, true, null);

            return source.Task;
        }

        static async IAsyncEnumerable<Contact> PlatformGetAllAsync()
        {
            await Task.CompletedTask;
            var keys = new[]
            {
                CNContactKey.NamePrefix,
                CNContactKey.GivenName,
                CNContactKey.MiddleName,
                CNContactKey.FamilyName,
                CNContactKey.NameSuffix,
                CNContactKey.EmailAddresses,
                CNContactKey.PhoneNumbers,
                CNContactKey.Type
            };

            using var store = new CNContactStore();
            var containers = store.GetContainers(null, out var error);
            if (containers == null)
                yield break;

            foreach (var container in containers)
            {
                using var pred = CNContact.GetPredicateForContactsInContainer(container.Identifier);
                var contacts = store.GetUnifiedContacts(pred, keys, out error);
                if (contacts == null)
                    continue;

                foreach (var contact in contacts)
                    yield return ConvertContact(contact);
            }
        }

        internal static Contact ConvertContact(CNContact contact)
        {
            if (contact == null)
                return default;

            try
            {
                var phones = contact.PhoneNumbers?.Select(
                   item => new ContactProperty(
                       item?.Value?.StringValue,
                       TypeConvert(item.Label?.ToString()),
                       item.Label?.ToString()));

                var emails = contact.EmailAddresses?.Select(
                   item => new ContactProperty(
                       item?.Value?.ToString(),
                       TypeConvert(item.Label?.ToString()),
                       item.Label?.ToString()));

                var name = $"{contact.NamePrefix} {contact.GivenName} {contact.MiddleName} {contact.FamilyName} {contact.NameSuffix}"
                    .Replace("  ", " ").TrimEnd();

                return new Contact(name, phones, emails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                contact.Dispose();
            }
        }

        static ContactType TypeConvert(string type)
        {
            if (type == CNLabelKey.Work || type == CNLabelPhoneNumberKey.WorkFax)
                return ContactType.Work;
            else if (
                type == CNLabelPhoneNumberKey.Main ||
                type == CNLabelPhoneNumberKey.Mobile ||
                type == CNLabelPhoneNumberKey.HomeFax ||
                type == CNLabelPhoneNumberKey.iPhone ||
                type == CNLabelKey.Home)
                return ContactType.Personal;
            else
                return ContactType.Unknown;
        }

        class ContactPickerDelegate : CNContactPickerDelegate
        {
            public ContactPickerDelegate(Action<CNContact> didSelectContactHandler) =>
                DidSelectContactHandler = didSelectContactHandler;

            public ContactPickerDelegate(IntPtr handle)
                : base(handle)
            {
            }

            public Action<CNContact> DidSelectContactHandler { get; }

            public override void ContactPickerDidCancel(CNContactPickerViewController picker)
            {
                DidSelectContactHandler?.Invoke(default);
                picker.DismissModalViewController(true);
            }

            public override void DidSelectContact(CNContactPickerViewController picker, CNContact contact)
            {
                DidSelectContactHandler?.Invoke(contact);
                picker.DismissModalViewController(true);
            }

            public override void DidSelectContactProperty(CNContactPickerViewController picker, CNContactProperty contactProperty) =>
                picker.DismissModalViewController(true);
        }
    }
}
