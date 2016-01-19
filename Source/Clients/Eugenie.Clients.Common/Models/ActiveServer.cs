namespace Eugenie.Clients.Common.Models
{
    using GalaSoft.MvvmLight;

    public class ActiveServer : ViewModelBase
    {
        private bool isSelected;

        public ActiveServer(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                this.Set(() => this.IsSelected, ref this.isSelected, value);
            }
        }
    }
}