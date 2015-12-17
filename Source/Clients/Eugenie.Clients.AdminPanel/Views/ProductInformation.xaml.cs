namespace Eugenie.Clients.AdminPanel.Views
{
    using System.Windows.Controls;

    using ViewModels;

    /// <summary>
    ///     Interaction logic for ProductInformation.xaml
    /// </summary>
    public partial class ProductInformation : UserControl
    {
        public ProductInformation(ProductInformationViewModel model)
        {
            this.InitializeComponent();
            this.DataContext = model;
        }
    }
}