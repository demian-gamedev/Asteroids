using CodeBase.CompositionRoot.EntryPoints;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.Factories;
using CodeBase.Gameplay.ObjectPool;
using CodeBase.Gameplay.Services.Providers;
using CodeBase.Gameplay.Services.SpawnService;
using CodeBase.Gameplay.ViewModels;
using CodeBase.UI.Factories;
using Zenject;

namespace CodeBase.CompositionRoot.Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameFactories();
            
            BindUIFactory();

            BindSpawnService();

            BindArena();
            
            BindPlayerProvider();

            BindViewModels();
            
            BindEntryPoint();
        }

        private void BindViewModels()
        {
            Container.BindInterfacesAndSelfTo<PlayerViewModel>().AsSingle().NonLazy();
        }
        
        private void BindUIFactory() =>
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();
        
        private void BindPlayerProvider()
        {
            Container.Bind<PlayerProvider>().To<PlayerProvider>().AsSingle();
        }

        private void BindSpawnService()
        {
            Container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle();
        }

        private void BindArena()
        {
            Container.BindTickableExecutionOrder<Arena>(-100);
            Container.BindInterfacesAndSelfTo<Arena>().AsSingle();
        }

        private void BindGameFactories()
        {
            Container.Bind<PlayerFactory>().ToSelf().AsSingle();
            Container.Bind<EnemyFactory>().ToSelf().AsSingle();
            Container.Bind<SpawnerFactory>().ToSelf().AsSingle();
            Container.Bind<ProjectileFactory>().ToSelf().AsSingle();
            Container.Bind<EnemyPoolFactory>().ToSelf().AsSingle();
        }

        private void BindEntryPoint()
        {
            Container.BindInterfacesAndSelfTo<GameplayEntryPoint>()
                .AsSingle()
                .NonLazy();
        }
        
    }
}