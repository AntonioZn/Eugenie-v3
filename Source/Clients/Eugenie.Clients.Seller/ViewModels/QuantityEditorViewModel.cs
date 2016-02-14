namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Models;
    using Common.Notifications;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using MaterialDesignThemes.Wpf;

    public class QuantityEditorViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly decimal maximumQuantity;
        private readonly MeasureType measure;
        private string quantity;

        public QuantityEditorViewModel(decimal startingQuantity, decimal maximumQuantity, MeasureType measure, string name)
        {
            this.maximumQuantity = maximumQuantity;
            this.measure = measure;
            this.Name = name;
            this.Quantity = startingQuantity.ToString();

            this.Add = new RelayCommand(this.HandleAdd);
        }

        public string Name { get; set; }

        public ICommand Add { get; }

        public void HandleBarcode(string barcode)
        {
            if (this.ValidateQuantity())
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
            else
            {
                DialogHost.CloseDialogCommand.Execute(false, null);
            }

            ViewModelLocator.container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }

        public string Quantity
        {
            get
            {
                return this.quantity;
            }

            set
            {
                this.Set(() => this.Quantity, ref this.quantity, this.RestrictQuantity(value));
            }
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
            decimal result;
            if (decimal.TryParse(this.Quantity, out result))
            {
                if (result >= this.GetMinimumQuantity())
                {
                    return true;
                }

                NotificationsHost.Error("Невалидно количество", $"Минималната позволена стойност е {this.GetMinimumQuantity()}.");
            }
            else
            {
                NotificationsHost.Error("Невалидно количество", "Въведената стойност е невалидна.");
            }

            return false;
        }

        private string RestrictQuantity(string userInput)
        {
            userInput = userInput.Replace("-", string.Empty);

            if (this.measure == MeasureType.бр)
            {
                userInput = userInput.Replace(",", string.Empty);

                decimal result;
                if (decimal.TryParse(userInput, out result))
                {
                    if (result > this.maximumQuantity)
                    {
                        userInput = this.maximumQuantity.ToString();
                        NotificationsHost.Error("Невалидно количество", $"Максималното позволено количество е {this.maximumQuantity}.");
                    }
                    else if (result < this.GetMinimumQuantity())
                    {
                        userInput = this.GetMinimumQuantity().ToString();
                        NotificationsHost.Error("Невалидно количество", $"Минималното позволено количество е {this.GetMinimumQuantity()}.");
                    }
                }
            }
            else
            {
                userInput = userInput.Replace(",,", ",");
            }

            return userInput;
        }

        private decimal GetMinimumQuantity()
        {
            return this.measure == MeasureType.бр ? 1 : 0.01M;
        }
    }
}