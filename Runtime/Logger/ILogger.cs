namespace UnityExtension
{
    public interface ILogger
    {
        void Log(int severity, string content);
        void SetSeverity(int severity);
    }
}