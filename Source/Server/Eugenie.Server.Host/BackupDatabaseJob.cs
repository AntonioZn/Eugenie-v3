namespace Eugenie.Server.Host
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.IO.Compression;

    using Quartz;

    public class BackupDatabaseJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var path = context.JobDetail.JobDataMap.GetString("path");

            var connectionString = ConfigurationManager.ConnectionStrings["Eugenie"].ConnectionString;

            var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);

            var tempPath = Path.Combine(path, "temp");
            Directory.CreateDirectory(tempPath);
            var backupFileName = Path.Combine(tempPath, $"{sqlConStrBuilder.InitialCatalog}-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}.bak");

            using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
            {
                var query = $"BACKUP DATABASE {sqlConStrBuilder.InitialCatalog} TO DISK='{backupFileName}'";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            ZipFile.CreateFromDirectory(tempPath, Path.Combine(path, $"{sqlConStrBuilder.InitialCatalog}-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}.zip"), CompressionLevel.Optimal, false);
            Directory.Delete(tempPath, true);
        }
    }
}
