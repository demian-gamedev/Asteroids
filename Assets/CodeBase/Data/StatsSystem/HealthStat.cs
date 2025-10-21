using CodeBase.Data.StatsSystem.Main;
using CodeBase.Data.Tools;

namespace CodeBase.Data.StatsSystem
{
    [JsonTypeName("health")]
    public struct HealthStat : IStat
    {
        public int Value;
        public HealthStat(int value)
        {
            Value = value;
        }
    }
}