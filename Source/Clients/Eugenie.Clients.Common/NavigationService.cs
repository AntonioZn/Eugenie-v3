namespace Eugenie.Clients.Common
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    using MaterialDesignThemes.Wpf;

    using Properties;

    using Sv.Wpf.Core.Mvvm;

    public class NavigationService : INotifyPropertyChanged, INavigationService
    {
        private UserControl content;

        public UserControl Content
        {
            get => this.content;
            private set
            {
                this.content = value;
                this.OnPropertyChanged();
            }
        }

        public async Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase
        {
            var view = this.CreateView(typeof(TViewModel));
            if (parameter != null)
            {
                await ((ViewModelBase)view.DataContext).InitializeAsync(parameter);
            }

            this.Content = view;
        }

        public async Task<TResult> ShowDialogAsync<TViewModel, TResult>(object parameter = null) where TViewModel : ViewModelBase
        {
            var view = this.CreateView(typeof(TViewModel));
            if (parameter != null)
            {
                await ((ViewModelBase)view.DataContext).InitializeAsync(parameter);
            }

            var result = await DialogHost.Show(view, "RootDialog");
            return (TResult) result;
        }

        public async Task ShowDialogAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase
        {
            var view = this.CreateView(typeof(TViewModel));
            if (parameter != null)
            {
                await ((ViewModelBase)view.DataContext).InitializeAsync(parameter);
            }

            await DialogHost.Show(view, "RootDialog");
        }

        private Type GetViewTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModels", "Views").Replace("ViewModel", "");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private UserControl CreateView(Type viewModelType)
        {
            var viewType = this.GetViewTypeForViewModel(viewModelType);
            if (viewType == null)
            {
                throw new Exception($"Cannot locate view type for {viewModelType}");
            }

            var view = Activator.CreateInstance(viewType) as UserControl;
            return view;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
