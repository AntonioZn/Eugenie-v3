namespace Eugenie.Clients.Common.Behaviors
{
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Contracts;

    using MaterialDesignThemes.Wpf;

    public class BarcodeScannerHandler
    {
        private readonly Window window;
        private readonly StringBuilder barcodeReader;
        private readonly KeyConverter keyConverter;
        private bool isScanning;

        public BarcodeScannerHandler(Window window)
        {
            this.window = window;
            this.barcodeReader = new StringBuilder();
            this.keyConverter = new KeyConverter();

            window.PreviewKeyDown += this.OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var startKey = Key.F20;
            var endKey = Key.F22;
#if DEBUG
            startKey = Key.F11;
            endKey = Key.F12;
#endif

            var handler = this.GetHandler();

            if (e.Key == startKey)
            {
                if (handler != null)
                {
                    this.isScanning = true;
                    e.Handled = true;
                }
            }
            else if (this.isScanning && e.Key == endKey)
            {
                if (this.barcodeReader.Length != 0)
                {
                    handler.HandleBarcode(this.barcodeReader.ToString());
                    this.barcodeReader.Clear();
                }

                this.isScanning = false;
                e.Handled = true;
            }
            else if (this.isScanning)
            {
                e.Handled = true;
                var xChar = this.keyConverter.ConvertToString(e.Key);
                this.barcodeReader.Append(xChar);
            }
        }

        private IBarcodeHandler GetHandler()
        {
            var dialogHost = this.window.FindName("dialogHost") as DialogHost;
            if (dialogHost.IsOpen)
            {
                var dialogContent = dialogHost.DialogContent as UserControl;
                return dialogContent?.DataContext as IBarcodeHandler;
            }

            return this.window.DataContext as IBarcodeHandler;
        }
    }
}