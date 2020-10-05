using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class FilePicker
    {
        static Task<IEnumerable<FileResult>> PlatformPickAsync(PickOptions options, bool allowMultiple = false)
            => ThrowHelper.ThrowNotImplementedException<Task<IEnumerable<FileResult>>>();
    }

    public partial class FilePickerFileType
    {
        static FilePickerFileType PlatformImageFileType()
            => ThrowHelper.ThrowNotImplementedException<FilePickerFileType>();

        static FilePickerFileType PlatformPngFileType()
            => ThrowHelper.ThrowNotImplementedException<FilePickerFileType>();

        static FilePickerFileType PlatformVideoFileType()
            => ThrowHelper.ThrowNotImplementedException<FilePickerFileType>();
    }
}
