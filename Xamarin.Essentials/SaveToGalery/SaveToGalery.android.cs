using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Java.IO;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using Path = System.IO.Path;

namespace Xamarin.Essentials.SaveToGalery
{
    public static partial class SaveToGalery
    {
        static async Task PlatformSaveImageAsync(byte[] data, string filename, string hhh)
        {
            try
            {
#if MONOANDROID10_0
                var bitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length);
                var resolver = Platform.AppContext.ContentResolver;
                var contentValues = new ContentValues();
                contentValues.Put(MediaStore.MediaColumns.DisplayName, filename);
                contentValues.Put(MediaStore.MediaColumns.MimeType, "image/*");
                contentValues.Put(MediaStore.MediaColumns.RelativePath, Environment.DirectoryPictures);
                contentValues.Put(
                    MediaStore.MediaColumns.RelativePath,
                    Path.Combine(Environment.DirectoryPictures, AppInfo.Name));
                var imageUri = resolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
                var fos = resolver.OpenOutputStream(imageUri);
                var result = await bitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 100, fos);
                fos?.Close();
#endif
#if !MONOANDROID10_0
                var imagesDir = new File(
                    Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures),
                    AppInfo.Name);
                imagesDir.Mkdirs();
                var fos = new FileOutputStream(new File(imagesDir, filename));
                await fos.WriteAsync(data);
                fos?.Close();
#endif
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HapticFeedback Exception: {ex.Message}");
            }

            // var folderDirectory = Platform.AppContext.GetExternalFilesDir(Environment.DirectoryDcim);

            // using (var bitmapFile = new File(folderDirectory, filename))
            // {
            //    bitmapFile.CreateNewFile();

            // using (var outputStream = new FileOutputStream(bitmapFile))
            //        await outputStream.WriteAsync(data);

            // MediaScannerConnection.ScanFile(
            //        Platform.CurrentActivity,
            //        new string[] { bitmapFile.Path },
            //        new string[] { "image/png", "image/jpeg" },
            //        null);
            // }
            // MediaStore.Images.Media.InsertImage

            // var picturesDirectory = Environment.DirectoryPictures;

            // var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures), "count.png");
            //         await System.IO.File.WriteAllBytesAsync(backingFile, data);

            // MediaScannerConnection.ScanFile(
            //            Platform.CurrentActivity,
            //            new string[] { backingFile },
            //            new string[] { "image/png", "image/jpeg" },
            //            null);
        }
    }
}
