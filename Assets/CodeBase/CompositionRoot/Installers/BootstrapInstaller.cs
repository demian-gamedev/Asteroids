using CodeBase.CompositionRoot.EntryPoints;
using CodeBase.Data.Signals;
using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.Services.AdsService;
using CodeBase.Infrastructure.Services.AnalyticsService;
using CodeBase.Infrastructure.Services.Camera;
using CodeBase.Infrastructure.Services.InputService;
using CodeBase.Infrastructure.Services.LogService;
using CodeBase.Infrastructure.Services.RandomizerService;
using CodeBase.Infrastructure.Services.ScoreService;
using CodeBase.Infrastructure.Services.StaticDataService;
using CodeBase.Interfaces.Infrastructure.Services;
using CodeBase.Interfaces.Infrastructure.Services.UI;
using CodeBase.UI.Controls;
using CodeBase.UI.ViewModels;
using UnityEngine;
using Zenject;

namespace CodeBase.CompositionRoot.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _cameraPrefab;
        public override void InstallBindings()
        {
            BindInputService();

            BindInputDetector();

            BindLogService();
            
            BindMobileInputProvider();
            
            BindCamera();
            
            BindSceneLoader();

            BindStaticDataService();

            BindRandomizeService();
            
            BindPlayerScoreService();
            
            BindAdsService();

            BindAnalyticsService();

            BindViewModels();

            InstallSignalBus();
            
            BindGameBootstrapper();
        }

        private void InstallSignalBus()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();
        }

        private void BindInputDetector()
        {
            Container.BindInterfacesAndSelfTo<InputTypeDetector>().AsSingle();
        }

        private void BindViewModels()
        {
            Container.BindInterfacesAndSelfTo<ScoreViewModel>().AsSingle().NonLazy();
        }

        private void BindLogService()
        {
            Container.Bind<ILogService>().To<LogService>().AsSingle();
        }
        private void BindCamera()
        {
            Container.Bind<Camera>().FromComponentInNewPrefab(_cameraPrefab)
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();
        }

        private void BindGameBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<BootstrapEntryPoint>()
                .AsSingle()
                .NonLazy();
        }

        private void BindSceneLoader() => 
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();

        private void BindStaticDataService() => 
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
        

        private void BindRandomizeService() => 
            Container.BindInterfacesAndSelfTo<RandomizerService>().AsSingle();

        private void BindPlayerScoreService()
        {
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle();
        }

        private void BindAdsService() => 
            Container.BindInterfacesAndSelfTo<LevelPlayAdsService>().AsSingle();

        private void BindAnalyticsService() => 
            Container.BindInterfacesTo<FirebaseAnalyticsService>().AsSingle();
                
        private void BindMobileInputProvider()
        {
            Container.Bind<IMobileInputProvider>().To<MobileInputProvider>().AsSingle();
        }
        private void BindInputService() =>
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
    }
}