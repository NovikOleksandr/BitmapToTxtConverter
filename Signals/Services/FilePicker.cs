using Signals.Contracts;
using Signals.Extensions;
using Signals.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Signals.Services
{
    public sealed class FilePicker : IFilePicker
    {
        public async Task<StorageFile> ShowSaveDialogAsync(params KeyValuePair<string, IList<string>>[] types)
        {
            var picker = new FileSavePicker()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "Captured frame"
            };

            picker.FileTypeChoices.AddRange(types);

            Win32Helper.ProvideWindowHandle(picker, App.MainWindow);

            return await picker.PickSaveFileAsync();
        }

        public async Task<StorageFile> ShowOpenDialogAsync(params string[] extensions)
        {
            var picker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };
            picker.FileTypeFilter.AddRange(extensions);

            Win32Helper.ProvideWindowHandle(picker, App.MainWindow);

            return await picker.PickSingleFileAsync();

        }
    }
}
