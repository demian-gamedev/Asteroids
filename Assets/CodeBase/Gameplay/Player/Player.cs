using System;
using CodeBase.Data;
using CodeBase.Data.StatsSystem;
using CodeBase.Data.StatsSystem.Main;
using CodeBase.Gameplay.Factories;
using CodeBase.Gameplay.Physic;
using CodeBase.Interfaces.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Player
{
    public class Player : ITickable, IInitializable
    {
        public readonly TransformData transformData;
        public readonly CustomVelocity velocity;

        private readonly IInputService _inputService;
        private readonly ProjectileFactory _projectileFactory;
        private readonly Stats _playerStats;

        private const float INVULNERABILITY_TIME = 3f;
        
        private int _currentHealth;

        private int _laserCharges;
        private float _laserChargesReload;
        
        private float _gunReload;
        

        public bool IsInvulnerable { get; private set; }
        public int LaserCharges => _laserCharges;
        public float LaserChargesReload => _laserChargesReload;
        public event Action Died;

        public Player(IInputService inputService, ProjectileFactory projectileFactory, Stats playerStats)
        {
            _inputService = inputService;
            _projectileFactory = projectileFactory;
            _playerStats = playerStats;

            transformData = new TransformData(Vector2.zero);
            velocity = new CustomVelocity(transformData);
        }

        public void Initialize()
        {
            _currentHealth = _playerStats.GetStat<HealthStat>().Value;
        }

        public void Tick()
        {
            HandleAttack();
            
            if (!IsInvulnerable)
            {
                velocity.AddForce(_inputService.GetMovement() * transformData.Direction * Time.deltaTime * _playerStats.GetStat<SpeedStat>().Value);
                velocity.AddAngularForce(_inputService.GetRotation());
            }
            velocity.Tick(Time.deltaTime);
        }

        private void HandleAttack()
        {
            ReloadTick();
            if (!IsInvulnerable)
            {
                if (_inputService.GetBaseAttack())
                    ShootBullet();
                if (_inputService.GetSpecialAttack())
                    SpecialAttack();
            }
        }

        public void TakeDamage()
        {
            if (!IsInvulnerable)
            {
                _currentHealth--;
                Invulnerability();
                if (_currentHealth <= 0)
                {
                    Died?.Invoke();
                }
            }
        }

        private async UniTask Invulnerability()
        {
            IsInvulnerable = true;
            await UniTask.WaitForSeconds(INVULNERABILITY_TIME);
            IsInvulnerable = false;
        }
        private void ReloadTick()
        {
            _gunReload -= Time.deltaTime;

            _laserChargesReload -= Time.deltaTime;
            _laserChargesReload = Mathf.Max(0f, _laserChargesReload);

            if (_laserChargesReload <= 0)
            {
                if (_laserCharges == _playerStats.GetStat<SkillStat>().MaxCharges)
                {
                    _laserChargesReload = 0f;
                }
                else
                {
                    _laserChargesReload = _playerStats.GetStat<SkillStat>().ReloadTime;
                    _laserCharges++;

                }
            }
        }

        private void SpecialAttack()
        {
            if (_laserCharges > 0)
            {
                _laserCharges--;
                _projectileFactory.CreateProjectile(_playerStats.GetStat<SkillStat>().ProjectileType,
                    transformData.Position,
                    transformData.RotationQuaternion);
            }
        }

        private void ShootBullet()
        {
            if (_gunReload <= 0)
            {
                _gunReload = _playerStats.GetStat<WeaponStat>().ReloadTime;
                _projectileFactory.CreateProjectile(_playerStats.GetStat<WeaponStat>().ProjectileType,
                    transformData.Position,
                    transformData.RotationQuaternion);
            }
        }
    }
}