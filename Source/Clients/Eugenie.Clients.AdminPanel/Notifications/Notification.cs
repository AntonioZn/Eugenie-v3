namespace Eugenie.Clients.AdminPanel.Notifications
{
    using GalaSoft.MvvmLight;

    public class Notification : ViewModelBase
    {
        private int id;
        private string title;
        private string message;

        public Notification(string title, string message)
        {
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
