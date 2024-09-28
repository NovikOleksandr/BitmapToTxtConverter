using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Signals.Contracts
{
    /// <summary>
    /// Contract for service that converts bitmap to txt file and txt file to bitmap
    /// </summary>
    public interface ITextBitmapConverter
    {
        /// <summary>
        /// Convert stream that stores bitmap to text and saves to txt file
        /// </summary>
        /// <param name="imageStream"> Stream with bitmap </param>
        /// <param name="file"> File to save image data into </param>
        public Task ToTextAsync(IRandomAccessStream imageStream, StorageFile file);

        /// <summary>
        /// Extracts bitmap from saved txt file
        /// </summary>
        /// <param name="txtFile"> Storage file that were selected </param>
        /// <returns> Stream that stores bitmap </returns>
        public Task<IRandomAccessStream> FromTextAsync(StorageFile txtFile);
    }
}
