using CodeBase.Data.StaticData;
using CodeBase.Gameplay.Enemies;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.Factories;
using UnityEngine;

namespace CodeBase.Gameplay.ObjectPool
{
    public class EnemyPoolFactory
    {
        private readonly EnemyFactory _enemyFactory;

        public EnemyPoolFactory(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }

        public ObjectPool<Enemy> Create(EnemyType type, int maxSize, Arena arena)
        {
            return new ObjectPool<Enemy>(
                createFunc: () => _enemyFactory.SpawnEnemy(type),
                onGet: enemy =>
                {
                    enemy.gameObject.SetActive(true);
                    arena.RegisterMember(enemy);
                },
                onRelease: enemy =>
                {
                    enemy.gameObject.SetActive(false);
                    arena.RemoveMember(enemy);
                },
                onDestroy: enemy =>
                {
                    if (enemy != null)
                        Object.Destroy(enemy.gameObject);
                },
                maxSize: maxSize
            );
        }
    }
}