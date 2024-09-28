using Signals.Contracts;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Signals.Services
{
    public sealed class TextBitmapConverter : ITextBitmapConverter
    {
        private const BitmapPixelFormat pixelFormat = BitmapPixelFormat.Bgra8;
        private const BitmapAlphaMode alphaMode = BitmapAlphaMode.Ignore;
        private const int DPI = 96;

        private Size _bmpSize;

        private readonly IBitmapBytesConverter _txtConverter;

        public TextBitmapConverter(IBitmapBytesConverter txtConverter)
        {
            _txtConverter = txtConverter;
        }

        /// <inheritdoc />
        public async Task ToTextAsync(IRandomAccessStream imageStream, StorageFile file)
        {
            var decoder = await BitmapDecoder.CreateAsync(imageStream);

            _bmpSize = new Size((int)decoder.PixelWidth, (int)decoder.PixelHeight);

            var pixelData = await decoder.GetPixelDataAsync(
                pixelFormat,
                alphaMode,
                new BitmapTransform(),
                ExifOrientationMode.IgnoreExifOrientation,
                ColorManagementMode.DoNotColorManage
            );

            byte[] pixels = pixelData.DetachPixelData();

            await _txtConverter.WriteToTxtAsync(file, pixels);
        }

        /// <inheritdoc />
        public async Task<IRandomAccessStream> FromTextAsync(StorageFile txtFile)
        {
            var pixels = await _txtConverter.GetBytesFromTxtAsync(txtFile, _bmpSize);
            var stream = new InMemoryRandomAccessStream();

            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);

            encoder.SetPixelData(
                pixelFormat,
                alphaMode,
                (uint)_bmpSize.Width,
                (uint)_bmpSize.Height,
                DPI,
                DPI,
                pixels
            );

            await encoder.FlushAsync();
            stream.Seek(0);

            return stream;
        }
    }
}
