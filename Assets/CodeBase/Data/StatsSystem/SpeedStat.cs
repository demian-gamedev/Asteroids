using CodeBase.Data.StatsSystem.Main;
using CodeBase.Data.Tools;

namespace CodeBase.Data.StatsSystem
{
    [JsonTypeName("speed")]
    public struct SpeedStat : IStat
    {
        public int Value;
        public SpeedStat(int value)
        {
            Value = value;
        }
    }
}