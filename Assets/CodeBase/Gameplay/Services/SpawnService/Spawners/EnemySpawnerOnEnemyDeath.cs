using System;
using CodeBase.Data.Signals;
using CodeBase.Data.StaticData;
using CodeBase.Gameplay.Enemies;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.Factories;
using CodeBase.Gameplay.ObjectPool;
using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Services.SpawnService.Spawners
{
    public class EnemySpawnerOnEnemyDeath : IInitializable, IDisposable
    {
        private readonly EnemyPoolFactory _poolFactory;
        private readonly Arena _arena;
        private readonly SignalBus _signalBus;
        private readonly IRandomizerService _randomizerService;
        private EnemyType _observingEnemyType;
        private EnemyType _spawningEnemyType;
        
        private ObjectPool<Enemy> _enemyPool;
        private int _maxEnemies;
        private int _spawnPerDeath;

        public EnemySpawnerOnEnemyDeath(EnemyPoolFactory poolFactory, IRandomizerService randomizerService,
            Arena arena, SignalBus signalBus)
        {
            _poolFactory = poolFactory;
            _arena = arena;
            _signalBus = signalBus;
            _randomizerService = randomizerService;
            _observingEnemyType = EnemyType.None;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDiedSignal>( OnEnemyDied);
        }

        private void OnEnemyDied(EnemyDiedSignal signal)
        {
            if (signal.Type == _observingEnemyType)
            {
                for (int i = 0; i < _spawnPerDeath; i++)
                {
                    Spawn(signal.Position);
                }
            }
        }

        public void SetSpawnData(EnemyType observingType, EnemyType spawningType,
            int col, int max)
        {
            _spawnPerDeath = col;
            _observingEnemyType = observingType;
            _spawningEnemyType = spawningType;
            _maxEnemies = max;
            
            _enemyPool = _poolFactory.Create(spawningType, max, _arena);
            _enemyPool.PreWarm(_maxEnemies);
        }

        private void Spawn(Vector2 pos)
        {
            if (_enemyPool.CountInactive == 0)
                return;
            Enemy enemy = _enemyPool.Get();
            enemy.TransformData.Position = pos;
            enemy.TransformData.Rotation = GetRandomRotation(pos);
        }

        private float GetRandomRotation(Vector2 position)
        {
            Vector2 randomPointInBounds = new Vector2(
                _randomizerService.Range(-_arena.Size.x, _arena.Size.x),
                _randomizerService.Range(-_arena.Size.y, _arena.Size.y)
            );
            Vector2 directionToRandomPoint = (randomPointInBounds - position).normalized;
            float angle = Mathf.Atan2(directionToRandomPoint.y, directionToRandomPoint.x) * Mathf.Rad2Deg;
            return angle;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDied);
        }
    }
}