using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QuoteServer
{
    public class FileLogger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new object();

        public FileLogger(string logFilePath)
        {
            if (string.IsNullOrEmpty(logFilePath))
            {
                throw new ArgumentNullException(nameof(logFilePath));
            }
            _logFilePath = logFilePath;

            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
        }

        public async Task LogErrorAsync(string message, Exception ex = null)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - ERROR: {message}";
            if (ex != null)
            {
                logEntry += $" | Exception: {ex.Message} | StackTrace: {ex.StackTrace}";
            }
            logEntry += Environment.NewLine;

            try
            {
                lock (_lock)
                {
                    // Use File.AppendAllText for simplicity; FileStream for high concurrency if needed
                    File.AppendAllText(_logFilePath, logEntry);
                }
            }
            catch (Exception)
            {
                // Silent fail to avoid crashing; could log to console in debug mode
#if DEBUG
                Console.WriteLine($"Failed to write to log file: {logEntry}");
#endif
            }
        }
    }
}