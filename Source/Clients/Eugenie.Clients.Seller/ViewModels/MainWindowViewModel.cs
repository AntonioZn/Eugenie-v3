namespace Eugenie.Clients.Seller.ViewModels
{
    using GalaSoft.MvvmLight;

    public class MainWindowViewModel : ViewModelBase
    {
        private object content;

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