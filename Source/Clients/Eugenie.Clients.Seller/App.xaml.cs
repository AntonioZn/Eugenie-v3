namespace Eugenie.Clients.Seller
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Principal;
    using System.Windows;
    using System.Windows.Threading;

    using Common.Helpers;

    using Properties;

    public partial class App
    {
        public App()
        {
            if (!this.IsRunAsAdministrator())
            {
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);
                
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";
                
                try
                {
                    Process.Start(processInfo);
                    Application.Current.Shutdown();
                }
                catch
                {
                }
            }

            this.InitializeComponent();
            this.DispatcherUnhandledException += this.OnUnhandledException;

            Settings.Default.Upgrade();
            Settings.Default.Save();
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogToFile(e.Exception, Settings.Default.LogPath);
            e.Handled = true;
        }

        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}