using Microsoft.UI.Xaml.Controls;
using Signals.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.Core;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;

namespace Signals.Services
{
    public sealed class CameraCaptureService : ICameraCaptureService
    {
        private bool isMirrored = false;

        private MediaPlayerElement _captureElement;
        private MediaFrameSourceGroup _mediaFrameSourceGroup;
        private MediaCapture _mediaCapture;

        /// <inheritdoc />
        public void MirrorPreview()
        {
            if (_captureElement is not null)
            {
                isMirrored = !isMirrored;
                _captureElement.MediaPlayer.PlaybackSession.IsMirroring = isMirrored;
            }
        }

        /// <inheritdoc />
        public async Task InitializeAsync(MediaPlayerElement captureElement, MediaFrameSourceGroup device)
        {
            _captureElement = captureElement;
            _mediaFrameSourceGroup = device;
            _mediaCapture = new MediaCapture();

            var mediaCaptureInitializationSettings = new MediaCaptureInitializationSettings()
            {
                SourceGroup = _mediaFrameSourceGroup,
                SharingMode = MediaCaptureSharingMode.SharedReadOnly,
                StreamingCaptureMode = StreamingCaptureMode.Video,
                MemoryPreference = MediaCaptureMemoryPreference.Cpu
            };

            await _mediaCapture.InitializeAsync(mediaCaptureInitializationSettings);

            // Set the MediaPlayerElement's Source property to the MediaSource for the mediaCapture.
            var frameSource = _mediaCapture.FrameSources[_mediaFrameSourceGroup.SourceInfos[0].Id];
            _captureElement.Source = MediaSource.CreateFromMediaFrameSource(frameSource);
        }

        /// <inheritdoc />
        public async Task SwitchDeviceAsync(MediaFrameSourceGroup device)
        {
            await InitializeAsync(_captureElement, device);
        }

        /// <inheritdoc />
        public async Task<IRandomAccessStream> CreateBitmapAsync()
        {
            // Capture a photo to a stream
            var imgFormat = ImageEncodingProperties.CreateBmp();
            var stream = new InMemoryRandomAccessStream();

            // Put photo to in memory stream
            await _mediaCapture.CapturePhotoToStreamAsync(imgFormat, stream);
            stream.Seek(0);

            return stream;
        }
    }
}
