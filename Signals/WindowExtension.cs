using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace Signals
{
    public class WindowExtension : Window
    {
        public async Task ShowMessageAsync(string message, string title)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = Content.XamlRoot,
                Content = message,
                Title = title,
                CloseButtonText = "Ok"
            };

            await dialog.ShowAsync();
        }
    }
}
