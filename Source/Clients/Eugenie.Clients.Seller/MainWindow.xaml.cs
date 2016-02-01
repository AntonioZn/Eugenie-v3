namespace Eugenie.Clients.Seller
{
    using System.Windows;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            #if DEBUG
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.Topmost = false;
            }
            #endif
        }
    }
}