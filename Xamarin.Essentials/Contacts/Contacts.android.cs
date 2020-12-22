using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Database;
using Android.Provider;
using CommonDataKinds = Android.Provider.ContactsContract.CommonDataKinds;
using StructuredName = Android.Provider.ContactsContract.CommonDataKinds.StructuredName;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static async Task<Contact> PlatformPickContactAsync()
        {
            var intent = new Intent(Intent.ActionPick, ContactsContract.Contacts.ContentUri);

            var result = await IntermediateActivity.StartAsync(intent, Platform.requestCodePickContact).ConfigureAwait(false);

            if (result?.Data == null)
                    return null;

            using var cursor = Platform.ContentResolver.Query(result?.Data, null, null, null, null);

            if (cursor.MoveToFirst())
                return GetContact(cursor);

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
                        var contact = GetContact(cursor);
                        if (contact != null)
                            yield return contact;
                    }
                    while (cursor.MoveToNext());
                }

                cursor.Close();
            }
        }

        static Contact GetContact(ICursor cursor)
        {
            var displayName = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
            var idQ = new string[1]
            {
                cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.Id))
            };
            var phones = GetNumbers(idQ)?.Select(
                p => new ContactPhone(p));
            var emails = GetEmails(idQ)?.Select(
                e => new ContactEmail(e));
            var name = GetName(idQ[0]);

            return new Contact(idQ[0], name.Prefix, name.Given, name.Middle, name.Family, name.Suffix, phones, emails, displayName);
        }

        static IEnumerable<string> GetNumbers(string[] idQ)
        {
            var uri = CommonDataKinds.Phone.ContentUri.BuildUpon()
                .AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();
            var cursor = Platform.ContentResolver.Query(uri, null, $"{CommonDataKinds.Phone.InterfaceConsts.ContactId}=?", idQ, null);

            return ReadCursorItems(cursor, CommonDataKinds.Phone.Number);
        }

        static IEnumerable<string> GetEmails(string[] idQ)
        {
            var uri = CommonDataKinds.Email.ContentUri.BuildUpon()
                .AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1").Build();
            var cursor = Platform.ContentResolver.Query(uri, null, $"{CommonDataKinds.Phone.InterfaceConsts.ContactId}=?", idQ, null);

            return ReadCursorItems(cursor, CommonDataKinds.Email.Address);
        }

        static IEnumerable<string> ReadCursorItems(ICursor cursor, string dataKey)
        {
            if (cursor?.MoveToFirst() ?? false)
            {
                do
                {
                    var data = cursor.GetString(cursor.GetColumnIndex(dataKey));

                    if (data != null)
                        yield return data;
                }
                while (cursor.MoveToNext());
            }
            cursor?.Close();
        }

        static (string Prefix, string Given, string Middle, string Family, string Suffix) GetName(string idQ)
        {
            var whereNameParams = new string[] { StructuredName.ContentItemType };
            var whereName = $"{ContactsContract.Data.InterfaceConsts.Mimetype} = ? " +
                $"AND {StructuredName.InterfaceConsts.ContactId} = {idQ}";

            using var cursor = Platform.ContentResolver.Query(
                ContactsContract.Data.ContentUri,
                null,
                whereName,
                whereNameParams,
                null);

            if (cursor?.MoveToFirst() ?? false)
            {
                return (
                    cursor.GetString(cursor.GetColumnIndex(StructuredName.Prefix)),
                    cursor.GetString(cursor.GetColumnIndex(StructuredName.GivenName)),
                    cursor.GetString(cursor.GetColumnIndex(StructuredName.MiddleName)),
                    cursor.GetString(cursor.GetColumnIndex(StructuredName.FamilyName)),
                    cursor.GetString(cursor.GetColumnIndex(StructuredName.Suffix)));
            }

            return (null, null, null, null, null);
        }
    }
}
