using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FilesMover
{
    /// <summary>
    /// A static class that writes to console and to log file
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Logging levels
        /// </summary>
        public enum Severity
        {
            Trace,
            Information,
            Warning,
            Error,
            Critical
        }

        /// <summary>
        /// Private field that holds output severity
        /// </summary>
        private static Severity _severity;

        /// <summary>
        /// Private field that holds path to log file
        /// </summary>
        private static string _logFilePath;

        /// <summary>
        /// Set logging level for output. Everything is always logged to file.
        /// </summary>
        public static void SetSeverity(Severity severity)
        {
            _severity = severity;
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        public static void Error(string messageFormat, params string[] args)
        {
            Log.Write(Severity.Error, messageFormat, args);
        }

        public static void Error(Exception ex)
        {
            Log.Write(Severity.Error, "Exception thrown: {0}", ex.ToString());
        }

        /// <summary>
        /// Logs an informative message
        /// </summary>
        public static void Information(string messageFormat, params string[] args)
        {
            Log.Write(Severity.Information, messageFormat, args);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        public static void Warning(string messageFormat, params string[] args)
        {
            Log.Write(Severity.Warning, messageFormat, args);
        }

        public static void Trace(string messageFormat, params string[] args)
        {
            Log.Write(Severity.Trace, messageFormat, args);
        }

        /// <summary>
        /// Private method that actually writes to console and log file
        /// </summary>
        /// <remarks>This is a bit slow since it writes synchronously to log file, but maximum speed is not the point of this program</remarks>
        private static void Write(Severity severity, string messageFormat, params string[] args)
        {
            var severityIdentifier = string.Empty;
            ConsoleColor consoleColor;
            //ConsoleColor consoleBackgroundColor;
            var message = string.Format(messageFormat, args);

            switch (severity)
            {
                case Severity.Error:
                    severityIdentifier = "ERR";
                    consoleColor = ConsoleColor.DarkRed;
                    //consoleBackgroundColor = ConsoleColor.Black;
                    break;
                case Severity.Warning:
                    severityIdentifier = "WAR";
                    consoleColor = ConsoleColor.DarkYellow;
//                    consoleBackgroundColor = ConsoleColor.Black;
                    break;
                case Severity.Trace:
                    severityIdentifier = "TRC";
                    consoleColor = ConsoleColor.Gray;
                    break;
                default:
                    severityIdentifier = "INF";
                    consoleColor = ConsoleColor.White;
//                    consoleBackgroundColor = ConsoleColor.Black;
                    break;
            }

            var consoleBackupColor = Console.ForegroundColor;
//            var consoleBackupBackgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = consoleColor;
//            Console.BackgroundColor = consoleBackgroundColor;
            Console.WriteLine(string.Format("'[{0:HH:mm:ss}] [{1}] {2}", DateTime.Now, severityIdentifier, message));
            Console.ForegroundColor = consoleBackupColor;
//            Console.BackgroundColor = consoleBackupBackgroundColor;

            //everything is written to file
            if (_logFilePath == null)
            {
                _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            }
            File.AppendAllText(_logFilePath, string.Format("'[{0:yyyy-MM-dd HH:mm:ss}] [{1}] {2}{3}", 
                    DateTime.Now, severityIdentifier, message, Environment.NewLine));
        }


    }
}
