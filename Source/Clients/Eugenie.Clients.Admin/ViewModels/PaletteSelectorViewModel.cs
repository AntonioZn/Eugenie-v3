namespace Eugenie.Clients.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.CommandWpf;

    using MaterialDesignColors;

    using MaterialDesignThemes.Wpf;

    using Properties;

    public class PaletteSelectorViewModel
    {
        private bool toggleBase = Settings.Default.IsDark;

        public bool ToggleBase
        {
            get
            {
                return this.toggleBase;
            }

            set
            {
                this.toggleBase = value;
                new PaletteHelper().SetLightDark(value);
                Settings.Default.IsDark = value;
                Settings.Default.Save();
            }
        }

        public ICommand ApplyPrimaryCommand => new RelayCommand<Swatch>(this.ApplyPrimary);

        public ICommand ApplyAccentCommand => new RelayCommand<Swatch>(this.ApplyAccent);

        public IEnumerable<Swatch> Swatches => new SwatchesProvider().Swatches;

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