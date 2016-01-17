namespace Eugenie.Clients.AdminPanel.Notifications
{
    using System.Windows;

    public static class NotificationsHost
    {
        private const int Offset = 60;
        private static readonly Notifications notifications = new Notifications();

        static NotificationsHost()
        {
            notifications.Top = SystemParameters.WorkArea.Top + Offset;
            notifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - Offset - notifications.Width;
        }

        public static void Add(string title, string message)
        {
            notifications.AddNotification(new Notification(title, message));
        }

        public static void Close()
        {
            notifications.Close();
        }

        public static void SetOwner(Window owner)
        {
            notifications.Owner = owner;
        }
    }
}