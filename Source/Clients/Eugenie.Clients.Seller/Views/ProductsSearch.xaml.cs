namespace Eugenie.Clients.Seller.Views
{
    using Sv.Wpf.Core.Mvvm;

    public partial class ProductsSearch
    {
        public ProductsSearch(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}