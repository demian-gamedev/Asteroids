using CodeBase.Data.StaticData;
using CodeBase.Gameplay.Enemies;
using CodeBase.Interfaces.Infrastructure.Services;
using Zenject;

namespace CodeBase.Gameplay.Factories
{
    public class EnemyFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;

        public EnemyFactory(DiContainer container, IStaticDataService staticDataService)
        {
            _container = container;
            _staticDataService = staticDataService;
        }

        public Enemy SpawnEnemy(EnemyType type)
        {
            EnemyConfig enemyConfig = _staticDataService.ForEnemy(type);
            object[] additionalArgs = new object[]{enemyConfig};
            
            Enemy component = _container.InstantiatePrefabResourceForComponent<Enemy>(
                enemyConfig.PrefabPath, additionalArgs
            );

            return component;
        }
    }
}