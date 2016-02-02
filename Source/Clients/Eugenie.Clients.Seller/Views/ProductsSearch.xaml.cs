namespace Eugenie.Clients.Seller.Views
{
    using GalaSoft.MvvmLight;

    public partial class ProductsSearch
    {
        public ProductsSearch(ViewModelBase viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}