namespace Eugenie.Clients.Seller.Views
{
    using GalaSoft.MvvmLight;

    public partial class ProductsSearch
    {
        public ProductsSearch(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}