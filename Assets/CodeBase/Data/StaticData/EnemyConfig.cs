using System;
using CodeBase.Data.StatsSystem.Main;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Converters;

namespace CodeBase.Data.StaticData
{
    [Serializable]
    public struct EnemyConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EnemyType Type;
        public string PrefabPath;
        public float SpawnRate;
        public int ScoreReward;
        public Stats Stats;
    }
}