using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.LogService
{
    public class LogService : ILogService
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }
    }
}