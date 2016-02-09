namespace Eugenie.Clients.Common.Notifications
{
    using System.Windows.Media;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    public class Notification : ViewModelBase
    {
        public Notification(PackIconKind icon, SolidColorBrush color, string title, string message)
        {
            this.Icon = icon;
            this.Color = color;
            this.Title = title;
            this.Message = message;
        }

        public int Id { get; set; }

        public PackIconKind Icon { get; set; }

        public SolidColorBrush Color { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }
    }
}