namespace Eugenie.Clients.Admin.Domain
{
    using System.Windows.Controls;

    public class MenuItem
    {
        public MenuItem(string name, UserControl content)
        {
            this.Name = name;
            this.Content = content;
        }

        public string Name { get; set; }

        public UserControl Content { get; set; }
    }
}