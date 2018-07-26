using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xunit;

namespace DeviceTests.Shared
{
#if __ANDROID__
    public class Android_FileProvider_Tests
    {
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public void Share_Simple_Text_File_Test()
        {
            const string TestStr = "Test Text File";

            // Save a local cache data directory file
            var file = Path.Combine(FileSystem.CacheDirectory, "test.txt");
            File.WriteAllText(file, TestStr);

            // Actually get a safe shareable file uri
            var shareableUri = Platform.GetShareableFileUri(file);

            // Launch an intent to let tye user pick where to open this content
            var intent = new Android.Content.Intent(Android.Content.Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Android.Content.Intent.ExtraStream, shareableUri);
            intent.PutExtra(Android.Content.Intent.ExtraTitle, TestStr);
            intent.SetFlags(Android.Content.ActivityFlags.GrantReadUriPermission);

            var intentChooser = Android.Content.Intent.CreateChooser(intent, TestStr);

            Platform.AppContext.StartActivity(intentChooser);
        }
    }
#endif
}
