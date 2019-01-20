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
using static Android.Provider.ContactsContract.CommonDataKinds;
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

            var cur = context.Query(contactUri, null, null, null, null);

            cur.MoveToFirst();

            var id = cur.GetString(cur.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.Id));
            var name = cur.GetString(cur.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
            try
            {
                var idContato = cur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId));

                var idQ = new string[] { idContato };

                var ecur = context.Query(
                    ContactsContract.CommonDataKinds.Email.ContentUri,
                    null,
                    ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + "= ?",
                    idQ,
                    null);

                if (ecur.MoveToFirst())
                {
                    var endereco = ecur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Email.Address));
                }
                ecur.Close();

                var pCur = context.Query(
                    ContactsContract.CommonDataKinds.Phone.ContentUri,
                    null,
                    ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = ?",
                    idQ,
                    null);

                if (pCur.MoveToFirst())
                {
                    var phone = pCur.GetString(pCur.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                }

                var contact = PlataformGetContacts(contactUri);
                return contact.Result.First();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
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
                        var pCur = context.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, Phone.InterfaceConsts.ContactId + " = ?", idQ, null);
                        while (pCur.MoveToNext())
                        {
                            var phone = pCur.GetString(pCur.GetColumnIndex(Phone.Number));
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
                        var query = CommonColumns.Type + " = " + 3
                      + " AND " + Event.InterfaceConsts.ContactId + " = ?";

                        var bCur = context.Query(ContactsContract.Data.ContentUri, null, query, idQ, null);
                        while (bCur.MoveToNext())
                        {
                            b = bCur.GetString(bCur.GetColumnIndex(Event.StartDate));

                            // bool t = false;
                        }
                        bCur.Close();
                    }

                    if (name.Contains("EuMesmo"))
                    {
                        var projectionS = new[] { StructuredPostal.Street, StructuredPostal.City, StructuredPostal.Postcode };
                        var aCur = context.Query(ContactsContract.Data.ContentUri, projectionS, ContactsContract.Data.InterfaceConsts.ContactId + " = ?", idQ, null);
                        while (aCur.MoveToNext())
                        {
                            var street = aCur.GetString(aCur.GetColumnIndex(StructuredPostal.Street));
                            var city = aCur.GetString(aCur.GetColumnIndex(StructuredPostal.City));
                            var postCode = aCur.GetString(aCur.GetColumnIndex(StructuredPostal.Postcode));
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
