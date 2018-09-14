using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class ScreenShot
    {
        public static Task<string> CaptureAsync() => PlataformCaptureAsync();
    }
}
