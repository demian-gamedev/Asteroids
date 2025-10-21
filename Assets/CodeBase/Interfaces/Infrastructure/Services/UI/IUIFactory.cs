namespace CodeBase.Interfaces.Infrastructure.Services.UI
{
    public interface IUIFactory
    {
        void CreateHUD();
        void CreateMainMenu();
        void CreateDeathScreen();
    }
}