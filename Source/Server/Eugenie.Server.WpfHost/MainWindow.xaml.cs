namespace Eugenie.Server.WpfHost
{
    using System.Runtime.InteropServices;

    using Api;

    using Microsoft.Owin.Hosting;

    public partial class MainWindow
    {
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;

        public MainWindow()
        {
            this.InitializeComponent();
            var baseAddress = "http://192.168.1.2:9000/";
            WebApp.Start<Startup>(baseAddress);

            SetThreadExecutionState(ES_SYSTEM_REQUIRED | ES_CONTINUOUS);

            //SetThreadExecutionState(ES_CONTINUOUS);
        }

        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);
    }
}