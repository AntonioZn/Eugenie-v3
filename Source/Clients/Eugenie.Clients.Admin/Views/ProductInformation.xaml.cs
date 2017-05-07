namespace Eugenie.Clients.Admin.Views
{
    using ViewModels;

    public partial class ProductInformation
    {
        public ProductInformation(ProductInformationViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}