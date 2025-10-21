using CodeBase.Interfaces.Infrastructure.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.SceneManagement
{
    public class SceneLoader : ISceneLoader{

        public async UniTask Load(string nextScene)
        {
            var handler = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
            await handler.ToUniTask();
        }
    }
}