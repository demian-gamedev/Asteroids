using CodeBase.Data;
using CodeBase.Data.StatsSystem;
using CodeBase.Data.StatsSystem.Main;
using CodeBase.Gameplay.Physic;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Enemies.Asteroids
{
    public class Asteroid : ITickable
    {
        public readonly CustomVelocity velocity;
        public readonly TransformData transformData;
        
        private readonly Stats _stats;

        public Asteroid(Stats stats)
        {
            _stats = stats;
            transformData = new TransformData();
            velocity = new CustomVelocity(transformData);
        }

        public void Tick()
        {
            velocity.AddForce(transformData.Direction * (Time.deltaTime * _stats.GetStat<SpeedStat>().Value));
            velocity.Tick(Time.deltaTime);
        }
    }
}