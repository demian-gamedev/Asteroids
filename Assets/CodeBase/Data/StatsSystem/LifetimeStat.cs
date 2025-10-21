using CodeBase.Data.StatsSystem.Main;
using CodeBase.Data.Tools;

namespace CodeBase.Data.StatsSystem
{
    [JsonTypeName("lifetime")]
    public struct LifetimeStat : IStat
    {
        public float Value;
        public LifetimeStat(float value)
        {
            Value = value;
        }
    }
}