using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
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
                var activity = Platform.GetCurrentActivity(true);
                var contactPicker = new Intent(Platform.AppContext, typeof(ContactActivity));
                contactPicker.SetFlags(ActivityFlags.NewTask);
                Platform.AppContext.StartActivity(contactPicker);

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

        static Task PlataformSaveContactAsync(string name, string phone, string email) => throw new NotImplementedException();
    }
}
