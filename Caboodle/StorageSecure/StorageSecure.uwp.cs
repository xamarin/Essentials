using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// </summary>
    public partial class StorageSecure
    {
        public static async Task<string> LoadAsync(string filename)
        {
            byte[] bytes_protected;
            var file = await StorageFile.GetFileFromPathAsync(filename);

            using (var stream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
            using (var reader = new BinaryReader(stream))
            {
                var length = reader.ReadInt32();
                var data = reader.ReadBytes(length);
                var buffer = await StorageSecureProtectionExtensions.UnprotectAsync(data.AsBuffer()).ConfigureAwait(false);
                bytes_protected = buffer.ToArray();
            }

            return Encoding.UTF8.GetString(bytes_protected, 0, bytes_protected.Length);
        }

        public static async Task SaveAsync(string data, string filename)
        {
            var file = await StorageFile.GetFileFromPathAsync(filename);

            var bytes = Encoding.UTF8.GetBytes(data);
            var buffer = await StorageSecureProtectionExtensions.ProtectAsync(bytes.AsBuffer()).ConfigureAwait(false);
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

    internal static class StorageSecureProtectionExtensions
    {
        /// <summary>
        /// These providers do not require the enterprise authentication capability on either platform:
        /// "LOCAL=user"
        /// "LOCAL=machine"
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="protectionDescriptor"></param>
        /// <returns></returns>
        public static async Task<IBuffer> ProtectAsync(this IBuffer buffer, string protectionDescriptor = "LOCAL=user")
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (protectionDescriptor == null)
            {
                throw new ArgumentNullException(nameof(protectionDescriptor));
            }

            var provider = new DataProtectionProvider(protectionDescriptor);

            return await provider.ProtectAsync(buffer).AsTask().ConfigureAwait(false);
        }

        public static async Task<IBuffer> UnprotectAsync(this IBuffer buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            var provider = new DataProtectionProvider();

            return await provider.UnprotectAsync(buffer).AsTask().ConfigureAwait(false);
        }
    }
}
