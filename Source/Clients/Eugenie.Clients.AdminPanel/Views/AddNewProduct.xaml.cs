namespace Eugenie.Clients.AdminPanel.Views
{
    using ViewModels;

    public partial class AddNewProduct
    {
        public AddNewProduct()
        {
            InitializeComponent();
            var resource = this.TryFindResource("ViewModelLocator");
            var viewModelLocator = resource as ViewModelLocator;
            var viewModel = viewModelLocator.AddNewProductViewModel;
            this.DataContext = viewModel;
        }

        public AddNewProduct(AddNewProductViewModel model)
        {
            this.DataContext = model;
        }
    }
}