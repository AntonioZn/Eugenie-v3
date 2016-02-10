namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.IO;

    using Notifications;

    public static class Logger
    {
        private const string LogFilename = "log.txt";

        public static void LogToFile(Exception e, string path)
        {
            Directory.CreateDirectory(path);
            var log = $"{DateTime.Now}{Environment.NewLine}{e.Message}{Environment.NewLine}{e.InnerException}{Environment.NewLine}{e.StackTrace}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";

            File.AppendAllText(path + "\\" + LogFilename, log);

            NotificationsHost.Error("Грешка", "Случи се неочаквана грешка и беше записана в лог файла.");
        }
    }
}