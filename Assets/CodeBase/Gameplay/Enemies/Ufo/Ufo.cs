using CodeBase.Data;
using CodeBase.Data.StatsSystem;
using CodeBase.Data.StatsSystem.Main;
using CodeBase.Gameplay.Physic;
using CodeBase.Gameplay.Services.Providers;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Enemies.Ufo
{
    public class Ufo : ITickable
    {
        public readonly CustomVelocity velocity;
        public readonly TransformData transformData;
        
        private readonly Stats _stats;
        private readonly PlayerProvider _playerProvider;
        private readonly Transform _viewTransform;
        
        private Vector2 _directionAxis;

        public Ufo(Stats stats, PlayerProvider playerProvider, Transform viewTransform)
        {
            _stats = stats;
            _playerProvider = playerProvider;
            _viewTransform = viewTransform;
            transformData = new TransformData();
            velocity = new CustomVelocity(transformData);
        }

        public void Tick()
        {
            SetMoveDirection(
                _playerProvider.PlayerPresentation.TransformData.Position - (Vector2)_viewTransform.position
            );
            
            velocity.AddForce(_directionAxis * (Time.deltaTime * _stats.GetStat<SpeedStat>().Value));
            velocity.Tick(Time.deltaTime);
        }

        public void SetMoveDirection(Vector2 dir)
        {
            _directionAxis = dir.normalized;
        }
    }
}