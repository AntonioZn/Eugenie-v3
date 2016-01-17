namespace Eugenie.Clients.AdminPanel
{
    using System;
    using System.Windows;

    using Properties;

    public partial class App
    {
        public App()
        {
            this.InitializeComponent();

            var isDark = new ResourceDictionary();
            var accent = new ResourceDictionary();
            var primary = new ResourceDictionary();

            if (Settings.Default.IsDark)
            {
                isDark.Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml");
            }
            else
            {
                isDark.Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml");
            }

            if (string.IsNullOrEmpty(Settings.Default.AccentColorUri))
            {
                Settings.Default.AccentColorUri = "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml";
                Settings.Default.Save();
            }

            if (string.IsNullOrEmpty(Settings.Default.PrimaryColorUri))
            {
                Settings.Default.PrimaryColorUri = "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml";
                Settings.Default.Save();
            }

            accent.Source = new Uri(Settings.Default.AccentColorUri);
            primary.Source = new Uri(Settings.Default.PrimaryColorUri);
            this.Resources.MergedDictionaries.Add(isDark);
            this.Resources.MergedDictionaries.Add(accent);
            this.Resources.MergedDictionaries.Add(primary);
        }
    }
}