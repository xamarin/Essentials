using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials.SaveToGalery
{
    public static partial class SaveToGalery
    {
        static Task PlatformSaveImageAsync(byte[] data, string filename, string albumName)
            => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
