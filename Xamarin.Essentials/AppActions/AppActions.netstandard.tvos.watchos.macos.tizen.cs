using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class AppActions
    {
        internal static bool PlatformIsSupported => false;

        static Task<IEnumerable<AppAction>> PlatformGetAsync() =>
            throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task PlatformSetAsync(IEnumerable<AppAction> actions) =>
            throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
