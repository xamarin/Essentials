﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Provider;
using Net = Android.Net;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static Activity Activity => Platform.GetCurrentActivity(true);

        internal static Action<PhoneContact> CallBack { get; set; }

        static Task<PhoneContact> PlataformPickContactAsync()
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

        internal static IEnumerable<PhoneContact> PlataformGetContacts(Net.Uri contactUri = null)
        {
            var context = Activity.ContentResolver;
            var myList = new List<PhoneContact>();
            if (contactUri == null)
                contactUri = ContactsContract.Contacts.ContentUri;

            var loader = new CursorLoader(Activity, contactUri, null, null, null, null);
            var cur = (ICursor)loader.LoadInBackground();

            // var cur = context.Query(contactUri, null, null, null, null);
            var emails = new List<string>();
            var phones = new List<string>();
            var birthday = string.Empty;

            // var task = Task.Run(() =>
            // {
            //     Thread.Sleep(1000);
            //     myList.Add(default);
            //     return Task.FromResult<IEnumerable<PhoneContact>>(myList);
            // });
            try
            {
                if (cur.MoveToFirst())
                {
                    do
                    {
                        var name = cur.GetString(cur.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));

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

                        cursor = context.Query(ContactsContract.CommonDataKinds.StructuredPostal.ContentUri, projection, ContactsContract.Data.InterfaceConsts.ContactId + " = ?", idQ, null);
                        if (cursor.MoveToLast())
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
                        myList.Add(new PhoneContact(name, phones, emails, birthday));
                    }
                    while (cur.MoveToNext());
                }
                return myList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static Task PlataformSaveContactAsync(string name, string phone, string email)
        {
            var intent = new Intent(Intent.ActionInsert);
            intent.SetType(ContactsContract.Contacts.ContentType);
            intent.PutExtra(ContactsContract.Intents.Insert.Name, name);
            intent.PutExtra(ContactsContract.Intents.Insert.Phone, phone);
            intent.PutExtra(ContactsContract.Intents.Insert.Email, email);
            Activity.StartActivity(intent);

            return Task.CompletedTask;
        }
    }
}
