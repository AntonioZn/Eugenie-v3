namespace Eugenie.Clients.AdminService
{
    using System;
    using System.ServiceProcess;

    static class Program
    {
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                var service = new AdminService();
                service.TestStartupAndStop();
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                                    {
                                        new AdminService()
                                    };

                ServiceBase.Run(servicesToRun);
            }
        }
    }
}