using CodeBase.Data.StaticData;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface IStaticDataService
    {
        void Initialize();
        PlayerConfig ForPlayer();
        EnemyConfig ForEnemy(EnemyType type);
        MapConfig ForMap();
        ProjectileConfig ForProjectile(ProjectileType type);
    }
}