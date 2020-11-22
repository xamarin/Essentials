using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tizen.Applications;
using Tizen.Pims.Contacts;
using TizenContact = Tizen.Pims.Contacts.ContactsViews.Contact;
using TizenEmail = Tizen.Pims.Contacts.ContactsViews.Email;
using TizenName = Tizen.Pims.Contacts.ContactsViews.Name;
using TizenNumber = Tizen.Pims.Contacts.ContactsViews.Number;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static ContactsManager manager = new ContactsManager();

        static async Task<Contact> PlatformPickContactAsync()
        {
            Permissions.EnsureDeclared<Permissions.ContactsRead>();
            Permissions.EnsureDeclared<Permissions.LaunchApp>();
            await Permissions.RequestAsync<Permissions.ContactsRead>();

            var tcs = new TaskCompletionSource<Contact>();

            var appControl = new AppControl();
            appControl.Operation = AppControlOperations.Pick;
            appControl.ExtraData.Add(AppControlData.SectionMode, "single");
            appControl.LaunchMode = AppControlLaunchMode.Single;
            appControl.Mime = "application/vnd.tizen.contact";

            AppControl.SendLaunchRequest(appControl, (request, reply, result) =>
            {
                Contact contact = null;

                if (result == AppControlReplyResult.Succeeded)
                {
                    var contactId = reply.ExtraData.Get<IEnumerable<string>>(AppControlData.Selected)?.FirstOrDefault();

                    if (int.TryParse(contactId, out var contactInt))
                    {
                        var record = manager.Database.Get(TizenContact.Uri, contactInt);
                        if (record != null)
                            contact = ToContact(record);
                    }
                }
                tcs.TrySetResult(contact);
            });
            return await tcs.Task;
        }

        static Task<IEnumerable<Contact>> PlatformGetAllAsync(CancellationToken cancellationToken)
        {
            var contactsList = manager.Database.GetAll(TizenContact.Uri, 0, 0);
            return Task.FromResult(GetEnumerable());

            IEnumerable<Contact> GetEnumerable()
            {
                for (var i = 0; i < contactsList?.Count; i++)
                {
                    yield return ToContact(contactsList.GetCurrentRecord());
                    contactsList.MoveNext();
                }
            }
        }

        static Contact ToContact(ContactsRecord contactsRecord)
        {
            var name = string.Empty;
            var id = string.Empty;
            var record = contactsRecord.GetChildRecord(TizenContact.Name, 0);
            if (record != null)
            {
                id = record.Get<string>(TizenName.ContactId);
                var first = record.Get<string>(TizenName.First);
                var last = record.Get<string>(TizenName.Last);
                name = $"{first}{GetName(last)}";

                if (!string.IsNullOrWhiteSpace(first))
                {
                    if (!string.IsNullOrWhiteSpace(last))
                        name = first + last;
                    else
                        name = first;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(last))
                        name = last;
                }
            }

            var phones = new List<ContactPhone>();
            var nameCount = contactsRecord.GetChildRecordCount(TizenContact.Number);
            for (var i = 0; i < nameCount; i++)
            {
                var nameRecord = contactsRecord.GetChildRecord(TizenContact.Number, i);
                var number = nameRecord.Get<string>(TizenNumber.NumberData);
                var type = (TizenNumber.Types)nameRecord.Get<int>(TizenNumber.Type);

                phones.Add(new ContactPhone(number, GetContactType(type)));
            }

            var emails = new List<ContactEmail>();
            var emailCount = contactsRecord.GetChildRecordCount(TizenContact.Email);
            for (var i = 0; i < emailCount; i++)
            {
                var emailRecord = contactsRecord.GetChildRecord(TizenContact.Email, i);
                var addr = emailRecord.Get<string>(TizenEmail.Address);
                var type = (TizenEmail.Types)emailRecord.Get<int>(TizenEmail.Type);

                emails.Add(new ContactEmail(addr, GetContactType(type)));
            }

            return new Contact(id, name, phones, emails);
        }

        static ContactType GetContactType(TizenEmail.Types emailType)
            => emailType switch
            {
                TizenEmail.Types.Home => ContactType.Personal,
                TizenEmail.Types.Mobile => ContactType.Personal,
                TizenEmail.Types.Work => ContactType.Work,
                _ => ContactType.Unknown
            };

        static ContactType GetContactType(TizenNumber.Types numberType)
            => numberType switch
            {
                TizenNumber.Types.Car => ContactType.Personal,
                TizenNumber.Types.Cell => ContactType.Personal,
                TizenNumber.Types.Home => ContactType.Personal,
                TizenNumber.Types.Main => ContactType.Personal,
                TizenNumber.Types.Message => ContactType.Personal,
                TizenNumber.Types.Video => ContactType.Personal,
                TizenNumber.Types.Voice => ContactType.Personal,
                TizenNumber.Types.Work => ContactType.Work,
                TizenNumber.Types.Pager => ContactType.Work,
                TizenNumber.Types.Assistant => ContactType.Work,
                TizenNumber.Types.Company => ContactType.Work,
                TizenNumber.Types.Fax => ContactType.Work,
                _ => ContactType.Unknown
            };
    }
}
