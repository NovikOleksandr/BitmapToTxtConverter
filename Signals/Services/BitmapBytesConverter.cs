using Signals.Contracts;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Signals.Services
{
    public sealed class BitmapBytesConverter : IBitmapBytesConverter
    {
        /// <inheritdoc />
        public async Task WriteToTxtAsync(StorageFile file, byte[] bmpBytes)
        {
            var fs = await file.OpenStreamForWriteAsync();

            using (var writer = new StreamWriter(fs))
            {
                for (int i = 0; i < bmpBytes.Length; i += 3)
                {
                    // Write r, b and g values as binary to the txt file
                    for (int c = 0; c < 3; c++)
                    {
                        string colorValue = Convert.ToString(bmpBytes[i + c], 2).PadLeft(8, '0');
                        writer.WriteLine(colorValue);
                    }
                }
            }
        }

        /// <inheritdoc />
        public async Task<byte[]> GetBytesFromTxtAsync(StorageFile txtFile, Size bmpSize)
        {
            if (Path.GetExtension(txtFile.Path) != ".txt")
                throw new FileLoadException("Provided file path is not .txt format");

            var stream = await txtFile.OpenStreamForReadAsync();
            var pixels = new byte[bmpSize.Width * bmpSize.Height * 4]; // 4 for R, G, B and Alpha of pixels

            using (var reader = new StreamReader(stream))
            {
                int index = 0;

                while (!reader.EndOfStream)
                {
                    string rLine = await reader.ReadLineAsync();
                    string gLine = await reader.ReadLineAsync();
                    string bLine = await reader.ReadLineAsync();

                    if (rLine == null || gLine == null || bLine == null || rLine.Length < 8 || gLine.Length < 8 || bLine.Length < 8)
                        throw new BadImageFormatException("Cannot convert txt to a bitmap");

                    byte red = Convert.ToByte(rLine, 2);
                    byte green = Convert.ToByte(gLine, 2);
                    byte blue = Convert.ToByte(bLine, 2);

                    pixels[index++] = red;
                    pixels[index++] = green;
                    pixels[index++] = blue;
                }
            }

            return pixels;
        }
    }
}
