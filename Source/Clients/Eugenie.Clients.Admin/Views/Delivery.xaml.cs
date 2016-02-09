namespace Eugenie.Clients.Admin.Views
{
    using System.Windows;

    public partial class Delivery
    {
        public Delivery()
        {
            this.InitializeComponent();
        }

        public Delivery(bool nothing)
        {
            this.InitializeComponent();
            this.mainCard.Margin = new Thickness(0);
        }
    }
}