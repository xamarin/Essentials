using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class AppleSignInAuthenticator
    {
        static bool PlatformIsSupported =>
            false;

        static Task<AuthResult> PlatformAuthenticateAsync(bool includeFullNameScope = true, bool includeEmailScope = true) =>
            throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
