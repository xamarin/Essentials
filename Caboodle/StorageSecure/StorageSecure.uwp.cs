﻿using System;
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

                var provider = new DataProtectionProvider();
                var buffer = await provider.UnprotectAsync(data.AsBuffer()).AsTask().ConfigureAwait(false);

                bytes_protected = buffer.ToArray();
            }

            return Encoding.UTF8.GetString(bytes_protected, 0, bytes_protected.Length);
        }

        public static async Task SaveAsync(string data, string filename)
        {
            var file = await StorageFile.GetFileFromPathAsync(filename);
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
