namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.CommandWpf;

    using MaterialDesignColors;

    using MaterialDesignThemes.Wpf;

    using Properties;

    public class PaletteSelectorViewModel
    {
        public ICommand ToggleBaseCommand => new RelayCommand<bool>(this.ApplyBase);

        public ICommand ApplyPrimaryCommand => new RelayCommand<Swatch>(this.ApplyPrimary);

        public ICommand ApplyAccentCommand => new RelayCommand<Swatch>(this.ApplyAccent);

        public IEnumerable<Swatch> Swatches => new SwatchesProvider().Swatches;

        private void ApplyBase(bool isDark)
        {
            new PaletteHelper().SetLightDark(isDark);
            Settings.Default.IsDark = isDark;
            Settings.Default.Save();
        }

        private void ApplyPrimary(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
            Settings.Default.PrimaryColorUri = $"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.{swatch.Name}.xaml";
            Settings.Default.Save();
        }

        private void ApplyAccent(Swatch swatch)
        {
            new PaletteHelper().ReplaceAccentColor(swatch);
            Settings.Default.AccentColorUri = $"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.{swatch.Name}.xaml";
            Settings.Default.Save();
        }
    }
}