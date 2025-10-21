using System;
using CodeBase.Data.Signals;
using CodeBase.Interfaces.Infrastructure.Services.UI;
using CodeBase.UI.Controls;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Factories
{
    public class UIFactory : IUIFactory, IInitializable, IDisposable
    {
        private readonly DiContainer _container;
        private readonly IMobileInputProvider _mobileInputProvider;
        private readonly SignalBus _signalBus;

        public UIFactory(DiContainer container, IMobileInputProvider mobileInputProvider,
            SignalBus signalBus)
        {
            _container = container;
            _mobileInputProvider = mobileInputProvider;
            _signalBus = signalBus;
        }

        public void CreateHUD()
        {
            GameObject gameObject = _container.InstantiatePrefabResource(UIFactoryAssets.HUD);
            _mobileInputProvider.Register(gameObject.GetComponentInChildren<MobileInput>());
        }

        public void CreateMainMenu()
        {
            _container.InstantiatePrefabResource(UIFactoryAssets.MainMenu);
        }

        public void CreateDeathScreen()
        {
            _container.InstantiatePrefabResource(UIFactoryAssets.DeathScreen);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }
        
        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            CreateDeathScreen();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }
    }
}