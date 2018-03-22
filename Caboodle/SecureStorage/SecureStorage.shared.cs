#if !NETSTANDARD1_0
using System.Security.Cryptography;
#endif
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class SecureStorage
    {
        // md5(Caboodle.SecureStorage)
        const string localDirName = "d2f74ffc823f1d9f015ee8edf1463498";

        public static Task<string> GetAsync(string key)
            => PlatformGetAsync(Md5Hash(key));

        public static Task SetAsync(string key, string value)
            => PlatformSetAsync(Md5Hash(key), value);

        internal static string Md5Hash(string input)
        {
#if NETSTANDARD1_0
            throw new NotImplementedInReferenceAssemblyException();
#else
            var hash = new StringBuilder();
            var md5provider = new MD5CryptoServiceProvider();
            var bytes = md5provider.ComputeHash(Encoding.UTF8.GetBytes(input));

            for (var i = 0; i < bytes.Length; i++)
                hash.Append(bytes[i].ToString("x2"));

            return hash.ToString();
#endif
        }
    }
}
