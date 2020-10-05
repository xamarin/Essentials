using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public partial class SecureStorage
    {
        static Task<string> PlatformGetAsync(string key) =>
            ThrowHelper.ThrowNotImplementedException<Task<string>>();

        static Task PlatformSetAsync(string key, string data) =>
            ThrowHelper.ThrowNotImplementedException<Task>();

        static bool PlatformRemove(string key) =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        static void PlatformRemoveAll() =>
            ThrowHelper.ThrowNotImplementedException();
    }
}
