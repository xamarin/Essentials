﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace Xamarin.Essentials
{
    public static partial class FilePicker
    {
        static async Task<IEnumerable<FileResult>> PlatformPickAsync(PickOptions options, bool allowMultiple = false)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            SetFileTypes(options, picker);

            var resultList = new List<StorageFile>();

            if (allowMultiple)
            {
                var fileList = await picker.PickMultipleFilesAsync();
                if (fileList != null)
                    resultList.AddRange(fileList);
            }
            else
            {
                var file = await picker.PickSingleFileAsync();
                if (file != null)
                    resultList.Add(file);
            }

            AddToAndCleanUpFutureAccessList(resultList);

            return resultList.Select(storageFile => new FileResult(storageFile));
        }

        static void AddToAndCleanUpFutureAccessList(List<StorageFile> pickedFiles)
        {
            var fal = StorageApplicationPermissions.FutureAccessList;

            try
            {
                // Check if (FutureAccessList current items + new picked files) is exceeding maximum items
                if ((fal.Entries.Count + pickedFiles.Count) >= fal.MaximumItemsAllowed)
                {
                    // We assume that the FutureAccessList items are saved in order, there is no metadata
                    // Therefore we start removing from the bottom of the list
                    var removeCount = Math.Min(fal.Entries.Count, pickedFiles.Count);
                    var falTokens = fal.Entries.TakeLast(removeCount).ToList();

                    foreach (var entry in falTokens)
                    {
                        fal.Remove(entry.Token);
                    }
                }

                // Check if the picked file count doesn't exceed the maximum, else take the last n number or picked files
                var pickedFilesLimitedToMax = pickedFiles;
                if (pickedFiles.Count > StorageApplicationPermissions.FutureAccessList.MaximumItemsAllowed)
                {
                    pickedFilesLimitedToMax =
                        pickedFilesLimitedToMax.TakeLast((int)fal.MaximumItemsAllowed).ToList();
                }

                foreach (var file in pickedFilesLimitedToMax)
                {
                    StorageApplicationPermissions.FutureAccessList.Add(file);
                }
            }
            catch (Exception ex)
            {
                // Just log, we don't want to break on this
                Debug.WriteLine($"Error adding to/cleaning up FutureAccessList: {ex.Message}");
            }
        }

        static void SetFileTypes(PickOptions options, FileOpenPicker picker)
        {
            var hasAtLeastOneType = false;

            if (options?.FileTypes?.Value != null)
            {
                foreach (var type in options.FileTypes.Value)
                {
                    var ext = FileSystem.Extensions.Clean(type);
                    if (!string.IsNullOrWhiteSpace(ext))
                    {
                        picker.FileTypeFilter.Add(ext);
                        hasAtLeastOneType = true;
                    }
                }
            }

            if (!hasAtLeastOneType)
                picker.FileTypeFilter.Add("*");
        }
    }

    public partial class FilePickerFileType
    {
        static FilePickerFileType PlatformImageFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.UWP, FileSystem.Extensions.AllImage }
            });

        static FilePickerFileType PlatformPngFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.UWP, new[] { FileSystem.Extensions.Png } }
            });

        static FilePickerFileType PlatformJpegFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.UWP, FileSystem.Extensions.AllJpeg }
            });

        static FilePickerFileType PlatformVideoFileType() =>
           new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
           {
                { DevicePlatform.UWP, FileSystem.Extensions.AllVideo }
           });

        static FilePickerFileType PlatformPdfFileType() =>
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.UWP, new[] { FileSystem.Extensions.Pdf } }
            });
    }
}
