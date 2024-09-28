using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Signals.Models
{
    public sealed partial class CapturedBitmap : ObservableObject
    {
        public BitmapImage Bitmap { get; }

        [ObservableProperty]
        private bool isActive;

        public CapturedBitmap()
        {
            Bitmap = new BitmapImage();
            isActive = false;
        }

        public async Task SetSourceAsync(IRandomAccessStream source)
        {
            await Bitmap.SetSourceAsync(source);
            IsActive = true;
        }
    }
}
