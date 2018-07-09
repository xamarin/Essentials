using System.Text;
using System.Threading.Tasks;
using Tizen.Security.SecureRepository;

namespace Xamarin.Essentials
{
    public partial class SecureStorage
    {
        static Task<string> PlatformGetAsync(string key)
        {
            try
            {
                return Task.FromResult(Encoding.UTF8.GetString(DataManager.Get(key, null)));
            }
            catch
            {
                Tizen.Log.Error(Platform.CurrentPackage.Label, "Failed to load data.");
                throw;
            }
        }

        static Task PlatformSetAsync(string key, string data)
        {
            try
            {
                DataManager.RemoveAlias(key);
            }
            catch
            {
                Tizen.Log.Info(Platform.CurrentPackage.Label, "No save data.");
            }

            try
            {
                DataManager.Save(key, Encoding.UTF8.GetBytes(data), new Policy());
            }
            catch
            {
                Tizen.Log.Error(Platform.CurrentPackage.Label, "Failed to save data.");
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
