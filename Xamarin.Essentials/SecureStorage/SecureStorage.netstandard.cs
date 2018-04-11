using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public partial class SecureStorage
    {
        private static Task<string> PlatformGetAsync(string key) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static Task PlatformSetAsync(string key, string data) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
