namespace Eugenie.Clients.Seller.Views
{
    using GalaSoft.MvvmLight;

    public partial class QuantityEditor
    {
        public QuantityEditor(ViewModelBase viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}