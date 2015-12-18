namespace Eugenie.Clients.AdminPanel
{
    using System.Windows.Input;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.MenuToggleButton.IsChecked = false;
        }
    }
}