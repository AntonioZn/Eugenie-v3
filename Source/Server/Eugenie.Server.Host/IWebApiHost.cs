namespace Eugenie.Server.Host
{
    using Quartz;

    public interface IWebApiHost
    {
        void AutoBackupDatabase(int hours, int minutes, string path);

        void HostWebApi(string address);

        void HostWebApi(int port);

        IScheduler Scheduler { get; }
    }
}