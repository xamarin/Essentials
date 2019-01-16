using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Contact
    {
        static Task<PhoneContact> PlataformPickContactAsync()
        {
            return null;
        }

        static Task PlataformSaveContactAsync(string name, string phone, string email) => throw new NotImplementedException();
    }
}
