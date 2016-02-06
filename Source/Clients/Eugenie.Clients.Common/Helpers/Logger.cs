namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.IO;

    public static class Logger
    {
        private const string LogFilename = "log.txt";

        public static void LogToFile(Exception exception, string path)
        {
            var log = DateTime.Now + Environment.NewLine + exception.InnerException + Environment.NewLine + exception.StackTrace + Environment.NewLine + Environment.NewLine;

            File.AppendAllText(path + "\\" + LogFilename, log);
        }
    }
}
