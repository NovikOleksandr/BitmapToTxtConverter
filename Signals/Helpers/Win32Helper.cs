using Microsoft.UI.Xaml;

namespace Signals.Helpers
{
    public static class Win32Helper
    {
        /// <summary>
        /// Provides main window handle for provided target
        /// </summary>
        /// <param name="target"> Target that needs main window handle </param>
        /// <param name="window"> Window that is providing handle </param>
        public static void ProvideWindowHandle(object target, Window window)
        {
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(target, handle);
        }
    }
}
