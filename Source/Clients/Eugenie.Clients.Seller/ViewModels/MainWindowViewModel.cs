namespace Eugenie.Clients.Seller.ViewModels
{
    using GalaSoft.MvvmLight;

    using Views;

    public class MainWindowViewModel : ViewModelBase
    {
        private object content;

        public MainWindowViewModel()
        {
            this.Content = new Login();
        }
        public object Content
        {
            get
            {
                return this.content;
            }

            set
            {
                this.Set(() => this.Content, ref this.content, value);
            }
        }
    }
}