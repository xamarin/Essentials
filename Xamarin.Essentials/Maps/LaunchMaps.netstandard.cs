using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class LaunchMaps
    {
        internal static Task PlatformOpenMapsAsync(double latitude, double longitude, MapLaunchOptions options)
            => throw new NotImplementedInReferenceAssemblyException();

        internal static Task PlatformOpenMapsAsync(Placemark placemark, MapLaunchOptions options)
            => throw new NotImplementedInReferenceAssemblyException();
    }
}
