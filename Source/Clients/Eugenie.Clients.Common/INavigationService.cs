using System.Threading.Tasks;
using System.Windows.Controls;
using Sv.Wpf.Core.Mvvm;

namespace Eugenie.Clients.Common
{
    public interface INavigationService
    {
        UserControl Content { get; }

        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase;
        Task<TResult> ShowDialogAsync<TViewModel, TResult>(object parameter = null) where TViewModel : ViewModelBase;
        Task ShowDialogAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase;
    }
}