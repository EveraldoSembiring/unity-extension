using System.Collections.Generic;
using UnityEngine;

namespace UnityExtension
{
    public class UnityExtensionLogger : ILogger
    {
        int severityLevel;

        public UnityExtensionLogger()
        {
            severityLevel = -1;
        }

        public void SetSeverity(int severityLevel)
        {
            this.severityLevel = severityLevel;
        }

        public void Log(int severityLevel, string content)
        {
            if(this.severityLevel < 0 || this.severityLevel < (int)severityLevel)
            {
                return;
            }

            LogSeverity severity = (LogSeverity)severityLevel;

            switch (severity)
            {
                case LogSeverity.Error:
                    Debug.LogError(content);
                    break;
                case LogSeverity.Warning:
                    Debug.LogWarning(content);
                    break;
                case LogSeverity.Log:
                case LogSeverity.Verbose:
                    Debug.Log(content);
                    break;
            }
        }
    }
}