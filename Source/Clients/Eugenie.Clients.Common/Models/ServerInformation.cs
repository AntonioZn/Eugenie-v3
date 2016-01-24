namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;
    using System.Net.Http;

    using Contracts;

    using GalaSoft.MvvmLight;

    using Helpers;

    using Newtonsoft.Json;

    public class ServerInformation : ViewModelBase, IValidatableObject
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Addresses { get; set; }

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
                && this[nameof(this.Password)] == null;
        }
    }
}