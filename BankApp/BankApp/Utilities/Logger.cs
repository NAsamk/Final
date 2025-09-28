using System;
using System.IO;

namespace BankApp.Utilities
{
    internal static class Logger
    {
        private static readonly string LogPath = "C:\\Users\\User\\Desktop\\homework-8\\BankApp\\BankApp\\Data\\log.txt";

        public static void LogError(Exception ex)
        {
            try
            {
                var directory = Path.GetDirectoryName(LogPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string logMessage = $"[{DateTime.Now}] ERROR: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
                File.AppendAllText(LogPath, logMessage);
            }
            catch
            {
                // თუ ლოგის ჩაწერა ვერ მოხერხდა, არაფერს ვაკეთებთ
            }
        }
    }
}
