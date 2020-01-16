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
                try
                {
                    // This throws an exception if key does not exist. The second parameter is not the default value.
                    var existing = DataManager.Get(key, null);
                    if (existing != null)
                    {
                        DataManager.RemoveAlias(key);
                    }
                }
                catch
                {
                    Tizen.Log.Error(Platform.CurrentPackage.Label, "Key did not exist when setting");
                }

                DataManager.Save(key, Encoding.UTF8.GetBytes(data), new Policy());

                return Task.CompletedTask;
            }
            catch
            {
                Tizen.Log.Error(Platform.CurrentPackage.Label, "Failed to save data.");
                throw;
            }
        }

        static void PlatformRemoveAll()
        {
            try
            {
                foreach (var key in DataManager.GetAliases())
                {
                    DataManager.RemoveAlias(key);
                }
            }
            catch
            {
                Tizen.Log.Info(Platform.CurrentPackage.Label, "No save data.");
            }
        }

        static bool PlatformRemove(string key)
        {
            try
            {
                DataManager.RemoveAlias(key);
                return true;
            }
            catch
            {
                Tizen.Log.Info(Platform.CurrentPackage.Label, "Failed to remove data.");
                return false;
            }
        }
    }
}
