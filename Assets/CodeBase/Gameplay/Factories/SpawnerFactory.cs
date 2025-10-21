using CodeBase.Gameplay.Services.SpawnService.Spawners;
using Zenject;

namespace CodeBase.Gameplay.Factories
{
    public class SpawnerFactory
    {
        private readonly DiContainer _container;

        public SpawnerFactory(DiContainer container)
        {
            _container = container;
        }
        public EnemySpawner CreateDefaultEnemySpawner()
        {
            return _container.Instantiate<EnemySpawner>();
        }

        public EnemySpawnerOnEnemyDeath CreateEnemySpawnerOnEnemyDeath()
        {
            return _container.Instantiate<EnemySpawnerOnEnemyDeath>();
        }
    }
}