using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Flashlight
    {
        private static Task PlatformTurnOnAsync() =>
            throw new NotImplementedInReferenceAssemblyException();

        private static Task PlatformTurnOffAsync() =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
