namespace Eugenie.Clients.Seller.Views
{
    public partial class Confirm
    {
        public Confirm(string question)
        {
            this.InitializeComponent();
            this.tbQuestion.Text = question;
        }
    }
}