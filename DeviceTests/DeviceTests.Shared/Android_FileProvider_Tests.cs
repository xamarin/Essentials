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
        [Fact]
        [Trait(Traits.InteractionType, Traits.InteractionTypes.Human)]
        public void Share_Simple_Text_File_Test()
        {
            // Save a local cache data directory file
            var file = Path.Combine(FileSystem.AppDataDirectory, "share-test.txt");
            File.WriteAllText(file, "Test file contents.");

            // Actually get a safe shareable file uri
            var shareableUri = Platform.GetShareableFileUri(file);

            // Launch an intent to let tye user pick where to open this content
            var intent = new Android.Content.Intent(Android.Content.Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Android.Content.Intent.ExtraStream, shareableUri);
            intent.PutExtra(Android.Content.Intent.ExtraTitle, "Title Here");
            intent.SetFlags(Android.Content.ActivityFlags.GrantReadUriPermission);

            var intentChooser = Android.Content.Intent.CreateChooser(intent, "Pick something");

            Platform.AppContext.StartActivity(intentChooser);
        }

        [Fact]
        public void Get_Internal_Shareable_Uri()
        {
            // Save a local cache data directory file
            var file = Path.Combine(FileSystem.AppDataDirectory, "internal-test.txt");
            File.WriteAllText(file, "Internal file contents.");

            // Actually get a safe shareable file uri
            var shareableUri = GetShareableUri(file, FileProviderLocation.Internal);

            // Make sure the uri is what we expected
            Assert.NotNull(shareableUri);
            Assert.Equal("content", shareableUri.Scheme);
            Assert.Equal("com.xamarin.essentials.devicetests.fileProvider", shareableUri.Authority);
            Assert.Equal(3, shareableUri.PathSegments.Count);
            Assert.Equal("internal_files", shareableUri.PathSegments[0]);
            Assert.True(Guid.TryParseExact(shareableUri.PathSegments[1], "N", out var guid));
            Assert.Equal("internal-test.txt", shareableUri.PathSegments[2]);

            // Make sure the underlying file exists
            var realPath = Path.Combine(
                Platform.AppContext.CacheDir.AbsolutePath, // the internal cache
                "2203693cc04e0be7f4f024d5f9499e13",        // the xamarin.essentials hash
                shareableUri.PathSegments[1],              // the temporary guid for this action
                shareableUri.PathSegments[2]);             // the temporary file
            Assert.True(File.Exists(realPath));
        }

        [Fact]
        public void Get_External_Shareable_Uri()
        {
            // Save a local cache data directory file
            var file = Path.Combine(FileSystem.AppDataDirectory, "external-test.txt");
            File.WriteAllText(file, "External file contents.");

            // Actually get a safe shareable file uri
            var shareableUri = GetShareableUri(file, FileProviderLocation.External);

            // Make sure the uri is what we expected
            Assert.NotNull(shareableUri);
            Assert.Equal("content", shareableUri.Scheme);
            Assert.Equal("com.xamarin.essentials.devicetests.fileProvider", shareableUri.Authority);
            Assert.Equal(3, shareableUri.PathSegments.Count);
            Assert.Equal("external_files", shareableUri.PathSegments[0]);
            Assert.True(Guid.TryParseExact(shareableUri.PathSegments[1], "N", out var guid));
            Assert.Equal("external-test.txt", shareableUri.PathSegments[2]);

            // Make sure the underlying file exists
            var realPath = Path.Combine(
                Platform.AppContext.ExternalCacheDir.AbsolutePath, // the external cache
                "2203693cc04e0be7f4f024d5f9499e13",                // the xamarin.essentials hash
                shareableUri.PathSegments[1],                      // the temporary guid for this action
                shareableUri.PathSegments[2]);                     // the temporary file
            Assert.True(File.Exists(realPath));
        }

        static Android.Net.Uri GetShareableUri(string file, FileProviderLocation location)
        {
            try
            {
                // use the specific location
                FileProvider.TemporaryLocation = location;

                // get the uri
                return Platform.GetShareableFileUri(file);
            }
            finally
            {
                // reset the location
                FileProvider.TemporaryLocation = FileProviderLocation.PreferExternal;
            }
        }
    }
#endif
}
