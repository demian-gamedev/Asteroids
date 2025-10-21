using System;
using System.Threading;
using CodeBase.Data.StaticData;
using CodeBase.Gameplay.Enemies;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.ObjectPool;
using CodeBase.Interfaces.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.Services.SpawnService.Spawners
{
    public class EnemySpawner : IDisposable
    {
        private const float ADDITIONAL_OFFSET = 200f;

        private readonly EnemyPoolFactory _poolFactory;
        private readonly Arena _arena;
        private readonly IRandomizerService _randomizerService;
        private float _spawnRate = 1f;
        
        private CancellationTokenSource _spawnCts;
        private bool _isSpawning;
        
        private ObjectPool<Enemy> _enemyPool;

        private int _maxEnemies;

        public EnemySpawner(EnemyPoolFactory poolFactory, IRandomizerService randomizerService,
            Arena arena)
        {
            _poolFactory = poolFactory;
            _arena = arena;
            _randomizerService = randomizerService;
        }

        public void SetSpawnData(EnemyType type, float newRate, int max)
        {
            _maxEnemies = max;
            _spawnRate = newRate;

            _enemyPool = _poolFactory.Create(type, max, _arena);
            _enemyPool.PreWarm(_maxEnemies);
        }

        public void StartSpawning()
        {
            if (_isSpawning) return;
            
            _isSpawning = true;
            _spawnCts = new CancellationTokenSource();
            SpawnLoop(_spawnCts.Token).Forget();
        }

        public void StopSpawning()
        {
            if (!_isSpawning) return;
            
            _spawnCts?.Cancel();
            _spawnCts?.Dispose();
            _spawnCts = null;
            _isSpawning = false;
        }

        private async UniTask SpawnLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Spawn();
                await UniTask
                    .WaitForSeconds(1/_spawnRate, cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
            }
        }

        private void Spawn()
        {
            if (_enemyPool.CountInactive == 0)
                return;
            Vector2 pos = _randomizerService.GetRandomPositionOnBoundsEdge(_arena.Size,
                _arena.Center, ADDITIONAL_OFFSET);

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
            _enemyPool.Clear();
        }
    }
}