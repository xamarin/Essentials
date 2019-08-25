using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static Task<PhoneContact> PlatformPickContactAsync() => throw new NotImplementedException();

        static Task PlatformSaveContactAsync(string name, string phone, string email) => throw new NotImplementedException();
    }
}
