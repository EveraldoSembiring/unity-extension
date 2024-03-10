using System.Collections.Generic;
using System.Diagnostics;

namespace UnityExtension
{
    public static class GameLogger
    {
        private static ILogger logger;
        private static List<KeyValuePair<LogSeverity, string>> unsentLogs = new List<KeyValuePair<LogSeverity, string>>();

        public static void Initialize(ILogger logger, LogSeverity severityLevel)
        {
            GameLogger.logger = logger;
            GameLogger.logger.SetSeverity((int)severityLevel);

            if (unsentLogs.Count > 0)
            {
                foreach (KeyValuePair<LogSeverity, string> log in unsentLogs)
                {
                    logger.Log((int)log.Key, log.Value);
                }
                unsentLogs.Clear();
            }
        }

        public static void Initialize(LogSeverity severityLevel)
        {
            Initialize(new UnityExtensionLogger(), severityLevel);
        }

        public static void SetSeverity(LogSeverity severityLevel)
        {
            logger.SetSeverity((int)severityLevel);
        }

        public static void Error(string content)
        {
            SendLog(LogSeverity.Error, content);
        }

        public static void Warning(string content)
        {
            SendLog(LogSeverity.Warning, content);
        }

        public static void Log(string content)
        {
            SendLog(LogSeverity.Log, content);
        }

        public static void Verbose(string content)
        {
            SendLog(LogSeverity.Verbose, content);
        }

        private static void SendLog(LogSeverity severityLevel, string content)
        {
            if(logger == null)
            {
                unsentLogs.Add(new KeyValuePair<LogSeverity, string>(severityLevel, content));
                return;
            }

            logger.Log((int)severityLevel, content);
        }
    }
}
