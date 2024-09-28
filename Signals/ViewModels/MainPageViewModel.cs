using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Signals.Contracts;
using Signals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Capture.Frames;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Signals.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool mirrorPreview;

        [ObservableProperty]
        private MediaFrameSourceGroup selectedDevice;

        private readonly ICameraCaptureService _captureService;
        private readonly ITextBitmapConverter _bitmapConverter;
        private readonly IFilePicker _picker;

        public IReadOnlyCollection<MediaFrameSourceGroup> Devices { get; private set; }
        public CapturedBitmap ImageSource { get; }

        public MainPageViewModel(ICameraCaptureService captureService, ITextBitmapConverter bitmapConverter, IFilePicker picker)
        {
            _picker = picker;
            _captureService = captureService;
            _bitmapConverter = bitmapConverter;
            mirrorPreview = false;

            ImageSource = new CapturedBitmap();
        }

        [RelayCommand]
        private async Task InitializeCaptureAsync(MediaPlayerElement captureElement)
        {
            // Find all available devices
            Devices = await MediaFrameSourceGroup.FindAllAsync();
            OnPropertyChanged(nameof(Devices));

            if (Devices.Count > 0)
            {
                SelectedDevice = Devices.First();

                // Initialize capture on first device
                await _captureService.InitializeAsync(captureElement, SelectedDevice);
            }
            else
            {
                //TODO: notify no cameras
            }
        }

        /// <summary>
        /// Captures and encodes image to txt file
        /// Then reads txt file and shows resulting image
        /// </summary>
        [RelayCommand]
        private async Task CaptureImageAsync()
        {
            // Get stream of bytes from current preview frame
            var imageStream = await _captureService.CreateBitmapAsync();

            // Save image data to the file
            var file = await SaveImageDataAsync(imageStream);

            await CreateImageFromFileAsync(file);
        }

        /// <summary>
        /// Saves image data to txt file
        /// </summary>
        /// <param name="imageStream"> Stream with image data </param>
        /// <returns> Storage file that represents created file </returns>
        private async Task<StorageFile> SaveImageDataAsync(IRandomAccessStream imageStream)
        {
            var txtType = new KeyValuePair<string, IList<string>>("Text files", [".txt"]);
            var file = await _picker.ShowSaveDialogAsync(txtType);

            // User selected file in picker
            if (file is not null)
            {
                await _bitmapConverter.ToTextAsync(imageStream, file);
            }

            return file;
        }

        /// <summary>
        /// Create image stream from provided file and shows it on UI 
        /// </summary>
        private async Task CreateImageFromFileAsync(StorageFile? file)
        {
            if (file is not null)
            {
                try
                {
                    // Read txt file's content to decode an image
                    var bmpStream = await _bitmapConverter.FromTextAsync(file);

                    // Show captured image on UI
                    await ImageSource.SetSourceAsync(bmpStream);
                }
                catch
                {
                    await App.MainWindow.ShowMessageAsync("Provided .txt file does not contain bitmap data", "Could not parse file");
                }
            }
        }

        /// <summary>
        /// When mirror preview property is changed will mirror the preview using service
        /// </summary>
        partial void OnMirrorPreviewChanged(bool value)
        {
            _captureService.MirrorPreview();
        }

        /// <summary>
        /// When selected device has changed will switch device using capture service
        /// </summary>
        async partial void OnSelectedDeviceChanged(MediaFrameSourceGroup value)
        {
            await _captureService.SwitchDeviceAsync(SelectedDevice);
        }
    }
}
