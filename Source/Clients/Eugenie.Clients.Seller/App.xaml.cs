﻿namespace Eugenie.Clients.Seller
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Principal;
    using System.Windows;
    using System.Windows.Input;

    using Sv.Wpf.Core.Extensions;

    public partial class App
    {
        public App()
        {
#if !DEBUG
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

            Mouse.OverrideCursor = Cursors.None;
#endif
            this.InitializeComponent();
            this.LogUnhandledExceptions("Notifications");
        }

        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}