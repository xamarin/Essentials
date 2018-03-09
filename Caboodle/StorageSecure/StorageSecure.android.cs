using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Hardware;
using Java.Security;
using Javax.Crypto;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// </summary>
    public partial class StorageSecure
    {
        public static string Load(string filename)
        {
            var keyStore = LoadKeyStore();

            var entry = keyStore.Item1.GetEntry(filename, keyStore.Item2) as KeyStore.SecretKeyEntry;

            if (entry != null)
            {
                var bytes = entry.SecretKey.GetEncoded();
                return System.Text.Encoding.UTF8.GetString(bytes);
            }

            return null;
        }

        public static void Save(string data, string filename)
        {
            var ks = LoadKeyStore();
            ks.Item1.SetEntry(filename, new KeyStore.SecretKeyEntry(new SecretEntry(data)), ks.Item2);
            SaveKeyStore(ks.Item1);

            return;
        }

        static readonly object fileLock = new object();

        static Tuple<KeyStore, KeyStore.PasswordProtection> LoadKeyStore()
        {
            var context = Platform.CurrentContext;
            var secureKey = GetSecureKey(context);
            var keyStore = KeyStore.GetInstance(KeyStore.DefaultType);
            var prot = new KeyStore.PasswordProtection(secureKey);

            try
            {
                lock (fileLock)
                {
                    if (context.GetFileStreamPath(StorageSecureFilename)?.Exists() ?? false)
                    {
                        using (var s = context.OpenFileInput(StorageSecureFilename))
                            keyStore.Load(s, secureKey);
                    }
                    else
                    {
                        keyStore.Load(null, secureKey);
                    }
                }
            }
            catch
            {
                keyStore.Load(null, secureKey);
            }

            return Tuple.Create(keyStore, prot);
        }

        static void SaveKeyStore(KeyStore keyStore)
        {
            var context = Platform.CurrentContext;

            lock (fileLock)
            {
                using (var s = context.OpenFileOutput(StorageSecureFilename, FileCreationMode.Private))
                {
                    keyStore.Store(s, GetSecureKey(context));
                    s.Flush();
                    s.Close();
                }
            }

            return;
        }

        static char[] GetSecureKey(Context context)
        {
            var cacheKey = string.Empty;
            var prefs = context.GetSharedPreferences(StorageSecureFilename, FileCreationMode.Private);

            if (prefs.Contains(StorageSecureCacheKey))
            {
                cacheKey = prefs.GetString(StorageSecureCacheKey, string.Empty);

                if (!string.IsNullOrEmpty(cacheKey))
                    return cacheKey.ToCharArray();
            }

            // Generate a 256-bit key
            const int outputKeyLength = 256;

            var secureRandom = new SecureRandom();

            // Do *not* seed secureRandom! Automatically seeded from system entropy.
            var keyGenerator = KeyGenerator.GetInstance("AES");
            keyGenerator.Init(outputKeyLength, secureRandom);
            var key = keyGenerator.GenerateKey();
            cacheKey = Convert.ToBase64String(key.GetEncoded());

            prefs.Edit()
                 .PutString(StorageSecureCacheKey, cacheKey)
                 .Commit();

            return cacheKey.ToCharArray();
        }

        class SecretEntry : Java.Lang.Object, ISecretKey
        {
            byte[] bytes;

            public SecretEntry(string value)
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(value);
            }

            public string Algorithm
            {
                get { return "RAW"; }
            }

            public string Format
            {
                get { return "RAW"; }
            }

            public byte[] GetEncoded()
            {
                return bytes;
            }
        }
    }
}
