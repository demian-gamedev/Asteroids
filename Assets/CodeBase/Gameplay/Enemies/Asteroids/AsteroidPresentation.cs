using CodeBase.Data.Signals;
using CodeBase.Data.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Enemies.Asteroids
{
    public class AsteroidPresentation : Enemy, IDamageable, IPushable
    {
        private Asteroid _model;
        private EnemyConfig _config;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(EnemyConfig config, SignalBus signalBus)
        {
            _config = config;
            _model = new Asteroid(config.Stats);
            TransformData = _model.transformData;
            _signalBus = signalBus;
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