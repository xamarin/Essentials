using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Flashlight
    {
        static Task PlatformTurnOnAsync() =>
            ThrowHelper.ThrowNotImplementedException<Task>();

        static Task PlatformTurnOffAsync() =>
            ThrowHelper.ThrowNotImplementedException<Task>();
    }
}
