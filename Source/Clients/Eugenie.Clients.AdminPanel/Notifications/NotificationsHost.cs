namespace Eugenie.Clients.AdminPanel.Notifications
{
    using System.Windows;
    using System.Windows.Media;

    using MaterialDesignThemes.Wpf;

    public static class NotificationsHost
    {
        private const int Offset = 60;
        private static readonly Notifications notifications = new Notifications();

        static NotificationsHost()
        {
            notifications.Top = SystemParameters.WorkArea.Top + Offset;
            notifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - Offset - notifications.Width;
        }

        public static void Success(string title, string message)
        {
            notifications.AddNotification(new Notification(PackIconKind.CheckAll, Brushes.LimeGreen, title, message));
        }

        public static void Error(string title, string message)
        {
            notifications.AddNotification(new Notification(PackIconKind.CloseCircleOutline, Brushes.OrangeRed, title, message));
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