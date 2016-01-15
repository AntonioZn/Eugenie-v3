namespace Eugenie.Clients.AdminPanel.Views
{
    using System.Windows;

    public partial class Delivery
    {
        public Delivery()
        {
            this.InitializeComponent();
            this.mainCard.Margin = new Thickness(0, 50, 0, 0);
        }

        public Delivery(bool nothing)
        {
            this.InitializeComponent();
            this.mainCard.Margin = new Thickness(0, 0, 0, 0);
        }
    }
}