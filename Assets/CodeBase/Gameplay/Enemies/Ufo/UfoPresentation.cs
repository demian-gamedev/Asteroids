using CodeBase.Data.Signals;
using CodeBase.Data.StaticData;
using CodeBase.Gameplay.Services.Providers;
using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Enemies.Ufo
{
    public class UfoPresentation : Enemy, IDamageable, IPushable
    {
        private Ufo _model;
        private IScoreService _scoreService;
        private EnemyConfig _config;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(EnemyConfig config, PlayerProvider playerProvider, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            _model = new Ufo(config.Stats, playerProvider, transform);
            TransformData = _model.transformData;
        }

        private void Update()
        {
            _model.Tick();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IPushable pushable))
            {
                pushable.Push((other.transform.position - transform.position).normalized * GameConstants.CollisionKnockbackForce);
            }
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }
        }

        public void TakeDamage()
        {
            _signalBus.Fire(new EnemyDiedSignal(_config, _model.transformData));
            ReturnToPool();
        }

        public void Push(Vector2 forceVector)
        {
            _model.velocity.Set(forceVector);
        }
    }
}