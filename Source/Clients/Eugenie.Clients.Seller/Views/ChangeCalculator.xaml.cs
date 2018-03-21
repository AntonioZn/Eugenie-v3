namespace Eugenie.Clients.Seller.Views
{
    using Sv.Wpf.Core.Mvvm;

    public partial class ChangeCalulator
    {
        public ChangeCalulator(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}