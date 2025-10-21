using CodeBase.Interfaces.Infrastructure.Services.UI;
using Zenject;

namespace CodeBase.CompositionRoot.EntryPoints
{
    public class MainMenuEntryPoint : IInitializable
    {
        private readonly IUIFactory _uiFactory;

        public MainMenuEntryPoint(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        public void Initialize()
        {
            _uiFactory.CreateMainMenu();
        }
    }
}