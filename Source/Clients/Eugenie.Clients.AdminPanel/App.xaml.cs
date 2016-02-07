namespace Eugenie.Clients.AdminPanel
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Threading;

    using Common.Helpers;

    using Properties;

    public partial class App
    {
        public App()
        {
            this.InitializeComponent();
            this.DispatcherUnhandledException += this.OnUnhandledException;

            var isDark = new ResourceDictionary();
            var accent = new ResourceDictionary();
            var primary = new ResourceDictionary();

            var isDarkSource = Settings.Default.IsDark ? "Dark" : "Light";
            isDark.Source = new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{isDarkSource}.xaml");

            accent.Source = new Uri(Settings.Default.AccentColorUri);
            primary.Source = new Uri(Settings.Default.PrimaryColorUri);
            this.Resources.MergedDictionaries.Add(isDark);
            this.Resources.MergedDictionaries.Add(accent);
            this.Resources.MergedDictionaries.Add(primary);
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogToFile(e.Exception, Settings.Default.LogPath);
            e.Handled = true;
        }
    }
}