namespace Eugenie.Server.Host
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.IO.Compression;

    public static class BackupDatabaseService
    {
        public static void Backup(string savePath)
        {
            Directory.CreateDirectory(savePath);

            var windowsDrive = Path.GetPathRoot(Environment.SystemDirectory);
            var tempPath = Path.Combine(windowsDrive, "eugenie_temp");
            Directory.CreateDirectory(tempPath);

            var connectionString = ConfigurationManager.ConnectionStrings["Eugenie"].ConnectionString;
            var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);
            var backupFileName = Path.Combine(tempPath, $"{sqlConStrBuilder.InitialCatalog}-{DateTime.Now:yyyy-MM-dd-HH-mm}.bak");

            using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
            {
                var query = $"BACKUP DATABASE {sqlConStrBuilder.InitialCatalog} TO DISK='{backupFileName}'";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            ZipFile.CreateFromDirectory(tempPath, Path.Combine(savePath, $"{sqlConStrBuilder.InitialCatalog}-{DateTime.Now:yyyy-MM-dd-HH-mm}.zip"), CompressionLevel.Optimal, false);

            Directory.Delete(tempPath, true);
        }
    }
}
