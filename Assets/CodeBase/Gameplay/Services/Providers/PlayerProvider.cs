namespace CodeBase.Gameplay.Services.Providers
{
    public class PlayerProvider
    {
        private Player.PlayerPresentation _playerPresentation;

        public Player.PlayerPresentation PlayerPresentation => _playerPresentation;
        public bool PlayerExists => _playerPresentation != null;
        
        public void RegisterPlayer(Player.PlayerPresentation playerPresentation)
        {
            _playerPresentation = playerPresentation;
        }

        public void UnregisterPlayer()
        {
            _playerPresentation = null;
        }
    }
}