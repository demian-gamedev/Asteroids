using CodeBase.Data;
using CodeBase.Data.Signals;
using CodeBase.Data.StatsSystem.Main;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.Factories;
using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Player
{
    public class PlayerPresentation : MonoBehaviour, IArenaMember, IDamageable, IPushable
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private ParticleSystem _particleSystem;
        
        private Player _player;
        private SignalBus _signalBus;

        public TransformData TransformData => _player.transformData;
        public float Speed => _player.velocity.Speed;
        public int LaserCharges => _player.LaserCharges;
        public float LaserChargeReload => _player.LaserChargesReload;


        [Inject]
        public void Construct(IInputService inputService, Stats playerStats, 
            ProjectileFactory projectileFactory, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _player = new Player(inputService, projectileFactory, playerStats);
        }
        private void Start()
        {
            _player.Initialize();
            _player.Died += PlayerOnDied;
        }

        private void PlayerOnDied()
        {
            Destroy(this.gameObject);
            _signalBus.Fire(new PlayerDiedSignal());
        }

        private void Update()
        {
            _player.Tick();
            transform.rotation = _player.transformData.RotationQuaternion;

            if (!_particleSystem.isPlaying && _player.IsInvulnerable)
            {
                _particleSystem.Play();
            }else if (_particleSystem.isPlaying && !_player.IsInvulnerable)
            {
                _particleSystem.Stop();
            }

            _collider.enabled = !_player.IsInvulnerable;
        }

        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IPushable pushable))
            {
                pushable.Push((other.transform.position - transform.position).normalized * GameConstants.CollisionKnockbackForce);
            }
        }

        public void TakeDamage()
        {
            _player.TakeDamage();
        }

        public void Push(Vector2 forceVector)
        {
            if (_player.IsInvulnerable) return;
            _player.velocity.Set(forceVector);
        }
    }
}