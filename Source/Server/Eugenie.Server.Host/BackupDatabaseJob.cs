namespace Eugenie.Server.Host
{
    using Quartz;

    public class BackupDatabaseJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var savePath = context.JobDetail.JobDataMap.GetString("savePath");
            BackupDatabaseService.Backup(savePath);
        }
    }
}
