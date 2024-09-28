using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Media.Capture.Frames;
using Windows.Storage.Streams;

namespace Signals.Contracts
{
    /// <summary>
    /// Contract for service that works with camera and captures recording from camera devices
    /// </summary>
    public interface ICameraCaptureService
    {
        /// <summary>
        /// Mirrors captured preview
        /// </summary>
        public void MirrorPreview();
        /// <summary>
        /// Initializes new capture to provided capture element and with provided device
        /// </summary>
        public Task InitializeAsync(MediaPlayerElement captureElement, MediaFrameSourceGroup device);

        /// <summary>
        /// Creates bitmap from a frame of current capture
        /// </summary>
        /// <returns> Stream that stores data for bitmap </returns>
        public Task<IRandomAccessStream> CreateBitmapAsync();

        /// <summary>
        /// Switches device for current capture
        /// </summary>
        /// <param name="device"> New device for a capture </param>
        public Task SwitchDeviceAsync(MediaFrameSourceGroup device);

    }
}
