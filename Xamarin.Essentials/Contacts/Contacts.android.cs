using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Provider;
using Net = Android.Net;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static async Task<Contact> PlatformPickContactAsync()
        {
            using var intent = new Intent(Intent.ActionPick);
            intent.SetType(ContactsContract.CommonDataKinds.Phone.ContentType);
            var result = await IntermediateActivity.StartAsync(intent, Platform.requestCodePickContact).ConfigureAwait(false);

            if (result?.Data != null)
            {
                var contact = GetContact(result.Data);
                return contact;
            }
            return null;
        }

        static IEnumerable<Task<IEnumerable<Contact>>> PlatformGetAllTasks()
            => new List<Task<IEnumerable<Contact>>> { Task.FromResult(PlatformGetAll()) };

        static IEnumerable<Contact> PlatformGetAll()
        {
            using var context = Platform.AppContext.ContentResolver;
            using var cursor = context.Query(ContactsContract.Contacts.ContentUri, null, null, null, null);

            if (cursor.Count == 0)
                yield break;

            if (cursor.MoveToFirst())
            {
                do
                {
                    var contact = GetContact(cursor, context, ContactsContract.Contacts.InterfaceConsts.Id);
                    if (contact != null)
                        yield return contact;
                }
                while (cursor.MoveToNext());
            }
        }

        internal static Contact GetContact(Net.Uri contactUri)
        {
            if (contactUri == null)
                return default;

            using var context = Platform.AppContext.ContentResolver;
            using var cursor = context.Query(contactUri, null, null, null, null);

            // TODO Emails and Numbers
            if (cursor.MoveToFirst())
                return GetContact(cursor, context, ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId);

            return default;
        }

        static Contact GetContact(ICursor cursor, ContentResolver context, string idKey)
        {
            var name = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));

            var id = cursor.GetString(cursor.GetColumnIndex(idKey));
            var idQ = new string[1] { id };
            var phones = GetNumbers(context, idQ);
            var emails = GetEmails(context, idQ);

            // TODO ContactType
            // var typeOfContact = cur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Type));
            // GetPhoneContactType(typeOfContact)
            return new Contact(
                name,
                phones?.Select(a => new ContactPhone(a, ContactType.Unknown))?.ToList(),
                emails?.Select(a => new ContactEmail(a, ContactType.Unknown))?.ToList(),
                ContactType.Unknown);
        }

        static IEnumerable<string> GetNumbers(ContentResolver context, string[] idQ)
        {
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri.BuildUpon().AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();
            var cursor = context.Query(uri, null, $"{ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId}=?", idQ, null);

            return ReadCursorItems(cursor, ContactsContract.CommonDataKinds.Phone.Number);
        }

        static IEnumerable<string> GetEmails(ContentResolver context, string[] idQ)
        {
            var uri = ContactsContract.CommonDataKinds.Email.ContentUri.BuildUpon().AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();
            var cursor = context.Query(uri, null, $"{ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId}=?", idQ, null);

            return ReadCursorItems(cursor, ContactsContract.CommonDataKinds.Email.Address);
        }

        static IEnumerable<string> ReadCursorItems(ICursor cursor, string dataKey)
        {
            if (cursor.MoveToFirst())
            {
                do
                {
                    var value = cursor.GetString(cursor.GetColumnIndex(dataKey));

                    if (value != null)
                        yield return value;
                }
                while (cursor.MoveToNext());
            }
            cursor.Close();
        }

        static ContactType GetPhoneContactType(string type)
        {
            if (int.TryParse(type, out var typeInt))
            {
                try
                {
                    var phoneKind = (PhoneDataKind)typeInt;
                    return phoneKind switch
                    {
                        PhoneDataKind.Home => ContactType.Personal,
                        PhoneDataKind.Mobile => ContactType.Personal,
                        PhoneDataKind.Main => ContactType.Personal,
                        PhoneDataKind.Work => ContactType.Work,
                        PhoneDataKind.WorkMobile => ContactType.Work,
                        PhoneDataKind.CompanyMain => ContactType.Work,
                        PhoneDataKind.WorkPager => ContactType.Work,
                        _ => ContactType.Unknown
                    };
                }
                catch (Exception)
                {
                    return ContactType.Unknown;
                }
            }
            return ContactType.Unknown;
        }

        static ContactType GetEmailContactType(string type)
        {
            if (int.TryParse(type, out var typeInt))
            {
                try
                {
                    var emailKind = (EmailDataKind)typeInt;
                    return emailKind switch
                    {
                        EmailDataKind.Home => ContactType.Personal,
                        EmailDataKind.Work => ContactType.Work,
                        _ => ContactType.Unknown
                    };
                }
                catch (Exception)
                {
                    return ContactType.Unknown;
                }
            }
            return ContactType.Unknown;
        }

        static void GetPhones(ref string[] idQ, ref global::Android.Database.ICursor cursor, List<ContactPhone> phones, ContentResolver context)
        {
            var projection = new string[2]
            {
                    ContactsContract.CommonDataKinds.Phone.Number,
                    ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Type,
            };

            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri.BuildUpon().AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();

            cursor = context.Query(
                  uri,
                  null,
                  ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + "=?",
                  idQ,
                  null);

            if (cursor.MoveToFirst())
            {
                do
                {
                    var phone = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                    var phoneType = cursor.GetString(cursor.GetColumnIndex(projection[1]));

                    var contactType = GetPhoneContactType(phoneType);

                    phones.Add(new ContactPhone(phone, contactType));
                }
                while (cursor.MoveToNext());
            }
            cursor.Close();
        }

        static void GetEmails(ref string[] idQ, ref global::Android.Database.ICursor cursor, List<ContactEmail> emails, ContentResolver context)
        {
            var projection = new string[2]
            {
                    ContactsContract.CommonDataKinds.Email.Address,
                    ContactsContract.CommonDataKinds.Email.InterfaceConsts.Type
            };

            var uri = ContactsContract.CommonDataKinds.Email.ContentUri.BuildUpon().AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();

            cursor = context.Query(uri, null, ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + "=?", idQ, null);

            while (cursor.MoveToNext())
            {
                var email = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                var emailType = cursor.GetString(cursor.GetColumnIndex(projection[1]));

                var contactType = GetEmailContactType(emailType);

                emails.Add(new ContactEmail(email, contactType));
            }

            cursor.Close();
        }
    }
}
