namespace Eugenie.Server.Host
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.IO;

    using Api;

    using Microsoft.Owin.Hosting;

    using Quartz;
    using Quartz.Impl;

    public class WebApiHost : IWebApiHost
    {
        public readonly IScheduler scheduler;

        public WebApiHost()
        {
            var schedulerFactory = new StdSchedulerFactory();
            this.scheduler = schedulerFactory.GetScheduler();
        }

        public IScheduler Scheduler => this.scheduler;

        public void HostWebApi(string address)
        {
            try
            {
                WebApp.Start<Startup>(address);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.ToString().ToLower().Contains("denied"))
                {
                    throw new AccessDeniedException();
                }

                throw new PortInUseException();
            }
        }

        public void HostWebApi(int port)
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var address = "http://" + localIp + ":" + port;

            this.HostWebApi(address);
        }

        public void AutoBackupDatabase(int hours, int minutes, string path)
        {
            Directory.CreateDirectory(path);

            var job = JobBuilder.Create<BackupDatabaseJob>().WithIdentity("myJob", "group1").UsingJobData("path", path).Build();
            var trigger = TriggerBuilder.Create().WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hours, minutes)).Build();

            this.Scheduler.DeleteJob(job.Key);
            this.Scheduler.ScheduleJob(job, trigger);
            this.Scheduler.Start();
        }
    }
}