using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static Task<Contact> PlatformPickContactAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;

#if !NETSTANDARD1_0
        static Task<IEnumerable<Contact>> PlatformGetAllAsync(CancellationToken cancellationToken) => throw ExceptionUtils.NotSupportedOrImplementedException;
#endif
    }
}
