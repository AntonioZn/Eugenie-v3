﻿namespace Eugenie.Clients.Admin.Views
{
    using GalaSoft.MvvmLight;

    public partial class ProductInformation
    {
        public ProductInformation(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}