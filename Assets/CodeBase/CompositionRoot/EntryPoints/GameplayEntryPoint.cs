using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.Factories;
using CodeBase.Gameplay.Services.SpawnService;
using CodeBase.Interfaces.Infrastructure.Services;
using CodeBase.Interfaces.Infrastructure.Services.UI;
using Zenject;

namespace CodeBase.CompositionRoot.EntryPoints
{
    public class GameplayEntryPoint : IInitializable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly Arena _arena;
        private readonly ICameraService _cameraService;
        private readonly EnemySpawnService _enemySpawnService;
        private readonly IUIFactory _uiFactory;
        private readonly IScoreService _scoreService;

        public GameplayEntryPoint(PlayerFactory playerFactory,
            Arena arena, ICameraService cameraService,
            EnemySpawnService enemySpawnService, IUIFactory uiFactory,
            IScoreService scoreService)
        {
            _playerFactory = playerFactory;
            _arena = arena;
            _cameraService = cameraService;
            _enemySpawnService = enemySpawnService;
            _uiFactory = uiFactory;
            _scoreService = scoreService;
        }
        public void Initialize()
        {
            _scoreService.ResetScore();
            
            Gameplay.Player.PlayerPresentation playerPresentation = _playerFactory.CreatePlayer();
            _arena.Initialize();
            _cameraService.Follow(playerPresentation.transform);
            
            _enemySpawnService.StartSpawn();
            
            _uiFactory.CreateHUD();
        }
    }
}