namespace Eugenie.Server.WpfHost
{
    using Api;

    using Microsoft.Owin.Hosting;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var baseAddress = "http://localhost:9000/";
            WebApp.Start<Startup>(url: baseAddress);
        }
    }
}
