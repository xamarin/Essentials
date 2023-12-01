﻿using Android.App;
using Android.Content;
using Android.OS;

namespace Xamarin.Essentials
{
    public abstract class WebAuthenticatorCallbackActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Check how we launched the flow initially
            if (WebAuthenticator.AuthenticatingWithCustomTabs)
            {
                // start the intermediate activity again with flags to close the custom tabs
                var intent = new Intent(this, typeof(WebAuthenticatorIntermediateActivity));
                intent.SetData(Intent.Data);
                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
                StartActivity(intent);
            }
            else
            {
                // No intermediate activity if we returned from a system browser
                // intent since there's no custom tab instance to clean up
                WebAuthenticator.OnResume(Intent);
            }

            // finish this activity
            Finish();
        }
    }
}
