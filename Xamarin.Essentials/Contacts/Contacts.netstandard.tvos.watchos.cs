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

        static IEnumerable<Task<IEnumerable<Contact>>> PlatformGetAllTasks() => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
