namespace Eugenie.Clients.Seller.ViewModels
{
    using System;
    using System.Globalization;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Models;

    using MaterialDesignThemes.Wpf;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Mvvm;

    public class QuantityEditorViewModel : ViewModelBase, IBarcodeHandler
    {
        private static readonly string delimiter = Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

        private readonly decimal maximumQuantity;
        private readonly MeasureType measure;
        private string quantity;

        public QuantityEditorViewModel(decimal startingQuantity, decimal maximumQuantity, MeasureType measure, string name)
        {
            this.maximumQuantity = maximumQuantity;
            this.measure = measure;
            this.Name = name;
            this.Quantity = startingQuantity.ToString();
        }

        public ICommand ConfirmCommand => new RelayCommand(this.HandleAdd);

        public string Name { get; }

        public void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(this.ValidateQuantity(), null);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }

        public string Quantity
        {
            get => this.quantity;
            set => this.Set(ref this.quantity, this.RestrictQuantity(value?.Replace(",", delimiter).Replace(".", delimiter).Replace($"{delimiter}{delimiter}", delimiter)));
        }

        private void HandleAdd()
        {
            if (this.ValidateQuantity())
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }

        private bool ValidateQuantity()
        {
            if (decimal.TryParse(this.Quantity, out var result))
            {
                if (result >= this.GetMinimumQuantity())
                {
                    return true;
                }

                NotificationsHost.Error("Notifications", "Невалидно количество", $"Минималната позволена стойност е {this.GetMinimumQuantity()}.");
            }
            else
            {
                NotificationsHost.Error("Notifications", "Невалидно количество", "Въведената стойност е невалидна.");
            }

            return false;
        }

        private string RestrictQuantity(string userInput)
        {
            if (this.measure == MeasureType.бр)
            {
                userInput = userInput.Replace(delimiter, string.Empty);

                if (decimal.TryParse(userInput, out var result))
                {
                    if (result > this.maximumQuantity)
                    {
                        userInput = this.maximumQuantity.ToString();
                        NotificationsHost.Error("Notifications", "Невалидно количество", $"Максималното позволено количество е {this.maximumQuantity}.");
                    }
                    else if (result < this.GetMinimumQuantity())
                    {
                        userInput = this.GetMinimumQuantity().ToString();
                        NotificationsHost.Error("Notifications", "Невалидно количество", $"Минималното позволено количество е {this.GetMinimumQuantity()}.");
                    }
                }
            }

            return userInput;
        }

        private decimal GetMinimumQuantity()
        {
            return this.measure == MeasureType.бр ? 1 : 0.01M;
        }
    }
}