using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static Task<Contact> PlatformPickContactAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;

#if !NETSTANDARD1_0
        static IAsyncEnumerable<Contact> PlatformGetAllAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;
#endif
    }
}
