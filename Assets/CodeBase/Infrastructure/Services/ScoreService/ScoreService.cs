using System;
using CodeBase.Data.Signals;
using CodeBase.Interfaces.Infrastructure.Services;
using Zenject;

namespace CodeBase.Infrastructure.Services.ScoreService
{
    public class ScoreService : IScoreService, IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        public int Score { get; private set; }
        public event Action ScoreChanged;

        public ScoreService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void OnEnemyDied(EnemyDiedSignal signal)
        {
            AddScore(signal.Score);
        }

        public void AddScore(int amount)
        {
            Score += amount;
            ScoreChanged?.Invoke();
        }

        public void ResetScore()
        { 
            Score = 0;
            ScoreChanged?.Invoke();
        }
    }
}