using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class AppleSignInAuthenticator
    {
        public static Task<AuthResult> AuthenticateAsync(bool includeFullNameScope = true, bool includeEmailScope = true)
            => PlatformAuthenticateAsync(includeFullNameScope, includeEmailScope);

        public static bool IsSupported
            => PlatformIsSupported;
    }
}
