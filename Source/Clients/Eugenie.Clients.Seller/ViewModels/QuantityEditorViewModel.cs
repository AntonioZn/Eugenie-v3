namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Windows.Input;

    using Common.Models;
    using Common.Notifications;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using MaterialDesignThemes.Wpf;

    public class QuantityEditorViewModel : ViewModelBase
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
            decimal result;
            if (decimal.TryParse(this.Quantity, out result))
            {
                if (result >= this.GetMinimumQuantity())
                {
                    DialogHost.CloseDialogCommand.Execute(true, null);
                }
                else
                {
                    NotificationsHost.Error("Невалидно количество", this.measure == MeasureType.бр ? "Минималната позволена стойност е 1." : "Минималната позволена стойност е 0,01.");
                }
            }
            else
            {
                NotificationsHost.Error("Невалидно количество", "Въведената стойност е невалидна.");
            }
        }

        private string RestrictQuantity(string userInput)
        {
            userInput = userInput.Replace("-", string.Empty);

            if (this.measure == MeasureType.бр)
            {
                userInput = userInput.Replace(",", string.Empty);

                decimal result;
                if(decimal.TryParse(userInput, out result))
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