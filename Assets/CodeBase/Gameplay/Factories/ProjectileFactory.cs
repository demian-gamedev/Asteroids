using CodeBase.Data.StaticData;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.Projectiles;
using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Factories
{
    public class ProjectileFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;
        private readonly Arena _arena;

        public ProjectileFactory(DiContainer container, IStaticDataService staticDataService,
            Arena arena)
        {
            _container = container;
            _staticDataService = staticDataService;
            _arena = arena;
        }
        
        
        public void CreateProjectile(ProjectileType type, Vector2 position, Quaternion rotation)
        {
            ProjectileConfig config = _staticDataService.ForProjectile(type);

            Projectile projectile = _container.InstantiatePrefabResourceForComponent<Projectile>(
                config.PrefabPath, position, rotation, null,
                new object[] {config.Stats}
            );

            if (projectile is IArenaMember member)
            {
                _arena.RegisterMember(member);
            } 
        }
    }
}