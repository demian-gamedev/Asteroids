using System;
using CodeBase.Interfaces.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using Unity.Services.LevelPlay;

namespace CodeBase.Infrastructure.Services.AdsService
{
    public class LevelPlayAdsService : IAdsService
    {
        private const string APP_KEY = "PLACEHOLDER_KEY";
        private LevelPlayInterstitialAd _interstitialAd;
        private Action _currentCallback;
        
        public bool IsInterstitialAvailable => _interstitialAd != null && _interstitialAd.IsAdReady();

        public UniTask Initialize()
        {
            LevelPlay.Init(APP_KEY);
            LevelPlay.OnInitSuccess += LevelPlayOnOnInitSuccess;
            return UniTask.CompletedTask;
        }

        private void LevelPlayOnOnInitSuccess(LevelPlayConfiguration obj)
        {
            SetupInterstitial();
        }

        public void ShowInterstitial(Action callback)
        {
            if (!IsInterstitialAvailable)
            {
                callback?.Invoke();
                return;
            }
            
            _currentCallback = callback;
            _interstitialAd.ShowAd();
        }

        private void SetupInterstitial()
        {
            _interstitialAd = new LevelPlayInterstitialAd("interstitial");
            
            _interstitialAd.OnAdClosed += InterstitialAdOnOnAdClosed;
            
            _interstitialAd.LoadAd();
        }

        private void InterstitialAdOnOnAdClosed(LevelPlayAdInfo obj)
        {
            _currentCallback?.Invoke();
            _currentCallback = null;
            _interstitialAd.LoadAd();
        }
    }
}