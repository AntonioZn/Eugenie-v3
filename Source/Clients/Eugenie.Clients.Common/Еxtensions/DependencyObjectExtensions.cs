namespace Eugenie.Clients.Common.Еxtensions
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public static class DependencyObjectExtensions
    {
        public static bool HasNoValidationErrors(this DependencyObject item)
        {
            return !Validation.GetHasError(item) &&
            LogicalTreeHelper.GetChildren(item)
            .OfType<DependencyObject>()
            .All(HasNoValidationErrors);
        }
    }
}