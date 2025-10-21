using CodeBase.Data.StaticData;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.Services.Providers;
using CodeBase.Interfaces.Infrastructure.Services;
using Zenject;

namespace CodeBase.Gameplay.Factories
{
    public class PlayerFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly Arena _arena;
        private readonly PlayerProvider _playerProvider;
        private readonly DiContainer _container;

        public PlayerFactory(DiContainer container,
            IStaticDataService staticDataService, PlayerProvider playerProvider)
        {
            _container = container;
            _staticDataService = staticDataService;
            _playerProvider = playerProvider;
        }

        public Player.PlayerPresentation CreatePlayer()
        {
            PlayerConfig playerData = _staticDataService.ForPlayer();
            Player.PlayerPresentation playerPresentation = _container.InstantiatePrefabResourceForComponent<Player.PlayerPresentation>(
                playerData.PrefabPath, new object[]{playerData.Stats}
                );
            
            _playerProvider.RegisterPlayer(playerPresentation);
            return playerPresentation;
        }
    }
}