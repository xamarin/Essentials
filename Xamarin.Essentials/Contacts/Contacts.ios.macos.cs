using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contacts;
#if __IOS__
using ContactsUI;
#endif

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
#if __MACOS__
        static Task<Contact> PlatformPickContactAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;

#elif __IOS__
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

#endif
        static Task<IEnumerable<Contact>> PlatformGetAllAsync(CancellationToken cancellationToken)
        {
            var keys = new[]
            {
                CNContactKey.Identifier,
                CNContactKey.NamePrefix,
                CNContactKey.GivenName,
                CNContactKey.MiddleName,
                CNContactKey.FamilyName,
                CNContactKey.NameSuffix,
                CNContactKey.EmailAddresses,
                CNContactKey.PhoneNumbers,
                CNContactKey.Type
            };

            var store = new CNContactStore();
            var containers = store.GetContainers(null, out var createError);
            if (containers == null)
                return Task.FromResult<IEnumerable<Contact>>(Array.Empty<Contact>());

            return Task.FromResult(GetEnumerable());

            IEnumerable<Contact> GetEnumerable()
            {
                foreach (var container in containers)
                {
                    using var pred = CNContact.GetPredicateForContactsInContainer(container.Identifier);
                    var contacts = store.GetUnifiedContacts(pred, keys, out var error);
                    if (contacts == null)
                        continue;

                    foreach (var contact in contacts)
                    {
                        yield return ConvertContact(contact);
                    }
                }
            }
        }

        internal static Contact ConvertContact(CNContact contact)
        {
            if (contact == null)
                return default;

            try
            {
                var phones = contact.PhoneNumbers?.Select(
                   item => new ContactPhone(
                       item?.Value?.StringValue,
                       TypePhoneConvert(item.Label?.ToString())));

                var emails = contact.EmailAddresses?.Select(
                   item => new ContactEmail(
                       item?.Value?.ToString(),
                       TypeEmailConvert(item.Label?.ToString())));

                var name = $"{contact.NamePrefix}{GetName(contact.GivenName)}{GetName(contact.MiddleName)}{GetName(contact.FamilyName)}{GetName(contact.NameSuffix)}";

                return new Contact(contact.Identifier, name, phones, emails);
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

        static ContactPhoneType TypePhoneConvert(string type)
        {
            if (type == CNLabelPhoneNumberKey.WorkFax)
                return ContactPhoneType.Work;
            else if (type == CNLabelPhoneNumberKey.Main)
                return ContactPhoneType.Main;
            else if (type == CNLabelPhoneNumberKey.Mobile)
                return ContactPhoneType.Mobile;
            else if (
                type == CNLabelPhoneNumberKey.HomeFax ||
                type == CNLabelPhoneNumberKey.iPhone)
                return ContactPhoneType.Personal;
            else
                return ContactPhoneType.Unknown;
        }

        static ContactEmailType TypeEmailConvert(string type)
        {
            if (type == CNLabelKey.Work)
                return ContactEmailType.Work;
            else if (type == CNLabelKey.Home)
                return ContactEmailType.Personal;
            else
                return ContactEmailType.Unknown;
        }

#if __IOS__
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
#endif
    }
}
