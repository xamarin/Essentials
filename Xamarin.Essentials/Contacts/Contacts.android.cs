using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                return GetContact(result.Data);

            return null;
        }

        static Task<IEnumerable<Contact>> PlatformGetAllAsync(CancellationToken cancellationToken)
        {
            var cursor = Platform.ContentResolver.Query(ContactsContract.Contacts.ContentUri, null, null, null, null);
            return Task.FromResult(GetEnumerable());

            IEnumerable<Contact> GetEnumerable()
            {
                if (cursor?.MoveToFirst() ?? false)
                {
                    do
                    {
                        var contact = GetContact(cursor, ContactsContract.Contacts.InterfaceConsts.Id);
                        if (contact != null)
                            yield return contact;
                    }
                    while (cursor.MoveToNext());
                }

                cursor.Close();
            }
        }

        internal static Contact GetContact(Net.Uri contactUri)
        {
            if (contactUri == null)
                return default;

            using var cursor = Platform.ContentResolver.Query(contactUri, null, null, null, null);

            if (cursor.MoveToFirst())
            {
                return GetContact(
                    cursor,
                    ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId);
            }

            return default;
        }

        static Contact GetContact(ICursor cursor, string idKey)
        {
            var name = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
            var idQ = new string[1] { cursor.GetString(cursor.GetColumnIndex(idKey)) };
            var phones = GetNumbers(idQ)?.Select(
                item => new ContactPhone(item.data, GetPhoneContactType(item.type)));
            var emails = GetEmails(idQ)?.Select(
                item => new ContactEmail(item.data, GetEmailContactType(item.type)));

            return new Contact(name, phones, emails);
        }

        static IEnumerable<(string data, string type)> GetNumbers(string[] idQ)
        {
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri.BuildUpon().AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();
            var cursor = Platform.ContentResolver.Query(uri, null, $"{ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId}=?", idQ, null);

            return ReadCursorItems(cursor, ContactsContract.CommonDataKinds.Phone.Number, ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Type);
        }

        static IEnumerable<(string data, string type)> GetEmails(string[] idQ)
        {
            var uri = ContactsContract.CommonDataKinds.Email.ContentUri.BuildUpon().AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();
            var cursor = Platform.ContentResolver.Query(uri, null, $"{ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId}=?", idQ, null);

            return ReadCursorItems(cursor, ContactsContract.CommonDataKinds.Email.Address, ContactsContract.CommonDataKinds.Email.InterfaceConsts.Type);
        }

        static IEnumerable<(string data, string type)> ReadCursorItems(ICursor cursor, string dataKey, string typeKey)
        {
            if (cursor?.MoveToFirst() ?? false)
            {
                do
                {
                    var data = cursor.GetString(cursor.GetColumnIndex(dataKey));
                    var type = cursor.GetString(cursor.GetColumnIndex(typeKey));

                    if (data != null)
                        yield return (data, type);
                }
                while (cursor.MoveToNext());
            }
            cursor?.Close();
        }

        static ContactType GetPhoneContactType(string type)
        {
            if (int.TryParse(type, out var typeInt))
            {
                try
                {
                    return (PhoneDataKind)typeInt switch
                    {
                        PhoneDataKind.Main => ContactType.Personal,
                        PhoneDataKind.Home => ContactType.Personal,
                        PhoneDataKind.Mobile => ContactType.Personal,
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
                    return (EmailDataKind)typeInt switch
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
    }
}
