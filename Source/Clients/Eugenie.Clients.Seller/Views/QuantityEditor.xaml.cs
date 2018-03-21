namespace Eugenie.Clients.Seller.Views
{
    using Sv.Wpf.Core.Mvvm;

    public partial class QuantityEditor
    {
        public QuantityEditor(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}