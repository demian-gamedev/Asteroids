using System;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface IScoreService
    {
        public int Score { get; }
        public event Action ScoreChanged;
        public void AddScore(int amount);
        public void ResetScore();
    }
}