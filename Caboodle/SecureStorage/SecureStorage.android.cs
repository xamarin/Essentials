using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using Android.Security;
using Android.Security.Keystore;
using Java.Security;
using Javax.Crypto;
using Javax.Crypto.Spec;

namespace Microsoft.Caboodle
{
    public static partial class SecureStorage
    {
        static string GetDirPath()
        {
            var p = Path.Combine(Application.Context.FilesDir.AbsolutePath);
            if (!Directory.Exists(p))
                Directory.CreateDirectory(p);
            return p;
        }

        static string GetFilePath(string filename) =>
            Path.Combine(GetDirPath(), filename + ".dat");

        static string Alias =>
            $"{localDirName}-{AppInfo.PackageName}";

        static Task<string> PlatformGetAsync(string key)
        {
            return Task.Run(() =>
            {
                var dirPath = GetDirPath();
                var filePath = GetFilePath(key);

                var encryptedData = File.ReadAllBytes(filePath);

                var ks = new AndroidKeyStore(Alias, dirPath);
                var decryptedData = ks.Decrypt(encryptedData);

                return Task.FromResult(decryptedData);
            });
        }

        static Task PlatformSetAsync(string key, string data)
        {
            return Task.Run(() =>
            {
                var dirPath = GetDirPath();
                var filePath = GetFilePath(key);

                var ks = new AndroidKeyStore(Alias, dirPath);
                var encryptedData = ks.Encrypt(data);

                File.WriteAllBytes(filePath, encryptedData);
            });
        }
    }

    class AndroidKeyStore
    {
        const string androidKeyStore = "AndroidKeyStore"; // this is an Android const value
        const string cipherTransformationAsymmetric = "RSA/ECB/PKCS1Padding";
        const string cipherTransformationSymmetric = "AES/CBC/PKCS7Padding";

        const int initializationVectorLen = 16;

        public AndroidKeyStore(string keystoreAlias, string storageDir)
        {
            alias = keystoreAlias;
            dir = storageDir;

            keyStore = KeyStore.GetInstance(androidKeyStore);
            keyStore.Load(null);
        }

        string alias;
        string dir;
        KeyStore keyStore;

        ISecretKey GetKey()
        {
            // If >= API 23 we can use the keystore's symmetric key
            if (HasMarshmallow)
                return GetSymmetricKey();

            // Otherwise we need to wrap our symmetric key with an asymmetric key
            // and save it somewhere convenient to use to decrypt data in the future
            // Look for existing default key
            var keyPath = Path.Combine(dir, "idx.dat");

            // Get or create our asymmetric key from the keystore
            var keyPair = GetAsymmetricKeyPair();

            if (File.Exists(keyPath))
            {
                var wrappedKey = File.ReadAllBytes(keyPath);

                return UnwrapKey(wrappedKey, keyPair.Private) as ISecretKey;
            }
            else
            {
                var keyGenerator = KeyGenerator.GetInstance("AES");
                var defSymmetricKey = keyGenerator.GenerateKey();

                var wrappedKey = WrapKey(defSymmetricKey, keyPair.Public);
                File.WriteAllBytes(keyPath, wrappedKey);

                return defSymmetricKey;
            }
        }


        // API 23+ Only
        ISecretKey GetSymmetricKey()
        {
            var existingKey = keyStore.GetKey(alias, null);

            if (existingKey != null)
            {
                var existingSecretKey = existingKey.JavaCast<ISecretKey>();
                return existingSecretKey;
            }
            var keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, androidKeyStore);

            var builder = new KeyGenParameterSpec.Builder(alias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                .SetBlockModes(KeyProperties.BlockModeCbc)
                .SetEncryptionPaddings(KeyProperties.EncryptionPaddingPkcs7);

            keyGenerator.Init(builder.Build());

            return keyGenerator.GenerateKey();
        }

        KeyPair GetAsymmetricKeyPair()
        {
            var privateKey = keyStore.GetKey(alias, null) as IPrivateKey;
            var publicKey = keyStore.GetCertificate(alias)?.PublicKey;

            // Return the existing key if found
            if (privateKey != null && publicKey != null)
                return new KeyPair(publicKey, privateKey);

            // Otherwise we create a new key
            var generator = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmRsa, androidKeyStore);

            if (HasMarshmallow)
            {
                var builder = new KeyGenParameterSpec.Builder(alias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                    .SetBlockModes(KeyProperties.BlockModeEcb)
                    .SetEncryptionPaddings(KeyProperties.EncryptionPaddingRsaPkcs1);

                generator.Initialize(builder.Build());
            }
            else
            {
                var end = DateTime.UtcNow.AddYears(20);
                var startDate = new Java.Util.Date();
                var endDate = new Java.Util.Date(end.Year, end.Month, end.Day);

                var builder = new KeyPairGeneratorSpec.Builder(Platform.CurrentContext)
                    .SetAlias(alias)
                    .SetSerialNumber(Java.Math.BigInteger.One)
                    .SetSubject(new Javax.Security.Auth.X500.X500Principal($"CN={alias} CA Certificate"))
                    .SetStartDate(startDate)
                    .SetEndDate(endDate);

                generator.Initialize(builder.Build());
            }

            return generator.GenerateKeyPair();
        }

        byte[] WrapKey(IKey keyToWrap, IKey withKey)
        {
            var cipher = Cipher.GetInstance(cipherTransformationAsymmetric);
            cipher.Init(CipherMode.WrapMode, withKey);
            return cipher.Wrap(keyToWrap);
        }

        IKey UnwrapKey(byte[] wrappedData, IKey withKey)
        {
            var cipher = Cipher.GetInstance(cipherTransformationAsymmetric);
            cipher.Init(CipherMode.UnwrapMode, withKey);
            return cipher.Unwrap(wrappedData, KeyProperties.KeyAlgorithmAes, KeyType.SecretKey);
        }

        public byte[] Encrypt(string data)
        {
            var key = GetKey();

            var cipher = Cipher.GetInstance(cipherTransformationSymmetric);
            cipher.Init(CipherMode.EncryptMode, key);

            // Get the android random generated IV used
            var iv = cipher.GetIV();

            var decryptedData = Encoding.UTF8.GetBytes(data);
            var encryptedBytes = cipher.DoFinal(decryptedData);

            // Combine the IV and the encrypted data into one array
            var r = new byte[iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, r, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, r, iv.Length, encryptedBytes.Length);

            return r;
        }

        public string Decrypt(byte[] data)
        {
            if (data.Length < initializationVectorLen)
                return null;

            var key = GetKey();
            var cipher = Cipher.GetInstance(cipherTransformationSymmetric);

            // IV will be the first 16 bytes of the encrypted data
            var iv = new IvParameterSpec(data, 0, initializationVectorLen);

            cipher.Init(CipherMode.DecryptMode, key, iv);

            // Decrypt starting after the first 16 bytes from the IV
            var decryptedData = cipher.DoFinal(data, initializationVectorLen, data.Length - initializationVectorLen);

            return Encoding.UTF8.GetString(decryptedData);
        }

        static bool HasMarshmallow =>
            (int)Android.OS.Build.VERSION.SdkInt >= 23;
    }
}
