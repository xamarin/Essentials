using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public partial class Permissions
    {
        public partial class BasePermission
        {
            public abstract Task<PermissionStatus> CheckStatusAsync();

            public abstract Task<PermissionStatus> RequestAsync();
        }
    }
}
