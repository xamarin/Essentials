using System;
using Android.App;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Content = Android.Content;

namespace Xamarin.Essentials
{
    [Activity(ConfigurationChanges = Content.PM.ConfigChanges.Orientation | Content.PM.ConfigChanges.ScreenSize)]
#pragma warning disable IDE0040 // Add accessibility modifiers
    internal class ContactActivity : Activity
#pragma warning restore IDE0040 // Add accessibility modifiers
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartContactPicker();
        }

        void StartContactPicker()
        {
            using var intent = new Content.Intent(Content.Intent.ActionPick);
            intent.SetType(ContactsContract.CommonDataKinds.Phone.ContentType);
            StartActivityForResult(intent, 101);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            try
            {
                if (requestCode != 101)
                    return;

                if (data?.Data != null)
                    Contact.CallBack(Contact.PlatformGetContacts(data.Data));
                else
                    Contact.CallBack(default);
            }
            catch (Exception ex)
            {
                Contact.ErrorCallBack(ex);
            }
            finally
            {
                Finish();
            }
        }
    }
}
