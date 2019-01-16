using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Content = Android.Content;

namespace Xamarin.Essentials
{
    [Activity(ConfigurationChanges = Content.PM.ConfigChanges.Orientation | Content.PM.ConfigChanges.ScreenSize)]
#pragma warning disable IDE0040 // Adicionar modificadores de acessibilidade
    internal class ContactActivity : Activity
#pragma warning restore IDE0040 // Adicionar modificadores de acessibilidade
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartContactPicker();
        }

        void StartContactPicker()
        {
            var intent = new Content.Intent(Content.Intent.ActionPick);
            intent.SetType(ContactsContract.CommonDataKinds.Phone.ContentType);
            StartActivityForResult(intent, 101);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Canceled)
                Finish();

            try
            {
                if (requestCode != 101)
                    return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Finish();
            }
        }
    }
}
