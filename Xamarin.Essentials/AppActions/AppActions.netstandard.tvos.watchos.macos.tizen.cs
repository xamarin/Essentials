using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class AppActions
    {
        internal static bool PlatformIsSupported
            => ThrowHelper.ThrowNotImplementedException<bool>();

        static Task<IEnumerable<AppAction>> PlatformGetAsync() =>
            ThrowHelper.ThrowNotImplementedException<Task<IEnumerable<AppAction>>>();

        static Task PlatformSetAsync(IEnumerable<AppAction> actions) =>
            ThrowHelper.ThrowNotImplementedException<Task>();
    }
}
