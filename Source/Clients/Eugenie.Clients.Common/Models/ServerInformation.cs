namespace Eugenie.Clients.Common.Models
{
    using System;
    using System.Net.Http;

    using Contracts;

    using GalaSoft.MvvmLight;

    using Helpers;

    using Newtonsoft.Json;

    using Еxtensions;

    public class ServerInformation : ViewModelBase, IValidatableObject
    {
        private string name;
        private string address = "http://";
        private string username;
        private string password;

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

        public string Address
        {
            get
            {
                return this.address;

            }
            set
            {
                this.Set(() => this.Address, ref this.address, value.Trim());
            }
        }

        [JsonIgnore]
        public string AuthToken { get; set; }

        [JsonIgnore]
        public TimeSpan Ping { get; set; }

        [JsonIgnore]
        public HttpClient Client { get; set; }

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
                    case nameof(this.Address):
                        return Validator.ValidateAddress(this.Address);
                    case nameof(this.Password):
                        return Validator.ValidatePassword(this.Password);
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
                && this[nameof(this.Address)] == null
                && this[nameof(this.Password)] == null;
        }
    }
}