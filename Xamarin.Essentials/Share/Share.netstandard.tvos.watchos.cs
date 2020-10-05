using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Share
    {
        static Task PlatformRequestAsync(ShareTextRequest request) =>
            ThrowHelper.ThrowNotImplementedException<Task>();

        static Task PlatformRequestAsync(ShareFileRequest request) =>
            ThrowHelper.ThrowNotImplementedException<Task>();
    }
}
