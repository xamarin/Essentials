using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Net = Android.Net;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static Activity Activity => Platform.GetCurrentActivity(true);

        internal static Action<PhoneContact> CallBack { get; set; }

        static async Task<PhoneContact> PlataformPickContactAsync()
        {
            var phoneContact = await GetContactFromActivity();
            return phoneContact;
        }

        static Task<PhoneContact> GetContactFromActivity()
        {
            var source = new TaskCompletionSource<PhoneContact>();
            try
            {
                var contactPicker = new Intent(Activity, typeof(ContactActivity));
                contactPicker.SetFlags(ActivityFlags.NewTask);
                Activity.StartActivity(contactPicker);

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

        internal static PhoneContact GetContactFromUri(Net.Uri contactUri)
        {
            var context = Activity.ContentResolver;

            if (contactUri == null)
                contactUri = ContactsContract.Contacts.ContentUri;

            var cur = context.Query(contactUri, null, null, null, null);
            var emails = new List<string>();
            var phones = new List<string>();
            var name = string.Empty;
            var birthday = string.Empty;

            while (cur.MoveToNext())
            {
                name = cur.GetString(cur.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));

                var id = cur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId));

                var idQ = new string[] { id };

                var cursor = context.Query(
                    ContactsContract.CommonDataKinds.Email.ContentUri,
                    null,
                    ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + "= ?",
                    idQ,
                    null);

                while (cursor.MoveToNext())
                {
                    var endereco = cursor.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Email.Address));
                    emails.Add(endereco);
                }
                cursor.Close();

                cursor = context.Query(
                    ContactsContract.CommonDataKinds.Phone.ContentUri,
                    null,
                    ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = ?",
                    idQ,
                    null);

                while (cursor.MoveToNext())
                {
                    var phone = cursor.GetString(cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                    phones.Add(phone);
                }
                cursor.Close();

                var projection = new string[]
                {
                    ContactsContract.CommonDataKinds.StructuredPostal.Street,
                    ContactsContract.CommonDataKinds.StructuredPostal.City,
                    ContactsContract.CommonDataKinds.StructuredPostal.Postcode
                };
                cursor = null;

                cursor = context.Query(ContactsContract.Data.ContentUri, projection, ContactsContract.Data.InterfaceConsts.ContactId + " = ?", idQ, null);
                while (cursor.MoveToNext())
                {
                    // Add street in PhoneContact struct

                    var street = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                    var city = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                    var postCode = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                }

                cursor.Close();

                var query = ContactsContract.CommonDataKinds.CommonColumns.Type + " = " + 3
                     + " AND " + ContactsContract.CommonDataKinds.Event.InterfaceConsts.ContactId + " = ?";

                cursor = context.Query(ContactsContract.Data.ContentUri, null, query, idQ, null);
                while (cursor.MoveToNext())
                {
                    birthday = cursor.GetString(cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Event.StartDate));
                }
                cursor.Close();
            }

            return new PhoneContact(name, phones, emails, birthday);
        }

        static Task<IEnumerable<PhoneContact>> PlataformGetContacts(Net.Uri contactUri = null)
        {
            try
            {
                var phoneContacts = new List<PhoneContact>();

                if (contactUri is null)
                    contactUri = ContactsContract.Contacts.ContentUri;

                var context = Application.Context.ContentResolver;
                var phoneNumbers = new List<string>();
                var emails = new List<string>();

                var cur = context.Query(contactUri, null, null, null, null);

                if (cur is null | cur.Count == 0)
                    return default;

                while (cur.MoveToNext())
                {
                    var id = cur.GetString(cur.GetColumnIndexOrThrow(ContactsContract.Contacts.InterfaceConsts.Id));
                    var name = cur.GetString(cur.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));

                    var idQ = new string[] { id };
                    if (ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber.Length > 0)
                    {
                        var pCur = context.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = ?", idQ, null);
                        while (pCur.MoveToNext())
                        {
                            var phone = pCur.GetString(pCur.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                            phoneNumbers.Add(phone);
                        }
                        pCur.Close();
                    }

                    var eCur = context.Query(ContactsContract.CommonDataKinds.Email.ContentUri, null, ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + " = ?", idQ, null);

                    while (eCur.MoveToNext())
                    {
                        var email = eCur.GetString(eCur.GetColumnIndex(ContactsContract.CommonDataKinds.Email.Address));
                        emails.Add(email);
                    }

                    eCur.Close();

                    var b = string.Empty;
                    if (name.Contains("EuMesmo"))
                    {
                        var query = ContactsContract.CommonDataKinds.CommonColumns.Type + " = " + 3
                      + " AND " + ContactsContract.CommonDataKinds.Event.InterfaceConsts.ContactId + " = ?";

                        var bCur = context.Query(ContactsContract.Data.ContentUri, null, query, idQ, null);
                        while (bCur.MoveToNext())
                        {
                            b = bCur.GetString(bCur.GetColumnIndex(ContactsContract.CommonDataKinds.Event.StartDate));

                            // bool t = false;
                        }
                        bCur.Close();
                    }

                    if (name.Contains("EuMesmo"))
                    {
                        var projectionS = new[] { ContactsContract.CommonDataKinds.StructuredPostal.Street, ContactsContract.CommonDataKinds.StructuredPostal.City, ContactsContract.CommonDataKinds.StructuredPostal.Postcode };
                        var aCur = context.Query(ContactsContract.Data.ContentUri, projectionS, ContactsContract.Data.InterfaceConsts.ContactId + " = ?", idQ, null);
                        while (aCur.MoveToNext())
                        {
                            var street = aCur.GetString(aCur.GetColumnIndex(projectionS[0]));
                            var city = aCur.GetString(aCur.GetColumnIndex(projectionS[1]));
                            var postCode = aCur.GetString(aCur.GetColumnIndex(projectionS[2]));
                        }

                        aCur.Close();
                    }

                    phoneContacts.Add(new PhoneContact(name, phoneNumbers, emails, b));
                    emails.Clear();
                    phoneNumbers.Clear();
                }
                cur.Close();

                return Task.FromResult(phoneContacts.AsEnumerable());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static Task PlataformSaveContactAsync(string name, string phone, string email) => throw new NotImplementedException();
    }
}
