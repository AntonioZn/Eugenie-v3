namespace Eugenie.Server.Host
{
    using System;

    using Quartz;

    public interface IWebApiHost
    {
        //void AutoBackupDatabase(int hours, int minutes, string path);

        IDisposable HostWebApi(string address);

        IDisposable HostWebApi(int port);

        //IScheduler Scheduler { get; }
    }
}