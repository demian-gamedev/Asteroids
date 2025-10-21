using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface IAnalyticsService
    {
        UniTask Initialize();
        UniTask SendEvent(string eventName);
        UniTask SendEvent(string eventName, Dictionary<string, object> parameters);
    }
}