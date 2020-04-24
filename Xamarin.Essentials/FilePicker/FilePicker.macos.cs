using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppKit;
using MobileCoreServices;

namespace Xamarin.Essentials
{
    public static partial class FilePicker
    {
        static NSSavePanel CreatePanel(PickOptions options, bool multipleSelect, bool save)
        {
            // We use NSSavePanel as NSOpenPanel inherits from this.
            NSSavePanel filePanel;
            if (save)
            {
                filePanel = new NSSavePanel();

                if (!string.IsNullOrEmpty(options.SuggestedFileName))
                    filePanel.NameFieldStringValue = options.SuggestedFileName;
            }
            else
            {
                filePanel = new NSOpenPanel()
                {
                    CanChooseFiles = true,
                    AllowsMultipleSelection = multipleSelect,
                    CanChooseDirectories = false,
                };
            }

            if (options.PickerTitle != null)
                filePanel.Title = options.PickerTitle;

            SetFileTypes(options, filePanel);

            return filePanel;
        }

        static Task<FilePickerResult> PlatformPickFileAsync(PickOptions options)
        {
            var openPanel = CreatePanel(options, false, false) as NSOpenPanel;

            FilePickerResult result = null;
            var panelResult = openPanel.RunModal();
            if (panelResult == (nint)(long)NSModalResponse.OK)
            {
                result = new FilePickerResult(openPanel.Urls[0].Path);
            }

            return Task.FromResult(result);
        }

        static Task<FilePickerResult> PlatformPickFileToSaveAsync(PickOptions options)
        {
            var savePanel = CreatePanel(options, false, true);

            FilePickerResult result = null;
            var panelResult = savePanel.RunModal();
            if (panelResult == (nint)(long)NSModalResponse.OK)
            {
                result = new FilePickerResult(savePanel.Url.Path);
            }

            return Task.FromResult(result);
        }

        static Task<IEnumerable<FilePickerResult>> PlatformPickMultipleFilesAsync(PickOptions options)
        {
            var openPanel = CreatePanel(options, true, false) as NSOpenPanel;

            var resultList = new List<FilePickerResult>();
            var panelResult = openPanel.RunModal();
            if (panelResult == (nint)(long)NSModalResponse.OK)
            {
                foreach (var url in openPanel.Urls)
                {
                    resultList.Add(new FilePickerResult(url.Path));
                }
            }

            return Task.FromResult<IEnumerable<FilePickerResult>>(resultList);
        }

        static void SetFileTypes(PickOptions options, NSSavePanel panel)
        {
            var allowedFileTypes = new List<string>();

            if (options?.FileTypes?.Value != null)
            {
                foreach (var type in options.FileTypes.Value)
                {
                    allowedFileTypes.Add(type.TrimStart('*', '.'));
                }
            }

            panel.AllowedFileTypes = allowedFileTypes.ToArray();
        }
    }

    public partial class FilePickerFileType
    {
        public static FilePickerFileType PlatformImageFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.macOS, new string[] { UTType.PNG, UTType.JPEG, "jpeg" } }
            });

        public static FilePickerFileType PlatformPngFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.macOS, new string[] { UTType.PNG } }
            });
    }

    public partial class FilePickerResult
    {
        internal FilePickerResult(string filePath)
            : base(filePath)
        {
            FileName = Path.GetFileName(filePath);
        }

        Task<Stream> PlatformOpenReadStreamAsync()
        {
            var stream = File.Open(FullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult<Stream>(stream);
        }

        Task<Stream> PlatformOpenWriteStreamAsync()
        {
            var stream = File.Open(FullPath, FileMode.OpenOrCreate, FileAccess.Write);
            return Task.FromResult<Stream>(stream);
        }
    }
}
