using System;
using CodeBase.Data;
using CodeBase.Data.StatsSystem;
using CodeBase.Data.StatsSystem.Main;
using CodeBase.Gameplay.Enviroment;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Projectiles
{
    public class Bullet : Projectile, IArenaMember
    {
        public TransformData TransformData => _transformData;
        
        private Stats _stats;
        private TransformData _transformData;
        
        [Inject]
        public void Construct(Stats stats)
        {
            _stats = stats;
            _transformData = new TransformData(transform);
        }

        private void Start()
        {
            DestroyAfterLifetime().Forget();
        }

        private void Update()
        {
            _transformData.Position += _transformData.Direction * Time.deltaTime* (_stats.GetStat<SpeedStat>().Value);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
                if (!_stats.ContainsStat<PierceStat>())
                {
                    Destroy(this.gameObject);
                }
            }
        }
        private async UniTask DestroyAfterLifetime()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_stats.GetStat<LifetimeStat>().Value));
            if (this != null)
                Destroy(this.gameObject);
        }

    }
}