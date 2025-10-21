using System;
using Cysharp.Threading.Tasks;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface IAdsService
    {
        bool IsInterstitialAvailable { get; }
        UniTask Initialize();
        void ShowInterstitial(Action callback);
    }
}