using CodeBase.Data.StaticData;
using UnityEngine;

namespace CodeBase.Data.Signals
{
    public class EnemyDiedSignal
    {
        public EnemyType Type;
        public int Score;

        public Vector2 Position;

        public EnemyDiedSignal(EnemyConfig config, TransformData transformData)
        {
            Type = config.Type;
            Score = config.ScoreReward;
            Position = transformData.Position;
        }
    }
}