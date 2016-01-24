namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Linq;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;

    public class NewServerViewModel : ViewModelBase, IValidatableObject
    {
        private string name;
        private string username;
        private string password;
        private string addresses;

        public NewServerViewModel()
        {
           this.AddressesArray = new string[0]; 
        }

        public string Name
        {
            get
            {
                return this.name;

            }
            set
            {
                this.Set(() => this.Name, ref this.name, value.RemoveMultipleWhiteSpaces());
            }
        }

        public string Username
        {
            get
            {
                return this.username;

            }
            set
            {
                this.Set(() => this.Username, ref this.username, value.Trim());
            }
        }

        public string Password
        {
            get
            {
                return this.password;

            }
            set
            {
                this.Set(() => this.Password, ref this.password, value.Trim());
            }
        }
        
        public string Addresses
        {
            get
            {
                return this.addresses;
            }

            set
            {
                this.Set(() => this.Addresses, ref this.addresses, value.Trim());
                this.AddressesArray = this.Addresses.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public string[] AddressesArray { get; private set; }

        public void Reset()
        {
            this.Name = string.Empty;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.Addresses = string.Empty;
            this.AddressesArray = new string[0];
        }

        public ServerInformation GetServer()
        {
            return new ServerInformation
            {
                Name = this.Name,
                Username = this.Username,
                Password = this.Password,
                Addresses = this.AddressesArray
            };
        }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(this.Name):
                        return Validator.ValidateServerName(this.Name);
                    case nameof(this.Username):
                        return Validator.ValidateUsername(this.Username);
                    case nameof(this.Password):
                        return Validator.ValidatePassword(this.Password);
                    case nameof(this.Addresses):
                        return Validator.ValidateAddresses(this.AddressesArray);
                    default:
                        return null;
                }
            }
        }

        public string Error { get; }

        public bool HasNoValidationErrors()
        {
            return this[nameof(this.Name)] == null
                && this[nameof(this.Username)] == null
                && this[nameof(this.Password)] == null
                && this[nameof(this.Addresses)] == null;
        }
    }
}