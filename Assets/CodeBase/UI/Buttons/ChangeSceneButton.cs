using CodeBase.Interfaces.Infrastructure.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class ChangeSceneButton : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private Button _button;
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _button.onClick.AddListener(() => _sceneLoader.Load(_sceneName));
        }
    }
}