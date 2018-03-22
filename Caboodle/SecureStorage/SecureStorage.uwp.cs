using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;

namespace Microsoft.Caboodle
{
    public partial class SecureStorage
    {
        static string GetFilePath(string filename)
        {
            var dir = Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                localDirName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return Path.Combine(dir, filename + ".dat");
        }

        static async Task<string> PlatformGetAsync(string key)
        {
            var path = GetFilePath(key);
            byte[] bytes_protected;
            var file = await StorageFile.GetFileFromPathAsync(path);

            using (var stream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
            using (var reader = new BinaryReader(stream))
            {
                var length = reader.ReadInt32();
                var data = reader.ReadBytes(length);

                var provider = new DataProtectionProvider();
                var buffer = await provider.UnprotectAsync(data.AsBuffer()).AsTask().ConfigureAwait(false);

                bytes_protected = buffer.ToArray();
            }

            return Encoding.UTF8.GetString(bytes_protected, 0, bytes_protected.Length);
        }

        static async Task PlatformSetAsync(string key, string data)
        {
            var path = GetFilePath(key);
            var file = await StorageFile.GetFileFromPathAsync(path);
            var bytes = Encoding.UTF8.GetBytes(data);

            // LOCAL=user and LOCAL=machine do not require enterprise auth capability
            var provider = new DataProtectionProvider("LOCAL=user");
            var buffer = await provider.ProtectAsync(bytes.AsBuffer()).AsTask().ConfigureAwait(false);
            var bytes_protected = buffer.ToArray();

            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((int)bytes_protected.Length);
                writer.Write(bytes_protected);
            }

            return;
        }
    }
}
