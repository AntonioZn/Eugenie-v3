namespace Eugenie.Clients.AdminPanel.Notifications
{
    using System.Windows.Media;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    public class Notification : ViewModelBase
    {
        private int id;
        private string title;
        private string message;

        public Notification(PackIconKind icon, SolidColorBrush color, string title, string message)
        {
            this.Icon = icon;
            this.Color = color;
            this.Title = title;
            this.Message = message;
        }
        
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.Set(() => this.Id, ref this.id, value);
            }
        }

        public PackIconKind Icon { get; set; }

        public SolidColorBrush Color { get; set; }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.Set(() => this.Title, ref this.title, value);
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.Set(() => this.Message, ref this.message, value);
            }
        }
    }
}
