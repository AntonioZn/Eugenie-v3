namespace Eugenie.Server.Host
{
    using System.Threading.Tasks;

    using Quartz;

    public class BackupDatabaseJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var savePath = context.JobDetail.JobDataMap.GetString("savePath");
            BackupDatabaseService.Backup(savePath);

            return Task.CompletedTask;
        }
    }
}
