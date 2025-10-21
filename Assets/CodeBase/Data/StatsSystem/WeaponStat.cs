using CodeBase.Data.StaticData;
using CodeBase.Data.StatsSystem.Main;
using CodeBase.Data.Tools;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Converters;

namespace CodeBase.Data.StatsSystem
{
    [JsonTypeName("weapon")]
    public struct WeaponStat : IStat
    {
        public float ReloadTime;
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectileType ProjectileType;
    }
}