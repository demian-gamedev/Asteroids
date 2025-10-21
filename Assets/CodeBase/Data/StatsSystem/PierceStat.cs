using CodeBase.Data.StatsSystem.Main;
using CodeBase.Data.Tools;

namespace CodeBase.Data.StatsSystem
{
    [JsonTypeName("pierce")]
    public struct PierceStat : IStat
    {
        public bool Infinite;
    }
}