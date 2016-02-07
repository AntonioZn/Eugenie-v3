namespace Eugenie.Clients.Common.Behaviors
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Text;
    using System.Windows;

    using Contracts;

    public class HandleBarcodeScannerWindowBehavior : Behavior<Window>
    {
        private readonly StringBuilder barcodeReader;
        private readonly KeyConverter keyConverter;
        private IBarcodeHandler handler;
        private bool isScanning;

        public HandleBarcodeScannerWindowBehavior()
        {
            this.barcodeReader = new StringBuilder();
            this.keyConverter = new KeyConverter();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewKeyDown += this.OnPreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewKeyDown -= this.OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F20)
            {
                var contentControl = this.AssociatedObject.FindName("MainFrame") as ContentControl;
                var userControl = contentControl?.Content as UserControl;
                this.handler = userControl?.DataContext as IBarcodeHandler;

                if (this.handler != null)
                {
                    this.isScanning = true;
                    e.Handled = true;
                }
            }
            else if (this.isScanning && e.Key == Key.F22)
            {
                if (this.barcodeReader.Length != 0)
                {
                    this.handler.HandleBarcode(this.barcodeReader.ToString());
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
    }
}