namespace Eugenie.Clients.AdminPanel.Notifications
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public partial class Notifications
    {
        private const byte MaxNotifications = 6;
        private readonly List<Notification> buffer = new List<Notification>();
        private readonly ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();

        public Notifications()
        {
            this.InitializeComponent();
            this.notificationsControl.DataContext = this.notifications;
        }

        public void AddNotification(Notification notification)
        {
            if (this.notifications.Count + 1 > MaxNotifications)
            {
                this.buffer.Add(notification);
            }
            else
            {
                this.notifications.Add(notification);
            }
            
            if (this.notifications.Count > 0 && !this.IsActive)
            {
                this.Show();
            }
        }

        public void RemoveNotification(Notification notification)
        {
            if (this.notifications.Contains(notification))
            {
                this.notifications.Remove(notification);
            }

            if (this.buffer.Count > 0)
            {
                this.notifications.Add(this.buffer[0]);
                this.buffer.RemoveAt(0);
            }

            if (this.notifications.Count < 1)
            {
                this.Hide();
            }
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height != 0)
            {
                return;
            }

            var element = sender as Grid;
            this.RemoveNotification(this.notifications.First(n => n.Id == int.Parse(element.Tag.ToString())));
        }
    }
}
