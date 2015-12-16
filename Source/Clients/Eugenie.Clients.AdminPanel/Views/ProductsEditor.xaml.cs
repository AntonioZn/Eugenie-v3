using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Eugenie.Clients.AdminPanel.Views
{
    using Data.Models;

    /// <summary>
    /// Interaction logic for ProductsPresenter.xaml
    /// </summary>
    public partial class ProductsEditor : UserControl
    {
        public ProductsEditor()
        {
            InitializeComponent();

            var products = new List<Product>();
            for (int i = 0; i < 3000; i++)
            {
                products.Add(new Product {Id = i + 1, Name = "Хляб " + i, SellingPrice = i, BuyingPrice = i * 2, Quantity = i, Measure = MeasureType.бр});
            }

            this.DataGrid.ItemsSource = products;
        }
    }
}