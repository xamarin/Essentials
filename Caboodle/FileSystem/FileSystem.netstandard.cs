using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class FileSystem
    {
        public static string CacheDirectory
            => throw new NotImplentedInReferenceAssembly();

        public static string AppDataDirectory
            => throw new NotImplentedInReferenceAssembly();

        public static string UserDataDirectory
            => throw new NotImplentedInReferenceAssembly();

        public static Task<Stream> OpenAppPackageFileAsync(string filename)
             => throw new NotImplentedInReferenceAssembly();
    }
}
