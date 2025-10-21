namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface ILogService
    {
        void Log(string message);
        void LogError(string message);
        void LogWarning(string message);
    }
}