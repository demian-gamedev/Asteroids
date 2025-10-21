using CodeBase.Data.StaticData;
using CodeBase.Data.StatsSystem.Main;
using CodeBase.Data.Tools;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Converters;

namespace CodeBase.Data.StatsSystem
{
    [JsonTypeName("skill")]
    public struct SkillStat : IStat
    {
        public float ReloadTime;
        public int MaxCharges;
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectileType ProjectileType;
    }
}