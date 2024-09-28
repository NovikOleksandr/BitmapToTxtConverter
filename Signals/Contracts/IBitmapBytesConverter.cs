using System.Drawing;
using System.Threading.Tasks;
using Windows.Storage;

namespace Signals.Contracts
{
    public interface IBitmapBytesConverter
    {
        /// <summary>
        /// Writes byte array to a provided storage file
        /// </summary>
        public Task WriteToTxtAsync(StorageFile file, byte[] bmpBytes);

        /// <summary>
        /// Reads bitmap with provided size from .txt file
        /// </summary>
        public Task<byte[]> GetBytesFromTxtAsync(StorageFile txtFile, Size bmpSize);
    }
}
