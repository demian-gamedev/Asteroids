using System;
using CodeBase.Data.StatsSystem.Main;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Converters;

namespace CodeBase.Data.StaticData
{
    [Serializable]
    public class ProjectileConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectileType Type;
        public string PrefabPath;
        public Stats Stats;
    }
}