namespace Eugenie.Clients.Common.Models
{
    using System;
    using System.ComponentModel;
    using System.Net.Http;

    using GalaSoft.MvvmLight;

    using Helpers;

    using Newtonsoft.Json;

    public class ServerInformation : ViewModelBase, IDataErrorInfo
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
                this.Set(() => this.Name, ref this.name, value.TrimStart());
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

        [JsonIgnore]
        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "Name":
                        return Validator.ValidateServerName(this.Name);
                    case "Username":
                        return Validator.ValidateUsername(this.Username);
                    case "Address":
                        return Validator.ValidateAddress(this.Address);
                    default:
                        return null;
                }
            }
        }

        [JsonIgnore]
        public string Error { get; }
    }
}