namespace Eugenie.Clients.AdminPanel.Views
{
    using ViewModels;

    public partial class ProductInformation
    {
        public ProductInformation(ProductInformationViewModel model)
        {
            this.InitializeComponent();
            this.DataContext = model;
        }
    }
}